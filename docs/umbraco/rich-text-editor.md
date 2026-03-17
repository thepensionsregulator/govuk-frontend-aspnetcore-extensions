# Configure the rich text editor

There are many [settings for the rich text editor](https://github.com/ProWorksCorporation/TinyMCE-Umbraco) in Umbraco. See [Configure a new Umbraco project](new-umbraco-project-govuk.md) for the recommended settings to apply for all projects using GOV.UK code. These recommended settings can be updated for your project if required.

Data types in this repo use the TinyMCE editor rather than the default TipTap editor. This is due to [limited support for list styles in TipTap](https://github.com/umbraco/Umbraco-CMS/issues/21146) and the inability to add custom formats easily to already-defined data types in the rich text editor.

## Add custom formats to the rich text editor

The 'Formats' dropdown in the rich text editor lets you apply custom CSS classes to elements within the editor. The most common formats for GOV.UK and The Pensions Regulator (TPR) projects are pre-configured, but you can add others that are specific to your project.

Additional formats are made available if they:

- are defined in `/wwwroot/css/site.css` (you can [use SASS for CSS](../aspnet/sass.md) to generate `site.css`)
- have a display name to show in the 'Formats' dropdown, defined in an `umb-name` comment above the selector
- have at least one CSS property defined

In this example 'Custom format' will be displayed in the 'Formats' dropdown and will apply the `.custom-format` class and convert the selected element to be a paragraph, if it is not already.

```css
/**umb_name:Custom format*/
p.custom-format {
  position: static;
}
```

## Create a new rich text editor data type

It is often useful to create a new rich text editor data type, so that only the formatting options relevant to the specific context can be offered. For example, you might want to offer only bold text and bulleted lists rather than all of the possible formatting options.

When creating a new TinyMCE rich text editor data type in the Umbraco backoffice, add the following stylesheets:

- `/govuk-umbraco-backoffice.css`
- `/site.css`

This project adds the ability to [format Umbraco property values](./format-property-values.md), including rich text editor values, at the time they are rendered on the page.
