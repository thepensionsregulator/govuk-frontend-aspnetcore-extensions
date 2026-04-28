# Address lookup

The address lookup component provides a multi-step postcode lookup flow as a progressive enhancement over standard address input fields. It wraps address inputs in a `<fieldset>` and, when JavaScript is available, replaces them with an interactive state machine that guides the user through postcode search, address selection, manual entry (UK and international), and address confirmation.

Without JavaScript, the original address input fields remain visible and usable.

## Example

```razor
@addTagHelper *, ThePensionsRegulator.Frontend
@model AddressLookupViewModel
@inject ITprAddressLookupEndpointUrlProvider urlProvider
@inject ITprCountryRepository countryRepository

<form method="post" novalidate>
    <tpr-address-lookup role="Primary"
                        data-address-lookup-search-url="@urlProvider.GetAddressLookupSearchEndpoint()"
                        data-address-lookup-id-url="@urlProvider.GetAddressLookupIdEndpoint()"
                        data-address-lookup-postcode-label="Postcode"
                        data-address-lookup-postcode-required="Enter a postcode"
                        data-address-lookup-postcode-maxlength="Postcode must be 20 characters or less"
                        data-address-lookup-postcode-pattern="Enter a valid UK postcode"
                        data-address-lookup-county-label="County (optional)"
                        data-address-lookup-county-maxlength="County must be 100 characters or less"
                        data-address-lookup-building-label="Building or house number (optional)"
                        data-address-lookup-building-maxlength="Building or house number must be 100 characters or less"
                        data-address-lookup-select-address-label="Select address"
                        data-address-lookup-select-address-required="Select an address">
        <tpr-address-lookup-legend>Enter your address</tpr-address-lookup-legend>
        <tpr-address-lookup-hint id="address-lookup-hint">This should be your primary address</tpr-address-lookup-hint>

        <govuk-client-side-validation error-message-required="Enter address line 1"
                                      error-message-maxlength="Address line 1 must be 200 characters or less">
            <govuk-input for="AddressLine1" autocomplete="address-line1"
                         input-attributes='@(new Dictionary<string, string?>{{"data-address-lookup", "address-line-1"}})'>
                <govuk-input-label>Address line 1</govuk-input-label>
                @if (ViewContext.ModelState.ContainsKey("AddressLine1") && ViewContext.ModelState["AddressLine1"]!.Errors.Count > 0)
                {
                    <govuk-input-error-message>@ViewContext.ModelState["AddressLine1"]!.Errors[0].ErrorMessage</govuk-input-error-message>
                }
            </govuk-input>
        </govuk-client-side-validation>

        <govuk-client-side-validation error-message-maxlength="Address line 2 must be 200 characters or less">
            <govuk-input for="AddressLine2" autocomplete="address-line2"
                         input-attributes='@(new Dictionary<string, string?>{{"data-address-lookup", "address-line-2"}})'>
                <govuk-input-label>Address line 2 (optional)</govuk-input-label>
                @if (ViewContext.ModelState.ContainsKey("AddressLine2") && ViewContext.ModelState["AddressLine2"]!.Errors.Count > 0)
                {
                    <govuk-input-error-message>@ViewContext.ModelState["AddressLine2"]!.Errors[0].ErrorMessage</govuk-input-error-message>
                }
            </govuk-input>
        </govuk-client-side-validation>

        <govuk-client-side-validation error-message-required="Enter a town or city"
                                      error-message-maxlength="Town or city must be 100 characters or less">
            <govuk-input for="TownOrCity" input-class="govuk-input--width-20" autocomplete="address-level2"
                         input-attributes='@(new Dictionary<string, string?>{{"data-address-lookup", "town-or-city"}})'>
                <govuk-input-label>Town or city</govuk-input-label>
                @if (ViewContext.ModelState.ContainsKey("TownOrCity") && ViewContext.ModelState["TownOrCity"]!.Errors.Count > 0)
                {
                    <govuk-input-error-message>@ViewContext.ModelState["TownOrCity"]!.Errors[0].ErrorMessage</govuk-input-error-message>
                }
            </govuk-input>
        </govuk-client-side-validation>

        <govuk-client-side-validation error-message-maxlength="Province, region or state must be 100 characters or less">
            <govuk-input for="County" input-class="govuk-input--width-20" autocomplete="address-level3"
                         input-attributes='@(new Dictionary<string, string?>{{"data-address-lookup", "region-international"}})'>
                <govuk-input-label>Province/ region/ state (optional)</govuk-input-label>
                @if (ViewContext.ModelState.ContainsKey("County") && ViewContext.ModelState["County"]!.Errors.Count > 0)
                {
                    <govuk-input-error-message>@ViewContext.ModelState["County"]!.Errors[0].ErrorMessage</govuk-input-error-message>
                }
            </govuk-input>
        </govuk-client-side-validation>

        <govuk-client-side-validation error-message-required="Select a country">
            <govuk-select for="Country" input-class="govuk-input--width-20" select-autocomplete="country"
                         select-data-address-lookup="country">
                <govuk-select-label>Country</govuk-select-label>
                @if (ViewContext.ModelState.ContainsKey("Country") && ViewContext.ModelState["Country"]!.Errors.Count > 0)
                {
                    <govuk-select-error-message>@ViewContext.ModelState["Country"]!.Errors[0].ErrorMessage</govuk-select-error-message>
                }
                @{
                    var countries = countryRepository.GetCountries();
                    var selectEmpty = !countries.ContainsKey(ViewContext.ModelState["Country"]?.AttemptedValue?.ToString() ?? string.Empty);
                    <govuk-select-item selected="@(selectEmpty)"></govuk-select-item>
                    foreach (var country in countries)
                    {
                        var selected = country.Key.ToString() == ViewContext.ModelState["Country"]?.AttemptedValue;
                        <govuk-select-item value="@country.Value" selected="@(selected)">@country.Key</govuk-select-item>
                    }
                }
            </govuk-select>
        </govuk-client-side-validation>

        <govuk-client-side-validation error-message-required="Enter a postal code or zip code"
                                      error-message-maxlength="Postal code or zip code must be 20 characters or less">
            <govuk-input for="Postcode" input-class="govuk-input--width-10" autocomplete="postal-code"
                         input-attributes='@(new Dictionary<string, string?>{{"data-address-lookup", "postcode-international"}})'>
                <govuk-input-label>Postal code/ zip code</govuk-input-label>
                @if (ViewContext.ModelState.ContainsKey("Postcode") && ViewContext.ModelState["Postcode"]!.Errors.Count > 0)
                {
                    <govuk-input-error-message>@ViewContext.ModelState["Postcode"]!.Errors[0].ErrorMessage</govuk-input-error-message>
                }
            </govuk-input>
        </govuk-client-side-validation>

        <input type="hidden" id="UPRN" name="UPRN" value="@Model?.UPRN" data-address-lookup="UPRN" />
        <input type="hidden" id="CountryCode" name="CountryCode" value="@Model?.CountryCode" data-address-lookup="country-code" />
    </tpr-address-lookup>

    <govuk-button type="submit">Submit</govuk-button>
</form>

@section scripts {
    <partial name="GOVUK/Validation" />
    <script src="~/_Content/ThePensionsRegulator.Frontend/tpr/address-lookup/index.js" type="module"></script>
}
```

