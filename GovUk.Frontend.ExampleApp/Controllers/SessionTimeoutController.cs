using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Services;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class SessionTimeoutController : BaseController
    {
        public IActionResult Index()
        {
            return View(CreateMessages());
        }

        public IActionResult Preview(string? messageKey)
        {
            var message = TprSessionTimeOutMessageSelector.SelectByKey(CreateMessages(), messageKey);
            if (message is null)
            {
                return NotFound();
            }

            message.Settings = new SessionTimeoutSettings
            {
                RemainingTime = TimeSpan.FromSeconds(125),
                ExtensionLength = TimeSpan.FromMinutes(3),
                WarningBeforeExpiry = TimeSpan.FromMinutes(2),
                ExpiryUrl = "/sessiontimeout"
            };

            return View(message);
        }

        private static TprSessionTimeOutModel[] CreateMessages() =>
        [
            new TprSessionTimeOutModel
            {
                MessageKey = "default",
                Title = "Your session is about to time out",
                ContentHtml = new HtmlString("<p>For your security, we will end this session in <strong>{remainingTime}</strong>.</p><p>Do you need more time?</p>"),
                DisplayPrimaryAction = true,
                PrimaryActionText = "Continue",
                PrimaryActionUrl = "/session/extend",
                DisplaySecondaryAction = true,
                SecondaryActionText = "Sign out",
                SecondaryActionUrl = "/sign-out"
            },
            new TprSessionTimeOutModel
            {
                MessageKey = "application-warning",
                Title = "Your application is about to close",
                Content = "We will close your application if you do not do anything in the next {remainingTime}. Your answers will be deleted. This is to protect your information.",
                DisplayPrimaryAction = true,
                PrimaryActionText = "Continue",
                PrimaryActionUrl = "/session/extend"
            },
            new TprSessionTimeOutModel
            {
                MessageKey = "sign-out-warning",
                Title = "You will be signed out soon",
                Content = "For security reasons, we will sign you out in {remainingTime}.",
                DisplayPrimaryAction = true,
                PrimaryActionText = "Stay signed in",
                PrimaryActionUrl = "/session/extend",
                DisplaySecondaryAction = true,
                SecondaryActionText = "Sign out",
                SecondaryActionUrl = "/sign-out"
            }
        ];
    }
}
