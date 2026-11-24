# TPR address lookup

The Pensions Regulator (TPR) address lookup lets users search for a UK address by postcode, select and confirm an address, or enter a UK or international address manually. A secondary address can use the same address as the primary address.

The component is provided by `ThePensionsRegulator.Frontend`. Follow [Configure a new ASP.NET project (TPR)](../aspnet/new-aspnet-project-tpr.md) for service registration, static assets and layout setup. Umbraco applications should follow [Configure a new Umbraco project (TPR)](../umbraco/new-umbraco-project-tpr.md).

Provide your application's country list by implementing `ITprCountryRepository` from `ThePensionsRegulator.Frontend.Services`. Its `GetCountries()` method returns `Task<IDictionary<string, int>>`, mapping country names to the Ids used by your model's `CountryId`. Include `United Kingdom`: the JavaScript uses that name to identify UK addresses. The default repository supplies a placeholder, not a production country list. See [the example repository](../../GovUk.Frontend.ExampleApp/Services/ExampleTprCountryRepository.cs).

Register your implementation after `AddTprFrontend()` (or `AddTprFrontendUmbraco()`):

```csharp
builder.Services.AddTransient<ITprCountryRepository, ExampleTprCountryRepository>();
```

## Example

Add separate address properties to your view model:

```csharp
using ThePensionsRegulator.Frontend.Models;

public class AddressLookupViewModel
{
    public TprAddress ShippingAddress { get; set; } = new TprAddress();
    public TprAddress BillingAddress { get; set; } = new TprAddress();
}
```

Render both components inside the same form, with the primary address before the secondary address. The tag helper uses a `name` prefix for each address; supply initial field values with the individual `*-value` attributes. The shipping component uses the library's default content. The billing component overrides selected text while keeping the other defaults.

```razor
@model AddressLookupViewModel
@using ThePensionsRegulator.Frontend.HtmlGeneration
@using ThePensionsRegulator.Frontend.Models
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, ThePensionsRegulator.Frontend
@{
    var billingContent = new TprAddressLookupContent
    {
        AddressLookupIntroductionHeading = "Billing address",
        AddressLookupIntroductionText = "Enter the address to use for billing.",
        AddressLookupHint = "Enter the address to use for billing.",
        SearchLegend = "Find your billing address",
        SameAsLabel = "Same as shipping address"
    };
}

<form method="post">
    <tpr-address-lookup
        name="@nameof(Model.ShippingAddress)"
        role="@TprAddressLookupRole.Primary"
        address-search-url="/AddressLookupData/Address.json"
        govuk-heading-class="govuk-heading-m" />

    <tpr-address-lookup
        name="@nameof(Model.BillingAddress)"
        role="@TprAddressLookupRole.Secondary"
        address-search-url="/AddressLookupData/Address.json"
        content="@billingContent"
        govuk-heading-class="govuk-heading-m"
        address-line-1-value="@Model.BillingAddress.AddressLine1"
        address-line-2-value="@Model.BillingAddress.AddressLine2"
        address-line-3-value="@Model.BillingAddress.AddressLine3"
        post-town-value="@Model.BillingAddress.PostTown"
        post-county-value="@Model.BillingAddress.PostCounty"
        postcode-value="@Model.BillingAddress.Postcode"
        country-value="@Model.BillingAddress.CountryId"
        uprn-value="@Model.BillingAddress.Uprn" />

    <button type="submit" class="govuk-button">Submit</button>
</form>

@section scripts {
    <script src="~/js/address-lookup-example.js" type="module"></script>
}
```

Create the module referenced by the view:

```javascript
import { initialiseAddressLookups } from "/ThePensionsRegulator.Frontend/js/address-lookup/index.js";

document.addEventListener("DOMContentLoaded", function () {
    initialiseAddressLookups();
});
```

Call the initializer once after the components are in the document. It enhances all address lookups on the page and prevents form submission until each address is confirmed. Users can change a confirmed address or select the secondary component's "Same as shipping address" checkbox instead of entering another address.

