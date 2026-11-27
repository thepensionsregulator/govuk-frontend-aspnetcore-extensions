using System.Globalization;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.Hosting;
using ThePensionsRegulator.Frontend.Models;

namespace ThePensionsRegulator.Frontend.Tests.Views
{
    public class TprSessionTimeOutWarningTests
    {
        [Theory]
        [InlineData(125000.5, "125000.5")]
        [InlineData(1800000, "1800000")]
        public async Task Render_ShouldIncludeRuntimeSettingsUsingInvariantMilliseconds_WhenCurrentCultureUsesCommaDecimalSeparator(
            double remainingMilliseconds,
            string expectedRemainingMilliseconds)
        {
            // Arrange
            var model = new TprSessionTimeOutModel
            {
                Settings = new SessionTimeoutSettings
                {
                    RemainingTime = TimeSpan.FromMilliseconds(remainingMilliseconds),
                    ExtensionLength = TimeSpan.FromMinutes(3),
                    WarningBeforeExpiry = TimeSpan.FromMinutes(2),
                    ExpiryUrl = "/signed-out?reason=timeout&source=warning"
                }
            };
            var originalCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");

                // Act
                var html = await Render(model);

                // Assert
                Assert.Contains("data-module=\"tpr-session-timeout\"", html);
                Assert.Contains($"data-timeout-ms=\"{expectedRemainingMilliseconds}\"", html);
                Assert.Contains("data-extension-ms=\"180000\"", html);
                Assert.Contains("data-warning-ms=\"120000\"", html);
                Assert.Contains("data-session-timeout-expiry-url=\"/signed-out?reason=timeout&amp;source=warning\"", html);
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
            }
        }

        [Fact]
        public async Task Render_ShouldLeaveMessageStatic_WhenSettingsAreNotConfigured()
        {
            // Arrange
            var model = new TprSessionTimeOutModel { Content = "Expires in {remainingTime}." };

            // Act
            var html = await Render(model);

            // Assert
            Assert.DoesNotContain("data-module=\"tpr-session-timeout\"", html);
            Assert.Contains("Expires in {remainingTime}.", html);
        }

        [Fact]
        public async Task Render_ShouldOmitExpiryNavigationAttribute_WhenExpiryUrlIsNotConfigured()
        {
            // Arrange
            var model = new TprSessionTimeOutModel
            {
                Settings = new SessionTimeoutSettings
                {
                    RemainingTime = TimeSpan.FromMinutes(30),
                    ExtensionLength = TimeSpan.FromMinutes(30),
                    WarningBeforeExpiry = TimeSpan.FromMinutes(2)
                }
            };

            // Act
            var html = await Render(model);

            // Assert
            Assert.Contains("data-timeout-ms=\"1800000\"", html);
            Assert.DoesNotContain("data-session-timeout-expiry-url=\"/", html);
        }

        private static async Task<string> Render(TprSessionTimeOutModel model)
        {
            var item = new RazorCompiledItemLoader().LoadItems(typeof(TprSessionTimeOutModel).Assembly)
                .Single(x => x.Identifier == "/Views/Shared/TPR/TPRSessionTimeOutWarning.cshtml");
            var page = (RazorPage<TprSessionTimeOutModel>)Activator.CreateInstance(item.Type)!;
            using var writer = new StringWriter();
            page.HtmlEncoder = HtmlEncoder.Default;
            page.ViewData = new ViewDataDictionary<TprSessionTimeOutModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };
            page.ViewContext = new ViewContext
            {
                Writer = writer,
                ViewData = page.ViewData
            };
            await page.ExecuteAsync();
            return writer.ToString();
        }
    }
}
