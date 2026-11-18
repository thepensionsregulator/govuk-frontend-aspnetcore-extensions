# TPR session timeout warning

The TPR session timeout warning is a reusable modal dialog for showing a configurable warning before a user's session expires. It is rendered using the `TPR/TPRSessionTimeOutWarning` Razor partial and styled by the TPR stylesheet.

The component provides the dialog markup, message model and an optional JavaScript module for warning timing, the remaining-time countdown and the session-extension button. Your application supplies the timeout, extension and warning durations, and a callback that performs the actual extension. Server-side session and authentication management remain the application's responsibility. The secondary action is a link.

## Use in an ASP.NET application

Create a `TprSessionTimeOutModel` and render the partial. Load the TPR stylesheet, then initialise the JavaScript module as shown below.

```razor
@using ThePensionsRegulator.Frontend.Models

@{
	var message = new TprSessionTimeOutModel
	{
		Title = "Your session is about to expire",
		Content = "Your session will end in {remainingTime}. Select continue to keep working, or sign out.",
		Settings = new SessionTimeoutSettings
		{
			RemainingTime = TimeSpan.FromMinutes(20),
			ExtensionLength = TimeSpan.FromMinutes(20),
			WarningBeforeExpiry = TimeSpan.FromMinutes(2),
			ExpiryUrl = "/signed-out"
		},
		DisplayPrimaryAction = true,
		PrimaryActionText = "Continue",
		PrimaryActionUrl = "/session/extend",
		DisplaySecondaryAction = true,
		SecondaryActionText = "Sign out",
		SecondaryActionUrl = "/sign-out"
	};
}

<partial name="TPR/TPRSessionTimeOutWarning" model="message" />
```

`PrimaryActionUrl` is exposed on the button as `data-session-timeout-extend-url`. The JavaScript module passes this value to your callback when the button marked `data-session-timeout-extend` is clicked; it does not fetch the URL itself. A callback can also use an application service instead of a URL.

The component uses a native `<dialog>` element with `role="alertdialog"`, and associates its title and description using `aria-labelledby` and `aria-describedby`. Without JavaScript initialisation the dialog remains hidden.

## Place the countdown in content

Write `{remainingTime}` wherever the live countdown belongs, for example: `Your session will end in {remainingTime}.` This works in the plain-text `Content` property, in `ContentHtml`, and in the Umbraco Content rich text editor. No Umbraco dependency is required. In rich text, you can format the complete placeholder (for example, make it bold), but do not split its characters across different formatting elements.

The JavaScript replaces each placeholder with an inline timer, preserving surrounding text, links and formatting. If no placeholder is present, it leaves the content unchanged and does not add a visible countdown. Warning timing and session-extension behaviour still work without the placeholder. To show a live countdown in existing Umbraco messages containing a fixed duration such as `2 minutes`, replace that duration with `{remainingTime}` and republish; the example uSync content uses this token too.

## Initialise the timeout behaviour

Import `initSessionTimeout` from the published module after rendering the partial. The shared partial renders `Settings` on the dialog as `data-timeout-ms`, `data-extension-ms`, `data-warning-ms` and `data-session-timeout-expiry-url`, with a `data-module="tpr-session-timeout"` marker. Durations are converted from `TimeSpan` to culture-independent milliseconds. 

```razor
@Html.AntiForgeryToken()

<script type="module">
    import { initSessionTimeout } from '/ThePensionsRegulator.Frontend/js/tpr-session-timeout-message.js';

    const dialog = document.querySelector('[data-module="tpr-session-timeout"]');
    const timeout = initSessionTimeout(dialog, {
        extendSession: async (url) => {
            const response = await fetch(url, {
                method: 'POST',
                redirect: 'error',
                headers: {
                    RequestVerificationToken: document.querySelector('input[name="__RequestVerificationToken"]').value
                }
            });
            return response.ok;
        }
    });
</script>
```

The example assumes your application implements the extension endpoint set in `PrimaryActionUrl`, accepts the anti-forgery header and returns a successful HTTP status only when the session was extended. Adapt the callback to your authentication and anti-forgery configuration. Return `true` only after a confirmed extension; `false`, other return values, thrown errors and rejected promises are treated as failures.

