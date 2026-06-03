using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.ModelBinding
{
    /// <summary>
    /// Remove permitted but unwanted characters from postcodes and apply postcode formatting
    /// </summary>
    /// <remarks>
    /// <a href="https://design-system.service.gov.uk/patterns/addresses/#allow-different-postcode-formats">GOV.UK guidance on postcode validation</a> says 
    /// that various unwanted characters should be permitted, but we don't want every application to be responsible for cleaning up submitted data, so this
    /// model binder does it before the value is passed to an application controller.
    /// </remarks>
    public class UkPostcodeModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (!string.IsNullOrEmpty(value.FirstValue))
            {
                var postcode = value.FirstValue.Trim()
                    .ToUpper(CultureInfo.CurrentCulture)
                    .Replace("(", string.Empty)
                    .Replace(")", string.Empty)
                    .Replace("-", string.Empty) // hyphen
                    .Replace("–", string.Empty) // n-dash
                    .Replace(".", string.Empty);

                // Remove all the whitespace from the user
                postcode = Regex.Replace(postcode, @"\s+", string.Empty);

                // Put a single space at the correct location in the postcode
                if (postcode.Length > 3 && postcode[^4] != ' ')
                {
                    postcode = $"{postcode[..^3]} {postcode[^3..]}";
                }
                bindingContext.Result = ModelBindingResult.Success(postcode);
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(string.Empty);
            }
            return Task.CompletedTask;
        }
    }
}
