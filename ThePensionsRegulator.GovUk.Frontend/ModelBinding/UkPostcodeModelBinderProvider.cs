using Microsoft.AspNetCore.Mvc.ModelBinding;
using ThePensionsRegulator.GovUk.Frontend.Validation;

namespace ThePensionsRegulator.GovUk.Frontend.ModelBinding
{
    public class UkPostcodeModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            Guard.ArgumentNotNull(nameof(context), context);

            if (context.Metadata.UnderlyingOrModelType == typeof(string) &&
                context.Metadata.ValidatorMetadata.Any(x => x.GetType() == typeof(UkPostcodeAttribute)))
            {
                return new UkPostcodeModelBinder();
            }

            return null;
        }
    }
}
