namespace videoapi.Models
{
    public class VideoSettings
    {
        /// <summary>
        /// Base path on the local filesystem (e.g. /mnt/nas/videos) where video files are stored.
        /// </summary>
        public string NasBasePath { get; set; } = string.Empty;
    }
}