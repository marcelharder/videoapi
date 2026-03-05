using Microsoft.EntityFrameworkCore;
using videoapi.Data;
using videoapi.Models;
using videoapi.Data.Seed;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// configure database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 9, 2)))); // adjust server version as needed

// configure file provider for NAS
builder.Services.Configure<VideoSettings>(builder.Configuration.GetSection("VideoSettings"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// video endpoints

app.MapGet("/videos", async (ApplicationDbContext db) =>
    await db.Videos.ToListAsync());

app.MapPost("/videos", async (ApplicationDbContext db, Video video) =>
{
    db.Videos.Add(video);
    await db.SaveChangesAsync();
    return Results.Created($"/videos/{video.Id}", video);
});

app.MapPost("/videos/upload", async (HttpRequest req, IOptions<VideoSettings> settings, ApplicationDbContext db) =>
{
    var form = await req.ReadFormAsync();
    var file = form.Files["file"];
    var title = form["title"];
    if (file == null || file.Length == 0)
        return Results.BadRequest("No file uploaded");

    var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
    var fullPath = Path.Combine(settings.Value.NasBasePath, fileName);
    using var stream = File.Create(fullPath);
    await file.CopyToAsync(stream);

    var video = new Video { Title = title, FilePath = fileName, ContentType = file.ContentType };
    db.Videos.Add(video);
    await db.SaveChangesAsync();
    return Results.Created($"/videos/{video.Id}", video);
});

app.MapGet("/videos/{id}/stream", async (int id, ApplicationDbContext db, IOptions<VideoSettings> settings) =>
{
    var video = await db.Videos.FindAsync(id);
    if (video == null)
        return Results.NotFound();

    var filePath = Path.Combine(settings.Value.NasBasePath, video.FilePath);
    if (!File.Exists(filePath))
        return Results.NotFound();

    // determine content type based on file extension or stored metadata
    var provider = new FileExtensionContentTypeProvider();
    if (!provider.TryGetContentType(video.FilePath, out var contentType))
    {
        // fall back to value saved in database or generic octet-stream
        contentType = video.ContentType ?? "application/octet-stream";
    }

    var stream = File.OpenRead(filePath);
    return Results.File(stream, contentType, enableRangeProcessing: true);
});

// perform data seeding before the app starts listening
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    SeedVideos.Initialize(db, app.Environment);
}

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
