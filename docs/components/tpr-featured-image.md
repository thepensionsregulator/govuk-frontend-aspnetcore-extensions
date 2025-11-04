# TPR featured image 

In TPR pages you can add an image with content below or next to it.

## Example

```razor
@addTagHelper *, ThePensionsRegulator.Frontend

<tpr-featured-image image-url="IMAGE_URL" image-alt="IMAGE_TEXT" horizontal="false" decorative-image="false">
    <h2>You can add your content here</h2>
    <p>Here is some example content</p>
</tpr-featured-image>
```

## API

### `<tpr-featured-image>`

| Attribute    | Type     | Required | Description                                                                                                                                                           |
| ------------ | -------- | ---------|------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `image-url`        | `string` | `True` | The url to the image.                                                                                                                                                  |
| `image-alt` | `string` | `True`   | You have to set the alt text property. If the image is decorative, please use the decorative-image property.
| `decorative-image` | `bool` | `False`   | Sets the alt="" to mark as a decorative image for screen readers.
| `horizontal`     | `bool` | `False`   | By default, the content is displayed below the image which is suited for narrow columns. You can change that by setting this value to true. This works best for 2/3rd and full width columns.                                                                 |


## Umbraco

Add a 'Featured image' component anywhere in a block grid using the 'TPR block grid' data type. For best results, add the component to a full width container.

![Add a Featured image component](/docs/images/tpr-featured-image-block.png)

You will see the following fields to fill in:

![Add a Featured image fields](/docs/images/tpr-featured-image-content.png)

You will have the following fields that you can fill in. The image is self explanatory. When clicking the Add content button, you will see the following options: 

![Featured image content options](/docs/images/tpr-featured-image-content-options.png)

The text field allows you to add text content. Where as the Link, styled as a button option allows you to add a button with a link.

## Settings

The featured image settings has the following properties for configuration:

- **Horizontal** - When this is false (default), the content will display below the image. If true, it will display to the right of the image.
- **Decorative Image** - When this is set to true, the image alt tag outputs as alt="".
- **CSS classes** - Classes to add to the outermost div element.

![Settings of Featured image](/docs/images/tpr-featured-image-settings-options.png)
