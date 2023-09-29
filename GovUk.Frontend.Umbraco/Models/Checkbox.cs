using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Strings;

namespace GovUk.Frontend.Umbraco.Models
{
    /// <summary>
    /// An checkbox in a GOV.UK 'Checkboxes' component.
    /// </summary>
    public class Checkbox : CheckboxItemBase
    {
        public Checkbox(string value, string label)
        {
            Value = value;
            Label = label;
        }
        public string Label { get; init; }
        public string Value { get; init; }
        public IHtmlEncodedString? Hint { get; set; }
        public OverridableBlockListModel? ConditionalBlocks { get; set; }
        public string? CssClasses { get; set; }
    }
}
