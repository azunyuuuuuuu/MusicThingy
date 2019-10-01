using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MusicThingy.DataModels.Ytdl
{
    public partial class Media : MetadataBase
    {
        [JsonPropertyName("channel_url")]
        public Uri ChannelUrl { get; set; }

        [JsonPropertyName("categories")]
        public List<string> Categories { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("playlist_uploader")]
        public string PlaylistUploader { get; set; }

        [JsonPropertyName("release_date")]
        public object ReleaseDate { get; set; }

        [JsonPropertyName("format_id")]
        public string FormatId { get; set; }

        [JsonPropertyName("requested_subtitles")]
        public object RequestedSubtitles { get; set; }

        [JsonPropertyName("channel_id")]
        public string ChannelId { get; set; }

        [JsonPropertyName("alt_title")]
        public string AltTitle { get; set; }

        [JsonPropertyName("n_entries")]
        public long NEntries { get; set; }

        [JsonPropertyName("is_live")]
        public object IsLive { get; set; }

        [JsonPropertyName("like_count")]
        public long LikeCount { get; set; }

        [JsonPropertyName("episode_number")]
        public object EpisodeNumber { get; set; }

        [JsonPropertyName("series")]
        public object Series { get; set; }

        [JsonPropertyName("thumbnail")]
        public Uri Thumbnail { get; set; }

        [JsonPropertyName("requested_formats")]
        public List<Format> RequestedFormats { get; set; }

        [JsonPropertyName("creator")]
        public string Creator { get; set; }

        [JsonPropertyName("dislike_count")]
        public long DislikeCount { get; set; }

        [JsonPropertyName("view_count")]
        public long ViewCount { get; set; }

        [JsonPropertyName("age_limit")]
        public long AgeLimit { get; set; }

        [JsonPropertyName("vbr")]
        public object Vbr { get; set; }

        [JsonPropertyName("subtitles")]
        public AutomaticCaptions Subtitles { get; set; }

        [JsonPropertyName("playlist_index")]
        public long PlaylistIndex { get; set; }

        [JsonPropertyName("vcodec")]
        public string Vcodec { get; set; }

        [JsonPropertyName("release_year")]
        public object ReleaseYear { get; set; }

        [JsonPropertyName("upload_date")]
        [JsonConverter(typeof(YtdlDateTimeOffsetConverter))]
        public DateTimeOffset UploadDate { get; set; }

        [JsonPropertyName("season_number")]
        public object SeasonNumber { get; set; }

        [JsonPropertyName("formats")]
        public List<Format> Formats { get; set; }

        [JsonPropertyName("playlist_id")]
        public string PlaylistId { get; set; }

        [JsonPropertyName("height")]
        public long Height { get; set; }

        [JsonPropertyName("end_time")]
        public object EndTime { get; set; }

        [JsonPropertyName("duration")]
        public long Duration { get; set; }

        [JsonPropertyName("fps")]
        public long Fps { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }

        [JsonPropertyName("acodec")]
        public string Acodec { get; set; }

        [JsonPropertyName("chapters")]
        public List<Chapter> Chapters { get; set; }

        [JsonPropertyName("abr")]
        public long Abr { get; set; }

        [JsonPropertyName("playlist")]
        public string Playlist { get; set; }

        [JsonPropertyName("display_id")]
        public string DisplayId { get; set; }

        [JsonPropertyName("width")]
        public long Width { get; set; }

        [JsonPropertyName("license")]
        public object License { get; set; }

        [JsonPropertyName("start_time")]
        public object StartTime { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("artist")]
        public string Artist { get; set; }

        [JsonPropertyName("average_rating")]
        public double AverageRating { get; set; }

        [JsonPropertyName("ext")]
        public string Ext { get; set; }

        [JsonPropertyName("playlist_title")]
        public string PlaylistTitle { get; set; }

        [JsonPropertyName("album")]
        public string Album { get; set; }

        [JsonPropertyName("automatic_captions")]
        public AutomaticCaptions AutomaticCaptions { get; set; }

        [JsonPropertyName("playlist_uploader_id")]
        public string PlaylistUploaderId { get; set; }

        [JsonPropertyName("track")]
        public string Track { get; set; }

        [JsonPropertyName("thumbnails")]
        public List<Thumbnail> Thumbnails { get; set; }

        [JsonPropertyName("annotations")]
        public object Annotations { get; set; }

        [JsonPropertyName("resolution")]
        public object Resolution { get; set; }

        [JsonPropertyName("stretched_ratio")]
        public object StretchedRatio { get; set; }
    }
}
