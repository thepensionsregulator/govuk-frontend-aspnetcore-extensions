namespace ThePensionsRegulator.GovUk.Frontend.ConformanceTests.OptionsJson
{
    /// <summary>
    /// The title of a task in a task list in a GOV.UK Frontend test fixture.
    /// </summary>
    public record TaskTitle
    {
        public string? Text { get; set; }
        public string? Html { get; set; }
        public string? Classes { get; set; }
    }

}
