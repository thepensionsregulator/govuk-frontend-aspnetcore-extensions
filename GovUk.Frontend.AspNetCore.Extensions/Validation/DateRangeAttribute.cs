using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

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
        /// <summary>
        /// Create a new <see cref="DateRangeAttribute"/>
        /// </summary>
        /// <param name="minimum">An ISO 8601 date string in the format YYYY-MM-DD</param>
        /// <param name="maximum">An ISO 8601 date string in the format YYYY-MM-DD</param>
        public DateRangeAttribute(string minimum, string maximum)
        {
            if (DateTime.TryParse(minimum, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var parsedMinimum))
            {
                Minimum = parsedMinimum;
            }
            else
            {
                throw new ArgumentException($"{nameof(minimum)} could not be parsed as a {nameof(DateTime)}", nameof(minimum));
            }
            if (DateTime.TryParse(maximum, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var parsedMaximum))
            {
                Maximum = parsedMaximum;
            }
            else
            {
                throw new ArgumentException($"{nameof(maximum)} could not be parsed as a {nameof(DateTime)}", nameof(maximum));
            }
        }

        /// <summary>
        /// Create a new <see cref="DateRangeAttribute"/> where the minimum and maximum dates are set using the <see cref="Minimum"/> and <see cref="Maximum"/> properties
        /// </summary>
        protected DateRangeAttribute() { }

        protected DateTime? Minimum { get; set; }
        protected DateTime? Maximum { get; set; }

        public override bool IsValid(object? value)
        {
            if (value is DateOnly)
            {
                value = ((DateOnly)value).ToDateTime(new TimeOnly());
            }

            if (!Minimum.HasValue) { throw new InvalidOperationException($"The {nameof(Minimum)} property cannot be null"); }
            if (!Maximum.HasValue) { throw new InvalidOperationException($"The {nameof(Maximum)} property cannot be null"); }

            var rangeAttribute = new RangeAttribute(typeof(DateTime), Minimum.Value.ToString("s", CultureInfo.InvariantCulture), Maximum.Value.ToString("s", CultureInfo.InvariantCulture));
            return rangeAttribute.IsValid(value);
        }
    }
}