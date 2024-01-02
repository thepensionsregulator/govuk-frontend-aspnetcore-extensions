using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.PropertyEditors
{
    public static class PropertyValueFormatterExtensions
    {
        /// <summary>
        /// Apply property value formatters compatible with a property type to a value from a property of that type.
        /// </summary>
        /// <param name="formatters">The formatters to apply, if compatible.</param>
        /// <param name="propertyType">The property type.</param>
        /// <param name="value">The value.</param>
        /// <returns>The modified value, after applying all compatible formatters.</returns>
        /// <exception cref="ArgumentNullException">Thrown if any argument is <c>null</c>.</exception>
        public static object ApplyFormatters(this IEnumerable<IPropertyValueFormatter> formatters, IPublishedPropertyType propertyType, object value)
        {
            if (formatters is null)
            {
                throw new ArgumentNullException(nameof(formatters));
            }

            if (propertyType is null)
            {
                throw new ArgumentNullException(nameof(propertyType));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value is OverridableBlockListModel blockList && blockList.PropertyValueFormatters is null)
            {
                blockList.PropertyValueFormatters = formatters;
            }

            if (value is OverridableBlockGridModel blockGrid && blockGrid.PropertyValueFormatters is null)
            {
                blockGrid.PropertyValueFormatters = formatters;
            }

            foreach (var formatter in formatters)
            {
                if (formatter.IsFormatter(propertyType))
                {
                    value = formatter.FormatValue(value);
                }
            }
            return value;
        }
    }
}
