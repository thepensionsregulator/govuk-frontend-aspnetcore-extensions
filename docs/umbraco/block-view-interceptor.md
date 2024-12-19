# Change how blocks are rendered

You should render a block grid or block list using one of the following partial views:

    ```razor
    <partial name="GOVUK/BlockGrid" model="Model.MyBlockGrid" />
    <partial name="GOVUK/BlockList" model="Model.MyBlockList" />
    ```

Each block may be rendered inside several container elements from the [GOV.UK grid system](https://design-system.service.gov.uk/styles/layout/).

The possible container elements are:

- A width container with the class `govuk-width-container`
- A row and column using classes `govuk-grid-row` and `govuk-grid-column-*`
- A fieldset error container using classes `govuk-form-group` and `govuk-form-group--error`

You can change which container elements are rendered by registering a class that implements `IBlockViewInterceptor`. This interface has one method `InterceptBlockView(BlockViewModel blockViewModel)`.

By modifying the `blockViewModel` for any block you can change which container elements are rendered with the block.
