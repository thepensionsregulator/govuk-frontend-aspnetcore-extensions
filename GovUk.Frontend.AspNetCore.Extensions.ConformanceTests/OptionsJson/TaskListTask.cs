namespace GovUk.Frontend.AspNetCore.Extensions.ConformanceTests.OptionsJson
{
    /// <summary>
    /// A task in a task list in a GOV.UK Frontend test fixture.
    /// </summary>
    public record TaskListTask
    {
        public TaskTitle Title { get; set; } = new();
        public Hint? Hint { get; set; }
        public string? Href { get; set; }
        public string? Classes { get; set; }
        public TaskStatus Status { get; set; } = new();
    }

}
