using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Umbraco;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace GovUk.Frontend.Umbraco.ExampleApp.Services
{
    public static class SessionTimeoutMessagesBuilder
    {
        public static List<TprSessionTimeOutModel> Build(
            IPublishedContent? settings,
            IPublishedValueFallback publishedValueFallback)
        {
            List<TprSessionTimeOutModel> messages = [];
            var variants = settings?.Value<OverridableBlockListModel>(
                publishedValueFallback,
                TprPropertyAliases.SessionTimeoutMessagesBlockList);

            if (variants == null) return messages;

            foreach (var variant in variants)
            {
                var content = variant?.Content;
                if (content == null) continue;

                var richText = content.Value<IHtmlEncodedString>(
                    publishedValueFallback, TprPropertyAliases.SessionTimeoutContent);
                var primaryActionText = content.Value<string>(
                    publishedValueFallback, TprPropertyAliases.SessionTimeoutPrimaryActionText);
                var secondaryActionText = content.Value<string>(
                    publishedValueFallback, TprPropertyAliases.SessionTimeoutSecondaryActionText);
                var primaryActionLink = content.Value<Link>(
                    publishedValueFallback, TprPropertyAliases.SessionTimeoutPrimaryActionUrl);
                var secondaryActionLink = content.Value<Link>(
                    publishedValueFallback, TprPropertyAliases.SessionTimeoutSecondaryActionUrl);

                messages.Add(new TprSessionTimeOutModel
                {
                    MessageKey = content.Value<string>(publishedValueFallback, TprPropertyAliases.SessionTimeoutMessageKey),
                    Title = content.Value<string>(publishedValueFallback, TprPropertyAliases.SessionTimeoutTitle),
                    ContentHtml = richText is null ? null : new HtmlString(richText.ToHtmlString()),
                    DisplayPrimaryAction = !string.IsNullOrWhiteSpace(primaryActionText),
                    PrimaryActionText = primaryActionText,
                    PrimaryActionUrl = primaryActionLink?.Url,
                    DisplaySecondaryAction = !string.IsNullOrWhiteSpace(secondaryActionText)
                        && !string.IsNullOrWhiteSpace(secondaryActionLink?.Url),
                    SecondaryActionText = secondaryActionText,
                    SecondaryActionUrl = secondaryActionLink?.Url
                });
            }

            return messages;
        }
    }
}
