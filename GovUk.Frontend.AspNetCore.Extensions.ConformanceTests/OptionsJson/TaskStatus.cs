namespace GovUk.Frontend.AspNetCore.Extensions.ConformanceTests.OptionsJson
{
    /// <summary>
    /// The status of a task in a task list in a GOV.UK Frontend test fixture.
    /// </summary>
    public record TaskStatus
    {
        public string? Text { get; set; }
        public string? Html { get; set; }
        public string? Classes { get; set; }
        public Tag? Tag { get; set; }
    }

}
