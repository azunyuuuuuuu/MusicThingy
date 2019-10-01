using System.Text.Json.Serialization;

namespace MusicThingy.DataModels.Ytdl
{
    public partial class Chapter
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("start_time")]
        public double StartTime { get; set; }

        [JsonPropertyName("end_time")]
        public double EndTime { get; set; }
    }
}
