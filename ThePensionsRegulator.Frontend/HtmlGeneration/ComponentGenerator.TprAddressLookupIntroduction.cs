using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThePensionsRegulator.Frontend.HtmlGeneration;

public partial class ComponentGenerator
{
    public const int AddressLookupMinHeadingLevel = 2;
    public const int AddressLookupMaxHeadingLevel = 6;

    public TagBuilder GenerateTprAddressLookupIntroduction(int headingLevel, string headingClass, TprAddressLookupContent content)
    {
        var contentDiv = new TagBuilder("div");
        contentDiv.AddCssClass("tpr-address-lookup-introduction");

        var heading = new TagBuilder($"h{headingLevel}");
        heading.AddCssClass(headingClass);
        heading.InnerHtml.Append(content.AddressLookupIntroductionHeading);

        var description = new TagBuilder("p");
        description.AddCssClass("govuk-body");
        description.InnerHtml.Append(content.AddressLookupIntroductionText);

        contentDiv.InnerHtml.AppendHtml(heading);
        contentDiv.InnerHtml.AppendHtml(description);
        return contentDiv;
    }
}
