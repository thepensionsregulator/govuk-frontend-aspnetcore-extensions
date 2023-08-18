using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace ThePensionsRegulator.Umbraco.PropertyEditors
{
    /// <summary>
    /// Format the value returned by a supporting <see cref="IPropertyValueConverter"/>.
    /// </summary>
    public interface IPropertyValueFormatter
    {
        /// <summary>
        /// Format the value returned by a supporting <see cref="IPropertyValueConverter"/>.
        /// </summary>
        /// <param name="value">The value to format</param>
        /// <returns>The formatted value</returns>
        object FormatValue(object? value);

        /// <summary>
        /// Gets a value indicating whether the formatter supports a property type.
        /// </summary>
        /// <param name="propertyType">The property type.</param>
        /// <returns>A value indicating whether the formatter supports a property type.</returns>
        bool IsFormatter(IPublishedPropertyType propertyType);
    }
}
