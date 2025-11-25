using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.ExampleApp.PropertyEditors.ValueFormatters
{
    public class NoParagraphsPropertyValueFormatter : ThePensionsRegulator.Frontend.Umbraco.PropertyEditors.ValueFormatters.NoParagraphsPropertyValueFormatter
    {
        public override bool IsFormatter(IPublishedPropertyType propertyType)
        {
            return propertyType.Alias == "manyLinksForHeader" && propertyType.ContentType?.Alias == "headerFooterTPR";
        }
    }
}
