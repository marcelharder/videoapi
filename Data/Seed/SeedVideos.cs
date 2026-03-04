using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using videoapi.Models;

namespace videoapi.Data.Seed
{
    public static class SeedVideos
    {
        /// <summary>
        /// Reads Videos.json (relative to the project root) and inserts records
        /// into the <see cref="ApplicationDbContext"/> if the table is empty.
        /// </summary>
        public static void Initialize(ApplicationDbContext context, IWebHostEnvironment env)
        {
            // if videos already seeded, nothing to do
            if (context.Videos.Any())
                return;

            var filePath = Path.Combine(env.ContentRootPath, "Data", "Seed", "Videos.json");
            if (!File.Exists(filePath))
                return; // nothing to seed if the json file isn't present

            var json = File.ReadAllText(filePath);
            if (string.IsNullOrWhiteSpace(json))
                return;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var videos = JsonSerializer.Deserialize<List<Video>>(json, options);
            if (videos == null || videos.Count == 0)
                return;

            context.Videos.AddRange(videos);
            context.SaveChanges();
        }
    }
}

