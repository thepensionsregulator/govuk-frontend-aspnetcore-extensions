# TPR session timeout warning

The TPR session timeout warning displays a modal dialog before a user's session expires. It is provided by `ThePensionsRegulator.Frontend` and can be used with or without Umbraco.

The component renders the warning and provides JavaScript for the countdown and extension button. Your application must configure the timing and implement session extension and expiry on the server. The component does not manage authentication or extend a server session itself.

## Example

1. Load the TPR stylesheet in your layout, for example with `<partial name="TPR/Head" />` in the `<head>`. See [Configure a new ASP.NET project (TPR)](../aspnet/new-aspnet-project-tpr.md) for package and layout setup.
2. Render a `TprSessionTimeOutModel` using the partial below. Use the actual remaining session lifetime at initialisation, rather than the original session duration.
3. Implement an extension endpoint in your application and initialise the JavaScript as shown in [Initialise the JavaScript](#initialise-the-javascript).

For example:

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

Register the ASP.NET Core MVC tag helpers if they are not already registered in your view imports. `PrimaryActionUrl` is passed to the extension callback; the component does not request it automatically. The secondary action is a separate link.

The partial uses a native `<dialog>` with `role="alertdialog"` and associated title and description. It remains hidden until application code opens or initialises it.

## Initialise the JavaScript

Import `initSessionTimeout` after rendering the partial. The component does not initialise itself. When `Settings` is supplied, the partial renders `data-timeout-ms`, `data-extension-ms`, `data-warning-ms` and, if configured, `data-session-timeout-expiry-url` on the dialog. It also adds the `data-module="tpr-session-timeout"` marker used below. Durations are rendered as culture-independent milliseconds. Applications that provide their own dialog markup can set these attributes directly on that dialog.

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

This example assumes the endpoint in `PrimaryActionUrl` accepts the anti-forgery header and returns a successful HTTP status only when the session has been extended. Adapt it to your application's authentication and anti-forgery configuration. Return `true` only after a confirmed extension, or return `{ expiryTime }` containing the server's absolute expiry in Unix milliseconds. `false`, invalid results, thrown errors and rejected promises are failures. `PrimaryActionUrl` may be omitted if the callback uses another mechanism.

JavaScript options override the rendered attributes. Without `Settings`, select the dialog directly and supply `timeoutMs` and `warningMs`, plus `extensionMs` and `extendSession` when an extension button is present. If there is no extension button, no extension callback or duration is required; when the timing attributes are present, call `initSessionTimeout(dialog)`.

### Options

| Option | Description |
| --- | --- |
| `timeoutMs` | Remaining session lifetime when initialisation runs. Read from `Settings.RemainingTime` unless explicitly supplied in JavaScript. Finite and greater than zero. |
| `extensionMs` | New remaining lifetime when the extension callback returns `true`, measured from callback completion. Not added when the callback returns `{ expiryTime }`. Read from `Settings.ExtensionLength` unless explicitly supplied. Required, finite and greater than zero only when an extension button is present; otherwise ignored. |
| `warningMs` | How long before expiry to open the warning. Read from `Settings.WarningBeforeExpiry` unless explicitly supplied. Finite, non-negative and no greater than `timeoutMs`. |
| `expiryUrl` | Optional HTTP(S) or relative destination to navigate to at zero. Read from `Settings.ExpiryUrl` unless explicitly supplied. Set to `null` to disable a model-configured destination. |
| `extendSession` | Required only when a `[data-session-timeout-extend]` button is rendered. Application callback receiving the button URL (or `undefined` if absent) and an `AbortSignal`. Returns `true`, `false` or `{ expiryTime: number }`, synchronously or in a promise. Callbacks that only accept the URL remain supported. Ignored without an extension button. |
| `extensionRequestTimeoutMs` | Optional JavaScript callback wait limit, in milliseconds. Defaults to `10000` (10 seconds). With an extension button, must be finite, greater than zero and at most `2147483647`; otherwise ignored. Separate from the session's extension length. |
| `formatRemainingTime` | Optional callback receiving non-negative remaining milliseconds and returning display text. By default, displays rounded-up whole minutes above one minute, then counts down from `60 seconds` to zero. Override for application-specific wording or formatting; this works without Umbraco too. |

## Countdown

Add `{remainingTime}` to `Content`, `ContentHtml` or Umbraco rich text where the countdown should appear. For example: `Your session will end in {remainingTime}.` In rich text, you can format the entire placeholder, but do not split its characters across formatting elements. The script replaces each placeholder with an inline timer while preserving surrounding content. Without a placeholder, no visible countdown is added, but warning and extension behaviour still work.

By default, the countdown displays rounded-up whole minutes above one minute, then seconds from `60 seconds` to zero. Partial seconds are rounded up and negative values are never displayed. For example, to display seconds throughout, add `formatRemainingTime: remainingMs => Math.ceil(remainingMs / 1000) + 's'` to the configuration. Omit the option to use the default formatting.

The countdown uses `role="timer"` and `aria-live="off"` so its visual updates are not announced every second. Remaining time is calculated from `expiryTime - Date.now()` and refreshed after delayed timer callbacks and on `visibilitychange` and `pageshow`.

## Session extension and expiry

When the user selects the primary button, the component disables it and calls `extendSession`. On confirmed success, it uses the returned `{ expiryTime }`, or the current time plus `extensionMs` when the callback returns `true`. It closes the warning if the new expiry is outside the warning period. On failure, it keeps the current expiry, re-enables the button, logs the error and emits a bubbling `tpr-session-timeout:error` event with the error in `event.detail`.

While the request is pending, the countdown can reach zero, but the expiry event and navigation wait until the request settles or `extensionRequestTimeoutMs` is reached. If the request fails or times out after the current deadline, expiry is processed immediately. Late results are ignored. Browser suspension can delay processing, but overdue responses are not accepted as successful extensions.

Each request receives a fresh `AbortSignal`, which is aborted on timeout or `destroy()`, with the timeout or destroy error in `signal.reason`. Pass it to `fetch` or another cooperative transport, as in the example above. Callbacks that accept only the URL still work. The bounded wait also applies when a callback ignores the signal, and cancellation cannot undo a server-side renewal that has already occurred. A request that settles within its deadline is not aborted.

At expiry, the component emits a bubbling, cancelable `tpr-session-timeout:expired` event once per expiry period, even without a countdown placeholder. If configured, `ExpiryUrl` then triggers a browser navigation (GET); the component does not itself invalidate the session. For application-specific handling, listen for the event before initialisation and call `event.preventDefault()` to suppress configured navigation. If sign-out requires a POST, leave `ExpiryUrl` unset and perform the request in your handler. The secondary action link is independent of automatic expiry navigation.

## Controller lifecycle

`initSessionTimeout` returns a controller with a read-only `expiryTime` timestamp, `setExpiryTime(timestamp)` and a `destroy()` method. Reinitialising the same dialog returns the existing controller without resetting its deadline or adding duplicate timers or handlers.

Call `destroy()` before removing the component or initialising it with different configuration. It closes an open dialog, aborts any pending extension signal, clears timers and handlers, and restores the countdown placeholders. Pending requests cannot restart a destroyed controller.

### Authoritative expiry and multiple tabs

Use `timeout.setExpiryTime(expiryTime)` when the application receives a confirmed expiry from the server, including updates from another tab. The argument must be a finite number of Unix milliseconds, not a duration, date string or seconds value. Invalid values throw a `RangeError` without changing expiry; calling it after `destroy()` throws an error.

The setter updates the countdown immediately and reschedules the warning without duplicate timers. It closes an open warning if the new expiry is outside the warning period, or opens it if the expiry is within that period. An expiry at or before now is processed immediately unless an extension request is pending; the bounded deferral still applies. Reapplying the same timestamp does not emit another expiry event. A new future deadline restarts a stopped countdown, but cannot undo navigation that has already occurred.

An extension callback can instead return `{ expiryTime: number }` from its server response. The timestamp is used unchanged, rather than adding `extensionMs` after the response arrives. Invalid timestamps use normal extension-error reporting without changing expiry. If `setExpiryTime` is called while a request is pending, the external update takes precedence over that request's result.

Cross-tab communication is application-owned. For example, using the `timeout` controller from above:

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

Only publish confirmed server expiries for the same session. The application must discard stale or out-of-order notifications, for example using a server-provided session revision; the library cannot determine freshness from the expiry timestamp alone. Earlier expiries are valid, so it does not simply keep the largest timestamp. `BroadcastChannel` does not persist messages: recheck the server on focus or reconnection to catch missed updates, and call `sessionUpdates.close()` when tearing down this wiring. Unix timestamps assume sufficiently aligned client and server clocks; clock-skew correction and server-side session enforcement remain application responsibilities.

## Umbraco

### Configure message variants

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

The example Umbraco application's `/session-timeout` page displays variants with a static example time. Its `/session-timeout-preview` page demonstrates a 125-second timeout, a warning after 5 seconds and a 3-minute extension. The non-Umbraco example app provides the same demonstration at `/sessiontimeout` and `/sessiontimeout/preview`. These previews simulate extension; they do not change a real session.

### Select a message by key

`TprSessionTimeOutMessageSelector.SelectByKey` selects a configured model by its message key. Matching is case-insensitive. If the key is blank or no variant matches, it returns the first non-null message as a fallback; it returns `null` when there are no non-null messages.

```csharp
using ThePensionsRegulator.Frontend.Services;

var message = TprSessionTimeOutMessageSelector.SelectByKey(messages, "session-expiry");
```

## API

### `TprSessionTimeOutModel`

| Property | Type | Description |
| --- | --- | --- |
| `Settings` | `SessionTimeoutSettings?` | Optional application-owned runtime settings. |
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

### `SessionTimeoutSettings`

| Property | Type | Description |
| --- | --- | --- |
| `RemainingTime` | `TimeSpan` | Remaining lifetime at initialisation; greater than zero. |
| `ExtensionLength` | `TimeSpan` | Lifetime after a confirmed extension. Must be greater than zero when an extension button is present. |
| `WarningBeforeExpiry` | `TimeSpan` | Warning period; non-negative and no greater than `RemainingTime`. |
| `ExpiryUrl` | `string?` | Optional application-owned navigation destination at zero, separate from the manual secondary action. |
