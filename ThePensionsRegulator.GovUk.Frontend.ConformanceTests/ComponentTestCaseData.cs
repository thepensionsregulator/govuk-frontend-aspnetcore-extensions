namespace ThePensionsRegulator.GovUk.Frontend.ConformanceTests
{
    public record ComponentTestCaseData<T>(string Name, T Options, string ExpectedHtml)
    {
        public override string ToString() => Name;
    }
}
