# Filter the block list

When using the block list editor, if you need to conditionally hide some of the blocks you can supply a filter. For example, if you wanted to hide all radio buttons where the value is `not-relevant`:

```csharp
/// Model
public class MyDocumentTypeViewModel
{
    public MyDocumentType Page { get; set; }

    public OverridableBlockListModel OverriddenBlocks { get; set; }
}

/// Controller
using ThePensionsRegulator.Umbraco.BlockLists

viewModel.OverriddenBlocks = new OverridableBlockListModel(viewModel.Page.Blocks, model =>
    model.Where(block =>
        block.Content.ContentType.Alias != GovukRadio.ModelTypeAlias ||
        ((GovukRadio)block.Content).Value != "not-relevant")
    );

/// View
<partial name="GOVUK/BlockList" model="Model.OverriddenBlocks" />
```
