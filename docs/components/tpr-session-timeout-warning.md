# TPR session timeout warning

Displays a session timeout warning message in a modal-style block with an overlay. The component supports:

- Multiple configured message variants (selected by key)
- Button-only session extension calls
- Client-side warning countdown based on elapsed time or inactivity
- Keyboard focus management when the warning is displayed

## Render the component

Render the warning by passing a `TprSessionTimeOutModel` to the `TPR/TPRSessionTimeOutWarning` partial view.

```razor
@using ThePensionsRegulator.Frontend.Models
@{
	var warning = new TprSessionTimeOutModel
	{
		Title = "For your security, we will sign you out soon",
		Content = "You have been inactive for 15 minutes.",
		DisplayPrimaryAction = true,
		PrimaryActionText = "Continue session",
		PrimaryActionUrl = "/session/extend",
		DisplaySecondaryAction = true,
		SecondaryActionText = "Sign out",
		SecondaryActionUrl = "/account/sign-out",
		EnableClientSideSessionTimeout = true,
		SessionTimeoutMilliseconds = 900000,
		ResetTimeoutCountdownOnActivity = true
	};
}

<partial name="TPR/TPRSessionTimeOutWarning" model="warning" />
```

## Behaviour

When `EnableClientSideSessionTimeout = true`:

- The component starts hidden and is shown after `SessionTimeoutMilliseconds`.
- Focus moves to the warning block when shown.
- The warning countdown is reset only in these cases:
  - The user clicks the primary action button and the extend request succeeds.
  - `ResetTimeoutCountdownOnActivity = true` and user activity is detected.

User activity events are: `click`, `keydown`, `pointerdown`, `scroll`, `touchstart`, `mousemove`.

`mousemove` is throttled to avoid excessive timer resets.

## Countdown mode guidance for consuming applications

Use these settings depending on your requirement:

- **Fixed elapsed countdown**:
  - `ResetTimeoutCountdownOnActivity = false`
  - The warning appears after a fixed interval since page load or the last successful extend click.

- **Inactivity countdown**:
  - `ResetTimeoutCountdownOnActivity = true`
  - User activity resets the warning countdown.

Set `SessionTimeoutMilliseconds` to your warning threshold, for example 20 minutes:

```csharp
SessionTimeoutMilliseconds = 20 * 60 * 1000 // 1200000
```

## Integrating with your session endpoint

The primary action URL should point to an endpoint that extends the authenticated session and returns a success response (`2xx`).

The client script sends:

- `POST` request
- `credentials: "same-origin"`

If the response is successful, the warning timer resets and the warning is hidden.

## Layout script requirement

Client-side behaviour is provided by `tpr-session-timeout-warning.min.js`.

This script is included by default when your layout renders:

```razor
<partial name="TPR/BodyClosing" />
```

If you do not use `TPR/BodyClosing`, include the script manually:

```html
<script src="/ThePensionsRegulator.Frontend/js/tpr-session-timeout-warning.min.js" type="module" asp-append-version="true"></script>
```

## Model reference

`TprSessionTimeOutModel` properties:

- `MessageKey` - identifier used when selecting a message variant.
- `Title` - warning heading text.
- `Content` - plain text message content.
- `ContentHtml` - rich text/HTML message content (used in preference to `Content`).
- `DisplayPrimaryAction` - whether to render the primary action button.
- `PrimaryActionText` - text for the primary action button.
- `PrimaryActionUrl` - URL used for the extend-session POST.
- `DisplaySecondaryAction` - whether to render the secondary action link.
- `SecondaryActionText` - text for the secondary action link.
- `SecondaryActionUrl` - URL for the secondary action link.
- `EnableClientSideSessionTimeout` - enables JavaScript countdown behaviour.
- `SessionTimeoutMilliseconds` - warning delay in milliseconds.
- `ResetTimeoutCountdownOnActivity` - chooses inactivity mode for the warning countdown.

## Umbraco

In Umbraco solutions, session timeout message variants can be configured in settings and selected by key, then rendered with the same `TPRSessionTimeOutWarning` partial.

Use `TprSessionTimeOutMessageSelector.SelectByKey(...)` to select the appropriate configured message, with fallback to the first configured message.
