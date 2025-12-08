using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CityOS.Data;
using CityOS.Models;

namespace CityOS.Controllers;

[ApiController]
[Route("api/cities")]
public class CityController : ControllerBase
{
	private readonly AppDbContext _db;
	private readonly ILogger<CityController> _logger;

	public CityController(AppDbContext db, ILogger<CityController> logger)
	{
		_db = db;
		_logger = logger;
	}

	/// <summary>
	/// Получить все города
	/// </summary>
	[HttpGet]
	public async Task<ActionResult<IEnumerable<object>>> GetCities()
	{
		var cities = await _db.Cities
			.Include(c => c.Budgets)
			.Include(c => c.DataSources)
			.Select(c => new
			{
				id = c.Id,
				name = c.Name,
				code = c.Code,
				budgets = c.Budgets.Select(b => new
				{
					id = b.Id,
					cityId = b.CityId,
					year = b.Year,
					name = b.Name,
					createdAt = b.CreatedAt,
					updatedAt = b.UpdatedAt
				}).ToList(),
				dataSources = c.DataSources.Select(ds => new
				{
					id = ds.Id,
					cityId = ds.CityId,
					datasetKind = ds.DatasetKind,
					sourceUrl = ds.SourceUrl,
					fileType = ds.FileType,
					description = ds.Description,
					isActive = ds.IsActive,
					lastLoadedAt = ds.LastLoadedAt,
					lastChecksum = ds.LastChecksum
				}).ToList()
			})
			.ToListAsync();

		return Ok(cities);
	}

	/// <summary>
	/// Получить город по ID
	/// </summary>
	[HttpGet("{id}")]
	public async Task<ActionResult<City>> GetCity(int id)
	{
		var city = await _db.Cities.FindAsync(id);

		if (city == null)
		{
			return NotFound(new { message = $"Город с ID {id} не найден" });
		}

		return Ok(city);
	}

	/// <summary>
	/// Создать новый город
	/// </summary>
	[HttpPost]
	public async Task<ActionResult<City>> AddCity([FromBody] City city)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		// Убеждаемся, что Id не установлен (должен генерироваться автоматически)
		city.Id = 0;

		_db.Cities.Add(city);
		await _db.SaveChangesAsync();

		return CreatedAtAction(nameof(GetCity), new { id = city.Id }, city);
	}

	/// <summary>
	/// Обновить город
	/// </summary>
	[HttpPatch("{id}")]
	public async Task<ActionResult<City>> UpdateCity(int id, [FromBody] UpdateCityPayload payload)
	{
		var city = await _db.Cities.FindAsync(id);
		if (city == null)
		{
			return NotFound(new { message = $"Город с ID {id} не найден" });
		}

		if (!string.IsNullOrEmpty(payload.Name))
		{
			city.Name = payload.Name;
		}

		if (payload.Code != null)
		{
			city.Code = payload.Code;
		}

		await _db.SaveChangesAsync();

		return Ok(city);
	}

	/// <summary>
	/// Удалить город
	/// </summary>
	[HttpDelete("{id}")]
	public async Task<ActionResult> DeleteCity(int id)
	{
		var city = await _db.Cities.FindAsync(id);
		if (city == null)
		{
			return NotFound(new { message = $"Город с ID {id} не найден" });
		}

		_db.Cities.Remove(city);
		await _db.SaveChangesAsync();

		return NoContent();
	}
}

