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

The JavaScript replaces each placeholder with an inline timer, preserving surrounding text, links and formatting. If no placeholder is present, it adds a countdown at the end of the content area, before the buttons. Existing Umbraco messages containing a fixed duration such as `2 minutes` should be edited to use `{remainingTime}` and republished; the example uSync content uses this token too.

## Initialise the timeout behaviour

Import `initSessionTimeout` from the published module after rendering the partial. All durations are in milliseconds. These example values belong to the consuming application, not the UI library:

```razor
@Html.AntiForgeryToken()

<script type="module">
    import { initSessionTimeout } from '/ThePensionsRegulator.Frontend/js/tpr-session-timeout-message.js';

    const dialog = document.querySelector('.tpr-session-timeout-warning');
    const timeout = initSessionTimeout(dialog, {
        timeoutMs: 20 * 60 * 1000,
        extensionMs: 20 * 60 * 1000,
        warningMs: 2 * 60 * 1000,
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
| `timeoutMs` | Remaining session lifetime when initialisation runs. Required, finite and greater than zero. |
| `extensionMs` | New remaining lifetime after a successful extension, measured from callback completion. Required, finite and greater than zero. |
| `warningMs` | How long before expiry to open the warning. Required, finite, non-negative and no greater than `timeoutMs`. |
| `extendSession` | Required application callback, receiving the button URL (or `undefined` if absent) and returning a boolean or a promise of a boolean. |
| `formatRemainingTime` | Optional callback receiving non-negative remaining milliseconds and returning display text. Defaults to minutes and seconds. Use it for alternative wording or minute-only text before the seconds countdown; it works without Umbraco too. |

Initialise when the application's session timing is known. If the session is already running, pass its remaining lifetime as `timeoutMs`, rather than the original full duration. Keep these values consistent with the server; the UI timer does not create, authenticate or extend a server session.

The module:

- Opens the existing warning with `showModal()` at the configured warning point.
- Calculates remaining time from `expiryTime - Date.now()` on every update, including after delayed callbacks. It refreshes immediately on `visibilitychange` and `pageshow` after browser suspension.
- Replaces `{remainingTime}` in content with inline `data-session-timeout-countdown` elements, displaying minutes and seconds with singular/plural labels. It rounds partial seconds up, stops at zero and never displays negative time.
- Marks countdowns with `role="timer"` and `aria-live="off"`, so visual updates are not live announcements every second. The dialog description includes the current time when opened, and the timer remains available to screen-reader users.
- Disables the primary button while an extension is pending. On success it closes the warning and resets expiry to the current time plus `extensionMs`. On failure the original expiry and countdown continue, and the button becomes available to retry.
- Logs extension failures and emits a bubbling `tpr-session-timeout:error` event on the dialog, with the error in `event.detail`. Your application can listen to this event to show its own error notification.
- Returns the same controller when initialised again for the same dialog, without resetting expiry or duplicating timers, markup or handlers. The original configuration remains in use.

The returned controller exposes the read-only `expiryTime` timestamp and a `destroy()` method to remove timers, event handlers and the generated countdown. Call `destroy()` before removing the component or initialising it with different configuration. Pending extensions cannot restart a destroyed controller. Reaching zero does not sign out or redirect the user; expiry policy remains with your application.

The example Umbraco application's `/session-timeout` page displays the configured variants inline, replacing `{remainingTime}` with a static example value of `2 minutes`; it does not start a countdown. Its `/session-timeout-preview` page uses application-owned demo settings: a 125-second timeout, a warning 2 minutes before expiry (after 5 seconds) and a 3-minute extension. Its `formatRemainingTime` callback rounds minutes up: `2 minutes` stays displayed for a full minute, then switches to `60 seconds` when one minute remains and counts down to zero. Longer durations decrease once per elapsed minute (for example, `30 minutes`, then `29 minutes` after one minute). Unchanged text is not rewritten each second. The preview's extension callback simulates success without making a server request or changing a real session. At zero it does not sign out or redirect; reload to restart. Message configuration and selection by key are unchanged.

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
