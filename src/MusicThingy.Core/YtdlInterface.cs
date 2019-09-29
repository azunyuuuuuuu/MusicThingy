using System;
using System.Threading.Tasks;
using System.Text.Json;
using CliWrap;

namespace MusicThingy.Core
{
    public class YtdlInterface
    {
        private const string _ytdl = "youtube-dl";

        public async Task<string> GetInstalledVersion()
        {
            var result = await Cli.Wrap(_ytdl)
                .SetArguments("--version")
                .ExecuteAsync();

            return result.StandardOutput;
        }


        public async Task<YtdlExtractorKeys> GetMediaType(string url)
        {
            return await GetMediaType(new Uri(url));
        }

        public async Task<YtdlExtractorKeys> GetMediaType(Uri uri)
        {
            var result = await Cli.Wrap(_ytdl)
                .SetArguments(new[]{
                    "-i",
                    "--dump-single-json",
                    uri.ToString()})
                .ExecuteAsync();

            var document = JsonDocument.Parse(result.StandardOutput);

            var extractorkey = document.RootElement.GetProperty("extractor_key").GetString();

            return (YtdlExtractorKeys)Enum.Parse(typeof(YtdlExtractorKeys), extractorkey);
        }
    }

    public enum YtdlExtractorKeys
    {
        Youtube,
        YoutubePlaylist
    }
}
