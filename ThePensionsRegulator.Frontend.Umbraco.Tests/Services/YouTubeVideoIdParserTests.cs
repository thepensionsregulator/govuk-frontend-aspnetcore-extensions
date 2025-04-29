using ThePensionsRegulator.Frontend.Umbraco.Services;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
    [TestFixture]
    public class YouTubeVideoIdParserTests
    {
        [TestCase("https://www.youtube.com/embed/tTQiv1xKVM4")]
        [TestCase("https://www.youtube.com/embed/tTQiv1xKVM4?utm_source=example")]
        [TestCase("https://www.youtube-nocookie.com/embed/tTQiv1xKVM4")]
        [TestCase("https://www.youtube-nocookie.com/embed/tTQiv1xKVM4?utm_source=example")]
        [TestCase("https://www.youtube.com/watch?v=tTQiv1xKVM4")]
        [TestCase("https://www.youtube.com/watch?v=tTQiv1xKVM4&utm_source=example")]
        [TestCase("https://youtu.be/tTQiv1xKVM4")]
        [TestCase("https://youtu.be/tTQiv1xKVM4?utm_source=example")]
        public void Valid_URL_returns_video_id(string originalUrl)
        {
            var normaliser = new YouTubeVideoIdParser();
            var result = normaliser.TryParseUrl(new Uri(originalUrl), out var videoId);

            Assert.True(result);
            Assert.That(videoId, Is.EqualTo("tTQiv1xKVM4"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("not-a-url")]
        [TestCase("https://www.youtube.com")]
        [TestCase("https://example.org")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings", Justification = "Testing for invalid URLs")]
        public void Invalid_URL_returns_false(string? originalUrl)
        {
            var normaliser = new YouTubeVideoIdParser();

            var result = normaliser.TryParseUrl(originalUrl!, out var videoId);

            Assert.False(result);
            Assert.Null(videoId);
        }
    }
}