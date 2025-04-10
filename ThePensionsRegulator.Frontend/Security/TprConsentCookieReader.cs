using GovUk.Frontend.AspNetCore.Extensions.Security;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace ThePensionsRegulator.Frontend.Security
{
    public class TprConsentCookieReader : IConsentCookieReader
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public const string TPR_CONSENT_COOKIE_NAME = "TPR_Cookie_CONSENT";
        public const string TPR_CONSENT_CATEGORY_PERFORMANCE = "performance";
        public const string TPR_CONSENT_CATEGORY_FUNCTIONALITY = "functionality";
        public const string TPR_CONSENT_CATEGORY_TARGETING = "targeting";

        public TprConsentCookieReader(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <inheritdoc/>
        public bool HasConsent()
        {
            var context = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext cannot be null");
            return (context.Request.Cookies.TryGetValue(TPR_CONSENT_COOKIE_NAME, out var cookieValue) && cookieValue == "All|");
        }

        /// <inheritdoc/>
        public bool HasConsent(string category)
        {
            var context = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext cannot be null");
            if (context.Request.Cookies.TryGetValue(TPR_CONSENT_COOKIE_NAME, out var cookieValue))
            {
                if (cookieValue == "All|") { return true; }
                if (cookieValue == "Reject|") { return false; }
                var categories = cookieValue.Split('|').Select(x => x.Split('='));
                foreach (var consentCategory in categories)
                {
                    if (consentCategory.Count() != 2) { throw new InvalidOperationException($"Invalid TPR consent cookie value {cookieValue}"); }
                    if (consentCategory[0].Equals(category, StringComparison.OrdinalIgnoreCase)) { return consentCategory[1] == "On"; }
                }
            }
            return false;
        }
    }
}
