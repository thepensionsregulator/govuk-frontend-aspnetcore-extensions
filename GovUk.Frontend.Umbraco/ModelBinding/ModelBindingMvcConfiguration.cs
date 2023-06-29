using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions.Configuration;
using GovUk.Frontend.AspNetCore.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.ModelBinding
{
    internal class ModelBindingMvcConfiguration : IConfigureOptions<MvcOptions>
    {
        private readonly IPublishedValueFallback? _publishedValueFallback;
        private readonly GovUkFrontendAspNetCoreOptions _options;

        public ModelBindingMvcConfiguration(GovUkFrontendAspNetCoreOptionsProvider optionsProvider, IPublishedValueFallback? publishedValueFallback)
        {
            _options = optionsProvider?.Options ?? throw new ArgumentNullException(nameof(optionsProvider));
            _publishedValueFallback = publishedValueFallback;
        }

        public void Configure(MvcOptions options)
        {
            // Replace the custom date model binder from the base project with a copy that has
            // been modified to make the error messages configurable in Umbraco
            var govukDateBinder = options.ModelBinderProviders.FirstOrDefault(x => x.GetType().FullName == typeof(DateInputModelBinderProvider).FullName);
            if (govukDateBinder != null) { options.ModelBinderProviders.Remove(govukDateBinder); }

            options.ModelBinderProviders.Insert(0, new UmbracoDateInputModelBinderProvider(_options, _publishedValueFallback));
        }
    }
}