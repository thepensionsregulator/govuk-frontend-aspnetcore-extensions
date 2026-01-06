# Filter the block list

When using the block grid or block list editor, if you need to conditionally hide some of the blocks you can supply a filter. For example, if you wanted to hide all radio buttons where the value is `not-relevant`:

```csharp
/// Controller
using ThePensionsRegulator.Umbraco.Core.Blocks

var viewModel = new MyDocumentType(CurrentPage, null);

viewModel.Blocks!.Filter = block =>
        block.Content.ContentType.Alias != GovukRadio.ModelTypeAlias ||
        ((GovukRadio)block.Content).Value != "not-relevant";

/// View
<partial name="GOVUK/BlockList" model="Model.Blocks" />
```

If you are not using the `GOVUK/BlockGrid` or `GOVUK/BlockList` partial view, your partial view will need to render the blocks returned by the `FilteredBlocks()` method.

```csharp
foreach (var block in Model.FilteredBlocks())
{
    @await Html.PartialAsync(block.Content.ContentType.Alias, block)
}
```
