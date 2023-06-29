using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.ModelBinding
{
    /// <summary>
    /// This is a copy of the date model binder provider from the base project, but this one works with our custom version of the date model binder 
    /// </summary>
    public class UmbracoDateInputModelBinderProvider : IModelBinderProvider
    {
        private readonly DateInputModelConverter[] _dateInputModelConverters;
        private readonly IPublishedValueFallback? _publishedValueFallback;

        public UmbracoDateInputModelBinderProvider(GovUkFrontendAspNetCoreOptions options, IPublishedValueFallback? publishedValueFallback)
        {
            Guard.ArgumentNotNull(nameof(options), options);

            _dateInputModelConverters = options.DateInputModelConverters.ToArray();
            _publishedValueFallback = publishedValueFallback;
        }

        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            Guard.ArgumentNotNull(nameof(context), context);

            var modelType = context.Metadata.UnderlyingOrModelType;

            foreach (var converter in _dateInputModelConverters)
            {
                if (converter.CanConvertModelType(modelType))
                {
                    return new UmbracoDateInputModelBinder(converter, _publishedValueFallback);
                }
            }

            return null;
        }
    }
}