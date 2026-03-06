using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.ModelBinding;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.ModelBinding
{
    /// <summary>
    /// Copied from GovUk.Frontend.AspNetCore
    /// </summary>
    internal class DateOnlyDateInputModelConverter : DateInputModelConverter
    {
        public static Type ModelType => typeof(DateOnly);

        public override DateInputItemTypes? DefaultItemTypes => null;

        protected override object ConvertToModelCore(DateInputConvertToModelContext context)
        {
            var values = context.ItemValues;
            return new DateOnly(values.Year!.Value, values.Month!.Value, values.Day!.Value);
        }

        protected override DateInputItemValues? ConvertFromModelCore(DateInputConvertFromModelContext context)
        {
            var dateOnly = (DateOnly)context.Model;
            return new DateInputItemValues(dateOnly.Day, dateOnly.Month, dateOnly.Year);
        }
    }
}