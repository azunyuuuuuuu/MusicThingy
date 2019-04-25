using System;

namespace MusicThingy.Models
{
    public class SourceVideo
    {
        public string SourceId { get; set; }
        public Source Source { get; set; }
        public string VideoId { get; set; }
        public Video Video { get; set; }
    }
}
