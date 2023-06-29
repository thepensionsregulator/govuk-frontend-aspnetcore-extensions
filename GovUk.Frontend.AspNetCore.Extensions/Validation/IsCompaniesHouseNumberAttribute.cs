using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation
{
    /// <summary>
    /// Validates a UK companies house number
    /// </summary>
    /// The companies house number should consist of
    ///
    /// - 8 numbers
    /// - 2 letters and 6 numbers
    /// - 1 letter and 7 numbers
    public class IsCompaniesHouseNumberAttribute : RegularExpressionAttribute
    {
        private const string EIGHT_NUMBERS_ONLY = @"(^[0-9]{8}$)";
        private const string TWO_CHARS_SIX_NUMBERS = @"(^[a-zA-Z]{2}[0-9]{6}$)";
        private const string ONE_CHAR_SEVEN_NUMBERS = @"(^[a-zA-Z]{1}[0-9]{7}$)";

        public IsCompaniesHouseNumberAttribute() : base($@"{EIGHT_NUMBERS_ONLY}|{TWO_CHARS_SIX_NUMBERS}|{ONE_CHAR_SEVEN_NUMBERS}")
        {
            ErrorMessage = "Enter a valid companies house number";
        }
    }
}