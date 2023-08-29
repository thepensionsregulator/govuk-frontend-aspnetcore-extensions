using System.Collections.Generic;
using Umbraco.Cms.Core.Strings;

namespace GovUk.Frontend.Umbraco.Models
{
    /// <summary>
    /// A summary list item in a GOV.UK 'Summary list' component.
    /// </summary>
    public class SummaryListItem
    {
        public SummaryListItem(string key, HtmlEncodedString value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; init; }
        public HtmlEncodedString Value { get; init; }
        public IList<SummaryListAction> Actions { get; } = new List<SummaryListAction>();
        public string? CssClasses { get; set; }
    }
}
