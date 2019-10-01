using System.Text.Json.Serialization;

namespace MusicThingy.DataModels.Ytdl
{
    public partial class DownloaderOptions
    {
        [JsonPropertyName("http_chunk_size")]
        public long HttpChunkSize { get; set; }
    }
}
