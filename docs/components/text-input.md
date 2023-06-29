# Text input

For examples see [ASP.NET syntax for the Text input component](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/text-input.md).

## Validating UK postcodes

Use the `[UkPostcode]` attribute to validate postcodes using the Text input component. This uses a regular expression rather than a check against a postcode database, so some false positives may be allowed.

For an example of this in use, see the 'Text input' page in either the ASP.NET and Umbraco example apps.

## Umbraco

You can add a text input component to a block list in Umbraco. For examples of this component in use, see the 'Text input' page in the Umbraco example app.

Error messages for text input components are configurable in Umbraco on the settings for the text input component.

For a `[UkPostcode]` attribute set the 'Text input width' setting to 'small' and use the 'Pattern' error message.

![Settings for a postcode using a text input component](/docs/images/uk-postcode-settings.png)
