using Moq;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.GovUk.Frontend.Umbraco;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Dictionary;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services;

public class TprAddressLookupValidationMessageResolverTests
{
    [Fact]
    public void Resolve_uses_block_values_and_dictionary_fallback_values()
    {
        using var testContext = new UmbracoTestContext();
        var blockContent = UmbracoBlockListFactory.CreateContentOrSettings("tprAddressLookup")
            .SetupUmbracoTextboxPropertyValue(TprPropertyAliases.AddressLookupAddressLine1Label, "Block address line 1")
            .Object;
        var blockSettings = UmbracoBlockListFactory.CreateContentOrSettings("tprAddressLookupSettings")
            .SetupUmbracoTextboxPropertyValue(PropertyAliases.ModelProperty, "ShippingAddress")
            .SetupUmbracoTextboxPropertyValue(TprPropertyAliases.AddressLookupAddressLine1RequiredErrorMessage, "Block required message")
            .Object;
        var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
            UmbracoBlockListFactory.CreateOverridableBlock(blockContent, blockSettings));
        testContext.CurrentPage.Object.SetupUmbracoBlockListPropertyValue("blocks", blockList);

        var dictionary = new Mock<ICultureDictionary>();
        dictionary.Setup(x => x[It.IsAny<string>()]).Returns(string.Empty);
        dictionary.Setup(x => x[TprAddressLookupDictionaryKeys.AddressLine2MaxLengthErrorMessage])
            .Returns("Dictionary max-length message");
        var resolver = new TprAddressLookupValidationMessageResolver(
            testContext.UmbracoContextAccessor.Object,
            testContext.PublishedValueFallback.Object,
            dictionary.Object);

        var content = resolver.Resolve("ShippingAddress");

        Assert.NotNull(content);
        Assert.Equal("Block address line 1", content.AddressLine1Label);
        Assert.Equal("Block required message", content.AddressLine1RequiredErrorMessage);
        Assert.Equal("Dictionary max-length message", content.AddressLine2MaxLengthErrorMessage);
    }

    [Fact]
    public void Resolve_returns_null_when_no_block_is_bound_to_the_model_property()
    {
        using var testContext = new UmbracoTestContext();
        var blockContent = UmbracoBlockListFactory.CreateContentOrSettings("tprAddressLookup").Object;
        var blockSettings = UmbracoBlockListFactory.CreateContentOrSettings("tprAddressLookupSettings")
            .SetupUmbracoTextboxPropertyValue(PropertyAliases.ModelProperty, "ShippingAddress")
            .Object;
        var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
            UmbracoBlockListFactory.CreateOverridableBlock(blockContent, blockSettings));
        testContext.CurrentPage.Object.SetupUmbracoBlockListPropertyValue("blocks", blockList);
        var resolver = new TprAddressLookupValidationMessageResolver(
            testContext.UmbracoContextAccessor.Object,
            testContext.PublishedValueFallback.Object,
            Mock.Of<ICultureDictionary>());

        Assert.Null(resolver.Resolve("BillingAddress"));
    }
}