namespace GovUk.Frontend.AspNetCore.Extensions.ConformanceTests
{
    public record ComponentTestCaseData<T>(string Name, T Options, string ExpectedHtml)
    {
        public override string ToString() => Name;
    }
}
