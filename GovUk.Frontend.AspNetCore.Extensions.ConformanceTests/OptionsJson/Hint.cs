namespace GovUk.Frontend.AspNetCore.Extensions.ConformanceTests.OptionsJson
{
    /// <summary>
    /// The hint for a component in a GOV.UK Frontend test fixture.
    /// </summary>
    public record Hint
    {
        public string? Text { get; set; }
        public string? Html { get; set; }
    }

}
