using System.ComponentModel;
using System.Globalization;
using Umbraco.Cms.Core.Models.Blocks;

namespace ThePensionsRegulator.Umbraco.Blocks
{
    internal class OverridableBlockGridTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) => sourceType == typeof(OverridableBlockGridModel);

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) => destinationType == typeof(BlockGridModel);

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value?.GetType() == typeof(OverridableBlockGridModel) && destinationType == typeof(BlockGridModel))
            {
                return (BlockGridModel)(OverridableBlockGridModel)value;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
