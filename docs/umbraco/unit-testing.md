# Umbraco unit testing

Add `ThePensionsRegulator.Umbraco.Testing` NuGet package.

Examples on this page are shown with NUnit, but these helper classes should work with any testing framework. However [Moq](https://github.com/moq/moq4) is required for mocking with these helper classes.

## Create an Umbraco context

Create an instance of `UmbracoTestContext` to get access to a mock Umbraco page request. Because `UmbracoTestContext` writes to a process-wide static field it **must always be disposed** and test classes that use it **must not run in parallel** with each other. See [Avoiding flaky tests](#avoiding-flaky-tests) for details.

### One context per test

Use `using var` so the context is disposed automatically at the end of the test. You can customise the context mocks with no side-effects.

```csharp
[Fact]
public void My_test()
{
    using var testContext = new UmbracoTestContext();

    var controller = new ExampleController(
        Mock.Of<ILogger<ExampleController>>(),
        testContext.CompositeViewEngine.Object,
        testContext.UmbracoContextAccessor.Object,
        testContext.VariationContextAccessor.Object,
        testContext.ServiceContext)
    {
        ControllerContext = testContext.ControllerContext
    };
}
```

### One context per test, with setup (xUnit)

If all tests in the class need the same setup (for example a `SetupContentType` call), implement `IDisposable` and create the context in the constructor. xUnit creates a new instance of the test class per test, so `Dispose` is called after each test:

```csharp
public class ExampleTests : IDisposable
{
    private readonly UmbracoTestContext _testContext;

    public ExampleTests()
    {
        _testContext = new UmbracoTestContext().SetupContentType("myContentTypeAlias");
    }

    public void Dispose() => _testContext.Dispose();

    [Fact]
    public void My_test()
    {
        var element = UmbracoContentFactory.CreateContent<IPublishedElement>("myContentTypeAlias");
    }
}
```

### One context per test, with setup (NUnit)

If all tests in the class need the same setup (for example a `SetupContentType` call), use `[SetUp]` and `[TearDown]`. NUnit calls these before and after each individual test:

```csharp
[TestFixture]
public class ExampleTests
{
    private UmbracoTestContext _testContext;

    [SetUp]
    public void SetUp() => _testContext = new UmbracoTestContext().SetupContentType("myContentTypeAlias");

    [TearDown]
    public void TearDown() => _testContext.Dispose();

    [Test]
    public void My_test()
    {
        var element = UmbracoContentFactory.CreateContent<IPublishedElement>("myContentTypeAlias");
    }
}
```

### One context per class (xUnit)

Implement `IClassFixture<UmbracoTestContext>`. xUnit creates one instance for the class and disposes it after all tests have run:

```csharp
public class ExampleTests(UmbracoTestContext _testContext) : IClassFixture<UmbracoTestContext>
{
    [Fact]
    public void My_test()
    {
        var controller = new ExampleController(
            Mock.Of<ILogger<ExampleController>>(),
            _testContext.CompositeViewEngine.Object,
            _testContext.UmbracoContextAccessor.Object,
            _testContext.VariationContextAccessor.Object,
            _testContext.ServiceContext)
        {
            ControllerContext = _testContext.ControllerContext
        };
    }
}
```

### One context per class (NUnit)

Use `[OneTimeSetUp]` and `[OneTimeTearDown]`:

```csharp
[TestFixture]
public class ExampleTests
{
    private UmbracoTestContext _testContext;

    [OneTimeSetUp]
    public void SetUp() => _testContext = new UmbracoTestContext();

    [OneTimeTearDown]
    public void TearDown() => _testContext.Dispose();
}
```

## Mock Umbraco content

`UmbracoTestContext` gives you a mock for the current page, but if you need to mock a content item other than the current page you can do so.

```csharp
var otherPage = UmbracoContentFactory.CreateContent<IPublishedContent>();
```

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
```

If the above overloads don't meet your needs you can create a property directly.

```csharp
var prop1 = UmbracoPropertyFactory.CreateProperty("myPropertyAlias", myPropertyType, string.Empty);
var prop2 = UmbracoPropertyFactory.CreateRichTextProperty("myRichTextPropertyAlias", string.Empty);
var prop2 = UmbracoPropertyFactory.CreateTextboxProperty("myTextPropertyAlias", string.Empty);
var prop2 = UmbracoPropertyFactory.CreateIntegerProperty("myIntegerPropertyAlias", 123);
var prop3 = UmbracoPropertyFactory.CreateBooleanProperty("myTrueFalsePropertyAlias", true);
var prop3 = UmbracoPropertyFactory.CreateContentPickerProperty("myContentPickerPropertyAlias", UmbracoContentFactory.CreateContent<IPublishedContent>("pickedContentAlias").Object);
var prop4 = UmbracoPropertyFactory.CreateMultiUrlPickerProperty("myUrlPropertyAlias", new Link() { Url = "https://example.org" });
var prop5 = UmbracoPropertyFactory.CreateBlockListProperty("myBlockListPropertyAlias", myBlockList);
var prop5 = UmbracoPropertyFactory.CreateBlockGridProperty("myBlockGridPropertyAlias", myBlockGrid);
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

## Avoiding flaky tests

`UmbracoTestContext` sets `Umbraco.Cms.Core.DependencyInjection.StaticServiceProvider.Instance` — a single static field shared by every thread in the process. Two things can go wrong if this is not managed carefully:

- **Stale provider after a test** — if the context is not disposed the static field retains its value and the next test may resolve services from the wrong context.
- **Mid-test overwrite** — if two test classes run in parallel on different threads, one thread can overwrite the static field while the other thread's test is still using it.

`UmbracoTestContext` addresses the first problem by implementing `IDisposable`: construction saves the current value of the static field and `Dispose` restores it. The second problem requires serialising test execution so that only one test class that uses `UmbracoTestContext` runs at a time.

### xUnit

Add an `AssemblyInfo.cs` file to each xUnit test project that contains tests using `UmbracoTestContext`:

```csharp
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
```

This prevents all test classes within the assembly from running in parallel with each other, which is sufficient to protect the static field.

### NUnit

NUnit does not run tests in parallel by default, so no additional configuration is needed.

### Resolving flaky tests when multiple test projects are run together

After disabling parallel tests as described above the remaining risk is cross-assembly: `dotnet test` runs multiple test projects in parallel by default, and the configuration above has no effect on that. `IDisposable` on `UmbracoTestContext` minimises the window during which that cross-assembly race can occur.

The best solution is to look into the Umbraco code to find out which services are using the static service locator, and then look for overloads that avoid it. For example `IPublishedContent.Parent()` uses the static service locator and is difficult to test, but `IPublishedContent.Parent<T>(IDocumentNavigationQueryService, IPublishedContentStatusFilteringService)` does not and can be tested.
