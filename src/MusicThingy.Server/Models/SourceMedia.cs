using System;

namespace MusicThingy.Models
{
    public class SourceMedia
    {
        public string SourceId { get; set; }
        public Source Source { get; set; }
        public string MediaId { get; set; }
        public Media Media { get; set; }
    }
}
