namespace ThePensionsRegulator.GovUk.Frontend.ConformanceTests.OptionsJson
{
    /// <summary>
    /// A GOV.UK tag component in a GOV.UK Frontend test fixture.
    /// </summary>
    public record Tag
    {
        public string? Text { get; set; }
        public string? Html { get; set; }
        public string? Classes { get; set; }
        public IDictionary<string, string?> Attributes { get; set; } = new Dictionary<string, string?>();
    }

}
