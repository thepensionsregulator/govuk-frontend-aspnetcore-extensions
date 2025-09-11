using ThePensionsRegulator.Frontend.Umbraco.Services;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
    public class YouTubeVideoIdParserTests
    {
        [Theory]
        [InlineData("https://www.youtube.com/embed/tTQiv1xKVM4")]
        [InlineData("https://www.youtube.com/embed/tTQiv1xKVM4?utm_source=example")]
        [InlineData("https://www.youtube-nocookie.com/embed/tTQiv1xKVM4")]
        [InlineData("https://www.youtube-nocookie.com/embed/tTQiv1xKVM4?utm_source=example")]
        [InlineData("https://www.youtube.com/watch?v=tTQiv1xKVM4")]
        [InlineData("https://www.youtube.com/watch?v=tTQiv1xKVM4&utm_source=example")]
        [InlineData("https://youtu.be/tTQiv1xKVM4")]
        [InlineData("https://youtu.be/tTQiv1xKVM4?utm_source=example")]
        public void Valid_URL_returns_video_id(string originalUrl)
        {
            var normaliser = new YouTubeVideoIdParser();
            var result = normaliser.TryParseUrl(new Uri(originalUrl), out var videoId);

            Assert.True(result);
            Assert.Equal("tTQiv1xKVM4", videoId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("not-a-url")]
        [InlineData("https://www.youtube.com")]
        [InlineData("https://example.org")]
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