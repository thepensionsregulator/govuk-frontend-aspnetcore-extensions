using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string YouTubeVideoDefaultPreload = "metadata";
        internal const string YouTubeVideoElement = "video";

        public virtual TagBuilder GenerateTprAblePlayer(TprYouTubeVideo video)
        {
            Guard.ArgumentNotNullOrEmpty(nameof(video.YouTubeVideoId), video.YouTubeVideoId);
            Guard.ArgumentNotNullOrEmpty(nameof(video.Preload), video.Preload);


            var videoTag = new TagBuilder("video");
            if (!video.Attributes.ContainsKey("id")) { video.Attributes.Add("id", Guid.NewGuid().ToString()); } // Able Player requires id
            videoTag.MergeAttributes(video.Attributes);
            videoTag.Attributes.Add("data-able-player", null);
            videoTag.Attributes.Add("data-youtube-nocookie", null);
            videoTag.Attributes.Add("data-youtube-id", video.YouTubeVideoId);
            videoTag.Attributes.Add("data-root-path", "/ThePensionsRegulator.Frontend/lib/ableplayer/");
            if (video.Autoplay) { videoTag.Attributes.Add("autoplay", null); }
            if (video.PlaysInline) { videoTag.Attributes.Add("playsinline", null); }
            videoTag.Attributes.Add("preload", video.Preload);

            return videoTag;
        }

        public virtual TagBuilder GenerateTprYouTubeNoCookiesEmbeddedPlayer(TprYouTubeVideo video)
        {
            Guard.ArgumentNotNullOrEmpty(nameof(video.YouTubeVideoId), video.YouTubeVideoId);
            Guard.ArgumentNotNullOrEmpty(nameof(video.HeadingLevel), video.HeadingLevel);

            var containerTag = new TagBuilder("div");
            containerTag.MergeAttributes(video.Attributes);
            containerTag.MergeCssClass("tpr-video-wrapper-no-cookies");

            var heading = new TagBuilder(video.HeadingLevel);
            if (!string.IsNullOrWhiteSpace(video.HeadingSize))
            {
                heading.MergeCssClass(video.HeadingSize);
            }
            heading.MergeCssClass("tpr-video-wrapper-no-cookies__heading");
            heading.InnerHtml.AppendHtml(video.IframeTitle);
            containerTag.InnerHtml.AppendHtml(heading);

            if (!string.IsNullOrWhiteSpace(video.Description))
            {
                var description = new TagBuilder("div");
                description.MergeCssClass("govuk-body");
                description.MergeCssClass("tpr-video-wrapper-no-cookies__description");
                description.InnerHtml.AppendHtml(video.Description);
                containerTag.InnerHtml.AppendHtml(description);
            }

            var wrapperTag = new TagBuilder("div");
            wrapperTag.MergeCssClass("tpr-video-wrapper-no-cookies__video-container");

            var iFrame = new TagBuilder("iframe");
            var src = "https://www.youtube-nocookie.com/embed/" + video.YouTubeVideoId;
            if (video.Autoplay)
            {
                src += "?autoplay=1&mute=1";
            }

            iFrame.Attributes.Add("src", src);
            iFrame.Attributes.Add("title", video.IframeTitle);
            iFrame.Attributes.Add("frameborder", "0");
            iFrame.Attributes.Add("allow", "accelerometer; autoplay;  encrypted-media; gyroscope; picture-in-picture; web-share");
            iFrame.Attributes.Add("referrerpolicy", "strict-origin-when-cross-origin");
            iFrame.Attributes.Add("allowfullscreen", null);
            iFrame.Attributes.Add("credentialless", null);
            iFrame.Attributes.Add("playsinline", video.PlaysInline ? "1" : "0");

            wrapperTag.InnerHtml.AppendHtml(iFrame);
            containerTag.InnerHtml.AppendHtml(wrapperTag);

            if (!string.IsNullOrWhiteSpace(video.TranscriptUrl))
            {
                var transcriptContainer = new TagBuilder("div");
                transcriptContainer.MergeCssClass("tpr-video-wrapper-no-cookies__transcript-container");

                var transcriptLink = new TagBuilder("a");
                transcriptLink.MergeCssClass("tpr-video-wrapper-no-cookies__transcript-link");
                transcriptLink.Attributes.Add("href", video.TranscriptUrl);
                if (!string.IsNullOrWhiteSpace(video.TranscriptTarget))
                {
                    transcriptLink.Attributes.Add("target", video.TranscriptTarget);
                }
                transcriptLink.InnerHtml.AppendHtml(video.TranscriptTitle ?? $"Read transcript: {video.Title}");

                transcriptContainer.InnerHtml.AppendHtml(transcriptLink);
                containerTag.InnerHtml.AppendHtml(transcriptContainer);
            }

            return containerTag;
        }

    }
}