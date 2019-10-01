using System;
using System.Collections.Generic;

namespace MusicThingy.DataModels.Ytdl
{
    public partial class Playlist
    {
        public string Title { get; set; }
        public string Uploader { get; set; }
        public string ExtractorKey { get; set; }
        public string Id { get; set; }
        public string UploaderId { get; set; }
        public Uri UploaderUrl { get; set; }
        public string Extractor { get; set; }
        public string WebpageUrlBasename { get; set; }
        public string Type { get; set; }
        public List<Media> Entries { get; set; }
        public Uri WebpageUrl { get; set; }
    }
}