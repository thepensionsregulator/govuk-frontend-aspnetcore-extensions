# Support full-width content

Using the [GOV.UK grid system](https://design-system.service.gov.uk/styles/layout/) page content is normally contained within a page wrapper that limits the width of the content.

You may need to break out of this limited width container to render full-width content. For example, TPR designs can include a full-width box:

![A TPR box set to full-width](/docs/images/tpr-box-full-width.png)

To enable this support you need to make two changes to the consuming application:

1. Remove `<div class="govuk-width-container">` from your layout file, eg `Views/Shared/_Layout.cshtml`.
2. Enable `RenderWidthContainerForBlocks` in `Program.cs`:

   ```csharp
   // for GOV.UK applications...

   builder.Services.AddTprGovUkFrontendUmbraco(options => options.RenderWidthContainerForBlocks = true);

   // or, for TPR applications...

   builder.Services.AddTprFrontendUmbraco(options => options.RenderWidthContainerForBlocks = true);
   ```

This will cause `<div class="govuk-width-container">` to be rendered around each block instead.

When using TPR styles you can now add a 'TPR box' component to the root of a block grid using the 'TPR block grid' data type, and set its box style to 'Full width'.

![The full-width setting for a TPR box](/docs/images/tpr-box-full-width-setting.png)

Support for full-width boxes is implemented internally using an `IBlockViewInterceptor`, which can be used as a reference implementation for how to update the view. See [change how blocks are rendered](/docs/umbraco/block-rendering.md).

When using GOV.UK styles it is up to the developer to take advantage of this support as there are no components using it by default to break out of the limited width container.
