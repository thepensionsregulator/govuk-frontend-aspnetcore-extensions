using ThePensionsRegulator.Frontend.Umbraco.Services;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
    public class YouTubeVideoIdParserTests
    {
        [Theory]
        [InlineData("https://www.youtube.com/embed/tTQiv1xKVM4", "tTQiv1xKVM4")]
        [InlineData("https://www.youtube.com/embed/tTQiv1xKVM4?utm_source=example", "tTQiv1xKVM4")]
        [InlineData("https://www.youtube-nocookie.com/embed/tTQiv1xKVM4", "tTQiv1xKVM4")]
        [InlineData("https://www.youtube-nocookie.com/embed/tTQiv1xKVM4?utm_source=example", "tTQiv1xKVM4")]
        [InlineData("https://www.youtube.com/watch?v=tTQiv1xKVM4", "tTQiv1xKVM4")]
        [InlineData("https://www.youtube.com/watch?v=tTQiv1xKVM4&utm_source=example", "tTQiv1xKVM4")]
        [InlineData("https://youtu.be/tTQiv1xKVM4", "tTQiv1xKVM4")]
        [InlineData("https://youtu.be/tTQiv1xKVM4?utm_source=example", "tTQiv1xKVM4")]
        [InlineData("https://www.youtube.com/embed/-w5YtMpS4J0", "-w5YtMpS4J0")]
        [InlineData("https://www.youtube.com/embed/-w5YtMpS4J0?utm_source=example", "-w5YtMpS4J0")]
        [InlineData("https://www.youtube-nocookie.com/embed/-w5YtMpS4J0", "-w5YtMpS4J0")]
        [InlineData("https://www.youtube-nocookie.com/embed/-w5YtMpS4J0?utm_source=example", "-w5YtMpS4J0")]
        [InlineData("https://www.youtube.com/watch?v=-w5YtMpS4J0", "-w5YtMpS4J0")]
        [InlineData("https://www.youtube.com/watch?v=-w5YtMpS4J0&utm_source=example", "-w5YtMpS4J0")]
        [InlineData("https://youtu.be/-w5YtMpS4J0", "-w5YtMpS4J0")]
        [InlineData("https://youtu.be/-w5YtMpS4J0?utm_source=example", "-w5YtMpS4J0")]
        public void Valid_URL_returns_video_id(string originalUrl, string expectedVideoId)
        {
            var normaliser = new YouTubeVideoIdParser();
            var result = normaliser.TryParseUrl(new Uri(originalUrl), out var videoId);

            Assert.True(result);
            Assert.Equal(expectedVideoId, videoId);
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