| Option | Description |
| --- | --- |
| `timeoutMs` | Remaining session lifetime when initialisation runs. Read from `Settings.RemainingTime` unless explicitly supplied in JavaScript. Finite and greater than zero. |
| `extensionMs` | New remaining lifetime after a successful extension, measured from callback completion. Read from `Settings.ExtensionLength` unless explicitly supplied. Finite and greater than zero. |
| `warningMs` | How long before expiry to open the warning. Read from `Settings.WarningBeforeExpiry` unless explicitly supplied. Finite, non-negative and no greater than `timeoutMs`. |
| `expiryUrl` | Optional HTTP(S) or relative destination to navigate to at zero. Read from `Settings.ExpiryUrl` unless explicitly supplied. Set to `null` to disable a model-configured destination. |
| `extendSession` | Required application callback, receiving the button URL (or `undefined` if absent) and returning a boolean or a promise of a boolean. |
| `formatRemainingTime` | Optional callback receiving non-negative remaining milliseconds and returning display text. By default, displays rounded-up whole minutes above one minute, then counts down from `60 seconds` to zero. Override for application-specific wording or formatting; this works without Umbraco too. |

For example, to display seconds throughout, add `formatRemainingTime: remainingMs => Math.ceil(remainingMs / 1000) + 's'` to the configuration. Omit the option to use the default formatting.

Initialise when the application's session timing is known. If the session is already running, supply its remaining lifetime through `Settings.RemainingTime` or `timeoutMs`, rather than the original full duration. Keep these values consistent with the server; the UI timer does not create, authenticate or extend a server session. Explicit JavaScript settings override the rendered attributes, including zero for `warningMs`. Existing JavaScript-only configuration remains supported: provide `timeoutMs`, `extensionMs` and `warningMs` when the model has no `Settings`. Nothing starts automatically; dialogs without runtime settings are not selected by the marker above.

`ExpiryUrl` performs a browser navigation (GET), not a sign-out POST. Use a destination your application permits, such as a signed-out page or an appropriate expiry endpoint. Authentication expiry, anti-forgery and identity-provider flows remain application-owned. If sign-out requires a POST, leave `ExpiryUrl` unset and handle `tpr-session-timeout:expired` in application code. If a destination is configured, an event listener can call `event.preventDefault()` to suppress default navigation and perform its own request instead. The secondary link's `SecondaryActionUrl` is independent of the automatic expiry destination.

The module:

- Opens the existing warning with `showModal()` at the configured warning point.
- Calculates remaining time from `expiryTime - Date.now()` on every update, including after delayed callbacks. It refreshes immediately on `visibilitychange` and `pageshow` after browser suspension.
- Replaces `{remainingTime}` in content with inline `data-session-timeout-countdown` elements. The default displays rounded-up whole minutes while more than one minute remains, then seconds from `60 seconds` to zero with singular/plural labels. It rounds partial seconds up, stops at zero and never displays negative time.
- Marks countdowns with `role="timer"` and `aria-live="off"`, so visual updates are not live announcements every second. The dialog description includes the current time when opened, and the timer remains available to screen-reader users.
- Disables the primary button while an extension is pending. On success it closes the warning and resets expiry to the current time plus `extensionMs`. On failure the original expiry and countdown continue, and the button becomes available to retry.
- Logs extension failures and emits a bubbling `tpr-session-timeout:error` event on the dialog, with the error in `event.detail`. Your application can listen to this event to show its own error notification.
- Emits a bubbling, cancelable `tpr-session-timeout:expired` event on the dialog once when the remaining time reaches zero, even without a countdown placeholder. It then navigates to `expiryUrl` if configured and not prevented. A successful extension starts a new expiry period. Listen before initialisation to implement application-owned expiry actions.
- Returns the same controller when initialised again for the same dialog, without resetting expiry or duplicating timers, markup or handlers. The original configuration remains in use.

