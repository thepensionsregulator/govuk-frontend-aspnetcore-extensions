# Read the TPR consent cookie

Applications can implement `IConsentCookieReader` to read their consent cookie, and any use of cookies in these packages will respect that consent.

TPR has a single consent cookie that is valid across the public `*.thepensionsregulator.gov.uk` domain. This is read by `TprConsentCookieReader` which implements `IConsentCookieReader` in the `ThePensionsRegulator.Frontend` package.

```razor
@using GovUk.Frontend.AspNetCore.Extensions.Security
@using ThePensionsRegulator.Frontend.Security
@inject IConsentCookieReader consentCookie

@if (consentCookie.HasConsent(TprConsentCookieReader.TPR_CONSENT_CATEGORY_FUNCTIONALITY))
{
    // do protected activity
}
```

The `TprConsentCookieReader` class contains constants representing each category of consent which can be granted for the `*.thepensionsregulator.gov.uk` domain.