[Run the ASP.NET example application](../aspnet/run-example-application.md) or [run the Umbraco example application](../umbraco/run-example-application.md) and open 'Address lookup'. Both examples use the same labels, roles and search endpoint. The MVC example prepopulates the billing address, while Umbraco prepopulates the shipping address; these initial values demonstrate the respective model-binding and initial-value patterns. Umbraco supplies the billing overrides through block properties instead of Razor.

## API

### `<tpr-address-lookup>`

| Attribute | Type | Description |
| --------- | ---- | ----------- |
| `name` | `string` | Required model property prefix, such as `ShippingAddress`. Generates names such as `ShippingAddress.AddressLine1`. |
| `address-line-1-value`, `address-line-2-value`, `address-line-3-value` | `string?` | Optional initial values for the corresponding address lines. |
| `post-town-value`, `post-county-value`, `postcode-value` | `string?` | Optional initial values for town/city, county and postcode. |
| `country-value` | `int?` | Optional initial country ID; the ID must correspond to an entry returned by `ITprCountryRepository`. |
| `uprn-value` | `string?` | Optional initial UPRN. |
| `address-search-url` | `string` | Required for postcode lookup. URL of an endpoint returning the response described below. |
| `role` | `TprAddressLookupRole?` | `Primary` or `Secondary`. Set `Primary` for a standalone lookup. For linked addresses, render the primary before the secondary. |
| `content` | `TprAddressLookupContent` | Labels, hints, legends, buttons, links and client-side error messages. Omit to use library defaults, or override individual properties. |
| `heading-level` | `int` | Introduction heading level, from `1` to `6`. Default is `2`. |
| `govuk-heading-class` | `string` | Introduction heading class: `govuk-heading-xl`, `govuk-heading-l`, `govuk-heading-m` or `govuk-heading-s`. Default is `govuk-heading-l`. |

### Model binding and validation

`TprAddress` contains `AddressLine1`, `AddressLine2`, `AddressLine3`, `PostTown`, `PostCounty`, `Postcode`, `CountryId` and `Uprn`. Give each component a distinct `name` prefix so the two addresses bind independently on POST. The same-as checkbox copies the primary address into the secondary address's fields.

Use the normal ASP.NET form and anti-forgery conventions. Validate `ModelState.IsValid` on the server before saving an address; JavaScript confirmation is not a substitute for server-side validation. `TprAddress` supplies data-annotation validation. Customising `TprAddressLookupContent` changes the component's text, not the server-side model's validation rules. See [Localisation and validation in ASP.NET projects](../aspnet/localisation-and-validation.md).

Align server-side rules with the addresses your application accepts. In particular, `TprAddress` requires a postcode, while the JavaScript international manual-entry form permits it to be empty.

For an invalid POST, return the submitted model to the view and preserve `ModelState`. ModelState values take precedence over the individual `*-value` attributes. When using `name` without initial-value attributes, fields without ModelState entries are empty. The Umbraco block renderer uses this pattern; to prepopulate it on GET, use `ModelState.SetInitialValues(address, propertyPrefix)` from `ThePensionsRegulator.Frontend.Validation`.

Alternatively, supply initial values explicitly on an MVC view:

```razor
<tpr-address-lookup name="ShippingAddress"
                    address-line-1-value="@Model.ShippingAddress.AddressLine1"
                    post-town-value="@Model.ShippingAddress.PostTown"
                    postcode-value="@Model.ShippingAddress.Postcode"
                    country-value="@Model.ShippingAddress.CountryId"
                    address-search-url="/AddressLookupData/Address.json" />
```

### Search endpoint

The JavaScript sends a GET request to `address-search-url`, adding a `postcode` query parameter and an `Accept: application/json` header. The response uses the Ordnance Survey Places API Delivery Point Address shape:

```json
{
  "results": [
    {
      "DPA": {
        "UPRN": "22053022",
        "UDPRN": "2199062",
        "ADDRESS": "103, PRESTON ROAD, BRIGHTON, BN1 6AF",
        "BUILDING_NUMBER": "103",
        "THOROUGHFARE_NAME": "PRESTON ROAD",
        "POST_TOWN": "BRIGHTON",
        "POSTCODE": "BN1 6AF"
      }
    }
  ]
}
```

