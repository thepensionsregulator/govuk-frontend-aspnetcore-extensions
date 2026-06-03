using Microsoft.AspNetCore.Mvc.Rendering;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation
{
    public interface IClientSideValidationHtmlEnhancer
    {
        string EnhanceHtml(string html,
            ViewContext viewContext,
            string? errorMessageRequired,
            string? errorMessageRegex,
            string? errorMessageEmail,
            string? errorMessagePhone,
            string? errorMessageLength,
            string? errorMessageMinLength,
            string? errorMessageMaxLength,
            string? errorMessageRange,
            string? errorMessageCompare);
    }
}