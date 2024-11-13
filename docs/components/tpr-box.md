# TPR box

In TPR pages you can add solid and bordered boxes surrounding content.

## HTML example (for ASP.NET applications)

```razor
<div class="tpr-box">
    <p class="govuk-body">Example content</p>
</div>

<div class="tpr-box tpr-box--bordered">
    <p class="govuk-body">Example content</p>
</div>
```

## Umbraco block list

Add `tpr-box` or `tpr-box tpr-box--bordered` in the 'CSS classes for row' property.

## Umbraco block grid

Add a 'Box' component anywhere in a block grid using the 'TPR block grid' data type.

![Add a box component](/docs/images/tpr-box-add.png)

You can set the style to either 'Solid' or 'Bordered' in the Settings of the 'Box' component. If you don't set it, it will default to 'Solid'.

![Box settings](/docs/images/tpr-box-settings.png)

A 'Box' will default to full-width. Place it inside a single-column layout such as 'Two thirds' to constrain it to a smaller width.

Add a 'Box' within a multi-column layout, or a multi-column layout within a 'Box' to create complex layouts. The following screenshot is from the [Umbraco example app](/docs/umbraco/run-example-application.md).

![Box examples in column layouts](/docs/images/tpr-box-examples.png)
