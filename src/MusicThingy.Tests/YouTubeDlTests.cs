using NUnit.Framework;

namespace MusicThingy.Tests
{
    public class YouTubeDlTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(@"https://www.youtube.com/watch?v=BaW_jenozKc&t=1s&end=9", @"youtube-dl test video ""'/\ä↭𝕐")]
        public void Fetch_YouTubeMetaData(string url, string title)
        {
            Assert.Pass();
        }
    }
}