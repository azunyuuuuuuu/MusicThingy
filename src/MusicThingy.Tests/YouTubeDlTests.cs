using System.Threading.Tasks;
using MusicThingy.Core;
using Xunit;

namespace MusicThingy.Tests
{
    public class YouTubeDlTests
    {
        private YtdlInterface _ytdl;

        public YouTubeDlTests()
        {
            _ytdl = new YtdlInterface();
        }

        [Fact]
        public async Task Check_CommandExists_youtubedlAsync()
        {
            Assert.IsType(typeof(string), await _ytdl.GetInstalledVersion());
        }

        [Theory]
        [InlineData(@"https://www.youtube.com/watch?v=BaW_jenozKc&t=1s&end=9", @"youtube-dl test video ""'/\ä↭𝕐")]
        public void Fetch_MetaData_Youtube(string url, string title)
        {
            Assert.True(true);
        }

        [Theory]
        [InlineData(@"https://www.youtube.com/playlist?list=PLzH6n4zXuckpfMu_4Ff8E7Z1behQks5ba", @"youtube-dl test video ""'/\ä↭𝕐")]
        public void Fetch_MetaData_YoutubePlaylist(string url, string title)
        {
            Assert.True(true);
        }

        [Theory]
        [InlineData(@"https://www.youtube.com/watch?v=BaW_jenozKc&t=1s&end=9", YtdlExtractorKeys.Youtube)]
        [InlineData(@"https://www.youtube.com/playlist?list=PLzH6n4zXuckpfMu_4Ff8E7Z1behQks5ba", YtdlExtractorKeys.YoutubePlaylist)]
        public async Task Check_ExtractorKey_Correct(string url, YtdlExtractorKeys extractorkey)
        {
            Assert.Equal(await _ytdl.GetMediaType(url), extractorkey);
        }
    }
}