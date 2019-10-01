using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MusicThingy.DataModels.Ytdl
{

    public partial class Playlist : MetadataBase
    {
        [JsonPropertyName("entries")]
        public List<Media> Entries { get; set; }
    }
}