`ADDRESS` supplies the result label. Structured building, street and locality fields supply the address lines; do not return only a display string. See the [sample response](../../GovUk.Frontend.ExampleApp/wwwroot/AddressLookupData/Address.json) and [address result contract](../../ThePensionsRegulator.Frontend/Scripts/address-lookup/types.ts) for the supported fields.

An empty `results` array displays the no-addresses message. A failed request displays the service-unavailable message. Users can enter an address manually instead.

The examples serve a static JSON fixture at `/AddressLookupData/Address.json`; it returns the same data regardless of the postcode. Use `BN1 6AF` to match the sample results. Production applications must provide their own lookup endpoint, handle provider authentication on the server, and return the expected response shape. Do not expose provider credentials in the page or JavaScript.

### Without JavaScript

The server-rendered component provides manual address fields. Postcode search, result selection, confirmation and the same-as checkbox require JavaScript. Server-side model binding and validation must still work when JavaScript is unavailable.

## Umbraco

Use the 'Address lookup' block (`tprAddressLookup`) with its address lookup settings element (`tprAddressLookupSettings`) in your block list. Bind each block to an `UmbracoTprAddress` property on the page's view model using the **Model property** (`modelProperty`) setting. Follow the existing [model type binding](../umbraco/model-type-attribute.md), [initial values](../umbraco/populate-initial-values.md) and [validation](../umbraco/validation.md) patterns.

To match the MVC example:

1. Add a shipping block with `modelProperty` set to `ShippingAddress` and **Is primary address lookup** (`addressLookupIsPrimary`) enabled. Leave its content properties empty.
2. Add a billing block after shipping with `modelProperty` set to `BillingAddress` and `addressLookupIsPrimary` disabled.
3. Set the following billing content properties. Leave the other properties empty to retain their defaults.

| Block property alias | Equivalent `TprAddressLookupContent` property | Example value |
| -------------------- | ------------------------------------------- | ------------- |
| `addressLookupLegend` | `AddressLookupIntroductionHeading` | Billing address |
| `addressLookupHint` | `AddressLookupHint` | Enter the address to use for billing. |
| `noJsHint` | `AddressLookupIntroductionText` | Enter the address to use for billing. |
| `searchLegend` | `SearchLegend` | Find your billing address |
| `errorPrefix` | `ErrorPrefix` | Error: |
| `ukManualEntryLinkLabel` | `UkManualEntryLinkLabel` | Enter address manually (optional) |
| `internationalManualEntryLabel` | `InternationalManualEntryLabel` | Enter an international address (optional) |
| `sameAsLabel` | `SameAsLabel` | Same as shipping address |

Render the block list inside the usual `Html.BeginUmbracoForm` form, with a submit button after both blocks. Keep content in the blocks rather than hardcoding it into the page template. Include the JavaScript module shown above and the normal `GOVUK/Validation` partial.

The block renderer reads the endpoint from application configuration:

```json
{
  "AddressLookup": {
    "SearchEndpoint": "/AddressLookupData/Address.json"
  }
}
```

### Content and dictionary overrides

Labels, hints, legends, button/link text and the error prefix are properties of the content block. Validation error-message overrides are properties of its settings element. Text is resolved separately for each field in this order:

1. A non-empty block property (content or settings, as appropriate).
2. A non-empty Umbraco dictionary translation, such as `Address lookup - county label`.
3. The library's `TprAddressLookupContent` default.

Whitespace-only values are treated as empty. An empty dictionary translation does not hide the label: it restores the library default when the block property is also empty. Dictionary translations provide shared text; block properties provide instance-specific overrides.

The example application's address lookup dictionary entries deliberately have empty translations. Its primary block therefore demonstrates library defaults, while its secondary block demonstrates content-property overrides. See [the dictionary key definitions](../../ThePensionsRegulator.Frontend.Umbraco/TprAddressLookupDictionaryKeys.cs) for the available keys.

The `addressLookupHint` is applied to the enhanced experience after JavaScript starts. The separate `noJsHint` is shown in the server-rendered fallback, so its text should not refer to JavaScript-only search or confirmation steps. Both properties, like the `errorPrefix` property, follow the same block → dictionary → library-default order.