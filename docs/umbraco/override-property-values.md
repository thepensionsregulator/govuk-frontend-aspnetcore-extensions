# Override property values in the block list

When using the block list editor, if you need to conditionally override some values in specific blocks you can do that.

For example, if you wanted to apply an additional CSS class to a grid row:

```csharp
/// Controller
using ThePensionsRegulator.Umbraco.BlockLists;
using System.Linq;

var viewModel = new MyDocumentType(CurrentPage, null);

viewModel.Blocks.First(x => x.Content.ContentType.Alias == "govukGridRow").Settings.OverrideValue("cssClassesForRow", "my-custom-class");

/// View
<partial name="GOVUK/BlockList" model="Model.Blocks" />
```

See `BlockListController.cs` in `GovUk.Frontend.Umbraco.ExampleApp` for a more complete example.
