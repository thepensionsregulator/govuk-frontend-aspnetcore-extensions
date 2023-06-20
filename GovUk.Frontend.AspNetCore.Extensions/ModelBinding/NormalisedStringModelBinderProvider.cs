using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GovUk.Frontend.AspNetCore.Extensions.ModelBinding
{
    /// <remarks>
    /// When running in a Windows environment, MaxLengthAttribute and StringLengthAttribute treat a new line as two characters, \r\n.
    /// This does not align with users' expectations or with client-side validation using jQuery validate, so register a custom model binder 
    /// when either of those attributes are in use.
    /// </remarks>
    public class NormalisedStringModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            Guard.ArgumentNotNull(nameof(context), context);

            if (context.Metadata.UnderlyingOrModelType == typeof(string) &&
                (context.Metadata.ValidatorMetadata.Any(x => x.GetType() == typeof(MaxLengthAttribute)) ||
                 context.Metadata.ValidatorMetadata.Any(x => x.GetType() == typeof(StringLengthAttribute))))
            {
                return new NormalisedStringModelBinder();
            }

            return null;
        }
    }
}