### Primary and secondary addresses

You can render two address lookups on the same page — for example, a shipping and billing address. The second address can have a "same as primary" checkbox so the user can copy the confirmed primary address.

```razor
<tpr-address-lookup
    data-address-lookup-search-url="/api/address/search"
    data-address-lookup-id-url="/api/address/byid">
    <tpr-address-lookup-legend>Enter shipping address</tpr-address-lookup-legend>
    <!-- shipping address inputs -->
</tpr-address-lookup>

<tpr-address-lookup
    role="Secondary"
    same-as-primary-checkbox-label="Billing address is the same as shipping address"
    data-address-lookup-search-url="/api/address/search"
    data-address-lookup-id-url="/api/address/byid">
    <tpr-address-lookup-legend>Enter billing address</tpr-address-lookup-legend>
    <!-- billing address inputs -->
</tpr-address-lookup>
```

When `role="Secondary"`, the component renders a checkbox with the label from `same-as-primary-checkbox-label`. If the user checks it, the secondary address is automatically set to the confirmed primary address. The `same-as-primary-checkbox-label` attribute is required when `role` is `Secondary`.

## API

### `<tpr-address-lookup>`

_Required_

| Attribute                        | Type                | Default   | Description                                                                                            |
| -------------------------------- | ------------------- | --------- | ------------------------------------------------------------------------------------------------------ |
| `data-address-lookup-search-url` | `string`            |           | API endpoint for postcode search. Receives `postcode` query parameter.                                 |
| `data-address-lookup-id-url`     | `string`            |           | API endpoint for looking up a single address by UPRN. Receives `uprn` query parameter.                 |
| `role`                           | `AddressLookupRole` | `Primary` | The role of this address lookup. Set to `Secondary` for a dependent address that can copy the primary. |
| `same-as-primary-checkbox-label` | `string`            |           | Label for the "same as primary" checkbox. **Required** when `role` is `Secondary`.                     |
| `described-by`                   | `string`            |           | ID(s) to set on the fieldset's `aria-describedby` attribute.                                           |

