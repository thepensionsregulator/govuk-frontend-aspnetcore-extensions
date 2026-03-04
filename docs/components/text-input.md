# Text input

For examples see [ASP.NET syntax for the Text input component](https://github.com/x-govuk/govuk-frontend-aspnetcore/blob/main/docs/components/text-input.md).

## Umbraco

You can add a text input component to a block grid or block list in Umbraco. For examples of this component in use, see the 'Text input' page in the Umbraco example app.

Error messages for text input components are configurable in Umbraco on the settings for the text input component.

## Validate UK postcodes

Use the `[UkPostcode]` attribute to validate postcodes using the Text input component. This uses a regular expression rather than a check against a postcode database, so some false positives may be allowed.

For an example of this in use, see the 'Text input' page in either the ASP.NET and Umbraco example apps.

For a `[UkPostcode]` attribute in Umbraco set the 'Text input width' setting to 'small' and use the 'Pattern' error message.

![Settings for a postcode using a text input component](/docs/images/uk-postcode-settings.png)

## Validate Companies House company numbers

Use the `[CompaniesHouseCompanyNumber]` attribute to validate company numbers using the Text input component. This uses a regular expression rather than a check against a database, so some false positives may be allowed.

For an example of this in use, see the 'Text input' page in either the ASP.NET and Umbraco example apps.

For a `[CompaniesHouseCompanyNumber]` attribute in Umbraco set the 'Text input width' setting to 'small' and use the 'Pattern' error message.

![Settings for a company number using a text input component](/docs/images/company-number-settings.png)

## Validate registered charity numbers

Use the `[RegisteredCharityNumber]` attribute to validate registered charity numbers using the Text input component. This uses a regular expression rather than a check against a database, so some false positives may be allowed.

For an example of this in use, see the 'Text input' page in either the ASP.NET and Umbraco example apps.

For a `[RegisteredCharityNumber]` attribute in Umbraco set the 'Text input width' setting to 'small' and use the 'Pattern' error message.

![Settings for a registered charity number using a text input component](/docs/images/registered-charity-number-settings.png)
