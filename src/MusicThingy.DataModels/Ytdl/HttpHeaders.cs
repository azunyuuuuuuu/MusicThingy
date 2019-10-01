using System.Text.Json.Serialization;

namespace MusicThingy.DataModels.Ytdl
{
    public partial class HttpHeaders
    {
        [JsonPropertyName("Accept")]
        public string Accept { get; set; }

        [JsonPropertyName("Accept-Charset")]
        public string AcceptCharset { get; set; }

        [JsonPropertyName("Accept-Language")]
        public string AcceptLanguage { get; set; }

        [JsonPropertyName("Accept-Encoding")]
        public string AcceptEncoding { get; set; }

        [JsonPropertyName("User-Agent")]
        public string UserAgent { get; set; }
    }
}
