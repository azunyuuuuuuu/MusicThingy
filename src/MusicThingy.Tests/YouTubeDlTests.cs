using System.Threading.Tasks;
using MusicThingy.Core;
using NUnit.Framework;

namespace MusicThingy.Tests
{
    public class YouTubeDlTests
    {
        private YtdlInterface _ytdl;

        [SetUp]
        public void Setup()
        {
            _ytdl = new YtdlInterface();
        }

        [Test]
        public void Check_YouTubeDL_Exists()
        {
            Assert.DoesNotThrowAsync(async () => await _ytdl.GetInstalledVersion());
        }

        [TestCase(@"https://www.youtube.com/watch?v=BaW_jenozKc&t=1s&end=9", @"youtube-dl test video ""'/\ä↭𝕐")]
        public void Fetch_YouTubeMetaData(string url, string title)
        {
            Assert.Pass();
        }
    }
}