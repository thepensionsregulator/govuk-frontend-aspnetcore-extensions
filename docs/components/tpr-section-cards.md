# TPR section cards

In TPR pages you can add cards that can display a linked heading and description.

## Example

```razor
@addTagHelper *, ThePensionsRegulator.Frontend

<tpr-section-cards new-tab-text="(opens in a new tab)" card-titles-heading-level="3" class="custom-class" card-titles-heading-class="govuk-heading-s" navigation-aria-label="Section cards example">
    <tpr-section-card>
            <tpr-section-card-title href="/example" target="_self">Title of card</tpr-section-card-title>
            <tpr-section-card-content>Description of card</tpr-section-card-content>
    </tpr-section-card>
</tpr-section-cards>
```

## API

### `<tpr-section-cards>`

| Attribute    | Type     | Required | Description                                                                                                                                                           |
| ------------ | -------- | ---------|------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `new-tab-text`        | `string` | `True` | For title links that open in a new tab, this text will be appended to the end of title link to inform users that a new tab will open when the link is clicked. e.g. '(opens in a new tab)'                                                                                                                                                  |
| `card-titles-heading-level` | `string` | `False`   | Sets the heading level for all the card titles. Default is `2`.
| `card-titles-heading-class` | `string` | `False`   | Sets the heading class for all the card titles. Default is `govuk-heading-m`.
| `navigation-aria-label`     | `string` | `False`   | Sets the `aria-label` attribute on the outermost `nav` element. If left empty, no aria-label attribute will be rendered.                                                                 |

### `<tpr-section-card>`

Creates a single section card. Must be inside a `<tpr-section-cards>` element.

### `<tpr-section-card-title>`

Configures the title and link for a single section card.

| Attribute    | Type     | Description                                                                                                                                                                       |
| ------------ | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `href`       | `string` | Sets the URL the title links to.                                                                                                                                                  |
| `target`     | `string` | Sets the target attribute on the link. Rarely needed because [links should not open in a new tab](https://design-system.service.gov.uk/styles/links/#opening-links-in-a-new-tab). |
| `allow-html` | `bool`   | Sets whether to render HTML without escaping. Default is `false`.                                                                                                                 |

Must be inside a `<tpr-section-card>` element.

### `<tpr-section-card-content>`

Configures the content for a single section card.

| Attribute    | Type   | Description                                                       |
| ------------ | ------ | ----------------------------------------------------------------- |
| `allow-html` | `bool` | Sets whether to render HTML without escaping. Default is `false`. |

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
- **Card titles heading level** - Sets the heading level for all the card titles. Default is `2`.
- **CSS classes** - Classes to add to the outermost `nav` element.

![Click the cog icon to get to the settings of the Section cards block](/docs/images/tpr-section-cards-settings-cog.png)
![Settings of a Section cards block](/docs/images/tpr-section-cards-settings-options.png)
