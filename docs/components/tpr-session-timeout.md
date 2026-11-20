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
        extensionRequestTimeoutMs: 10000,
        extendSession: async (url, signal) => {
            const response = await fetch(url, {
                method: 'POST',
                redirect: 'error',
                signal,
                headers: {
                    RequestVerificationToken: document.querySelector('input[name="__RequestVerificationToken"]').value
                }
            });
            return response.ok;
        }
    });
</script>
```

The example assumes your application implements the extension endpoint set in `PrimaryActionUrl`, accepts the anti-forgery header and returns a successful HTTP status only when the session was extended. Adapt the callback to your authentication and anti-forgery configuration. Return `true` only after a confirmed extension to use `extensionMs`, or return `{ expiryTime }` with the server's absolute expiry in Unix milliseconds. `false`, invalid results, thrown errors and rejected promises are treated as failures.

| Option | Description |
| --- | --- |
| `timeoutMs` | Remaining session lifetime when initialisation runs. Read from `Settings.RemainingTime` unless explicitly supplied in JavaScript. Finite and greater than zero. |
| `extensionMs` | New remaining lifetime when the extension callback returns `true`, measured from callback completion. Not added when the callback returns `{ expiryTime }`. Read from `Settings.ExtensionLength` unless explicitly supplied. Required, finite and greater than zero only when an extension button is present; otherwise ignored. |
| `warningMs` | How long before expiry to open the warning. Read from `Settings.WarningBeforeExpiry` unless explicitly supplied. Finite, non-negative and no greater than `timeoutMs`. |
| `expiryUrl` | Optional HTTP(S) or relative destination to navigate to at zero. Read from `Settings.ExpiryUrl` unless explicitly supplied. Set to `null` to disable a model-configured destination. |
| `extendSession` | Required only when a `[data-session-timeout-extend]` button is rendered. Application callback receiving the button URL (or `undefined` if absent) and an `AbortSignal`. Returns `true`, `false` or `{ expiryTime: number }`, synchronously or in a promise. Callbacks that only accept the URL remain supported. Ignored without an extension button. |
| `extensionRequestTimeoutMs` | Optional JavaScript callback wait limit, in milliseconds. Defaults to `10000` (10 seconds). With an extension button, must be finite, greater than zero and at most `2147483647`; otherwise ignored. Separate from the session's extension length. |
| `formatRemainingTime` | Optional callback receiving non-negative remaining milliseconds and returning display text. By default, displays rounded-up whole minutes above one minute, then counts down from `60 seconds` to zero. Override for application-specific wording or formatting; this works without Umbraco too. |

For example, to display seconds throughout, add `formatRemainingTime: remainingMs => Math.ceil(remainingMs / 1000) + 's'` to the configuration. Omit the option to use the default formatting.

Warnings without a primary extension button need no extension callback or extension duration. If the dialog's data attributes supply its remaining lifetime and warning period, simply call `initSessionTimeout(dialog)`. Countdown, warning and expiry navigation still work; timeout, warning, formatter and expiry URL validation still apply.

Initialise when the application's session timing is known. If the session is already running, supply its remaining lifetime through `Settings.RemainingTime` or `timeoutMs`, rather than the original full duration. Keep these values consistent with the server; the UI timer does not create, authenticate or extend a server session. Explicit JavaScript settings override the rendered attributes, including zero for `warningMs`. Existing JavaScript-only configuration remains supported: provide `timeoutMs`, `extensionMs` and `warningMs` when the model has no `Settings`. Nothing starts automatically; dialogs without runtime settings are not selected by the marker above.

`ExpiryUrl` performs a browser navigation (GET), not a sign-out POST. Use a destination your application permits, such as a signed-out page or an appropriate expiry endpoint. Authentication expiry, anti-forgery and identity-provider flows remain application-owned. If sign-out requires a POST, leave `ExpiryUrl` unset and handle `tpr-session-timeout:expired` in application code. If a destination is configured, an event listener can call `event.preventDefault()` to suppress default navigation and perform its own request instead. The secondary link's `SecondaryActionUrl` is independent of the automatic expiry destination.

The module:

- Opens the existing warning with `showModal()` at the configured warning point.
- Calculates remaining time from `expiryTime - Date.now()` on every update, including after delayed callbacks. It refreshes immediately on `visibilitychange` and `pageshow` after browser suspension.
- Replaces `{remainingTime}` in content with inline `data-session-timeout-countdown` elements. The default displays rounded-up whole minutes while more than one minute remains, then seconds from `60 seconds` to zero with singular/plural labels. It rounds partial seconds up, stops at zero and never displays negative time.
- Marks countdowns with `role="timer"` and `aria-live="off"`, so visual updates are not live announcements every second. The dialog description includes the current time when opened, and the timer remains available to screen-reader users.
- Disables the primary button while an extension is pending. The countdown continues, clamped at zero, but automatic expiry navigation and the expiry event are deferred until the request settles or reaches `extensionRequestTimeoutMs`. A confirmed success within that limit uses the returned `{ expiryTime }`, or the current time plus `extensionMs` for `true`. The warning closes if the new expiry is outside the warning period; otherwise it stays open. The old deadline no longer triggers expiry.
- On failure or request timeout, leaves the current expiry unchanged (including any newer external update). If that deadline has passed, processes expiry immediately; otherwise, continues towards that deadline. The button becomes available to retry. Late results after request timeout are ignored and cannot reset the countdown. Absolute request deadlines are checked on browser resume and callback completion as well as by the timeout timer.
- Logs extension failures (including request timeouts) and emits a bubbling `tpr-session-timeout:error` event on the dialog, with the error in `event.detail`. Your application can listen to this event to show its own error notification.
- Emits a bubbling, cancelable `tpr-session-timeout:expired` event on the dialog once when the remaining time reaches zero and no extension is pending, even without a countdown placeholder. It then navigates to `expiryUrl` if configured and not prevented. A successful extension starts a new expiry period. Listen before initialisation to implement application-owned expiry actions.
- Returns the same controller when initialised again for the same dialog, without resetting expiry or duplicating timers, markup or handlers. The original configuration remains in use.

The request timeout bounds the frontend wait, even if the callback ignores cancellation. Each extension attempt receives a new `AbortSignal`, which is aborted when the request times out (including an overdue request detected after browser suspension) or when `destroy()` is called; `signal.reason` contains the timeout or destroy error. The signal is not aborted when the callback settles in time. Pass the signal to `fetch` (or your own transport) to cancel the network request. Cancellation is cooperative: it cannot undo server-side renewal if the server already processed the request. As with all browser timers, timeout processing resumes when JavaScript can run after suspension; an overdue response is not accepted as a successful extension.

The returned controller exposes the read-only `expiryTime` timestamp, `setExpiryTime(timestamp)` and a `destroy()` method to close the dialog if open, abort any pending extension signal, and remove timers (including pending request timeouts), event handlers and the generated countdown. Call `destroy()` before removing the component or initialising it with different configuration. Pending extensions cannot restart a destroyed controller. Without an expiry destination or an application event handler, reaching zero does not navigate or sign out; expiry policy remains with your application.

### Authoritative expiry and multiple tabs

Use `timeout.setExpiryTime(expiryTime)` when the application receives a confirmed expiry from the server, including updates from another tab. The argument must be a finite number of milliseconds since the Unix epoch, not a duration, date string or seconds value. Invalid values throw a `RangeError` without changing the existing expiry. Calling it after `destroy()` throws an error.

The setter immediately updates the countdown, closes an open warning if the expiry is outside the warning period, and reschedules the warning without adding duplicate timers. A nearer expiry opens the warning immediately. An expiry at or before now processes expiry immediately, unless an extension request is still pending; the existing bounded deferral then applies. Reapplying the same timestamp does not emit another expiry event. Updating to a new future deadline restarts a stopped countdown, but cannot undo navigation that has already occurred.

An extension callback may return `{ expiryTime: number }` from its server response instead of `true`. This timestamp is used unchanged, rather than adding `extensionMs` after the response arrives, so network delay does not grant extra time. Invalid timestamps use the normal extension-error reporting without changing expiry. Request timeouts and cancellation still apply. If `setExpiryTime` is called while a request is pending, that external update takes precedence over the pending request's result; neither success nor failure overwrites it.

Cross-tab communication remains application-owned. For example, using the `timeout` controller from above:

```javascript
const sessionUpdates = new BroadcastChannel('session-expiry');

// Call after obtaining and validating a confirmed expiry from the server.
function applyConfirmedExpiry(expiryTime) {
    timeout.setExpiryTime(expiryTime);
    sessionUpdates.postMessage({ expiryTime });
}

sessionUpdates.addEventListener('message', ({ data }) => {
    timeout.setExpiryTime(data.expiryTime);
});
```

Only publish confirmed server expiries for the same session. The application must discard stale or out-of-order notifications, for example using a server-provided session revision; the library cannot determine response freshness from a timestamp alone. Earlier expiries are valid too, so it does not simply keep the largest timestamp. `BroadcastChannel` does not persist messages: applications should recheck the server on focus or reconnection to catch missed updates, and call `sessionUpdates.close()` when tearing down this wiring. Unix timestamps assume the client and server clocks are sufficiently aligned; the application owns clock-skew correction and server-side session enforcement. Updating the frontend timer does not itself extend a session.

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
