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

## Create a new rich text editor data type

It is often useful to create a new rich text editor data type, so that only the formatting options relevant to the specific context can be offered. For example, you might want to offer only bold text and bulleted lists rather than all of the possible formatting options.

This project adds the ability to [format Umbraco property values](./format-property-values.md) including rich text editor values. This changes the process to add a new rich text editor data type from the Umbraco default.

1. Create a new class which inherits from `Umbraco.Cms.Core.PropertyEditors.RichTextPropertyEditor`. This should not change anything from the base class, except the `alias` and `name` in the `[DataEditor]` attribute. See `GovUkInlineRichTextPropertyEditor` for an example. This class will be discovered automatically by Umbraco.
2. Create a new class which implements `ThePensionsRegulator.Umbraco.PropertyEditors.IRichTextPropertyEditorAliasProvider` and returns the alias of your new property editor. Register your implementation with dependency injection. See `GovUkRichTextPropertyEditorAliasProvider` for an example.
3. Create a new data type in the Umbraco backoffice which uses your new property editor.
