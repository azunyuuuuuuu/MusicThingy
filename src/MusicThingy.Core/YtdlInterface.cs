using System;
using System.Threading.Tasks;
using CliWrap;

namespace MusicThingy.Core
{
    public class YtdlInterface
    {
        private const string _ytdl = "youtube-dl";

        public async Task<string> GetInstalledVersion()
        {
            var result = await Cli.Wrap(_ytdl)
                .SetArguments(new[]{
                    "--version"
                })
                .ExecuteAsync();

            return result.StandardOutput;
        }
    }
}
