# Configure the rich text editor

There are many [settings for the rich text editor](https://docs.umbraco.com/umbraco-cms/reference/configuration/richtexteditorsettings) in Umbraco. See [Configure a new Umbraco project](new-umbraco-project.md) for the recommended settings to apply for all projects using GOV.UK code. These recommended settings can be updated for your project if required.

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
