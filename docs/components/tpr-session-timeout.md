# TPR session timeout warning

The TPR session timeout warning is a reusable modal dialog for showing a configurable warning before a user's session expires. It is rendered using the `TPR/TPRSessionTimeOutWarning` Razor partial and styled by the TPR stylesheet.

The component provides the dialog markup, message model and an optional JavaScript module for warning timing, the remaining-time countdown and the session-extension button. Your application supplies the timeout, extension and warning durations, and a callback that performs the actual extension. Server-side session and authentication management remain the application's responsibility. The secondary action is a link.

## Example

Create a `TprSessionTimeOutModel` and render the partial. Load the TPR stylesheet and initialise the JavaScript module as shown below.

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

## Countdown placeholder

Write `{remainingTime}` wherever the live countdown belongs, for example: `Your session will end in {remainingTime}.` It works in the plain-text `Content` property, `ContentHtml` and Umbraco rich text; no Umbraco dependency is required. In rich text, format the complete placeholder if needed, but do not split its characters across formatting elements.

The JavaScript replaces each placeholder with an inline timer while preserving surrounding content and formatting. Without a placeholder, the content stays unchanged and no visible countdown is added; warning and extension behaviour still work. To add a countdown to an existing message, replace its fixed duration (such as `2 minutes`) with `{remainingTime}` and republish it.

## JavaScript API

### Initialise the timeout behaviour

Import `initSessionTimeout` from the published module after rendering the partial. When `Settings` is supplied, the partial renders timing values as `data-timeout-ms`, `data-extension-ms`, `data-warning-ms` and `data-session-timeout-expiry-url`, and adds the `data-module="tpr-session-timeout"` marker. Durations use culture-independent milliseconds.

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

For example, to display seconds throughout, add `formatRemainingTime: remainingMs => Math.ceil(remainingMs / 1000) + 's'` to the configuration. Omit the option to use the default formatting.

Initialise when the application's session timing is known. Supply the remaining lifetime, not the original full duration. JavaScript options override rendered attributes. Without `Settings`, provide `timeoutMs` and `warningMs` (and `extensionMs` when an extension button is present). A dialog with no runtime settings is not initialized automatically.

Without an extension button, no callback or extension duration is required. If the dialog has timing data attributes, call `initSessionTimeout(dialog)`; warning and expiry behaviour remain available.

`ExpiryUrl` triggers browser navigation (GET), not sign-out. For sign-out POSTs or other application-specific handling, leave it unset and listen for `tpr-session-timeout:expired`; call `event.preventDefault()` to suppress default navigation. The secondary action link is independent of automatic expiry navigation.

### Behaviour

- Opens the existing warning with `showModal()` at the configured warning point and updates remaining time from `expiryTime - Date.now()`, including after delayed timer callbacks, `visibilitychange` and `pageshow`.
- Replaces `{remainingTime}` placeholders with inline countdowns. By default, it displays rounded-up whole minutes above one minute, then seconds from `60 seconds` to zero. It rounds partial seconds up and never displays a negative value.
- Sets `role="timer"` and `aria-live="off"` so frequent visual updates are not announced every second.
- Disables the extension button while a request is pending. Expiry navigation and the expiry event wait until the request settles or reaches `extensionRequestTimeoutMs`. On confirmed success, expiry becomes the current time plus `extensionMs`; failure or timeout preserves the original deadline. Late results are ignored.
- Logs extension failures and emits a bubbling `tpr-session-timeout:error` event with the error in `event.detail`.
- Emits one bubbling, cancelable `tpr-session-timeout:expired` event at expiry, even without a countdown placeholder. It navigates to `expiryUrl` unless the event is prevented.
- Returns the same controller for repeated initialization of a dialog, without resetting its deadline or adding duplicate timers, markup or handlers.

The request timeout bounds frontend waiting but does not cancel the application's network request or undo server-side renewal. The callback owns transport cancellation. Timer processing resumes when JavaScript can run after browser suspension; overdue callback results are not accepted as successful extensions.

The controller exposes a read-only `expiryTime` and `destroy()`. Destroying closes an open dialog, clears timers and event handlers, and restores the original countdown placeholder. Pending requests cannot restart a destroyed controller. Reaching zero does not sign out the user; expiry policy remains application-owned.

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

The example Umbraco application's `/session-timeout` page shows message variants with a static example time; it does not run the countdown. Its `/session-timeout-preview` page uses demo settings: a 125-second timeout, a warning after 5 seconds, and a 3-minute extension. The preview simulates extension without changing a real session and returns to `/session-timeout` at expiry. These settings and simulated behavior belong to the example application.

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
| `ExtensionLength` | `TimeSpan` | Lifetime after a confirmed extension; greater than zero. |
| `WarningBeforeExpiry` | `TimeSpan` | Warning period; non-negative and no greater than `RemainingTime`. |
| `ExpiryUrl` | `string?` | Optional application-owned navigation destination at zero, separate from the manual secondary action. |
