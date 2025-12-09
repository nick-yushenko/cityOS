using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CityOS.Data;
using CityOS.Models;
using System.Text.Json;
using OfficeOpenXml;

namespace CityOS.Controllers;

[ApiController]
[Route("api/bi")]
public class BIController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<BIController> _logger;

    public BIController(AppDbContext db, ILogger<BIController> logger)
    {
        _db = db;
        _logger = logger;
    }

    #region Entities CRUD

    /// <summary>
    /// Получить список всех сущностей
    /// </summary>
    [HttpGet("entities")]
    public async Task<ActionResult<IEnumerable<object>>> GetEntities()
    {
        var entities = await _db.BIEntities
            .Select(e => new
            {
                id = e.Id,
                name = e.Name,
                code = e.Code,
                description = e.Description
            })
            .ToListAsync();

        return Ok(entities);
    }

    /// <summary>
    /// Получить сущность по ID с полями и связями
    /// </summary>
    [HttpGet("entities/{id}")]
    public async Task<ActionResult<object>> GetEntity(int id)
    {
        var entity = await _db.BIEntities.FindAsync(id);
        if (entity == null)
        {
            return NotFound(new { message = $"Сущность с ID {id} не найдена" });
        }

        var fields = await _db.BIFields
            .Where(f => f.EntityId == id)
            .OrderBy(f => f.OrderIndex)
            .Select(f => new
            {
                id = f.Id,
                entityId = f.EntityId,
                name = f.Name,
                code = f.Code,
                dataType = f.DataType,
                isRequired = f.IsRequired,
                isKey = f.IsKey,
                orderIndex = f.OrderIndex,
                refEntityId = f.RefEntityId
            })
            .ToListAsync();

        var relations = await _db.BIRelations
            .Where(r => r.FromEntityId == id || r.ToEntityId == id)
            .Select(r => new
            {
                id = r.Id,
                fromEntityId = r.FromEntityId,
                toEntityId = r.ToEntityId,
                fromFieldCode = r.FromFieldCode,
                toFieldCode = r.ToFieldCode,
                relationType = r.RelationType
            })
            .ToListAsync();

        return Ok(new
        {
            id = entity.Id,
            name = entity.Name,
            code = entity.Code,
            description = entity.Description,
            fields = fields,
            relations = relations
        });
    }

    /// <summary>
    /// Создать новую сущность
    /// </summary>
    [HttpPost("entities")]
    public async Task<ActionResult<BIEntity>> CreateEntity([FromBody] BIEntity entity)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        entity.Id = 0;
        _db.BIEntities.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEntity), new { id = entity.Id }, entity);
    }

    /// <summary>
    /// Обновить сущность
    /// </summary>
    [HttpPut("entities/{id}")]
    public async Task<ActionResult<BIEntity>> UpdateEntity(int id, [FromBody] BIEntity entity)
    {
        if (id != entity.Id)
        {
            return BadRequest(new { message = "ID в URL не совпадает с ID в теле запроса" });
        }

        var existingEntity = await _db.BIEntities.FindAsync(id);
        if (existingEntity == null)
        {
            return NotFound(new { message = $"Сущность с ID {id} не найдена" });
        }

        existingEntity.Name = entity.Name;
        existingEntity.Code = entity.Code;
        existingEntity.Description = entity.Description;

        await _db.SaveChangesAsync();

        return Ok(existingEntity);
    }

    /// <summary>
    /// Удалить сущность (только если нет данных)
    /// </summary>
    [HttpDelete("entities/{id}")]
    public async Task<ActionResult> DeleteEntity(int id)
    {
        var entity = await _db.BIEntities.FindAsync(id);
        if (entity == null)
        {
            return NotFound(new { message = $"Сущность с ID {id} не найдена" });
        }

        // Проверяем наличие данных
        var hasRows = await _db.BIEntityRows.AnyAsync(r => r.EntityId == id);
        if (hasRows)
        {
            return BadRequest(new { message = "Невозможно удалить сущность, так как в ней есть данные. Сначала удалите все данные." });
        }

        // Удаляем поля и связи
        var fields = await _db.BIFields.Where(f => f.EntityId == id).ToListAsync();
        _db.BIFields.RemoveRange(fields);

        var relations = await _db.BIRelations
            .Where(r => r.FromEntityId == id || r.ToEntityId == id)
            .ToListAsync();
        _db.BIRelations.RemoveRange(relations);

        _db.BIEntities.Remove(entity);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    #endregion

    #region Fields CRUD

    /// <summary>
    /// Добавить поле к сущности
    /// </summary>
    [HttpPost("entities/{entityId}/fields")]
    public async Task<ActionResult<BIField>> AddField(int entityId, [FromBody] BIField field)
    {
        var entity = await _db.BIEntities.FindAsync(entityId);
        if (entity == null)
        {
            return NotFound(new { message = $"Сущность с ID {entityId} не найдена" });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        field.Id = 0;
        field.EntityId = entityId;

        _db.BIFields.Add(field);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEntity), new { id = entityId }, field);
    }

    /// <summary>
    /// Обновить поле
    /// </summary>
    [HttpPut("entities/{entityId}/fields/{fieldId}")]
    public async Task<ActionResult<BIField>> UpdateField(int entityId, int fieldId, [FromBody] BIField field)
    {
        var entity = await _db.BIEntities.FindAsync(entityId);
        if (entity == null)
        {
            return NotFound(new { message = $"Сущность с ID {entityId} не найдена" });
        }

        if (fieldId != field.Id)
        {
            return BadRequest(new { message = "ID поля в URL не совпадает с ID в теле запроса" });
        }

        var existingField = await _db.BIFields.FindAsync(fieldId);
        if (existingField == null)
        {
            return NotFound(new { message = $"Поле с ID {fieldId} не найдено" });
        }

        if (existingField.EntityId != entityId)
        {
            return BadRequest(new { message = "Поле не принадлежит указанной сущности" });
        }

        existingField.Name = field.Name;
        existingField.Code = field.Code;
        existingField.DataType = field.DataType;
        existingField.IsRequired = field.IsRequired;
        existingField.IsKey = field.IsKey;
        existingField.OrderIndex = field.OrderIndex;
        existingField.RefEntityId = field.RefEntityId;

        await _db.SaveChangesAsync();

        return Ok(existingField);
    }

    /// <summary>
    /// Удалить поле
    /// </summary>
    [HttpDelete("entities/{entityId}/fields/{fieldId}")]
    public async Task<ActionResult> DeleteField(int entityId, int fieldId)
    {
        var entity = await _db.BIEntities.FindAsync(entityId);
        if (entity == null)
        {
            return NotFound(new { message = $"Сущность с ID {entityId} не найдена" });
        }

        var field = await _db.BIFields.FindAsync(fieldId);
        if (field == null)
        {
            return NotFound(new { message = $"Поле с ID {fieldId} не найдено" });
        }

        if (field.EntityId != entityId)
        {
            return BadRequest(new { message = "Поле не принадлежит указанной сущности" });
        }

        _db.BIFields.Remove(field);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    #endregion

    #region Rows CRUD

    /// <summary>
    /// Получить список строк сущности с пагинацией
    /// </summary>
    [HttpGet("entities/{entityId}/rows")]
    public async Task<ActionResult<object>> GetRows(
        int entityId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var entity = await _db.BIEntities.FindAsync(entityId);
        if (entity == null)
        {
            return NotFound(new { message = $"Сущность с ID {entityId} не найдена" });
        }

        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        var totalCount = await _db.BIEntityRows.CountAsync(r => r.EntityId == entityId);

        var rows = await _db.BIEntityRows
            .Where(r => r.EntityId == entityId)
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new
            {
                id = r.Id,
                entityId = r.EntityId,
                data = r.Data,
                createdAt = r.CreatedAt
            })
            .ToListAsync();

        return Ok(new
        {
            items = rows,
            totalCount = totalCount,
            page = page,
            pageSize = pageSize,
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        });
    }

    /// <summary>
    /// Создать строку данных
    /// </summary>
    [HttpPost("entities/{entityId}/rows")]
    public async Task<ActionResult<BIEntityRow>> CreateRow(int entityId, [FromBody] Dictionary<string, object?> data)
    {
        var entity = await _db.BIEntities.FindAsync(entityId);
        if (entity == null)
        {
            return NotFound(new { message = $"Сущность с ID {entityId} не найдена" });
        }

        // Валидация данных по метаданным сущности
        var fields = await _db.BIFields
            .Where(f => f.EntityId == entityId)
            .ToListAsync();

        // Проверка обязательных полей
        foreach (var field in fields.Where(f => f.IsRequired))
        {
            var fieldCode = field.Code ?? field.Name;
            if (!data.ContainsKey(fieldCode))
            {
                return BadRequest(new { message = $"Обязательное поле '{fieldCode}' отсутствует" });
            }
        }

        // Валидация типов данных (базовая)
        foreach (var kvp in data)
        {
            var field = fields.FirstOrDefault(f => (f.Code ?? f.Name) == kvp.Key);
            if (field != null && kvp.Value != null)
            {
                // Простая проверка типов (можно расширить)
                var valueType = kvp.Value.GetType().Name.ToLower();
                var expectedType = field.DataType.ToLower();

                if (expectedType == "number" && !(valueType.Contains("int") || valueType.Contains("double") || valueType.Contains("decimal") || valueType.Contains("float")))
                {
                    return BadRequest(new { message = $"Поле '{kvp.Key}' должно быть числом" });
                }
                else if (expectedType == "boolean" && !(valueType.Contains("bool")))
                {
                    return BadRequest(new { message = $"Поле '{kvp.Key}' должно быть булевым" });
                }
            }
        }

        var jsonData = JsonSerializer.Serialize(data);

        var row = new BIEntityRow
        {
            EntityId = entityId,
            Data = jsonData,
            CreatedAt = DateTime.UtcNow
        };

        _db.BIEntityRows.Add(row);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRows), new { entityId = entityId }, row);
    }

    /// <summary>
    /// Очистить все данные сущности
    /// </summary>
    [HttpDelete("entities/{entityId}/rows")]
    public async Task<ActionResult> DeleteRows(int entityId)
    {
        var entity = await _db.BIEntities.FindAsync(entityId);
        if (entity == null)
        {
            return NotFound(new { message = $"Сущность с ID {entityId} не найдена" });
        }

        var rows = await _db.BIEntityRows.Where(r => r.EntityId == entityId).ToListAsync();
        _db.BIEntityRows.RemoveRange(rows);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    #endregion

    #region Import

    /// <summary>
    /// Импортировать данные из файла (xls/xlsx/csv)
    /// </summary>
    [HttpPost("entities/{entityId}/import")]
    public async Task<ActionResult<object>> ImportData(int entityId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "Файл не предоставлен" });
        }

        var entity = await _db.BIEntities.FindAsync(entityId);
        if (entity == null)
        {
            return NotFound(new { message = $"Сущность с ID {entityId} не найдена" });
        }

        var fields = await _db.BIFields
            .Where(f => f.EntityId == entityId)
            .OrderBy(f => f.OrderIndex)
            .ToListAsync();

        if (!fields.Any())
        {
            return BadRequest(new { message = "У сущности нет полей. Сначала добавьте поля." });
        }

        var extension = Path.GetExtension(file.FileName).ToLower();
        var importedCount = 0;
        var errors = new List<string>();

        try
        {
            using var stream = file.OpenReadStream();

            if (extension == ".csv")
            {
                importedCount = await ImportFromCsv(stream, entityId, fields, errors);
            }
            else if (extension == ".xlsx" || extension == ".xls")
            {
                importedCount = await ImportFromExcel(stream, extension, entityId, fields, errors);
            }
            else
            {
                return BadRequest(new { message = $"Неподдерживаемый формат файла: {extension}. Поддерживаются: .csv, .xlsx, .xls" });
            }

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Импорт завершен",
                importedCount = importedCount,
                errors = errors
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при импорте файла");
            return StatusCode(500, new { message = "Ошибка при импорте файла", error = ex.Message });
        }
    }

    private async Task<int> ImportFromCsv(Stream stream, int entityId, List<BIField> fields, List<string> errors)
    {
        var importedCount = 0;
        using var reader = new StreamReader(stream);
        
        // Читаем заголовки
        var headerLine = await reader.ReadLineAsync();
        if (headerLine == null)
        {
            errors.Add("Файл пустой");
            return 0;
        }

        var headers = headerLine.Split(',').Select(h => h.Trim().Trim('"')).ToArray();
        var fieldMap = new Dictionary<int, BIField>();

        // Сопоставляем заголовки с полями сущности
        for (int i = 0; i < headers.Length; i++)
        {
            var header = headers[i];
            var field = fields.FirstOrDefault(f => 
                (f.Code ?? f.Name).Equals(header, StringComparison.OrdinalIgnoreCase));
            
            if (field != null)
            {
                fieldMap[i] = field;
            }
        }

        if (!fieldMap.Any())
        {
            errors.Add("Не найдено соответствий между заголовками файла и полями сущности");
            return 0;
        }

        // Читаем данные
        int lineNumber = 1;
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            lineNumber++;
            if (string.IsNullOrWhiteSpace(line)) continue;

            var values = ParseCsvLine(line);
            if (values.Length != headers.Length)
            {
                errors.Add($"Строка {lineNumber}: количество значений не совпадает с количеством заголовков");
                continue;
            }

            try
            {
                var rowData = new Dictionary<string, object?>();

                foreach (var kvp in fieldMap)
                {
                    var field = kvp.Value;
                    var fieldCode = field.Code ?? field.Name;
                    var value = values[kvp.Key]?.Trim();

                    if (string.IsNullOrEmpty(value) && field.IsRequired)
                    {
                        errors.Add($"Строка {lineNumber}: поле '{fieldCode}' обязательно для заполнения");
                        continue;
                    }

                    if (!string.IsNullOrEmpty(value))
                    {
                        rowData[fieldCode] = ConvertValue(value, field.DataType);
                    }
                    else
                    {
                        rowData[fieldCode] = null;
                    }
                }

                var jsonData = JsonSerializer.Serialize(rowData);
                var row = new BIEntityRow
                {
                    EntityId = entityId,
                    Data = jsonData,
                    CreatedAt = DateTime.UtcNow
                };

                _db.BIEntityRows.Add(row);
                importedCount++;
            }
            catch (Exception ex)
            {
                errors.Add($"Строка {lineNumber}: {ex.Message}");
            }
        }

        return importedCount;
    }

    private Task<int> ImportFromExcel(Stream stream, string extension, int entityId, List<BIField> fields, List<string> errors)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        var importedCount = 0;
        using var package = new ExcelPackage(stream);
        var worksheet = package.Workbook.Worksheets[0];

        if (worksheet.Dimension == null)
        {
            errors.Add("Лист пустой");
            return Task.FromResult(0);
        }

        var startRow = worksheet.Dimension.Start.Row;
        var endRow = worksheet.Dimension.End.Row;
        var startCol = worksheet.Dimension.Start.Column;
        var endCol = worksheet.Dimension.End.Column;

        // Читаем заголовки из первой строки
        var headers = new Dictionary<int, string>();
        for (int col = startCol; col <= endCol; col++)
        {
            var headerValue = worksheet.Cells[startRow, col].Value?.ToString()?.Trim();
            if (!string.IsNullOrEmpty(headerValue))
            {
                headers[col] = headerValue;
            }
        }

        var fieldMap = new Dictionary<int, BIField>();

        // Сопоставляем заголовки с полями сущности
        foreach (var kvp in headers)
        {
            var header = kvp.Value;
            var field = fields.FirstOrDefault(f =>
                (f.Code ?? f.Name).Equals(header, StringComparison.OrdinalIgnoreCase));

            if (field != null)
            {
                fieldMap[kvp.Key] = field;
            }
        }

        if (!fieldMap.Any())
        {
            errors.Add("Не найдено соответствий между заголовками файла и полями сущности");
            return Task.FromResult(0);
        }

        // Читаем данные начиная со второй строки
        for (int row = startRow + 1; row <= endRow; row++)
        {
            try
            {
                var rowData = new Dictionary<string, object?>();
                bool isEmptyRow = true;

                foreach (var kvp in fieldMap)
                {
                    var col = kvp.Key;
                    var field = kvp.Value;
                    var fieldCode = field.Code ?? field.Name;
                    var cellValue = worksheet.Cells[row, col].Value;

                    if (cellValue != null)
                    {
                        isEmptyRow = false;
                        var valueStr = cellValue.ToString()?.Trim();

                        if (string.IsNullOrEmpty(valueStr) && field.IsRequired)
                        {
                            errors.Add($"Строка {row}: поле '{fieldCode}' обязательно для заполнения");
                            continue;
                        }

                        if (!string.IsNullOrEmpty(valueStr))
                        {
                            rowData[fieldCode] = ConvertValue(valueStr, field.DataType);
                        }
                        else
                        {
                            rowData[fieldCode] = null;
                        }
                    }
                    else
                    {
                        if (field.IsRequired)
                        {
                            errors.Add($"Строка {row}: поле '{fieldCode}' обязательно для заполнения");
                        }
                        rowData[fieldCode] = null;
                    }
                }

                // Пропускаем пустые строки
                if (isEmptyRow && !rowData.Values.Any(v => v != null))
                {
                    continue;
                }

                var jsonData = JsonSerializer.Serialize(rowData);
                var entityRow = new BIEntityRow
                {
                    EntityId = entityId,
                    Data = jsonData,
                    CreatedAt = DateTime.UtcNow
                };

                _db.BIEntityRows.Add(entityRow);
                importedCount++;
            }
            catch (Exception ex)
            {
                errors.Add($"Строка {row}: {ex.Message}");
            }
        }

        return Task.FromResult(importedCount);
    }

    private string[] ParseCsvLine(string line)
    {
        var values = new List<string>();
        var currentValue = new System.Text.StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    currentValue.Append('"');
                    i++; // Пропускаем следующий символ
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                values.Add(currentValue.ToString());
                currentValue.Clear();
            }
            else
            {
                currentValue.Append(c);
            }
        }

        values.Add(currentValue.ToString());
        return values.ToArray();
    }

    private object? ConvertValue(string value, string dataType)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return dataType.ToLower() switch
        {
            "number" or "int" => int.TryParse(value, out var intVal) ? intVal : 
                                 double.TryParse(value, out var doubleVal) ? doubleVal : value,
            "decimal" => decimal.TryParse(value, out var decVal) ? decVal : value,
            "boolean" or "bool" => bool.TryParse(value, out var boolVal) ? boolVal : value,
            "date" or "datetime" => DateTime.TryParse(value, out var dateVal) ? dateVal : value,
            _ => value
        };
    }

    #endregion
}

