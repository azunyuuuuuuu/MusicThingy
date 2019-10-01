using System;

namespace MusicThingy.DataModels
{
    public class Track
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Comment { get; set; }
        public byte[] AlbumArt { get; set; }
    }
}
