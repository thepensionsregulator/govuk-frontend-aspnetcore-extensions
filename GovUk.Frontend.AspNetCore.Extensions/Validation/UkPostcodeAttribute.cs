using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation
{
    /// <summary>Specifies that a data field value must be a UK postcode</summary>
    /// <remarks>
    /// <a href="https://design-system.service.gov.uk/patterns/addresses/#allow-different-postcode-formats">GOV.UK guidance on postcode validation</a> says 
    /// you should let users enter postcodes that contain:
    /// 
    /// - upper and lower case letters
    /// - no spaces
    /// - additional spaces at the beginning, middle or end
    /// - punctuation like hyphens, brackets, dashes and full stops
    /// </remarks>
    public class UkPostcodeAttribute : RegularExpressionAttribute
    {
        private const string ALLOWED_PUNCTUATION = @"()-."; // can't allow n-dash here as the regex stops working 
        private const string ALLOWED_PUNCTUATION_OR_WHITESPACE = $@"[\s{ALLOWED_PUNCTUATION}]*";

        public UkPostcodeAttribute() : base($@"^{ALLOWED_PUNCTUATION_OR_WHITESPACE}[A-Za-z{ALLOWED_PUNCTUATION}]{{1,2}}{ALLOWED_PUNCTUATION_OR_WHITESPACE}[0-9{ALLOWED_PUNCTUATION}]{{1,2}}{ALLOWED_PUNCTUATION_OR_WHITESPACE}[A-Za-z{ALLOWED_PUNCTUATION}]?{ALLOWED_PUNCTUATION_OR_WHITESPACE}[0-9{ALLOWED_PUNCTUATION}]{ALLOWED_PUNCTUATION_OR_WHITESPACE}[ABDEFGHJLNPQRSTUWXYZabdefghjlnpqrstuwxyz{ALLOWED_PUNCTUATION}]{{2}}{ALLOWED_PUNCTUATION_OR_WHITESPACE}$")
        {
            ErrorMessage = "Enter a real postcode";
        }
    }
}
