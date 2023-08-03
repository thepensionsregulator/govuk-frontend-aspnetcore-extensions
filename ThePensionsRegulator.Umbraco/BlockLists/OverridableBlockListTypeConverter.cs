using System.ComponentModel;
using System.Globalization;
using Umbraco.Cms.Core.Models.Blocks;

namespace ThePensionsRegulator.Umbraco.BlockLists
{
    internal class OverridableBlockListTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) => sourceType == typeof(OverridableBlockListModel);

        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) => destinationType == typeof(BlockListModel);

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        {
            if (value?.GetType() == typeof(OverridableBlockListModel) && destinationType == typeof(BlockListModel))
            {
                return (BlockListModel)(OverridableBlockListModel)value;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
