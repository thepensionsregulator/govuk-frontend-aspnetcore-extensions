using System;
using System.ComponentModel.DataAnnotations;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation
{
    /// <summary>Specifies the range constraints for the value of a date field.</summary>
    /// <remarks>
    /// Use a wrapper attribute for date ranges to avoid rendering the default data-val-range-* HTML attributes.
    /// A GOV.UK date component comprises three separate integer fields, and the default RangeAttribute would render
    /// data-val-range-* attributes specifing that each field would contain a complete date.
    /// 
    /// It also allows a property to use the DateOnly type rather than being forced to use DateTime.
    /// </remarks>
    public class DateRangeAttribute : ValidationAttribute
    {
        private readonly RangeAttribute _rangeAttribute;

        /// <summary>
        /// Create a new <see cref="RangeAttribute"/>
        /// </summary>
        /// <param name="minimum">An ISO 8601 date string in the format YYYY-MM-DD</param>
        /// <param name="maximum">An ISO 8601 date string in the format YYYY-MM-DD</param>
        public DateRangeAttribute(string minimum, string maximum)
        {
            _rangeAttribute = new RangeAttribute(typeof(DateTime), minimum, maximum);
        }

        public override bool IsValid(object? value)
        {
            if (value is DateOnly)
            {
                value = ((DateOnly)value).ToDateTime(new TimeOnly());
            }
            return _rangeAttribute.IsValid(value);
        }
    }
}