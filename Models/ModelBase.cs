using System;

namespace MusicThingy.Models
{
    public class ModelBase
    {
        public Guid Id { get; set; }
        public DateTimeOffset TimeCreated { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset TimeChanged { get; set; } = DateTimeOffset.Now;
    }

    public class Track : ModelBase
    {
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }

        public string Comment { get; set; }
        public bool IsActive { get; set; } = true;
        public int Rating { get; set; } = 0; // 5 star rating, 0 = no rating

        public MediaFileBase MediaFile { get; set; }
    }

    public class MediaFileBase : ModelBase
    {
        public string LibraryFilePath { get; set; }
        public bool IsAvailable { get; set; } = true;
    }

    public class DiskFile : MediaFileBase
    {

    }

    public class YouTubeFile : MediaFileBase
    {
        public string YtId { get; set; }
        public string YtAuthorId { get; set; }

        public string YtTitle { get; set; }
        public string YtDescription { get; set; }
        public string YtKeywords { get; set; }
    }
}