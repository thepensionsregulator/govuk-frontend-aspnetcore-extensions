# Date input

For examples see [ASP.NET syntax for the Date input component](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/date-input.md).

## Validating date ranges

Use the `[DateRange]` attribute to validate dates using the Date Input component.

For an example of this in use, see the 'Date input' page in either the ASP.NET and Umbraco example apps.

## Umbraco

You can add a date input component to a block grid or block list in Umbraco. For an example of this component in use, see the 'Date input' page in the Umbraco example app.

Error messages for date input components using `[Required]` or `[DateRange]` are configurable in Umbraco on the settings for the date input component.

Error messages for parsing dates are configurable by creating Umbraco dictionary entries with the following names:

- `Date must be a real date`
- `Date must include a day`
- `Date must include a day and month`
- `Date must include a day and year`
- `Date must include a month`
- `Date must include a month and year`
- `Date must include a year`

In each case you should include `{0}` in the error message, which is replaced with the name of the field on the view model.

To use a display name instead of the name of the field on the view model, configure the display name in the settings of the date input component. You do not need a `[DisplayName]` attribute.

### Month and year only

You can choose to not show the day field on the settings for the date input component. Only the month and year fields will show, and the submitted date will be the 1st of the submitted month.

![Month and year only](/docs/images/date-input-month-year.png)

This is not available outside Umbraco because the default `DateInputModelBinder` expects the day to be present. Umbraco is using a custom `UmbracoDateInputModelBinder`.
