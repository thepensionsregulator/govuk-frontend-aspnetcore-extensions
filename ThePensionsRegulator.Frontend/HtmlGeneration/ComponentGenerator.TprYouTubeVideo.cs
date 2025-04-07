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
            videoTag.MergeAttributes(video.Attributes);
            if (!string.IsNullOrWhiteSpace(video.VideoId))
            {
                if (videoTag.Attributes.ContainsKey("id")) { videoTag.Attributes.Remove("id"); }
                videoTag.Attributes.Add("id", video.VideoId);
            }
            videoTag.Attributes.Add("data-able-player", null);
            videoTag.Attributes.Add("data-youtube-nocookie", null);
            videoTag.Attributes.Add("data-youtube-id", video.YouTubeVideoId);
            videoTag.Attributes.Add("data-root-path", "/_content/ThePensionsRegulator.Frontend.Umbraco/tpr/lib/ableplayer/");
            if (video.Autoplay) { videoTag.Attributes.Add("autoplay", null); }
            if (video.PlaysInline) { videoTag.Attributes.Add("playsinline", null); }
            videoTag.Attributes.Add("preload", video.Preload);

            return videoTag;
        }

        public virtual TagBuilder GenerateTprYouTubeNoCookiesEmbeddedPlayer(TprYouTubeVideo video)
        {
            Guard.ArgumentNotNullOrEmpty(nameof(video.YouTubeVideoId), video.YouTubeVideoId);

            var containerTag = new TagBuilder("div");
            containerTag.MergeAttributes(video.Attributes);
            containerTag.MergeCssClass("tpr-video-wrapper-no-cookies");
            var wrapperTag = new TagBuilder("div");
            wrapperTag.MergeCssClass("tpr-video-wrapper-no-cookies__video-container");

            var iFrame = new TagBuilder("iframe");
            var src = "https://www.youtube-nocookie.com/embed/" + video.YouTubeVideoId;
            if (video.Autoplay)
            {
                src += "?autoplay=1&mute=1";
            }

            iFrame.Attributes.Add("src", src);

            if (!string.IsNullOrWhiteSpace(video.VideoId)) { iFrame.Attributes.Add("id", video.VideoId); }
            iFrame.Attributes.Add("title", video.Title);
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
                var transcriptLink = new TagBuilder("a");
                transcriptLink.MergeCssClass("tpr-video-wrapper-no-cookies__transcript-link");
                transcriptLink.Attributes.Add("href", video.TranscriptUrl);
                if (!string.IsNullOrWhiteSpace(video.TranscriptTarget))
                {
                    transcriptLink.Attributes.Add("target", video.TranscriptTarget);
                }
                transcriptLink.InnerHtml.AppendHtml(video.TranscriptTitle);
                containerTag.InnerHtml.AppendHtml(transcriptLink);
            }

            return containerTag;
        }

    }
}
