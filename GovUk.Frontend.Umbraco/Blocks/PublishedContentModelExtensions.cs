﻿using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.Blocks
{
	public static class PublishedContentModelExtensions
	{
		/// <summary>
		/// Gets the page title from a page heading block if present, otherwise from the current content node name.
		/// </summary>
		/// <param name="content">The model for the current content node.</param>
		/// <returns>A page title.</returns>
		public static string? PageHeadingOrName(this PublishedContentModel content)
		{
			var pageHeading = content.FindOverridableBlockModels().FindBlockByContentTypeAlias(ElementTypeAliases.PageHeading);
			if (pageHeading is not null)
			{
				var text = pageHeading.Content.Value<string>(PropertyAliases.PageHeading);
				if (!string.IsNullOrWhiteSpace(text)) { return text; }
			}

			return content.Name;
		}
	}
}
