using System.ComponentModel.DataAnnotations;

namespace videoapi.Models
{
    public class Video
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        /// <summary>
        /// Relative or absolute path to the file on the NAS
        /// </summary>
        [Required]
        public string FilePath { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public string? thumbnailPath { get; set; }

        public TimeSpan? Duration { get; set; }

        public string? ContentType { get; set; }

        // additional metadata (duration, content type, etc.)
    }
}