The returned controller exposes the read-only `expiryTime` timestamp and a `destroy()` method to remove timers, event handlers and the generated countdown. Call `destroy()` before removing the component or initialising it with different configuration. Pending extensions cannot restart a destroyed controller. Without an expiry destination or an application event handler, reaching zero does not navigate or sign out; expiry policy remains with your application.

The example Umbraco application's `/session-timeout` page displays the configured variants inline, replacing `{remainingTime}` with a static example value of `2 minutes`; it does not start a countdown. Its `/session-timeout-preview` page uses application-owned demo settings: a 125-second timeout, a warning 2 minutes before expiry (after 5 seconds) and a 3-minute extension. It uses the library's default formatting without a `formatRemainingTime` override: `2 minutes` stays displayed for a full minute, then switches to `60 seconds` when one minute remains and counts down to zero. Longer durations decrease once per elapsed minute (for example, `30 minutes`, then `29 minutes` after one minute). Unchanged text is not rewritten each second. The preview's extension callback simulates success without making a server request or changing a real session. The preview attaches `SessionTimeoutSettings` to the selected message, with `ExpiryUrl = "/session-timeout"`; the shared partial and library handle the return at zero. The preview script only supplies its simulated extension callback. The static variants page does not attach runtime settings.

## Configure message variants in Umbraco

The example Umbraco application adds a **TPR session timeout messages** section to the Settings content type. Use its **Warning messages** block list to add one or more **TPR session timeout variant** blocks. Each variant has a required message key and inherits these optional fields:

| Field | Description |
| --- | --- |
| Title | Heading for the warning. |
| Content | Rich text shown below the heading. |
| Primary action text | Text for the session-extension button. When provided and enabled, the button is rendered. |
| Primary action URL | URL exposed as the button's `data-session-timeout-extend-url` value for application code. |
| Secondary action text | Text for the secondary link. |
| Secondary action URL | Destination of the secondary link. Both its text and URL are needed for the link to render. |

The Umbraco content configuration stores these message variants; the consuming application still needs to retrieve the published values and map them to `TprSessionTimeOutModel` instances. The example application's `SessionTimeoutDemoMessages` service shows one way to do this.

## Select a message by key

`TprSessionTimeOutMessageSelector.SelectByKey` selects a configured model by its message key. Matching is case-insensitive. If the key is blank or no variant matches, it returns the first non-null message as a fallback; it returns `null` when there are no non-null messages.

```csharp
using ThePensionsRegulator.Frontend.Services;

var message = TprSessionTimeOutMessageSelector.SelectByKey(messages, "session-expiry");
```

## Model properties

| Property | Type | Description |
| --- | --- | --- |
| `Settings` | `SessionTimeoutSettings?` | Optional application-owned runtime settings.|
| `MessageKey` | `string?` | Stable key used to select a message variant. |
| `Title` | `string?` | Dialog heading. |
| `Content` | `string?` | Plain-text content, rendered inside a paragraph when `ContentHtml` is not set. |
| `ContentHtml` | `IHtmlContent?` | Rich content. Takes precedence over `Content` when set. |
| `DisplayPrimaryAction` | `bool` | Enables rendering the primary button when `PrimaryActionText` is not blank. |
| `PrimaryActionText` | `string?` | Primary button label. |
| `PrimaryActionUrl` | `string?` | Value exposed in the primary button's `data-session-timeout-extend-url` attribute. |
| `DisplaySecondaryAction` | `bool` | Enables rendering the secondary link when both its text and URL are not blank. |
| `SecondaryActionText` | `string?` | Secondary link label. |
| `SecondaryActionUrl` | `string?` | Secondary link destination. |

### SessionTimeoutSettings

| Property | Type | Description |
| --- | --- | --- |
| `RemainingTime` | `TimeSpan` | Remaining lifetime at initialisation; greater than zero. |
| `ExtensionLength` | `TimeSpan` | Lifetime after a confirmed extension; greater than zero. |
| `WarningBeforeExpiry` | `TimeSpan` | Warning period; non-negative and no greater than `RemainingTime`. |
| `ExpiryUrl` | `string?` | Optional application-owned navigation destination at zero, separate from the manual secondary action. |
