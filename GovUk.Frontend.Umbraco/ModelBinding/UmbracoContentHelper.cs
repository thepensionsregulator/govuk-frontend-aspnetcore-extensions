using GovUk.Frontend.Umbraco.Services;
using Microsoft.AspNetCore.Http;
using System;
using Umbraco.Cms.Core.Dictionary;

namespace GovUk.Frontend.Umbraco.ModelBinding
{
    /// <summary>
    /// Uses IUmbracoPublishedContentAccessor to retrieve a value from umbraco settings
    /// </summary>
    internal static class UmbracoContentHelper
    {
        private static IServiceProvider GetServiceProvider()
        {
            var serviceProvider = new HttpContextAccessor()?.HttpContext?.RequestServices;

            if (serviceProvider is null)
            {
                throw new InvalidOperationException($"{nameof(serviceProvider)} service not found!");
            }

            return serviceProvider;
        }

        internal static string? GetSettingsValue(string? modelProperty, string umbracoProperty)
        {
            string? settingValue = null;

            if (modelProperty is not null)
            {
                IUmbracoPublishedContentAccessor? contentAccessor = GetServiceProvider()?.GetService(typeof(IUmbracoPublishedContentAccessor)) as IUmbracoPublishedContentAccessor;

                if (contentAccessor is null)
                {
                    throw new InvalidOperationException($"{nameof(contentAccessor)} service not found!");
                }

                var blocks = contentAccessor?.FindBlockByBoundProperty(modelProperty);

                settingValue = blocks?.Settings?.GetProperty(umbracoProperty)?.GetValue()?.ToString() ?? string.Empty;
            }

            return !string.IsNullOrWhiteSpace(settingValue) ? settingValue : null;
        }

        internal static string? GetDictionaryValue(string dictionaryKey)
        {
            var cultureDictionary = GetServiceProvider()?.GetService(typeof(ICultureDictionary)) as ICultureDictionary;

            if (cultureDictionary is null)
            {
                throw new InvalidOperationException($"{nameof(cultureDictionary)} service not found!");
            }

            return cultureDictionary[dictionaryKey];
        }
    }
}