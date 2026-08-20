using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using ThePensionsRegulator.Frontend.TagHelpers;

namespace ThePensionsRegulator.Frontend.Tests.TagHelpers
{
    public class TprHeaderMenuTagHelperTest
    {
        [Fact]
        public async Task All_values_are_correctly_stored_in_context()
        {
            // Arrange
            var expectedAriaLabel = "Main Navigation";
            var expectedNoJSNavPage = "/site-map";
            var expectedToggleClosed = "Menu";
            var expectedToggleOpen = "Closed";
            var expectedItemAriaLabel = "Nav Item";
            var expectedSearchId = "form-1";


            var tagHelper = new TprHeaderMenuTagHelper
            {
                MenuAriaLabel = expectedAriaLabel,
                NoJsNavPage =  expectedNoJSNavPage,
                ToggleClosed = expectedToggleClosed,
                ToggleOpen = expectedToggleOpen,
                MenuItemAriaLabel = expectedItemAriaLabel,
                SearchFormId = expectedSearchId
            };

            var attributes = new TagHelperAttributeList();

            var headerBarContext = new TprHeaderBarContext();

            var context = new TagHelperContext(attributes, new Dictionary<object, object> { { typeof(TprHeaderBarContext), headerBarContext } }, Guid.NewGuid().ToString());

            var output = new TagHelperOutput("tpr-header-menu", attributes, (result, encoder) =>
            {
                return Task.FromResult<TagHelperContent>(new DefaultTagHelperContent());
            });

            // Act
            await tagHelper.ProcessAsync(context, output);

            // Assert
            Assert.Equal(expectedAriaLabel, headerBarContext.HeaderMenuAriaLabel);
            Assert.Equal(expectedNoJSNavPage, headerBarContext.MobileMenuNoJsNavPage);
            Assert.Equal(expectedToggleClosed, headerBarContext.HeaderMenuToggleClosed);
            Assert.Equal(expectedToggleOpen, headerBarContext.HeaderMenuToggleOpen);
            Assert.Equal(expectedItemAriaLabel, headerBarContext.HeaderMenuItemAriaLabel);
            Assert.Equal(expectedSearchId, headerBarContext.HeaderMenuSearchFormId);
        }
    }
}
