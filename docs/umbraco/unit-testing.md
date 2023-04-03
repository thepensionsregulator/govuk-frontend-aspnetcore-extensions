# Umbraco unit testing

Add `ThePensionsRegulator.GovUk.Frontend.Umbraco.Testing` NuGet package.

Examples on this page are shown with NUnit, but these helper classes should work with any testing framework. However [Moq](https://github.com/moq/moq4) is required for mocking with these helper classes.

## Create an Umbraco context

Create an instance of `UmbracoTestContext` in your setup method. This will give you access to an Umbraco context that mocks a page request.

```csharp
using GovUk.Frontend.Umbraco.Testing;

private UmbracoTestContext _testContext;
private ExampleController _controllerUnderTest;

[SetUp]
public void SetUp()
{
    _testContext = new();

    _controllerUnderTest = new(
        Mock.Of<ILogger<ExampleController>>(),
        _testContext.CompositeViewEngine.Object,
        _testContext.UmbracoContextAccessor.Object,
        _testContext.VariationContextAccessor.Object,
        _testContext.ServiceContext
        )
    {
        ControllerContext = _testContext.ControllerContext
    };
}
```

## Mock Umbraco content

`UmbracoTestContext` gives you a mock for the current page, but if you need to mock a content item other than the current page you can do so.

```csharp
var otherPage = UmbracoContentFactory.CreateContent<IPublishedContent>();
```

## Mock Umbraco property values

Extension methods allow you to mock property values on any content node. For example, with an `UmbracoTestContext` instantiated as shown above:

```csharp
_testContext.CurrentPage.SetupUmbracoPropertyValue("myPropertyAlias", "The text saved in the property");
```

There are several overloads for common property types. You should use these where possible to get a more complete mocked instance.

```csharp
_testContext.CurrentPage.SetupUmbracoTextboxPropertyValue("myTextPropertyAlias", "The text saved in the property");
_testContext.CurrentPage.SetupUmbracoIntegerPropertyValue("myIntegerPropertyAlias", 123);
_testContext.CurrentPage.SetupUmbracoBooleanPropertyValue("myTrueFalsePropertyAlias", true);
_testContext.CurrentPage.SetupUmbracoMultiUrlPickerPropertyValue("myUrlPropertyAlias", new Link() { Url = "https://example.org" });
_testContext.CurrentPage.SetupUmbracoBlockListPropertyValue("myBlockListPropertyAlias", myBlockList);
```

If the above overloads don't meet your needs you can create a property directly.

```csharp
var prop1 = UmbracoPropertyFactory.CreateProperty("myPropertyAlias", myPropertyType, string.Empty);
var prop2 = UmbracoPropertyFactory.CreateTextboxProperty("myTextPropertyAlias", string.Empty);
var prop2 = UmbracoPropertyFactory.CreateIntegerProperty("myIntegerPropertyAlias", 123);
var prop3 = UmbracoPropertyFactory.CreateBooleanProperty("myTrueFalsePropertyAlias", true);
var prop4 = UmbracoPropertyFactory.CreateMultiUrlPickerProperty("myUrlPropertyAlias", new Link() { Url = "https://example.org" });
var prop5 = UmbracoPropertyFactory.CreateBlockListProperty("myBlockListPropertyAlias", myBlockList);
```

## Mock Umbraco block lists

`UmbracoBlockListFactory` makes it easy to mock blocks in a block list. You can add as many properties as you need to the block content and settings using the extension methods described above in a fluent syntax.

```csharp
var blockList = UmbracoBlockListFactory.CreateBlockListModel(
                    UmbracoBlockListFactory.CreateBlock(
                        // Block content
                        UmbracoBlockListFactory.CreateContentOrSettings()
                        .SetupUmbracoTextboxPropertyValue("myContentPropertyAlias", "value on the block content")
                        .Object,
                        // Block settings (optional)
                        UmbracoBlockListFactory.CreateContentOrSettings()
                        .SetupUmbracoTextboxPropertyValue("mySettingsPropertyAlias", "value on the block settings")
                        .Object
                    )
                );
_testContext.CurrentPage.SetupUmbracoBlockListPropertyValue("myBlockListPropertyAlias", blockList);
```

If your block list has another block list as a child, it's just another property.

```csharp
var blockList = UmbracoBlockListFactory.CreateBlockListModel(
                    UmbracoBlockListFactory.CreateBlock(
                        UmbracoBlockListFactory.CreateContentOrSettings()
                        .SetupUmbracoTextboxPropertyValue("myContentPropertyAlias", "value on the parent block content")
                        .Object
                    )
                );

var childBlockList = UmbracoBlockListFactory.CreateBlockListModel(
                    UmbracoBlockListFactory.CreateBlock(
                        UmbracoBlockListFactory.CreateContentOrSettings()
                        .SetupUmbracoTextboxPropertyValue("myChildContentPropertyAlias", "value on the child block content")
                        .Object
                    )
                );

blockList[0].Content.SetupUmbracoBlockListPropertyValue("myChildBlockListPropertyAlias", childBlockList);
```

There are also overridable versions of these methods.

```csharp
var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                    UmbracoBlockListFactory.CreateOverridableBlock(
                        UmbracoBlockListFactory.CreateContentOrSettings()
                        .SetupUmbracoTextboxPropertyValue("myContentPropertyAlias", "value on the block content")
                        .Object
                    )
                );
_testContext.CurrentPage.SetupUmbracoBlockListPropertyValue("myBlockListPropertyAlias", blockList);
```

When writing unit tests with blocks you may see the following error:

> System.TypeInitializationException : The type initializer for 'Umbraco.Extensions.FriendlyPublishedElementExtensions' threw an exception.

      ----> System.ArgumentNullException : Value cannot be null. (Parameter 'provider')

This is thrown by the built-in `.Value<T>` extension method. To resolve this your code may need to cast a block to `OverridableBlockListItem`, which overrides this extension method with one that works during testing.

```csharp
// Throws an error
var fieldset1 = blockList.FindBlockByContentTypeAlias(GovukFieldset.ModelTypeAlias);
var fieldsetBlocks1 = fieldset1.Content.Value<OverridableBlockListModel>(nameof(GovukFieldset.Blocks));

// Works, because the block is cast to OverridableBlockListItem
var fieldset2 = ((OverridableBlockListItem)blockList.FindBlockByContentTypeAlias(GovukFieldset.ModelTypeAlias));
var fieldsetBlocks2 = fieldset2.Content.Value<OverridableBlockListModel>(nameof(GovukFieldset.Blocks));
```
