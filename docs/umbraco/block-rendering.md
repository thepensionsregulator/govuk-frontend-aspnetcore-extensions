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

## Beware merged rows and columns

By default, consecutive blocks with the same row and column configuration are merged into a single row and column. This reduces the amount of HTML and therefore the size of the response. It also ensures correct spacing between components, by allowing adjacent components to immediately follow each other in the HTML and thereby allow [CSS margin collapsing](https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_box_model/Mastering_margin_collapsing) to take effect.

You need to take care when implementing `IBlockViewInterceptor` because the decision of whether to merge with the next or previous row and column has already been taken. For example, if you set a row class it may not be rendered because the decision has already been taken that row classes were the same as the previous component and a new row is not required. A row (or other grid element) that is merged with the previous one may render a closing `</div>` but not an opening one, so if you change a property that controls whether to render an element you may create invalid HTML nesting.

**You should only implement `IBlockViewInterceptor` when no other solution is available.**

## Change default column classes

[The GOV.UK Design System recommends most page content is two-thirds wide](https://design-system.service.gov.uk/styles/layout/#screen-size), so we render most page components at full-width for mobile and tablet breakpoints and two-thirds wide for desktop (within `govuk-width-container`).

This is true except when a component is rendered as the child of another component, in which case it is assumed that the parent component is controlling the overall width and the child is set to take up the full width of the parent.

The 'Caption' and 'Page heading' components render at full-width by default, since it does not make sense for the larger text to wrap earlier. This is achieved by implementing `IDefaultColumnClassProvider`.

You can implement `IDefaultColumnClassProvider` to control the default column classes applied to your component when it is not the child of another component.
