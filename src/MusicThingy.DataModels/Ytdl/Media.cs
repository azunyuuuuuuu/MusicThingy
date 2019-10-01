using System;
using System.Collections.Generic;

namespace MusicThingy.DataModels.Ytdl
{
    public partial class Media
    {
        public string Uploader { get; set; }
        public Uri ChannelUrl { get; set; }
        public List<string> Categories { get; set; }
        public string Format { get; set; }
        public string PlaylistUploader { get; set; }
        public object ReleaseDate { get; set; }
        public string FormatId { get; set; }
        public object RequestedSubtitles { get; set; }
        public string ChannelId { get; set; }
        public string AltTitle { get; set; }
        public long NEntries { get; set; }
        public object IsLive { get; set; }
        public Uri UploaderUrl { get; set; }
        public long LikeCount { get; set; }
        public object EpisodeNumber { get; set; }
        public object Series { get; set; }
        public Uri Thumbnail { get; set; }
        public List<Format> RequestedFormats { get; set; }
        public string Creator { get; set; }
        public long DislikeCount { get; set; }
        public long ViewCount { get; set; }
        public long AgeLimit { get; set; }
        public object Vbr { get; set; }
        public AutomaticCaptions Subtitles { get; set; }
        public long PlaylistIndex { get; set; }
        public string Vcodec { get; set; }
        public object ReleaseYear { get; set; }
        public long UploadDate { get; set; }
        public object SeasonNumber { get; set; }
        public string ExtractorKey { get; set; }
        public List<Format> Formats { get; set; }
        public string Extractor { get; set; }
        public string PlaylistId { get; set; }
        public long Height { get; set; }
        public object EndTime { get; set; }
        public long Duration { get; set; }
        public string Id { get; set; }
        public long Fps { get; set; }
        public List<string> Tags { get; set; }
        public string Acodec { get; set; }
        public string WebpageUrlBasename { get; set; }
        public List<Chapter> Chapters { get; set; }
        public long Abr { get; set; }
        public string UploaderId { get; set; }
        public string Playlist { get; set; }
        public string DisplayId { get; set; }
        public long Width { get; set; }
        public object License { get; set; }
        public string Title { get; set; }
        public object StartTime { get; set; }
        public string Description { get; set; }
        public string Artist { get; set; }
        public double AverageRating { get; set; }
        public string Ext { get; set; }
        public string PlaylistTitle { get; set; }
        public string Album { get; set; }
        public AutomaticCaptions AutomaticCaptions { get; set; }
        public string PlaylistUploaderId { get; set; }
        public Uri WebpageUrl { get; set; }
        public string Track { get; set; }
        public List<Thumbnail> Thumbnails { get; set; }
        public object Annotations { get; set; }
        public object Resolution { get; set; }
        public object StretchedRatio { get; set; }
    }
}