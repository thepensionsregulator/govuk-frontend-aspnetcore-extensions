# Read the TPR consent cookie

Applications can implement `IConsentCookieReader` to read their consent cookie, and any use of cookies in these packages will respect that consent.

TPR has a single consent cookie that is valid across the public `*.thepensionsregulator.gov.uk` domain. This is read by `TprConsentCookieReader` which implements `IConsentCookieReader` in the `ThePensionsRegulator.Frontend` package.

```razor
@using ThePensionsRegulator.GovUk.Frontend.Security
@using ThePensionsRegulator.Frontend.Security
@inject IConsentCookieReader consentCookie

@if (consentCookie.HasConsent(TprConsentCookieReader.TPR_CONSENT_CATEGORY_FUNCTIONALITY))
{
    // do protected activity
}
```

Aditionally, subscribers can test for the consent cookie presence to assess whether a user has explicitly specified cookie consent levels.

```razor
@using ThePensionsRegulator.GovUk.Frontend.Security
@using ThePensionsRegulator.Frontend.Security
@inject IConsentCookieReader consentCookie

@if (!consentCookie.IsConsentCookiePresent())
{
    // Display cookie banner, and collect user preferences
}
```

The `TprConsentCookieReader` class contains constants representing each category of consent which can be granted for the `*.thepensionsregulator.gov.uk` domain.
