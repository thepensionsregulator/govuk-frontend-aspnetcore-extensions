
using ThePensionsRegulator.GovUk.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.GovUk.Frontend.ConformanceTests
{
    public partial class ComponentTests
    {
        private readonly ComponentGenerator _componentGenerator;

        public ComponentTests()
        {
            _componentGenerator = new ComponentGenerator();
        }

        private void CheckComponentHtmlMatchesExpectedHtml<TOptions>(
            ComponentTestCaseData<TOptions> testCaseData,
            Func<ComponentGenerator, TOptions, string> generateComponent)
        {
            var html = generateComponent(_componentGenerator, testCaseData.Options);

            Assert.HtmlEqual(testCaseData.ExpectedHtml, html);
        }
    }
}