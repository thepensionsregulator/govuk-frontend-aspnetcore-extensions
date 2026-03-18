# Address lookup

The address lookup component provides a multi-step postcode lookup flow as a progressive enhancement over standard address input fields. It wraps address inputs in a `<fieldset>` and, when JavaScript is available, replaces them with an interactive state machine that guides the user through postcode search, address selection, manual entry (UK and international), and address confirmation.

Without JavaScript, the original address input fields remain visible and usable.

## Example

```razor
@addTagHelper *, ThePensionsRegulator.Frontend

<form method="post" novalidate>
    <tpr-address-lookup
        data-address-lookup-search-url="/api/address/search"
        data-address-lookup-id-url="/api/address/byid">
        <tpr-address-lookup-legend>Enter your address</tpr-address-lookup-legend>
        <govuk-client-side-validation>
            <govuk-input for="AddressLine1" input-class="govuk-input--width-30" autocomplete="address-line1"
                         data-address-lookup="address-line-1">
                <govuk-input-label>Address line 1</govuk-input-label>
            </govuk-input>
            <govuk-input for="AddressLine2" input-class="govuk-input--width-30" autocomplete="address-line2"
                         data-address-lookup="address-line-2">
                <govuk-input-label>Address line 2 (optional)</govuk-input-label>
            </govuk-input>
            <govuk-input for="TownOrCity" input-class="govuk-input--width-20" autocomplete="address-level2"
                         data-address-lookup="town-or-city">
                <govuk-input-label>Town or city</govuk-input-label>
            </govuk-input>
            <govuk-input for="County" input-class="govuk-input--width-20" autocomplete="address-level3"
                         data-address-lookup="county">
                <govuk-input-label>County (optional)</govuk-input-label>
            </govuk-input>
            <govuk-input for="Country" input-class="govuk-input--width-20" autocomplete="country"
                         data-address-lookup="country">
                <govuk-input-label>Country</govuk-input-label>
            </govuk-input>
            <govuk-input for="Postcode" input-class="govuk-input--width-10" autocomplete="postal-code"
                         data-address-lookup="postcode">
                <govuk-input-label>Postcode</govuk-input-label>
            </govuk-input>
        </govuk-client-side-validation>
    </tpr-address-lookup>

    <govuk-button type="submit">Submit</govuk-button>
</form>

@section scripts {
    <partial name="GOVUK/Validation" />
    <script src="_Content/ThePensionsRegulator.Frontend/tpr/address-lookup/index.js" type="module"></script>
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

| Attribute                          | Type                | Default   | Description                                                                                                           |
| ---------------------------------- | ------------------- | --------- | --------------------------------------------------------------------------------------------------------------------- |
| `data-address-lookup-search-url`   | `string`            |           | API endpoint for postcode search. Receives `postcode` query parameter.                                                |
| `data-address-lookup-id-url`       | `string`            |           | API endpoint for looking up a single address by UPRN. Receives `uprn` query parameter.                                |
| `role`                             | `AddressLookupRole` | `Primary` | The role of this address lookup. Set to `Secondary` for a dependent address that can copy the primary.                |
| `same-as-primary-checkbox-label`   | `string`            |           | Label for the "same as primary" checkbox. **Required** when `role` is `Secondary`.                                    |
| `described-by`                     | `string`            |           | ID(s) to set on the fieldset's `aria-describedby` attribute.                                                          |

### `<tpr-address-lookup-legend>`

_Required._ Must be a direct child of `<tpr-address-lookup>`. Only one legend is permitted.

| Attribute         | Type   | Default | Description                                                                 |
| ----------------- | ------ | ------- | --------------------------------------------------------------------------- |
| `is-page-heading` | `bool` | `false` | When `true`, the legend text is wrapped in an `<h1>` inside the `<legend>`. |

### Child inputs

Each `<input>` inside the component must have a `data-address-lookup` attribute set to one of the following values so the JavaScript can capture and restore field values across state transitions:

| `data-address-lookup` value | Description          |
| --------------------------- | -------------------- |
| `address-line-1`            | Address line 1       |
| `address-line-2`            | Address line 2       |
| `town-or-city`              | Town or city         |
| `county`                    | County               |
| `country`                   | Country              |
| `postcode`                  | Postcode             |
| `UPRN`                      | Unique Property Reference Number (hidden) |

## View model

The backing view model should have properties for each address field. Apply standard data annotations for server-side validation.

```csharp
public class AddressViewModel
{
    [Required]
    [MaxLength(500)]
    public string? AddressLine1 { get; set; }

