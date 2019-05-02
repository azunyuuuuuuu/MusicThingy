using System;
using System.Collections.Generic;

namespace MusicThingy.Models
{
    public abstract class Media
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TimeSpan Time { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDownloaded { get; set; } = false;
        public string FilePath { get; set; } = string.Empty;

        public ICollection<SourceMedia> SourceMedia { get; set; } = new List<SourceMedia>();
    }

    public class YouTubeSourceMedia : Media
    {
        public string YouTubeId { get; set; }
        public string YouTubeTitle { get; set; }
        public string YouTubeUploader { get; set; }
        public string YouTubeDescription { get; set; }
        // public List<string> Keywords { get; set; }
        public TimeSpan YouTubeDuration { get; set; }
        public DateTimeOffset YouTubeUploadDate { get; set; }
    }
}
