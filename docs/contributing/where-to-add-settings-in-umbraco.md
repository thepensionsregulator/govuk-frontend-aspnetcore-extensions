# Where to add settings in Umbraco

When you need to make something configurable in Umbraco, there are several different ways you can make the setting available.

## Is the setting specific to one component?

Components are implemented as blocks, and every block should have an element type for its content and another for its settings. Add the new setting to the settings element type for the component.

In most cases add your setting to the 'Settings' group on the element type. If you have several related properties you can create a new group.

Use the setting in the Razor view for the component.

## Is the setting likely to be reused on multiple components, or would a C# interface be useful?

'CSS classes' is a good example of this, which accepts classes that should always be applied to the outermost HTML element of a component.

This is the same as creating a setting for one component (above), except that you should create a new composition and add the setting property there. Add the composition to the settings element types for any components that need to use it.

Models Builder will create a C# interface for each composition, so use a composition if it would be useful to reference the property in C# without referencing the model for a specific component.

Use the setting in the Razor view for the components.

## Is the setting a simple text string that should be localised if the site is multi-lingual?

Allow users to create a dictionary entry with a well-known key that will override your default value. Add a constant to `DictionaryConstants.cs` with the key for your dictionary entry, and add this feature to the documentation for the component.

For example, you can [change the text used for statuses on the 'Task list' component](/docs/components/task-list.md) using dictionary entries.

Use the dictionary entry in the Razor view for the component, and provide a default value to use if the dictionary entry is not present.

## Is the setting applicable sitewide or for a whole page?

Create a new composition in the 'Page settings' folder and add the setting property there. Don't add the composition to any document types, but instead document that the composition is available for consuming sites to use.

In most cases add your setting to the 'Settings' tab on the composition. If you have several related properties you can create a new tab, or a new group on the 'Settings' tab.

Add a static helper method to `GovUkPageSetting.cs` (or create/update `TprPageSetting.cs` for a TPR setting) which accesses your property and, if appropriate, specifies a fallback strategy. For a sitewide setting use `Fallback.ToAncestors`, and then the composition can be added to a 'Home' document type and be available automatically on any page.

Use the new setting in any code where it's relevant, but anticipate that it may not be present.
