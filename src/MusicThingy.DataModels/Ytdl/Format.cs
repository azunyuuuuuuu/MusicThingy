using System;

namespace MusicThingy.DataModels.Ytdl
{
    public partial class Format
    {
        public long? Filesize { get; set; }
        public double? Tbr { get; set; }
        public string Ext { get; set; }
        public long? Asr { get; set; }
        public string Acodec { get; set; }
        public long? Width { get; set; }
        public string Vcodec { get; set; }
        public long FormatId { get; set; }
        public DownloaderOptions DownloaderOptions { get; set; }
        public long? Abr { get; set; }
        public HttpHeaders HttpHeaders { get; set; }
        public string PlayerUrl { get; set; }
        public string FormatFormat { get; set; }
        public long? Fps { get; set; }
        public Uri Url { get; set; }
        public string FormatNote { get; set; }
        public string Protocol { get; set; }
        public long? Height { get; set; }
        public string Container { get; set; }
    }
}