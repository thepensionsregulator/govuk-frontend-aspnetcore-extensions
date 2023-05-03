using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GovUk.Frontend.Umbraco.ModelBinding
{
    public class UmbracoDateInputModelBinderProvider : IModelBinderProvider
    {
        private readonly DateInputModelConverter[] _dateInputModelConverters;

        public UmbracoDateInputModelBinderProvider(GovUkFrontendAspNetCoreOptions options)
        {
            Guard.ArgumentNotNull(nameof(options), options);

            _dateInputModelConverters = options.DateInputModelConverters.ToArray();
        }

        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            Guard.ArgumentNotNull(nameof(context), context);

            var modelType = context.Metadata.UnderlyingOrModelType;

            foreach (var converter in _dateInputModelConverters)
            {
                if (converter.CanConvertModelType(modelType))
                {
                    return new UmbracoDateInputModelBinder(converter);
                }
            }

            return null;
        }
    }
}
