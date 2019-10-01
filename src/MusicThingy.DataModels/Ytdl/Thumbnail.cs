using System;
using System.Text.Json.Serialization;

namespace MusicThingy.DataModels.Ytdl
{
    public partial class Thumbnail
    {
        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
