using System;
using System.Collections.Generic;

namespace MusicThingy.Models
{
    public class Video
    {
        public string Id { get; set; }
        public string YouTubeId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        // public List<string> Keywords { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTimeOffset UploadDate { get; set; }
        public List<SourceVideo> SourceVideos { get; set; } = new List<SourceVideo>();
    }
}
