using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CityOS.Data;
using CityOS.Models;

namespace CityOS.Controllers;

[ApiController]
[Route("api/data-sources")]
public class DataSourceController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<DataSourceController> _logger;

    public DataSourceController(AppDbContext db, ILogger<DataSourceController> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Получить все источники данных
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DataSource>>> GetSources()
    {
        var sources = await _db.DataSources.ToListAsync();
        return Ok(sources);
    }

    /// <summary>
    /// Получить источник данных по ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<DataSource>> GetSource(int id)
    {
        var source = await _db.DataSources.FindAsync(id);

        if (source == null)
        {
            return NotFound(new { message = $"Источник данных с ID {id} не найден" });
        }

        return Ok(source);
    }

    /// <summary>
    /// Создать новый источник данных
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<DataSource>> AddSource([FromBody] DataSource source)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Проверяем существование города
        var cityExists = await _db.Cities.AnyAsync(c => c.Id == source.CityId);
        if (!cityExists)
        {
            return BadRequest(new { message = $"Город с ID {source.CityId} не найден" });
        }

        // Убеждаемся, что Id не установлен (должен генерироваться автоматически)
        source.Id = 0;
        // Устанавливаем значение по умолчанию для FileType, если не указан
        if (string.IsNullOrEmpty(source.FileType))
        {
            source.FileType = "csv";
        }

        _db.DataSources.Add(source);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSource), new { id = source.Id }, source);
    }

    /// <summary>
    /// Обновить источник данных
    /// </summary>
    [HttpPatch("{id}")]
    public async Task<ActionResult<DataSource>> UpdateSource(int id, [FromBody] UpdateDataSourcePayload payload)
    {
        var source = await _db.DataSources.FindAsync(id);
        if (source == null)
        {
            return NotFound(new { message = $"Источник данных с ID {id} не найден" });
        }

        // Проверяем существование города, если cityId меняется
        if (payload.CityId.HasValue && payload.CityId.Value != source.CityId)
        {
            var cityExists = await _db.Cities.AnyAsync(c => c.Id == payload.CityId.Value);
            if (!cityExists)
            {
                return BadRequest(new { message = $"Город с ID {payload.CityId} не найден" });
            }
            source.CityId = payload.CityId.Value;
        }

        if (!string.IsNullOrEmpty(payload.DatasetKind))
        {
            source.DatasetKind = payload.DatasetKind;
        }

        if (!string.IsNullOrEmpty(payload.SourceUrl))
        {
            source.SourceUrl = payload.SourceUrl;
        }

        if (!string.IsNullOrEmpty(payload.FileType))
        {
            source.FileType = payload.FileType;
        }

        if (payload.Description != null)
        {
            source.Description = payload.Description;
        }

        if (payload.IsActive.HasValue)
        {
            source.IsActive = payload.IsActive.Value;
        }

        if (payload.LastChecksum != null)
        {
            source.LastChecksum = payload.LastChecksum;
        }

        await _db.SaveChangesAsync();

        return Ok(source);
    }

    /// <summary>
    /// Удалить источник данных
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSource(int id)
    {
        var source = await _db.DataSources.FindAsync(id);
        if (source == null)
        {
            return NotFound(new { message = $"Источник данных с ID {id} не найден" });
        }

        _db.DataSources.Remove(source);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}

