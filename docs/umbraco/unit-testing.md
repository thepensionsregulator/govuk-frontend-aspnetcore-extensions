# Umbraco unit testing

Add `ThePensionsRegulator.Umbraco.Testing` NuGet package.

Examples on this page are shown with XUnit, but these helper classes should work with any testing framework. However [Moq](https://github.com/moq/moq4) is required for mocking with these helper classes.

## Create an Umbraco context

Create an instance of `UmbracoTestContext` in your setup method. This will give you access to an Umbraco context that mocks a page request.

```csharp
using ThePensionsRegulator.Umbraco.Testing;

private UmbracoTestContext _testContext;
private ExampleController _controllerUnderTest;

public MyTestClass()
{
    _testContext = new();

    _controllerUnderTest = new(
        Mock.Of<ILogger<ExampleController>>(),
        _testContext.CompositeViewEngine.Object,
        _testContext.UmbracoContextAccessor.Object
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

If you need to mock a content hierarchy you can do this by starting with the child page and setting one or more ancestors. These will be ordered by their `Level` property.

```csharp
var context = new UmbracoTestContext();

var grandparent = UmbracoContentFactory.CreateContent<IPublishedContent>();
grandparent.Setup(x => x.Level).Returns(1);

var parent = UmbracoContentFactory.CreateContent<IPublishedContent>();
parent.Setup(x => x.Level).Returns(2);
parent.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [grandparent.Object]);

context.CurrentPage.Setup(x => x.Level).Returns(3);
context.CurrentPage.SetupAncestors(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [parent.Object, grandparent.Object]);
```

You can also setup child pages:

```csharp
var context = new UmbracoTestContext();

var child = UmbracoContentFactory.CreateContent<IPublishedContent>();
child.Setup(x => x.Level).Returns(2);

context.CurrentPage.Setup(x => x.Level).Returns(1);
context.CurrentPage.SetupChildren(context.DocumentNavigationQueryService, context.PublishedContentStatusFilteringService, [child.Object]);

```

When working with hierarchy methods like `.Root()`, `.Children()`, `.Parent()` and `.Ancestors()` be sure to use overloads that take instances of `IDocumentNavigationQueryService` and `IPublishedContentStatusFilteringService` to avoid side effects in other tests.

## Mock Umbraco content types

Mock Umbraco content types by adding them to the `UmbracoTestContext` using a fluent interface.

```csharp
_testContext = new UmbracoTestContext()
        .SetupContentType("myContentTypeAlias");

var contentType = _testContext.ContentTypes["myContentTypeAlias"].Object;
```

## Mock Umbraco property values

Extension methods allow you to mock property values on any content node. For example, with an `UmbracoTestContext` instantiated as shown above:

```csharp
_testContext.CurrentPage.SetupUmbracoPropertyValue("myPropertyAlias", "The text saved in the property");
```

There are several overloads for common property types. You should use these where possible to get a more complete mocked instance.

```csharp
_testContext.CurrentPage.SetupUmbracoRichTextPropertyValue("myRichTextPropertyAlias", "<p>The HTML saved in the property</p>");
_testContext.CurrentPage.SetupUmbracoTextboxPropertyValue("myTextPropertyAlias", "The text saved in the property");
_testContext.CurrentPage.SetupUmbracoIntegerPropertyValue("myIntegerPropertyAlias", 123);
_testContext.CurrentPage.SetupUmbracoBooleanPropertyValue("myTrueFalsePropertyAlias", true);
_testContext.CurrentPage.SetupUmbracoContentPickerPropertyValue("myContentPropertyAlias", UmbracoContentFactory.CreateContent<IPublishedContent>("pickedContentAlias").Object);
_testContext.CurrentPage.SetupUmbracoMultiUrlPickerPropertyValue("myUrlPropertyAlias", new Link() { Url = "https://example.org" });
_testContext.CurrentPage.SetupUmbracoBlockListPropertyValue("myBlockListPropertyAlias", myBlockList);
_testContext.CurrentPage.SetupUmbracoBlockGridPropertyValue("myBlockGridPropertyAlias", myBlockGrid);
```

If the above overloads don't meet your needs you can create a property directly.

```csharp
var prop1 = UmbracoPropertyFactory.CreateProperty("myPropertyAlias", myPropertyType, string.Empty);
var prop2 = UmbracoPropertyFactory.CreateRichTextProperty("myRichTextPropertyAlias", string.Empty);
var prop3 = UmbracoPropertyFactory.CreateTextboxProperty("myTextPropertyAlias", string.Empty);
var prop4 = UmbracoPropertyFactory.CreateIntegerProperty("myIntegerPropertyAlias", 123);
var prop5 = UmbracoPropertyFactory.CreateBooleanProperty("myTrueFalsePropertyAlias", true);
var prop6 = UmbracoPropertyFactory.CreateContentPickerProperty("myContentPickerPropertyAlias", UmbracoContentFactory.CreateContent<IPublishedContent>("pickedContentAlias").Object);
var prop7 = UmbracoPropertyFactory.CreateMultiUrlPickerProperty("myUrlPropertyAlias", new Link() { Url = "https://example.org" });
var prop8 = UmbracoPropertyFactory.CreateBlockListProperty("myBlockListPropertyAlias", myBlockList);
var prop9 = UmbracoPropertyFactory.CreateBlockGridProperty("myBlockGridPropertyAlias", myBlockGrid);
```

## Mock Umbraco block lists and block grids

`UmbracoBlockListFactory` and `UmbracoBlockGridFactory` make it easy to mock blocks in a block list or block grid. You can add as many properties as you need to the block content and settings using the extension methods described above in a fluent syntax.

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

 var blockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                    UmbracoBlockGridFactory.CreateOverridableBlock("blockWithArea")
                        .AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area"))
                ]);
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

## Mock Umbraco Dictionary items

`LocalizationServiceExtensions` provides an easy way to mock an Umbraco dictionary. You can add as many dictionary values as needed using a fluent syntax. You can also provide different translations.

```csharp
var dictionary = new Mock<ILocalizationService>();

