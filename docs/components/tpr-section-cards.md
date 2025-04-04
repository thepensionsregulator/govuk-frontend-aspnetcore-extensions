# TPR section cards

In TPR pages you can add cards that can display a linked heading and description.

## Example

```razor
<tpr-section-cards>
    <tpr-section-card>
            <tpr-section-card-title href="/example" target="_self">Title of card</tpr-section-card-title>
            <tpr-section-card-content>Description of card</tpr-section-card-content>
    </tpr-section-card>
</tpr-section-cards>
```

## API

### `<tpr-section-cards>`

_Required_

### `<tpr-section-card>`

Creates a single section card. Must be inside a `<tpr-section-cards>` element.

### `<tpr-section-card-title>`

Configures the title and link for a single section card.

| Attribute | Type     | Description                                                                                                                                                                       |
| --------- | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `href`    | `string` | Sets the URL the title links to.                                                                                                                                                  |
| `target`  | `string` | Sets the target attribute on the link. Rarely needed because [links should not open in a new tab](https://design-system.service.gov.uk/styles/links/#opening-links-in-a-new-tab). |

Must be inside a `<tpr-section-card>` element.

### `<tpr-section-card-content>`

Configures the content for a single section card. HTML is allowed.

Must be inside a `<tpr-section-card>` element.

## Umbraco

Add a 'Section cards' component anywhere in a block grid using the 'TPR block grid' data type. For best results, add the component to a full width container.

![Add a Section cards component](/docs/images/tpr-section-cards-add.png)

You can then add cards to the `Cards` property.

![Add cards](/docs/images/tpr-section-cards-add-content.png)

You will have two options:

- **Section cards for child pages** creates a card for every child page of the current page, unless the child page's `umbracoNaviHide` property is set to `true`.
- **Section card** adds a single editable card to the list.

![Add content](/docs/images/tpr-section-cards-add-content-options.png)

When you add a 'Section card' block, you will have to fill in the link and description fields.

![Add content](/docs/images/tpr-section-cards-add-content-card.png)

## Settings

The section cards block has the following properties for configuration:

- **Title field name** - is the alias of the property that contains the title for child items. This defaults to the node name.
- **Description field name** - is the alias of the property that contains the description for child items. This defaults to `description`.
- **CSS classes** - Classes to add to the outermost `nav` element.

![Click the cog icon to get to the settings of the Section cards block](/docs/images/tpr-section-cards-settings-cog.png)
![Settings of a Section cards block](/docs/images/tpr-section-cards-settings-options.png)
