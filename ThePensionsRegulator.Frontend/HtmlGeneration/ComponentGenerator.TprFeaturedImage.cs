using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ThePensionsRegulator.GovUk.Frontend;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string FeaturedImageElement = "div";
        internal const string FeaturedImageThumbnailElement = "div";
        internal const string FeaturedImageInfoElement = "div";

        public virtual TagBuilder GenerateTprFeaturedImage(
            AttributeDictionary? attributes,
            string? imageUrl,
            string? imageAlt,
            IHtmlContent? htmlContent,
            bool horizontal,
            bool decorativeImage)

        {
            Guard.ArgumentNotNull(nameof(imageUrl), imageUrl);
            Guard.ArgumentNotNull(nameof(imageAlt), imageAlt);
            Guard.ArgumentNotNull(nameof(htmlContent), htmlContent);

            var featuredImageTagHelper = new TagBuilder(FeaturedImageElement);
            if (attributes is not null) { featuredImageTagHelper.MergeAttributes(attributes); }
            featuredImageTagHelper.MergeCssClass("tpr-featured-image");

            if (horizontal)
            {
                featuredImageTagHelper.MergeCssClass("tpr-featured-image--horizontal");
            }


            var featuredImageThumbnailTagHelper = new TagBuilder(FeaturedImageThumbnailElement);

            featuredImageThumbnailTagHelper.MergeCssClass("tpr-featured-image_thumbnail");


            var imageTagHelper = new TagBuilder("img");
            imageTagHelper.Attributes.Add("src", imageUrl);
            var imageAltValue = string.Empty;
            if (!decorativeImage)
            {
                imageAltValue = imageAlt ?? string.Empty;
            }
            imageTagHelper.Attributes.Add("alt", imageAltValue);
            featuredImageThumbnailTagHelper.InnerHtml.AppendHtml(imageTagHelper);

            featuredImageTagHelper.InnerHtml.AppendHtml(featuredImageThumbnailTagHelper);


            var featuredImageInfoTagHelper = new TagBuilder(FeaturedImageInfoElement);

            featuredImageInfoTagHelper.MergeCssClass("tpr-featured-image_info");
            featuredImageInfoTagHelper.MergeCssClass("tpr-box");


            featuredImageInfoTagHelper.InnerHtml.AppendHtml(htmlContent);

            featuredImageTagHelper.InnerHtml.AppendHtml(featuredImageInfoTagHelper);
            return featuredImageTagHelper;
        }

    }
}
