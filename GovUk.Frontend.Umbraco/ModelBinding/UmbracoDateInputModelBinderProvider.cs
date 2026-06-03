using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.AspNetCore.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Umbraco.Cms.Core.Dictionary;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common;
using CopiedInternalClasses = GovUk.Frontend.Umbraco.ModelBinding;

namespace GovUk.Frontend.Umbraco.ModelBinding
{
    /// <summary>
    /// This is a copy of the date model binder provider from the base project, but this one works with our custom version of the date model binder 
    /// </summary>
    public class UmbracoDateInputModelBinderProvider : IModelBinderProvider
    {
        private readonly Dictionary<Type, DateInputModelConverter> _dateInputModelConverters;
        private readonly bool _acceptMonthNamesInDateInputs;
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly ICultureDictionary _cultureDictionary;
        private readonly IPublishedValueFallback? _publishedValueFallback;
        private readonly IUmbracoHelperAccessor _umbracoHelperAccessor;

        public UmbracoDateInputModelBinderProvider(
            GovUkFrontendOptions options,
            IUmbracoContextAccessor umbracoContextAccessor,
            ICultureDictionary cultureDictionary,
            IPublishedValueFallback? publishedValueFallback,
            IUmbracoHelperAccessor umbracoHelperAccessor)
        {
            Guard.ArgumentNotNull(nameof(options), options);

            _dateInputModelConverters = new Dictionary<Type, DateInputModelConverter>
            {
                { CopiedInternalClasses.DateTimeDateInputModelConverter.ModelType, new CopiedInternalClasses.DateTimeDateInputModelConverter() },
                { CopiedInternalClasses.DateOnlyDateInputModelConverter.ModelType, new CopiedInternalClasses.DateOnlyDateInputModelConverter() }
            };
            _acceptMonthNamesInDateInputs = options.AcceptMonthNamesInDateInputs;
            _umbracoContextAccessor = umbracoContextAccessor ?? throw new ArgumentNullException(nameof(umbracoContextAccessor));
            _cultureDictionary = cultureDictionary ?? throw new ArgumentNullException(nameof(cultureDictionary));
            _publishedValueFallback = publishedValueFallback;
            _umbracoHelperAccessor = umbracoHelperAccessor;
        }

        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            Guard.ArgumentNotNull(nameof(context), context);

            var modelType = context.Metadata.UnderlyingOrModelType;

            foreach (var convertableType in _dateInputModelConverters.Keys)
            {
                if (convertableType == modelType)
                {
                    return new UmbracoDateInputModelBinder(_dateInputModelConverters[convertableType], _umbracoContextAccessor, _cultureDictionary, _publishedValueFallback, _acceptMonthNamesInDateInputs, _umbracoHelperAccessor);
                }
            }

            return null;
        }
    }
}