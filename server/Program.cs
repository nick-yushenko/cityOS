using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using CityOS.Data;
using CityOS.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? builder.Configuration["ConnectionStrings__DefaultConnection"];

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// SignalR
builder.Services.AddSignalR();

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });


// CORS для фронта (React на другом порту)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true); // для dev можно так, потом — по списку
    });
});

var app = builder.Build();

// Автомиграция БД при старте с retry логикой
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    var maxRetries = 10;
    var delay = TimeSpan.FromSeconds(3);
    
    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            logger.LogInformation("Попытка подключения к БД... (попытка {Attempt}/{MaxRetries})", i + 1, maxRetries);
            db.Database.Migrate();
            logger.LogInformation("Миграции БД успешно применены");
            break;
        }
        catch (Exception ex)
        {
            if (i == maxRetries - 1)
            {
                logger.LogError(ex, "Не удалось применить миграции после {MaxRetries} попыток", maxRetries);
                throw;
            }
            logger.LogWarning(ex, "Ошибка при подключении к БД. Повтор через {Delay} секунд...", delay.TotalSeconds);
            Task.Delay(delay).Wait();
        }
    }
}
app.UseCors();

// Controllers
app.MapControllers();

// Простейший API
app.MapGet("/ping", () => new { ok = true, time = DateTime.UtcNow });

// API для получения списка городов
app.MapGet("/api/cities", async (AppDbContext db) =>
{
    var cities = await db.Cities
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
    
    return Results.Ok(cities);
});

app.Run();