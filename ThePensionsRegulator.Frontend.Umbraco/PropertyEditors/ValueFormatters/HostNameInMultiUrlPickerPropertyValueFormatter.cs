using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using ThePensionsRegulator.Frontend.Services;
using ThePensionsRegulator.Umbraco.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Frontend.Umbraco.PropertyEditors.ValueFormatters
{
    /// <summary>
    /// Apply the configured <see cref="IContextAwareHostUpdater"/> to links configured in a multi-URL picker property editor.
    /// </summary>
    public class HostNameInMultiUrlPickerPropertyValueFormatter : IPropertyValueFormatter
    {
        private readonly string? _hostName;
        private readonly IContextAwareHostUpdater _contextAwareHostUpdater;

        public HostNameInMultiUrlPickerPropertyValueFormatter(IHttpContextAccessor httpContextAccessor, IContextAwareHostUpdater contextAwareHostUpdater)
        {
            if (httpContextAccessor == null) { throw new ArgumentNullException(nameof(httpContextAccessor)); }
            _hostName = httpContextAccessor.HttpContext?.Request?.Host.Host;
            if (string.IsNullOrEmpty(_hostName))
            {
                throw new ArgumentException($"{nameof(httpContextAccessor)} must return a context with an HTTP request and a hostname", nameof(httpContextAccessor));
            }
            _contextAwareHostUpdater = contextAwareHostUpdater ?? throw new ArgumentNullException(nameof(contextAwareHostUpdater));
        }

        /// <inheritdoc />
        public bool IsFormatter(IPublishedPropertyType propertyType) => Constants.PropertyEditors.Aliases.MultiUrlPicker.Equals(propertyType.EditorAlias);

        /// <inheritdoc />
        /// <remarks>
        /// This property type should return <see cref="Link"/> or <see cref="List&lt;Link&gt;"/> depending on what it was given.
        /// </remarks>
        public object FormatValue(object value)
        {
            if (value is Link singleLink)
            {
                if (!string.IsNullOrEmpty(singleLink.Url))
                {
                    singleLink.Url = _contextAwareHostUpdater.UpdateHost(singleLink.Url, _hostName!);
                }
            }

            else if (value is IEnumerable<Link> list)
            {
                foreach (var link in list)
                {
                    if (!string.IsNullOrEmpty(link.Url))
                    {
                        link.Url = _contextAwareHostUpdater.UpdateHost(link.Url, _hostName!);
                    }
                }
            }

            return value;
        }
    }
}
