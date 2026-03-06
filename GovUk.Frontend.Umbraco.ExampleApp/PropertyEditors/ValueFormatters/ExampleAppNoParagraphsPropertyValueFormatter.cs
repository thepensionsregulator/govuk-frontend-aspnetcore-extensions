using ThePensionsRegulator.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.ExampleApp.PropertyEditors.ValueFormatters
{
    public class ExampleAppNoParagraphsPropertyValueFormatter : NoParagraphsPropertyValueFormatter
    {
        public override bool IsFormatter(IPublishedPropertyType propertyType)
        {
            return propertyType.Alias == "manyLinksForHeader" && propertyType.ContentType?.Alias == "headerTPR";
        }
    }
}
