using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string YoutubeVideoDefaultPreload = "metadata";
        internal const string YoutubeVideoElement = "video";

        public virtual TagBuilder GenerateTprAblePlayer(
            string id,
            string title,
            string videoId,
            bool autoplay,
            bool playsinline,
            string preload)
        {
            Guard.ArgumentNotNullOrEmpty(nameof(id), id);
            Guard.ArgumentNotNullOrEmpty(nameof(videoId), videoId);
            Guard.ArgumentNotNullOrEmpty(nameof(preload), preload);


            var videoTag = new TagBuilder("video");
            videoTag.Attributes.Add("id",id);
            videoTag.Attributes.Add("data-able-player",null);
            videoTag.Attributes.Add("data-youtube-nocookie",null);
            videoTag.Attributes.Add("data-youtube-id",videoId);
            videoTag.Attributes.Add("data-root-path", "/_content/ThePensionsRegulator.Frontend.Umbraco/tpr/lib/ableplayer/");
            if(autoplay)
                videoTag.Attributes.Add("autoplay",null);

            if(playsinline)
                videoTag.Attributes.Add("playsinline",null);

            videoTag.Attributes.Add("preload",preload);

            return videoTag;
       }

        public virtual TagBuilder GenerateTprYoutubeNoCookiesEmbeddedPlayer(
            string id,
            string title,
            string videoId,
            bool autoplay,
            bool playsinline,
            string preload)
        {
            Guard.ArgumentNotNullOrEmpty(nameof(id), id);
            Guard.ArgumentNotNullOrEmpty(nameof(videoId), videoId);

            var wrapperTag = new TagBuilder("div");
            wrapperTag.MergeCssClass("tpr-video-wrapper-no-cookies");

            var iFrame = new TagBuilder("iframe");
            var src = "https://www.youtube-nocookie.com/embed/" + videoId;
            if(autoplay)
            {
                
                src += "/?autoplay=1&mute=1";
            }

            iFrame.Attributes.Add("src",src);
            
            iFrame.Attributes.Add("id",id);
            iFrame.Attributes.Add("title",title);
            iFrame.Attributes.Add("frameborder","0");
            iFrame.Attributes.Add("allow","accelerometer; autoplay;  encrypted-media; gyroscope; picture-in-picture; web-share");
            iFrame.Attributes.Add("referrerpolicy","strict-origin-when-cross-origin");
            iFrame.Attributes.Add("allowfullscreen", null);
            iFrame.Attributes.Add("credentialless",null);
           
            wrapperTag.InnerHtml.AppendHtml(iFrame);
            return wrapperTag;
       }

    }
}