### `<tpr-address-lookup-legend>`

_Required._ Must be a direct child of `<tpr-address-lookup>`. Only one legend is permitted.

### `<tpr-address-lookup-hint>`

Must be a direct child of `<tpr-address-lookup>`. Only one hint is permitted.

### Child inputs

Each `<input>` inside the component must have a `data-address-lookup` attribute set to one of the following values so the JavaScript can capture and restore field values across state transitions:

| `data-address-lookup` value | Description                                                |
| --------------------------- | ---------------------------------------------------------- |
| `address-line-1`            | Address line 1                                             |
| `address-line-2`            | Address line 2                                             |
| `town-or-city`              | Town or city                                               |
| `county`                    | County (UK addresses)                                      |
| `region-international`      | Province, region, or state (international addresses)       |
| `country`                   | Country                                                    |
| `postcode`                  | Postcode (UK addresses)                                    |
| `postcode-international`    | Postal code or zip code (international addresses)          |
| `UPRN`                      | Unique Property Reference Number (hidden)                  |
| `country-code`              | Country code (hidden)                                      |

## Customising labels and error messages

Labels and validation error messages can be customised in three ways, listed here in priority order:

1. **Server-rendered inputs** — The component captures the label text, required message, max length, and pattern message from any `<input>` or `<select>` elements rendered inside the `<tpr-address-lookup>` element. These are used when JavaScript rebuilds the component.

2. **`data-*` attributes on the container** — For fields that only exist in JavaScript (building name, UK postcode for the search view, county, and select address), you can set `data-address-lookup-{field}-{property}` attributes on the `<tpr-address-lookup>` element. For example, `data-address-lookup-postcode-label` or `data-address-lookup-building-maxlength`.

3. **Built-in defaults** — If no value is provided by either of the above methods, the component falls back to default values defined in `config.js`.

### `data-*` attributes for JavaScript-only fields

The following `data-*` attributes can be set on the `<tpr-address-lookup>` element to customise fields that are created by JavaScript and do not have server-rendered inputs:

| Attribute                                     | Description                                         |
| --------------------------------------------- | --------------------------------------------------- |
| `data-address-lookup-postcode-label`          | Label for the postcode field in the search view.    |
| `data-address-lookup-postcode-required`       | Error message when the postcode is missing.         |
| `data-address-lookup-postcode-maxlength`      | Error message when the postcode exceeds max length. |
| `data-address-lookup-postcode-pattern`        | Error message when the postcode format is invalid.  |
| `data-address-lookup-building-label`          | Label for the building name/number field.           |
| `data-address-lookup-building-maxlength`      | Error message when building name exceeds max length.|
| `data-address-lookup-county-label`            | Label for the county field.                         |
| `data-address-lookup-county-maxlength`        | Error message when county exceeds max length.       |
| `data-address-lookup-select-address-label`    | Label for the address select dropdown.              |
| `data-address-lookup-select-address-required` | Error message when no address is selected.          |

## View model

The backing view model should have properties for each address field. Apply standard data annotations for server-side validation.

