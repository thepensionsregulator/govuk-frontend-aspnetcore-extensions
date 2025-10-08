using System;
using System.Collections.Generic;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using Umbraco.Cms.Core.Models;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public interface ITprGlobalNavigationService
    {
        public IList<TprHeaderMenuParentItem>? GetMenuItems(IContent settingsNode, string propertyAlias, string linkTextAlias, string linkUrlAlias, string childAlias);
        public string? GetUrlFromJson(string? json);
    }
}
