# Override property values in the block list

When using the block list editor, if you need to conditionally override some values in specific blocks you can do that.

For example, if you wanted to apply an additional CSS class to a grid row:

```csharp
/// Model
public class MyDocumentTypeViewModel
{
    public MyDocumentType Page { get; set; }

    public OverridableBlockListModel OverriddenBlocks { get; set; }
}

/// Controller
using ThePensionsRegulator.Umbraco.BlockLists;
using System.Linq;

viewModel.OverriddenBlocks = new OverridableBlockListModel(viewModel.Page.Blocks);
viewModel.OverriddenBlocks.First(x => x.Content.ContentType.Alias == "govukGridRow").Settings.OverrideValue("cssClassesForRow", "my-custom-class");

/// View
<partial name="GOVUK/BlockList" model="Model.OverriddenBlocks" />
```

See `BlockListController.cs` in `GovUk.Frontend.Umbraco.ExampleApp` for a more complete example.
