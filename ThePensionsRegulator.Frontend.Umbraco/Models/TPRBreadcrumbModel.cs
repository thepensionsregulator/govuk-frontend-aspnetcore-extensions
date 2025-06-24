using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Frontend.Umbraco.Models
{
    public class TPRBreadcrumbModel
    {
        public string[]? PriorityArray { get; set; }

        public string GetHighestPriorityField(IPublishedContent Page)
        {
            if (PriorityArray is null)
            {
                return Page.Name;
            }
            else
            {
                var highestValidPriorityFieldValue = string.Empty;

                foreach (var namingPriority in PriorityArray)
                {
                    if (namingPriority.Contains('>'))
                    {
                        var splitComplexNamingPriority = namingPriority.Split(">");
                        OverridableBlockGridModel? contentGrid = Page.Value<OverridableBlockGridModel>(splitComplexNamingPriority[0]);

                        if (contentGrid is not null)
                        {
                            foreach (var block in contentGrid)
                            {
                                if (block.Content.ContentType.Alias == splitComplexNamingPriority[1])
                                {
                                    highestValidPriorityFieldValue = block.Content.Value<string>(splitComplexNamingPriority[2]);
                                    break;
                                }
                            }
                            if (!string.IsNullOrEmpty(highestValidPriorityFieldValue))
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Page.Value<string>(namingPriority)))
                        {
                            highestValidPriorityFieldValue = Page.Value<string>(namingPriority);
                            break;
                        }
                    }
                }
                if (string.IsNullOrEmpty(highestValidPriorityFieldValue))
                {
                    highestValidPriorityFieldValue = Page.Name;
                }

                return highestValidPriorityFieldValue;
            }
        }
    }
}
