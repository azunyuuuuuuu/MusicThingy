﻿using System;
using System.Collections.Generic;

namespace MusicThingy.Server.Models
{
    public abstract class Media : ModelBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TimeSpan Time { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDownloaded { get; set; } = false;
        public string FilePath { get; set; } = string.Empty;
        public string ArtworkPath { get; set; } = string.Empty;

        public ICollection<SourceMedia> SourceMedia { get; set; } = new List<SourceMedia>();
    }

    public class YouTubeSourceMedia : Media
    {
        public string YouTubeId { get; set; }
        public string YouTubeTitle { get; set; }
        public string YouTubeUploader { get; set; }
        public string YouTubeDescription { get; set; }
        public string YouTubeThumbnailFile { get; set; }
        public TimeSpan YouTubeDuration { get; set; }
        public DateTimeOffset YouTubeUploadDate { get; set; }
    }
}
