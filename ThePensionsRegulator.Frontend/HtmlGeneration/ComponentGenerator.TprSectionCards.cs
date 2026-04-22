using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Text.Encodings.Web;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        public const int SectionCardTitlesMinHeadingLevel = 2;
        public const int SectionCardTitlesMaxHeadingLevel = 5;

        public virtual TagBuilder GenerateTprSectionCards(TprSectionCards tprSectionCards)
        {
            var nav = new TagBuilder("nav");

            if (tprSectionCards.Attributes != null) { nav.MergeAttributes(tprSectionCards.Attributes); }
            nav.AddCssClass("tpr-section-cards");

            if (tprSectionCards.NavigationAriaLabel is not null && !string.IsNullOrWhiteSpace(tprSectionCards.NavigationAriaLabel))
            {
                nav.Attributes.Add("aria-label", tprSectionCards.NavigationAriaLabel);
            }

            var ulTag = new TagBuilder("ul");
            nav.InnerHtml.AppendHtml(ulTag);
            ulTag.AddCssClass("govuk-list");
            var culture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();

            foreach (var card in tprSectionCards.Cards)
            {
                var tprSectionCard = new TagBuilder("li");
                if (card.CardAttributes != null) { tprSectionCard.MergeAttributes(card.CardAttributes); }
                tprSectionCard.AddCssClass("tpr-section-card");

                var tprSectionCardBody = new TagBuilder("div");
                tprSectionCardBody.AddCssClass("tpr-section-card__body");

                if (card.Title is not null && !string.IsNullOrWhiteSpace(card.Title.ToHtmlString(HtmlEncoder.Default)))
                {
                    var tprSectionCardTitle = new TagBuilder($"h{tprSectionCards.SectionCardsTitleHeadingLevel}");
                    if (card.TitleAttributes != null) { tprSectionCardTitle.MergeAttributes(card.TitleAttributes); }
                    tprSectionCardTitle.AddCssClass(tprSectionCards.SectionCardsTitleHeadingClass);
                    tprSectionCardTitle.AddCssClass("tpr-section-card__title");

                    if (!string.IsNullOrEmpty(card.TitleUrl))
                    {
                        var anchorElement = new TagBuilder("a");
                        anchorElement.AddCssClass("govuk-link");
                        anchorElement.Attributes.Add("href", card.TitleUrl);

                        if (card.TitleAllowHtml)
                        {
                            anchorElement.InnerHtml.AppendHtml(card.Title);
                        }
                        else
                        {
                            anchorElement.InnerHtml.Append(card.Title.ToHtmlString(HtmlEncoder.Default));
                        }

                        if (!string.IsNullOrWhiteSpace(card.TitleTarget) && card.TitleTarget.ToLower() == "_blank" && !string.IsNullOrWhiteSpace(tprSectionCards.NewTabText))
                        {
                            anchorElement.Attributes.Add("target", "_blank");
                            anchorElement.Attributes.Add("rel", "noopener noreferrer");

                            var newTabText = new TagBuilder("span");
                            newTabText.InnerHtml.Append($" {tprSectionCards.NewTabText}");
                            anchorElement.InnerHtml.AppendHtml(newTabText);
                        }
                        tprSectionCardTitle.InnerHtml.AppendHtml(anchorElement);
                    }
                    else
                    {
                        if (card.TitleAllowHtml)
                        {
                            tprSectionCardTitle.InnerHtml.AppendHtml(card.Title);
                        }
                        else
                        {
                            tprSectionCardTitle.InnerHtml.Append(card.Title.ToHtmlString(HtmlEncoder.Default));
                        }
                    }

                    tprSectionCardBody.InnerHtml.AppendHtml(tprSectionCardTitle);
                }

                if (card.Content is not null && !string.IsNullOrWhiteSpace(card.Content.ToHtmlString(HtmlEncoder.Default)))
                {
                    var tprSectionCardContent = new TagBuilder("div");
                    tprSectionCardContent.AddCssClass("tpr-section-card__content");
                    if (card.ContentAttributes != null) { tprSectionCardContent.MergeAttributes(card.ContentAttributes); }

                    if (card.ContentAllowHtml)
                    {
                        tprSectionCardContent.InnerHtml.AppendHtml(card.Content);
                    }
                    else
                    {
                        var tprSectionCardPara = new TagBuilder("p");
                        tprSectionCardPara.AddCssClass("govuk-body");
                        tprSectionCardPara.InnerHtml.Append(card.Content.ToHtmlString(HtmlEncoder.Default));
                        tprSectionCardContent.InnerHtml.AppendHtml(tprSectionCardPara);
                    }

                    tprSectionCardBody.InnerHtml.AppendHtml(tprSectionCardContent);
                }

                tprSectionCard.InnerHtml.AppendHtml(tprSectionCardBody);
                ulTag.InnerHtml.AppendHtml(tprSectionCard);
            }

            return nav;
        }
    }
}