```csharp
public class AddressViewModel
{
    [Required(ErrorMessage = "Enter address line 1")]
    [MaxLength(500, ErrorMessage = "Address line 1 must not exceed 500 characters")]
    public string? AddressLine1 { get; set; }

    [MaxLength(500, ErrorMessage = "Address line 2 must not exceed 500 characters")]
    public string? AddressLine2 { get; set; }

    [Required(ErrorMessage = "Enter a town or city")]
    [MaxLength(500, ErrorMessage = "Town or city must not exceed 500 characters")]
    public string? TownOrCity { get; set; }

    [MaxLength(500, ErrorMessage = "County must not exceed 500 characters")]
    public string? County { get; set; }

    [Required(ErrorMessage = "Enter a country")]
    public string? Country { get; set; }

    public int? CountryCode { get; set; }

    [Required(ErrorMessage = "Enter a postcode")]
    [MaxLength(20, ErrorMessage = "Postcode must not exceed 20 characters")]
    public string? Postcode { get; set; }

    public string? UPRN { get; set; }
}
```

## State machine

The JavaScript component is driven by a state machine with the following states:

| State                        | Description                                                                                         |
| ---------------------------- | --------------------------------------------------------------------------------------------------- |
| `SEARCH`                     | User enters a building name/number and postcode, then clicks "Find address".                        |
| `SELECT`                     | A dropdown of matching addresses is shown. User picks one and clicks "Confirm address".             |
| `CONFIRMED`                  | The confirmed address is displayed as text. Hidden inputs preserve the values for form submission.  |
| `MANUAL_UK_ENTRY`            | Manual entry form for UK addresses (line 1, line 2, town, county, postcode).                        |
| `MANUAL_INTERNATIONAL_ENTRY` | Manual entry form for international addresses (line 1, line 2, town, region, country, postal code). |

### State transitions

```
SEARCH ──────────────→ SELECT ──────────────→ CONFIRMED
  │                      │  ↑                     ↕ (self / edit)
  │                      ↓  │                     ↓
  │                 MANUAL_UK_ENTRY ──→ CONFIRMED  SEARCH
  ↓
MANUAL_INTERNATIONAL_ENTRY ──────────→ CONFIRMED
```

- From **SEARCH**: the user can proceed to **SELECT** (when results are found), **MANUAL_INTERNATIONAL_ENTRY** (via "Enter an international address" link), or **CONFIRMED** (if inputs already contain a valid address on load).
- From **SELECT**: the user can confirm an address (**CONFIRMED**), enter an address not on the list (**MANUAL_UK_ENTRY**), or return to **SEARCH**.
- From **CONFIRMED**: the user can edit the address (back to **SEARCH**).
- From **MANUAL_UK_ENTRY** / **MANUAL_INTERNATIONAL_ENTRY**: the user can confirm (**CONFIRMED**) or return to **SEARCH**.

## Client-side validation

The component uses jQuery Validation Unobtrusive and GOV.UK-styled error messages. Validation rules include:

| Field               | Rules                          |
| ------------------- | ------------------------------ |
| Postcode (search)   | Required, UK postcode pattern  |
| Building name       | Max length 100                 |
| Address line 1      | Required, max length 500       |
| Address line 2      | Max length 500                 |
| Town or city        | Required, max length 500       |
| County / Region     | Max length 500                 |
| Country             | Required, max length 500       |
| Postal code (intl.) | Required, max length 20        |
| Address select      | Required ("Select an address") |

On form submission, the component checks that all address lookups are in the `CONFIRMED` state. If not, it prevents submission and displays an appropriate error.

## API endpoints

The component calls two endpoints configured via `data-address-lookup-search-url` and `data-address-lookup-id-url`.

### Search endpoint

**Request:** `GET {search-url}?postcode={postcode}`

**Response:** An object with a `results` array, where each result contains a `DPA` (Delivery Point Address) object:

```json
{
  "results": [
    {
      "DPA": {
        "UPRN": "100023336956",
        "ADDRESS": "1, EXAMPLE STREET, LONDON, SW1A 1AA",
        "BUILDING_NUMBER": "1",
        "BUILDING_NAME": "",
        "THOROUGHFARE_NAME": "EXAMPLE STREET",
        "POST_TOWN": "LONDON",
        "POSTCODE": "SW1A 1AA",
        "ORGANISATION_NAME": ""
      }
    }
  ]
}
```

If a building name or number is provided, results are filtered client-side: 
- numeric values (e.g `1`) match `BUILDING_NUMBER` exactly
- non-numeric values match `BUILDING_NAME` or `SUB_BUILDING_NAME` (case-insensitive substring). This includes values with suffixes (e.g `12a`), ranges (e.g. `110-112`) or prefixes (e.g `Flat 8`) .

