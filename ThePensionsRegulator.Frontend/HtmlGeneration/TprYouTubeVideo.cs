using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public record TprYouTubeVideo
    {
        public required AttributeDictionary Attributes { get; set; }
        public required string Title { get; set; }
        public required string YouTubeVideoId { get; set; }
        public required bool Autoplay { get; set; }
        public required bool PlaysInline { get; set; }
        public required string Preload { get; set; }
        public required string? Description { get; set; }
        public required bool SmallerHeading { get; set; }
        public required string? TranscriptUrl { get; set; }
        public required string? TranscriptTitle { get; set; }
        public required string? TranscriptTarget { get; set; }
    }
}
