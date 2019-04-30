using System;
using System.Collections.Generic;

namespace MusicThingy.Models
{
    public class Source
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string PlaylistId { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        
        public ICollection<SourceVideo> SourceVideos { get; set; } = new List<SourceVideo>();
    }
}
