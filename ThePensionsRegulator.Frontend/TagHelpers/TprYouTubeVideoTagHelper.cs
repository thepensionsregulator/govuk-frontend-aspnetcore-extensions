using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    /// <summary>
    /// Generates a GOV.UK back link component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.YouTubeVideoElement)]
    public class TprYouTubeVideoTagHelper : TagHelper
    {
        internal const string TagName = "tpr-youtube-video";
        private const string IdAttributeName = "id";
        private const string TitleAttributeName = "title";
        private const string VideoIdAttributeName = "videoId";
        private const string AutoplayAttributeName = "autoplay";
        private const string PlaysInlineAttributeName = "playsinline";
        private const string PreloadAttributeName = "preload";
        private const string UseAblePlayerAttributeName = "useAblePlayer";
        private const string TranscriptUrlAttributeName = "transcriptUrl";
        private const string TranscriptTitleAttributeName = "transcriptTitle";
        private const string TranscriptTargetAttributeName = "transcriptTarget";
        private readonly string[] MinimisedAttributeList = { "autoplay", "playsinline", "data-able-player", "data-youtube-nocookie", "allowfullscreen", "credentialless" };

        private string _id = string.Empty;
        private string _title = string.Empty;
        private string _videoId = string.Empty;
        private bool? _autoplay = false;
        private bool? _playsInline = true;
        private string _preload = ComponentGenerator.YouTubeVideoDefaultPreload;
        private bool? _useAblePlayer = true;
        private string _transcriptUrl = string.Empty;
        private string _transcriptTitle = string.Empty;
        private string _transcriptTarget = string.Empty;
        private readonly ITprHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="TprYouTubeVideoTagHelper"/>.
        /// </summary>
        public TprYouTubeVideoTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal TprYouTubeVideoTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        [HtmlAttributeName(IdAttributeName)]
        public string Id
        {
            get => _id;
            set => _id = Guard.ArgumentNotNullOrEmpty(nameof(value), value);
        }
        [HtmlAttributeName(TitleAttributeName)]
        public string Title
        {
            get => _title;
            set => _title = Guard.ArgumentNotNullOrEmpty(nameof(value), value);
        }


        [HtmlAttributeName(VideoIdAttributeName)]
        public string VideoId
        {
            get => _videoId;
            set => _videoId = Guard.ArgumentNotNullOrEmpty(nameof(value), value);
        }

        [HtmlAttributeName(AutoplayAttributeName)]
        public bool? Autoplay
        {
            get => _autoplay;
            set => _autoplay = value;
        }

        [HtmlAttributeName(PlaysInlineAttributeName)]
        public bool? PlaysInline
        {
            get => _playsInline;
            set => _playsInline = value;
        }

        [HtmlAttributeName(PreloadAttributeName)]
        public string Preload
        {
            get => _preload;
            set => _preload = Guard.ArgumentNotNullOrEmpty(nameof(value), value);
        }

        [HtmlAttributeName(UseAblePlayerAttributeName)]
        public bool? UseAblePlayer
        {
            get => _useAblePlayer;
            set => _useAblePlayer = value;
        }

        [HtmlAttributeName(TranscriptUrlAttributeName)]
        public string TranscriptUrl
        {
            get => _transcriptUrl;
            set => _transcriptUrl = value;
        }

        [HtmlAttributeName(TranscriptTitleAttributeName)]
        public string TranscriptTitle
        {
            get => _transcriptTitle;
            set => _transcriptTitle = value;
        }

        [HtmlAttributeName(TranscriptTargetAttributeName)]
        public string TranscriptTarget
        {
            get => _transcriptTarget;
            set => _transcriptTarget = value;
        }


        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder tagBuilder;
            bool useAutoplay = Autoplay.HasValue ? Autoplay.Value : false;
            bool playsInLine = PlaysInline.HasValue ? PlaysInline.Value : true;
            if (UseAblePlayer.HasValue && UseAblePlayer.Value)
            {

                tagBuilder = _htmlGenerator.GenerateTprAblePlayer(Id, Title, VideoId, useAutoplay, playsInLine, Preload, TranscriptUrl, TranscriptTitle, TranscriptTarget);
            }
            else
            {

                tagBuilder = _htmlGenerator.GenerateTprYouTubeNoCookiesEmbeddedPlayer(Id, Title, VideoId, useAutoplay, playsInLine, Preload, TranscriptUrl, TranscriptTitle, TranscriptTarget);
            }

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            /// If we don't do the following, we end up with attributes with empty quotes like data-youtube-nocookie=""
            /// I can't find a way to set the HtmlAttributeValueStyle.Minimized on the tagBuilder attributes. Therefore we 
            /// have to do it here in the following way
            foreach (var attr in tagBuilder.Attributes.Keys)
            {
                if (MinimisedAttributeList.Contains(attr) && string.IsNullOrEmpty(tagBuilder.Attributes[attr]))
                {
                    output.Attributes.SetAttribute(new TagHelperAttribute(attr, null, HtmlAttributeValueStyle.Minimized));
                }
            }
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);

        }
    }
}
