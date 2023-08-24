namespace GovUk.Frontend.Umbraco.Models
{
    /// <summary>
    /// An option in a GOV.UK 'Select' component.
    /// </summary>
    public class SelectOption
    {
        public SelectOption(string text, string value)
        {
            Text = text;
            Value = value;
        }

        public string Text { get; init; }
        public string Value { get; init; }
    }
}
