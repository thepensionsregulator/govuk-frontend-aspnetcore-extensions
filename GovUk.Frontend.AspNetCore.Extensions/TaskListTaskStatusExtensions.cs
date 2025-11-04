using Microsoft.AspNetCore.Html;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace GovUk.Frontend.AspNetCore.Extensions
{
    public static partial class TaskListTaskStatusExtensions
    {
        public static string AsText(this TaskListTaskStatus status, IHtmlContent? customStatus = null) => AsText((TaskListTaskStatus?)status, customStatus);

        public static string AsText(this TaskListTaskStatus? status, IHtmlContent? customStatus = null)
        {
            if (!string.IsNullOrEmpty(customStatus?.ToHtmlString(HtmlEncoder.Default)))
            {
                return customStatus.ToHtmlString(HtmlEncoder.Default);
            }
            else if (status.HasValue)
            {
                var statusText = CapitalLetters().Replace(status.ToString()!, " $1").ToLowerInvariant().Trim();
                return statusText.Substring(0, 1).ToUpperInvariant() + statusText.Substring(1);
            }
            else return string.Empty;
        }

        [GeneratedRegex("([A-Z])")]
        private static partial Regex CapitalLetters();
    }
}