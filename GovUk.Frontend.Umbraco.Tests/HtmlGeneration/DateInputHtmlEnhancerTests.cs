using GovUk.Frontend.Umbraco.HtmlGeneration;
using HtmlAgilityPack;
using NUnit.Framework;

namespace GovUk.Frontend.Umbraco.Tests.HtmlGeneration
{
    [TestFixture]
    public class DateInputHtmlEnhancerTests
    {
        private const string DATE_INPUT_HTML = @"
            <div class=""govuk-form-group"">
                <div class=""govuk-date-input"" id=""Example"">
                    <div class=""govuk-date-input__item"">
                        <div class=""govuk-form-group"">
                            <label class=""govuk-label govuk-date-input__label"" for=""Example.Day"">Day</label>
                            <input aria-invalid=""false"" class=""govuk-date-input__input govuk-input govuk-input--width-2"" id=""Example.Day"" inputmode=""numeric"" maxlength=""2"" name=""Example.Day"" type=""text"" value="""" required=""required"" />
                        </div>
                    </div>
                    <div class=""govuk-date-input__item"">
                        <div class=""govuk-form-group"">
                            <label class=""govuk-label govuk-date-input__label"" for=""Example.Month"">Month</label>
                            <input aria-invalid=""false"" class=""govuk-date-input__input govuk-input govuk-input--width-2"" id=""Example.Month"" inputmode=""numeric"" maxlength=""2"" name=""Example.Month"" type=""text"" value="""" required=""required"" />
                        </div>
                    </div>
                    <div class=""govuk-date-input__item"">
                        <div class=""govuk-form-group"">
                            <label class=""govuk-label govuk-date-input__label"" for=""Example.Year"">Year</label>
                            <input aria-invalid=""false"" class=""govuk-date-input__input govuk-input govuk-input--width-4"" id=""Example.Year"" inputmode=""numeric"" maxlength=""4"" name=""Example.Year"" type=""text"" value="""" required=""required"" />
                        </div>
                    </div>
                </div>
            </div>";

        [Test]
        public void When_DayEnabled_Is_True_Html_Is_Unchanged()
        {
            // Arrange
            var enhancer = new DateInputHtmlEnhancer();

            // Act
            var result = enhancer.EnhanceHtml(DATE_INPUT_HTML, true);

            // Assert
            Assert.That(result, Is.EqualTo(DATE_INPUT_HTML));
        }

        [Test]
        public void When_DayEnabled_Is_False_Day_Field_Is_Removed()
        {
            // Arrange
            var enhancer = new DateInputHtmlEnhancer();

            // Act
            var result = enhancer.EnhanceHtml(DATE_INPUT_HTML, false);

            // Assert
            var doc = new HtmlDocument();
            doc.LoadHtml(result);

            Assert.That(doc.DocumentNode.SelectNodes("//div[@class='govuk-date-input__item']").Count, Is.EqualTo(2));
            Assert.That(doc.DocumentNode.SelectNodes("//input[@id='Example.Day']"), Is.Null);
            Assert.That(doc.DocumentNode.SelectNodes("//input[@id='Example.Month']").Count, Is.EqualTo(1));
            Assert.That(doc.DocumentNode.SelectNodes("//input[@id='Example.Year']").Count, Is.EqualTo(1));
        }
    }
}
