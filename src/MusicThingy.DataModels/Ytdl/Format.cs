using System;
using System.Text.Json.Serialization;

namespace MusicThingy.DataModels.Ytdl
{
    public partial class Format
    {
        [JsonPropertyName("filesize")]
        public long? Filesize { get; set; }

        [JsonPropertyName("tbr")]
        public double? Tbr { get; set; }

        [JsonPropertyName("ext")]
        public string Ext { get; set; }

        [JsonPropertyName("asr")]
        public long? Asr { get; set; }

        [JsonPropertyName("acodec")]
        public string Acodec { get; set; }

        [JsonPropertyName("width")]
        public long? Width { get; set; }

        [JsonPropertyName("vcodec")]
        public string Vcodec { get; set; }

        [JsonPropertyName("format_id")]
        public string FormatId { get; set; }

        [JsonPropertyName("downloader_options")]
        public DownloaderOptions DownloaderOptions { get; set; }

        [JsonPropertyName("abr")]
        public long? Abr { get; set; }

        [JsonPropertyName("http_headers")]
        public HttpHeaders HttpHeaders { get; set; }

        [JsonPropertyName("player_url")]
        public string PlayerUrl { get; set; }

        [JsonPropertyName("format")]
        public string FormatFormat { get; set; }

        [JsonPropertyName("fps")]
        public long? Fps { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("format_note")]
        public string FormatNote { get; set; }

        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }

        [JsonPropertyName("height")]
        public long? Height { get; set; }

        [JsonPropertyName("container")]
        public string Container { get; set; }
    }
}