    [MaxLength(500)]
    public string? AddressLine2 { get; set; }

    [Required]
    [MaxLength(500)]
    public string? TownOrCity { get; set; }

    [MaxLength(500)]
    public string? County { get; set; }

    [Required]
    public string? Country { get; set; }

    [Required]
    [MaxLength(20)]
    public string? Postcode { get; set; }
}
```

## State machine

The JavaScript component is driven by a state machine with the following states:

| State                        | Description                                                                                                  |
| ---------------------------- | ------------------------------------------------------------------------------------------------------------ |
| `SEARCH`                     | User enters a building name/number and postcode, then clicks "Find address".                                 |
| `SELECT`                     | A dropdown of matching addresses is shown. User picks one and clicks "Confirm address".                      |
| `CONFIRMED`                  | The confirmed address is displayed as text. Hidden inputs preserve the values for form submission.            |
| `MANUAL_UK_ENTRY`            | Manual entry form for UK addresses (line 1, line 2, town, county, postcode).                                 |
| `MANUAL_INTERNATIONAL_ENTRY` | Manual entry form for international addresses (line 1, line 2, town, region, country, postal code).          |

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

| Field               | Rules                                                                 |
| ------------------- | --------------------------------------------------------------------- |
| Postcode (search)   | Required, UK postcode pattern                                         |
| Building name       | Max length 100                                                        |
| Address line 1      | Required, max length 500                                              |
| Address line 2      | Max length 500                                                        |
| Town or city        | Required, max length 500                                              |
| County / Region     | Max length 500                                                        |
| Country             | Required, max length 500                                              |
| Postal code (intl.) | Required, max length 20                                               |
| Address select      | Required ("Select an address")                                        |

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

If a building name or number is provided, results are filtered client-side: numeric values match `BUILDING_NUMBER` exactly, non-numeric values match `BUILDING_NAME` (case-insensitive substring).

### Address-by-ID endpoint

**Request:** `GET {id-url}?uprn={uprn}`

**Response:** Same structure as the search endpoint, returning the single matching address.

## Configuration

### JavaScript

Include the JavaScript module in any view that uses this component:

```html
<script src="_Content/ThePensionsRegulator.Frontend/tpr/address-lookup/index.js" type="module"></script>
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

## Generated HTML

The tag helper produces the following server-rendered HTML. The JavaScript then replaces the fieldset contents on page load.

```html
<div class="tpr-address-lookup"
     data-address-lookup-role="primary"
     data-address-lookup-search-url="/api/address/search"
     data-address-lookup-id-url="/api/address/byid">
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
<div class="tpr-address-lookup"
     data-address-lookup-role="secondary"
     data-address-lookup-same-as-primary-checkbox-label="Billing address is the same as shipping address"
     data-address-lookup-search-url="/api/address/search"
     data-address-lookup-id-url="/api/address/byid">
  <!-- ... -->
</div>
```

## JavaScript architecture

The client-side code is structured as ES modules:

| Module              | Responsibility                                                                 |
| ------------------- | ------------------------------------------------------------------------------ |
| `index.js`          | Entry point. Bootstraps `TprAddressLookup` instances on `DOMContentLoaded`.    |
| `config.js`         | Central configuration: selectors, labels, error messages, patterns.            |
| `state-machine.js`  | `AddressLookupStateMachine` — manages states and valid transitions.            |
| `api-service.js`    | `AddressLookupApiService` — fetches addresses from search and ID endpoints.    |
| `component-builder.js` | `AddressLookupComponentBuilder` — creates GOV.UK-styled DOM elements.       |
| `mapper.js`         | `AddressMapper` — maps between DPA results, form inputs, and internal address objects. |
| `validator.js`      | `AddressLookupValidator` — integrates with jQuery Validation Unobtrusive.      |
| `input-builder.js`  | `InputBuilder` — fluent API for adding validation attributes to inputs.        |
| `postcode-sanitiser.js` | `PostcodeSanitiser` — trims, uppercases, and removes hyphens from postcodes. |
| `utils.js`          | Utility functions (e.g. `toSentenceCase`).                                     |
