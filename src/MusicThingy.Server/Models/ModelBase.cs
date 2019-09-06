using System;

namespace MusicThingy.Server.Models
{
    public class ModelBase
    {
        public DateTimeOffset TimeCreated { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset TimeChanged { get; set; } = DateTimeOffset.Now;
    }
}
