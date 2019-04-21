using System;

namespace MusicThingy.Models
{
    public class SourceVideo
    {
        public Guid SourceId { get; set; }
        public Source Source { get; set; }
        public Guid VideoId { get; set; }
        public Video Video { get; set; }
    }
}
