using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation
{
    public class RegisteredCharityNumberAttribute : RegularExpressionAttribute
    {
        private const string SIX_NUMBERS_ONLY = @"(^[0-9]{6}$)";
        private const string SEVEN_NUMBERS_ONLY = @"(^[0-9]{7}$)";
        private const string SC_PREFIX_AND_SIX_NUMBERS = @"(^[Ss][Cc][0-9]{6}$)";

        public RegisteredCharityNumberAttribute()
            : base($@"{SIX_NUMBERS_ONLY}|{SEVEN_NUMBERS_ONLY}|{SC_PREFIX_AND_SIX_NUMBERS}")
        {
            ErrorMessage = "Enter a registered charity number in the correct format, such as 123456, 1234567 or SC123456";
        }
    }
}