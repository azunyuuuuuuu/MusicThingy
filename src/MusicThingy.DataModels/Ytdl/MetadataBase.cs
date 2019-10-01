using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MusicThingy.DataModels.Ytdl
{
    public partial class MetadataBase
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("uploader")]
        public string Uploader { get; set; }

        [JsonPropertyName("extractor_key")]
        public string ExtractorKey { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("uploader_id")]
        public string UploaderId { get; set; }

        [JsonPropertyName("uploader_url")]
        public Uri UploaderUrl { get; set; }

        [JsonPropertyName("extractor")]
        public string Extractor { get; set; }

        [JsonPropertyName("webpage_url_basename")]
        public string WebpageUrlBasename { get; set; }

        [JsonPropertyName("_type")]
        public string _Type { get; set; }

        [JsonPropertyName("webpage_url")]
        public Uri WebpageUrl { get; set; }
    }
}
