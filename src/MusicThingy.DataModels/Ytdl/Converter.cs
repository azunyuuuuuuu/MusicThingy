using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MusicThingy.DataModels.Ytdl
{
    public class YtdlDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        private const string _dateformat = "yyyyMMdd";

        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTimeOffset));
            return DateTime.ParseExact(reader.GetString(), _dateformat, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_dateformat, CultureInfo.InvariantCulture));
        }
    }
}
