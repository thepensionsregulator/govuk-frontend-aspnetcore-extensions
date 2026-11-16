# TPR session timeout warning

The TPR session timeout warning is a reusable modal dialog for showing a configurable warning before a user's session expires. It is rendered using the `TPR/TPRSessionTimeOutWarning` Razor partial and styled by the TPR stylesheet.

The component provides the dialog markup and message model. Your application is responsible for deciding when to show it, starting and tracking the timeout, and implementing the action that extends or ends the session. The primary action is rendered as a button with `data-session-timeout-extend` and `data-session-timeout-extend-url` attributes; the component does not itself call the URL or extend the session. The secondary action is a link.

## Use in an ASP.NET application

Create a `TprSessionTimeOutModel` and render the partial. Load the TPR stylesheet and add application JavaScript to open the dialog at the appropriate time and handle its actions.

```razor
@using ThePensionsRegulator.Frontend.Models

@{
	var message = new TprSessionTimeOutModel
	{
		Title = "Your session is about to expire",
		Content = "Select continue to keep working, or sign out.",
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

`PrimaryActionUrl` is exposed on the button as a data attribute for your application script to use. The partial does not provide the timer or the behavior for either action. The component uses a native `<dialog>` element with `role="alertdialog"`, and associates its title and description using `aria-labelledby` and `aria-describedby`.

A dialog is hidden until your application opens it. Call `showModal()` to display the warning as a modal dialog (with the rest of the page made inactive), or `show()` to display it non-modally. For example, your timeout script can select the rendered dialog and call `showModal()` when the warning period begins:

```javascript
var dialog = document.querySelector('.tpr-session-timeout-warning');
dialog.showModal();
```

The example Umbraco application includes `/session-timeout` and `/session-timeout-preview` pages to demonstrate configured messages. Its demo script opens the dialog on page load and closes it when the primary action is clicked; this is demonstration behavior, not a session timeout implementation.

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
