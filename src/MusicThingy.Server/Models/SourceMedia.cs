using System;

namespace MusicThingy.Server.Models
{
    public class SourceMedia
    {
        public string SourceId { get; set; }
        public Source Source { get; set; }
        public string MediaId { get; set; }
        public Media Media { get; set; }
    }
}
