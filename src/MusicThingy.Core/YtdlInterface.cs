using System;
using System.Threading.Tasks;
using System.Text.Json;
using CliWrap;
using System.Collections.Generic;
using MusicThingy.DataModels;
using System.Linq;

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

        public Task<string> GetExtractorKey(string url)
            => GetExtractorKey(new Uri(url));

        public async Task<string> GetExtractorKey(Uri uri)
        {
            var result = await Cli.Wrap(_ytdl)
                .SetArguments(new[]{
                    "-i",
                    "--dump-single-json",
                    uri.ToString()})
                .ExecuteAsync();

            var document = JsonDocument.Parse(result.StandardOutput);

            return document.RootElement.GetProperty("extractor_key").GetString();
        }

        public Task<IEnumerable<Track>> GetTracksFromPlaylist(string url)
            => GetTracksFromPlaylist(new Uri(url));

        public async Task<IEnumerable<Track>> GetTracksFromPlaylist(Uri uri)
        {
            var result = await Cli.Wrap(_ytdl)
                .SetArguments(new[]{
                    "-i",
                    "--dump-single-json",
                    uri.ToString()})
                .ExecuteAsync();

            var playlist = JsonSerializer.Deserialize<DataModels.Ytdl.Playlist>(result.StandardOutput);

            if (playlist.ExtractorKey != "YoutubePlaylist")
                throw new NotSupportedException($"ExtractorKey {playlist.ExtractorKey} not supported.");

            return playlist.Entries
                .Where(x => x.Track != null)
                .Select(x => new Track
                {
                    Title = x.Track,
                    Artist = x.Creator,
                    Album = x.Album,
                    Comment = x.Description
                });
        }

        public Task DownloadTrack(string url, string path)
            => DownloadTrack(new Uri(url), path);

        public async Task DownloadTrack(Uri uri, string path)
        {
            var result = await Cli.Wrap(_ytdl)
                .SetArguments(new[]{
                    "--extract-audio",
                    "--audio-format", "m4a",
                    "--output", path,
                    uri.ToString()})
                .ExecuteAsync();
        }
    }
}
