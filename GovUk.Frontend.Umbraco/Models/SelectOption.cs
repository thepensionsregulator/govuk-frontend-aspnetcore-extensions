namespace GovUk.Frontend.Umbraco.Models
{
    /// <summary>
    /// An option in a GOV.UK 'Select' component.
    /// </summary>
    public class SelectOption
    {
        public SelectOption(string value, string label)
        {
            Label = label;
            Value = value;
        }

        public string Label { get; init; }
        public string Value { get; init; }
    }
}
