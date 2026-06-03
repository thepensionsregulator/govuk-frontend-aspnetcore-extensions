using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Extensions.Typography
{
    public static class GovUkTypography
    {
        public static string Apply(string? html, TypographyOptions? options = null)
        {
            if (!string.IsNullOrWhiteSpace(html))
            {
                options = options ?? new TypographyOptions();

                var document = new HtmlDocument();
                document.LoadHtml(html);

                ApplyClass(document, "//a", "govuk-link");
                if (options.BackgroundType == BackgroundType.Dark)
                {
                    ApplyInverseClasses(document);
                }

                var allHeadingClasses = new[] { "govuk-heading-xl", "govuk-heading-l", "govuk-heading-m", "govuk-heading-s" };
                ApplyClass(document, "//h2", options.HeadingClasses.Heading2, allHeadingClasses);
                ApplyClass(document, "//h3", options.HeadingClasses.Heading3, allHeadingClasses);
                ApplyClass(document, "//h4", options.HeadingClasses.Heading4, allHeadingClasses);
                ApplyClass(document, "//h5", options.HeadingClasses.Heading5, allHeadingClasses);
                ApplyClass(document, "//h6", options.HeadingClasses.Heading6, allHeadingClasses);
                ApplyClass(document, "//p", "govuk-body");
                ApplyClass(document, "//ul", "govuk-list");
                ApplyClass(document, "//ul", "govuk-list--bullet");
                ApplyClass(document, "//ol", "govuk-list");
                ApplyClass(document, "//ol", "govuk-list--number");
                ApplyClass(document, "//table", "govuk-table");
                ApplyClass(document, "//caption", "govuk-table__caption");
                ApplyClass(document, "//thead", "govuk-table__head");
                ApplyClass(document, "//tbody", "govuk-table__body");
                ApplyClass(document, "//tr", "govuk-table__row");
                ApplyClass(document, "//th", "govuk-table__header");
                ApplyClass(document, "//td", "govuk-table__cell");

                return document.DocumentNode.OuterHtml;
            }
            return string.Empty;
        }

        private static void ApplyInverseClasses(HtmlDocument document)
        {
            ApplyClass(document, "//a", "govuk-link--inverse");
        }

        private static void ApplyClass(HtmlDocument doc, string xpath, string className, IEnumerable<string>? unlessTheseClassesAreApplied = null)
        {
            var nodes = doc.DocumentNode.SelectNodes(xpath);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var addClass = true;
                    if (unlessTheseClassesAreApplied != null)
                    {
                        foreach (var unlessClass in unlessTheseClassesAreApplied)
                        {
                            if (node.HasClass(unlessClass))
                            {
                                addClass = false;
                                break;
                            }
                        }
                    }
                    if (addClass)
                    {
                        node.AddClass(className);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the default classes to apply to HTML heading levels.
        /// </summary>
        /// <param name="scaleStartsWith">A GOV.UK Frontend class name starting with <c>govuk-heading-</c></param>
        /// <returns>Default classes to apply to each HTML heading level.</returns>
        public static HeadingClasses HeadingClasses(string? scaleStartsWith = null)
        {
            if ("govuk-heading-xl".Equals(scaleStartsWith, StringComparison.OrdinalIgnoreCase))
            {
                return new HeadingClasses
                {
                    Caption = "govuk-caption-xl",
                    Heading1 = "govuk-heading-xl",
                    LabelAsHeading1 = "govuk-label--xl",
                    LegendAsHeading1 = "govuk-fieldset__legend--xl",
                    Heading2 = "govuk-heading-l",
                    Heading3 = "govuk-heading-m",
                    Heading4 = "govuk-heading-s",
                    Heading5 = "govuk-heading-s",
                    Heading6 = "govuk-heading-s"
                };
            }

            return new HeadingClasses();
        }
    }
}
