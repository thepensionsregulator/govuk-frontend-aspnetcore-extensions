using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.GovUk.Frontend;

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
        private const string TitleAttributeName = "title";
        private const string YouTubeVideoIdAttributeName = "youtube-video-id";
        private const string AutoplayAttributeName = "autoplay";
        private const string PlaysInlineAttributeName = "plays-inline";
        private const string PreloadAttributeName = "preload";
        private const string UseAblePlayerAttributeName = "use-able-player";
        private const string DescriptionAttributeName = "description";
        private const string HeadingLevelAttributeName = "heading-level";
        private const string HeadingSizeAttributeName = "heading-size";
        private const string TranscriptUrlAttributeName = "transcript-url";
        private const string TranscriptTitleAttributeName = "transcript-title";
        private const string TranscriptTargetAttributeName = "transcript-target";
        private const string IframeTitleAttributeName = "iframe-title";
        private readonly string[] MinimisedAttributeList = { "autoplay", "playsinline", "data-able-player", "data-youtube-nocookie", "allowfullscreen", "credentialless" };

        private string _title = string.Empty;
        private string _youTubeVideoId = string.Empty;
        private bool _autoplay = false;
        private bool _playsInline = true;
        private string _preload = ComponentGenerator.YouTubeVideoDefaultPreload;
        private bool _useAblePlayer = false;
        private string? _description = null;
        private string _headingLevel = "h2";
        private string? _headingClass = null;
        private string? _transcriptUrl = null;
        private string? _transcriptTitle = null;
        private string? _transcriptTarget = null;
        private string? _iframeTitle = null;
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

        [HtmlAttributeName(TitleAttributeName)]
        public string Title
        {
            get => _title;
            set => _title = Guard.ArgumentNotNullOrEmpty(nameof(value), value);
        }


        [HtmlAttributeName(YouTubeVideoIdAttributeName)]
        public string YouTubeVideoId
        {
            get => _youTubeVideoId;
            set => _youTubeVideoId = Guard.ArgumentNotNullOrEmpty(nameof(value), value);
        }

        [HtmlAttributeName(AutoplayAttributeName)]
        public bool Autoplay
        {
            get => _autoplay;
            set => _autoplay = value;
        }

        [HtmlAttributeName(PlaysInlineAttributeName)]
        public bool PlaysInline
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
        public bool UseAblePlayer
        {
            get => _useAblePlayer;
            set => _useAblePlayer = value;
        }

        [HtmlAttributeName(DescriptionAttributeName)]
        public string? Description
        {
            get => _description;
            set => _description = value;
        }

        [HtmlAttributeName(HeadingLevelAttributeName)]
        public string HeadingLevel
        {
            get => _headingLevel;
            set => _headingLevel = Guard.ArgumentNotNullOrEmpty(nameof(value), value);
        }

        [HtmlAttributeName(HeadingSizeAttributeName)]
        public string? HeadingSize
        {
            get => _headingClass;
            set => _headingClass = value;
        }

        [HtmlAttributeName(TranscriptUrlAttributeName)]
        public string? TranscriptUrl
        {
            get => _transcriptUrl;
            set => _transcriptUrl = value;
        }

        [HtmlAttributeName(TranscriptTitleAttributeName)]
        public string? TranscriptTitle
        {
            get => _transcriptTitle;
            set => _transcriptTitle = value;
        }

        [HtmlAttributeName(TranscriptTargetAttributeName)]
        public string? TranscriptTarget
        {
            get => _transcriptTarget;
            set => _transcriptTarget = value;
        }


        [HtmlAttributeName(IframeTitleAttributeName)]
        public string? IframeTitle
        {
            get => _iframeTitle;
            set => _iframeTitle = value;
        }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder tagBuilder;
            if (UseAblePlayer)
            {
                tagBuilder = _htmlGenerator.GenerateTprAblePlayer(new TprYouTubeVideo
                {
                    Attributes = output.Attributes.ToAttributeDictionary(),
                    Title = Title,
                    YouTubeVideoId = YouTubeVideoId,
                    Autoplay = Autoplay,
                    PlaysInline = PlaysInline,
                    Preload = Preload,
                    Description = Description,
                    HeadingLevel = HeadingLevel,
                    HeadingSize = HeadingSize,
                    TranscriptUrl = TranscriptUrl,
                    TranscriptTitle = TranscriptTitle,
                    TranscriptTarget = TranscriptTarget,
                    IframeTitle = IframeTitle ?? Title
                });
            }
            else
            {
                tagBuilder = _htmlGenerator.GenerateTprYouTubeNoCookiesEmbeddedPlayer(new TprYouTubeVideo
                {
                    Attributes = output.Attributes.ToAttributeDictionary(),
                    Title = Title,
                    YouTubeVideoId = YouTubeVideoId,
                    Autoplay = Autoplay,
                    PlaysInline = PlaysInline,
                    Preload = Preload,
                    Description = Description,
                    HeadingLevel = HeadingLevel,
                    HeadingSize = HeadingSize,
                    TranscriptUrl = TranscriptUrl,
                    TranscriptTitle = TranscriptTitle,
                    TranscriptTarget = TranscriptTarget,
                    IframeTitle = IframeTitle ?? Title
                });
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