### Address-by-ID endpoint

**Request:** `GET {id-url}?uprn={uprn}`

**Response:** Same structure as the search endpoint, returning the single matching address.

## Configuration

### JavaScript

Include the JavaScript module in any view that uses this component:

```html
<script
  src="_Content/ThePensionsRegulator.Frontend/tpr/address-lookup/index.js"
  type="module"
></script>
```

Also include the GOV.UK client-side validation partial:

```razor
<partial name="GOVUK/Validation" />
```

### appsettings.json

When using the Umbraco integration (`ThePensionsRegulator.Frontend.Umbraco`), configure the API endpoints in `appsettings.json`:

```json
{
  "AddressLookup": {
    "SearchEndpoint": "/api/address/search",
    "AddressByIdEndpoint": "/api/address/byid"
  }
}
```

These values are read by `TprAddressLookupEndpointUrlProvider`, which implements `ITprAddressLookupEndpointUrlProvider`. You can register a custom implementation if you need to resolve endpoints differently.

### Country repository

You must register an implementation of `ITprCountryRepository` in the dependency injection container. The view injects this service and serialises the result of `GetCountries()` into the `data-address-lookup-countries` attribute on the component element. The JavaScript reads this attribute to populate the country dropdown in the manual international address entry view.

```csharp
public interface ITprCountryRepository
{
    /// <summary>
    /// Returns a dictionary of country names to country codes.
    /// </summary>
    IDictionary<string, int> GetCountries();
}
```

Register your implementation at startup:

```csharp
services.AddSingleton<ITprCountryRepository, MyCountryRepository>();
```

## Generated HTML

The tag helper produces the following server-rendered HTML. The JavaScript then replaces the fieldset contents on page load.

```html
<div
  class="tpr-address-lookup"
  data-address-lookup-role="primary"
  data-address-lookup-search-url="/api/address/search"
  data-address-lookup-id-url="/api/address/byid"
>
  <fieldset class="govuk-fieldset">
    <legend class="govuk-fieldset__legend govuk-fieldset__legend--for-fieldset">
      Enter your address
    </legend>
    <!-- child input elements -->
  </fieldset>
</div>
```

For a secondary address, additional attributes are rendered:

```html
<div
  class="tpr-address-lookup"
  data-address-lookup-role="secondary"
  data-address-lookup-same-as-primary-checkbox-label="Billing address is the same as shipping address"
  data-address-lookup-search-url="/api/address/search"
  data-address-lookup-id-url="/api/address/byid"
>
  <!-- ... -->
</div>
```

## JavaScript architecture

The client-side code is structured as ES modules:

| Module                  | Responsibility                                                                         |
| ----------------------- | -------------------------------------------------------------------------------------- |
| `index.js`              | Entry point. Bootstraps `TprAddressLookup` instances on `DOMContentLoaded`.            |
| `config.js`             | Central configuration: selectors, labels, error messages, patterns.                    |
| `state-machine.js`      | `AddressLookupStateMachine` — manages states and valid transitions.                    |
| `api-service.js`        | `AddressLookupApiService` — fetches addresses from search and ID endpoints.            |
| `component-builder.js`  | `AddressLookupComponentBuilder` — creates GOV.UK-styled DOM elements.                  |
| `mapper.js`             | `AddressMapper` — maps between DPA results, form inputs, and internal address objects. |
| `validator.js`          | `AddressLookupValidator` — integrates with jQuery Validation Unobtrusive.              |
| `input-builder.js`      | `InputBuilder` — fluent API for adding validation attributes to inputs.                |
| `postcode-sanitiser.js` | `PostcodeSanitiser` — trims, uppercases, and removes hyphens from postcodes.           |
| `utils.js`              | Utility functions (e.g. `toTitleCase`).                                             |

## Umbraco

You can add an 'Address lookup' block to a block list or block grid in Umbraco.

You can configure the sitewide default labels for fields by setting the following dictionary entries:

- `TPR address lookup line 1 label`
- `TPR address lookup line 2 label`
- `TPR address lookup town or city label`
- `TPR address lookup county label`
- `TPR address lookup postcode label`
- `TPR address lookup country label`

These in turn can be overridden by setting label text on the settings for each address lookup block.
