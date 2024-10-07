namespace GovUk.Frontend.AspNetCore.Extensions.ConformanceTests.OptionsJson
{
    /// <summary>
    /// A task list in a GOV.UK Frontend test fixture.
    /// </summary>
    public record TaskList
    {
        public IList<TaskListTask> Items { get; set; } = new List<TaskListTask>();
        public string? Html { get; set; }
        public string? Classes { get; set; }
        public string? IdPrefix { get; set; }
        public IDictionary<string, string?> Attributes { get; set; } = new Dictionary<string, string?>();
    }
}
