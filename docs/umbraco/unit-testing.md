# Umbraco unit testing

Add `ThePensionsRegulator.Umbraco.Testing` NuGet package.

Examples on this page are shown with NUnit, but these helper classes should work with any testing framework. However [Moq](https://github.com/moq/moq4) is required for mocking with these helper classes.

## Create an Umbraco context

Create an instance of `UmbracoTestContext` to get access to a mock Umbraco page request. Because `UmbracoTestContext` writes to a process-wide static field it **must always be disposed** and test classes that use it **must not run in parallel** with each other. See [Avoiding flaky tests](#avoiding-flaky-tests) for details.

### One context shared across all tests in a class (recommended)

Sharing one context across the whole class avoids the overhead of constructing and disposing `UmbracoTestContext` for every test. See the xUnit and NUnit sections below.

### One context per test

This is simpler when tests have no shared setup and do not call `SetupContentType`. Use `using var` so the context is disposed automatically at the end of the test:

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

### One context per class (xUnit)

Implement `IClassFixture<UmbracoTestContext>`. xUnit creates one instance for the class and disposes it after all tests have run:

```csharp
[Collection("UmbracoTests")]
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

If you need to call `SetupContentType` or do other one-time setup on the context, use a `Fixture` class that owns the `UmbracoTestContext`. xUnit constructs the fixture **once per class** regardless of how many tests there are, so `SetupContentType` is only called once:

```csharp
[Collection("UmbracoTests")]
public class ExampleTests : IClassFixture<ExampleTests.Fixture>
{
    public class Fixture : IDisposable
    {
        private readonly UmbracoTestContext _context = new();

        public Fixture()
        {
            _context.SetupContentType("myContentTypeAlias");
        }

        public void Dispose() => _context.Dispose();
    }
}
```

Note that `UmbracoTestContext` itself cannot be used directly as the fixture type in this case because `SetupContentType` would be called once per test (each test gets a new instance of the test class) rather than once per class.

### One context per class (NUnit)

Use `[OneTimeSetUp]` and `[OneTimeTearDown]`:

```csharp
[TestFixture]
[NonParallelizable]
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

Two attributes are needed in each xUnit test project that contains tests using `UmbracoTestContext`.

#### 1. Disable parallelisation across all collections in the assembly

Add an `AssemblyInfo.cs` file to the project:

```csharp
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
```

This prevents test collections within the assembly from running in parallel with each other. Without it, different collections could still race on the static field even though each individual collection is serialised.

#### 2. Group tests into a non-parallel collection

Add a collection definition class to the project:

```csharp
[CollectionDefinition("UmbracoTests", DisableParallelization = true)]
public class UmbracoTestsCollection { }
```

Then apply `[Collection("UmbracoTests")]` to every test class in that project that uses `UmbracoTestContext`.

> **Note:** The remaining risk is cross-assembly: `dotnet test` runs multiple test projects in parallel by default, and this attribute has no effect on that. `IDisposable` on `UmbracoTestContext` minimises the window during which that cross-assembly race can occur.

### NUnit

Apply `[NonParallelizable]` to every `[TestFixture]` that uses `UmbracoTestContext`.
