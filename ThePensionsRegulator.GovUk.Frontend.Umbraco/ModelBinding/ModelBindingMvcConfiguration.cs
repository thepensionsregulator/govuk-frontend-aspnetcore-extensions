using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions.Configuration;
using GovUk.Frontend.AspNetCore.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Dictionary;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.ModelBinding
{
    internal class ModelBindingMvcConfiguration : IConfigureOptions<MvcOptions>
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly ICultureDictionary _cultureDictionary;
        private readonly IPublishedValueFallback? _publishedValueFallback;
        private readonly GovUkFrontendOptions _options;
        private readonly IUmbracoHelperAccessor _umbracoHelperAccessor;

        public ModelBindingMvcConfiguration(
            GovUkFrontendOptionsProvider optionsProvider,
            IUmbracoContextAccessor umbracoContextAccessor,
            ICultureDictionary cultureDictionary,
            IPublishedValueFallback? publishedValueFallback,
            IUmbracoHelperAccessor umbracoHelperAccessor)
        {
            _options = optionsProvider?.Options ?? throw new ArgumentNullException(nameof(optionsProvider));
            _umbracoContextAccessor = umbracoContextAccessor ?? throw new ArgumentNullException(nameof(umbracoContextAccessor));
            _cultureDictionary = cultureDictionary ?? throw new ArgumentNullException(nameof(cultureDictionary));
            _publishedValueFallback = publishedValueFallback;
            _umbracoHelperAccessor = umbracoHelperAccessor;
        }

        public void Configure(MvcOptions options)
        {
            // Replace the custom date model binder from the base project with a copy that has
            // been modified to make the error messages configurable in Umbraco
            var govukDateBinder = options.ModelBinderProviders.FirstOrDefault(x => x.GetType().FullName == typeof(DateInputModelBinderProvider).FullName);
            if (govukDateBinder != null) { options.ModelBinderProviders.Remove(govukDateBinder); }

            options.ModelBinderProviders.Insert(0, new UmbracoDateInputModelBinderProvider(_options, _umbracoContextAccessor, _cultureDictionary, _publishedValueFallback, _umbracoHelperAccessor));
        }
    }
}