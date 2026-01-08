using GovUk.Frontend.Umbraco.HtmlGeneration;
using HtmlAgilityPack;

namespace GovUk.Frontend.Umbraco.Tests.HtmlGeneration
{
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

        [Fact]
        public void When_DayEnabled_Is_True_And_YearEnabled_Is_True_Html_Is_Unchanged()
        {
            // Arrange
            var enhancer = new DateInputHtmlEnhancer();

            // Act
            var result = enhancer.EnhanceHtml(DATE_INPUT_HTML, true, true);

            // Assert
            Assert.Equal(DATE_INPUT_HTML, result);
        }

        [Fact]
        public void When_DayEnabled_Is_False_Day_Field_Is_Removed()
        {
            // Arrange
            var enhancer = new DateInputHtmlEnhancer();

            // Act
            var result = enhancer.EnhanceHtml(DATE_INPUT_HTML, false, true);

            // Assert
            var doc = new HtmlDocument();
            doc.LoadHtml(result);

            Assert.Equal(2, doc.DocumentNode.SelectNodes("//div[@class='govuk-date-input__item']").Count);
            Assert.Null(doc.DocumentNode.SelectNodes("//input[@id='Example.Day']"));
            Assert.Equal(1, doc.DocumentNode.SelectNodes("//input[@id='Example.Month']").Count);
            Assert.Equal(1, doc.DocumentNode.SelectNodes("//input[@id='Example.Year']").Count);
        }

        [Fact]
        public void When_YearEnabled_Is_False_Year_Field_Is_Removed()
        {
            // Arrange
            var enhancer = new DateInputHtmlEnhancer();

            // Act
            var result = enhancer.EnhanceHtml(DATE_INPUT_HTML, true, false);

            // Assert
            var doc = new HtmlDocument();
            doc.LoadHtml(result);

            Assert.Equal(2, doc.DocumentNode.SelectNodes("//div[@class='govuk-date-input__item']").Count);
            Assert.Equal(1, doc.DocumentNode.SelectNodes("//input[@id='Example.Day']").Count);
            Assert.Equal(1, doc.DocumentNode.SelectNodes("//input[@id='Example.Month']").Count);
            Assert.Null(doc.DocumentNode.SelectNodes("//input[@id='Example.Year']"));
        }
    }
}