dictionary
    .AddDictionaryValue("myKey", "myValue") // languageId defaults to 2 - English GB
    .AddDictionaryValue("myKey", "myValue", 1); // Setting languageId to 1 - English US
```

## Troubleshooting tests

### Value cannot be null. (Parameter 'contentType')

You may see the following error:

```csharp
System.ArgumentNullException : Value cannot be null. (Parameter 'contentType')
```

The solution is to set up the relevant content type on the `UmbracoTestContext` - see [Mock Umbraco content types](#mock-umbraco-content-types).

### TypeInitializationException

When writing unit tests with blocks you may see the following error:

```csharp
System.TypeInitializationException : The type initializer for 'Umbraco.Extensions.FriendlyPublishedElementExtensions' threw an exception.
----> System.ArgumentNullException : Value cannot be null. (Parameter 'provider')
```

This is thrown by the built-in `.Value<T>` extension method. To resolve this make sure you are creating an instance of `UmbracoTestContext`, which sets up the dependency injection to make this work. Alternatively, your code may need to cast a block to `OverridableBlockListItem`, which overrides this extension method with one that works during testing.

```csharp
// Throws an error
var fieldset1 = blockList.FindBlockByContentTypeAlias(GovukFieldset.ModelTypeAlias);
var fieldsetBlocks1 = fieldset1.Content.Value<OverridableBlockListModel>(nameof(GovukFieldset.Blocks));

// Works, because the block is cast to OverridableBlockListItem
var fieldset2 = ((OverridableBlockListItem)blockList.FindBlockByContentTypeAlias(GovukFieldset.ModelTypeAlias));
var fieldsetBlocks2 = fieldset2.Content.Value<OverridableBlockListModel>(nameof(GovukFieldset.Blocks));
```

### InvalidCastException

When writing unit tests with blocks that override content you may see the following error:

```csharp
System.InvalidCastException : Unable to cast object of type 'Castle.Proxies.IPublishedElementProxy' to type 'ThePensionsRegulator.Umbraco.IOverridablePublishedElement'
```

This is because your mock is `IPublishedElement` when `IOverridablePublishedElement` is expected.

```csharp
// Throws an error
UmbracoContentFactory.CreateContent<IPublishedElement>();

// Works, because the mock is the expected type
UmbracoContentFactory.CreateContent<IOverridablePublishedElement>();
```

### Tests pass on their own but fail when others are run

Umbraco uses internally a static service locator at `Umbraco.Cms.Core.DependencyInjection.StaticServiceProvider.Instance`.

`UmbracoTestContext` registers implementations with this service locator to support Umbraco methods that expect it. However, because it's static, this instance is shared across all tests in a run. In some cases this is fine, but in cases where you update the mocks this can cause problems in other tests.

The best solution is to look into the Umbraco code to find out which services are using the static service locator, and then look for overloads that avoid it. For example `IPublishedContent.Parent()` uses the static service locator and is difficult to test, but `IPublishedContent.Parent<T>(IDocumentNavigationQueryService, IPublishedContentStatusFilteringService)` does not and can be tested.
