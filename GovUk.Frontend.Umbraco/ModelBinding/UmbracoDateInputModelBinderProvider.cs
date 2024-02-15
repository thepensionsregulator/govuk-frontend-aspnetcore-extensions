using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.AspNetCore.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using Umbraco.Cms.Core.Dictionary;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace GovUk.Frontend.Umbraco.ModelBinding
{
    /// <summary>
    /// This is a copy of the date model binder provider from the base project, but this one works with our custom version of the date model binder 
    /// </summary>
    public class UmbracoDateInputModelBinderProvider : IModelBinderProvider
    {
        private readonly DateInputModelConverter[] _dateInputModelConverters;
        private readonly bool _acceptMonthNamesInDateInputs;
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly ICultureDictionary _cultureDictionary;
        private readonly IPublishedValueFallback? _publishedValueFallback;

        public UmbracoDateInputModelBinderProvider(
            GovUkFrontendAspNetCoreOptions options,
            IUmbracoContextAccessor umbracoContextAccessor,
            ICultureDictionary cultureDictionary,
            IPublishedValueFallback? publishedValueFallback)
        {
            Guard.ArgumentNotNull(nameof(options), options);

            _dateInputModelConverters = options.DateInputModelConverters.ToArray();
            _acceptMonthNamesInDateInputs = options.AcceptMonthNamesInDateInputs;
            _umbracoContextAccessor = umbracoContextAccessor ?? throw new ArgumentNullException(nameof(umbracoContextAccessor));
            _cultureDictionary = cultureDictionary ?? throw new ArgumentNullException(nameof(cultureDictionary));
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
                    return new UmbracoDateInputModelBinder(converter, _umbracoContextAccessor, _cultureDictionary, _publishedValueFallback, _acceptMonthNamesInDateInputs);
                }
            }

            return null;
        }
    }
}