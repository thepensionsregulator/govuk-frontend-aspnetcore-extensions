using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace GovUk.Frontend.AspNetCore.Extensions.ModelBinding
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
