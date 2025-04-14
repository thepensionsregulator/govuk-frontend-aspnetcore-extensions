using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        public virtual TagBuilder GenerateTprSectionCards(TprSectionCards tprSectionCards)
        {
            var nav = new TagBuilder("nav");
            if (tprSectionCards.Attributes != null) { nav.MergeAttributes(tprSectionCards.Attributes); }
            nav.MergeCssClass("tpr-section-cards");

            var ulTag = new TagBuilder("ul");
            nav.InnerHtml.AppendHtml(ulTag);
            ulTag.MergeCssClass("govuk-list");

            foreach (var card in tprSectionCards.Cards)
            {
                var tprSectionCard = new TagBuilder("li");
                if (card.CardAttributes != null) { tprSectionCard.MergeAttributes(card.CardAttributes); }
                tprSectionCard.MergeCssClass("tpr-section-card");

                var tprSectionCardBody = new TagBuilder("div");
                tprSectionCardBody.MergeCssClass("tpr-section-card__body");

                if (card.Title is not null && !string.IsNullOrWhiteSpace(card.Title.ToHtmlString()))
                {
                    var tprSectionCardTitle = new TagBuilder("h2");
                    if (card.TitleAttributes != null) { tprSectionCardTitle.MergeAttributes(card.TitleAttributes); }
                    tprSectionCardTitle.MergeCssClass("tpr-section-card__title");

                    if (!string.IsNullOrEmpty(card.TitleUrl))
                    {
                        var anchorElement = new TagBuilder("a");
                        anchorElement.MergeCssClass("govuk-link");
                        anchorElement.Attributes.Add("href", card.TitleUrl);

                        if (!string.IsNullOrWhiteSpace(card.TitleTarget))
                        {
                            anchorElement.Attributes.Add("target", card.TitleTarget);
                        }

                        if (card.TitleAllowHtml)
                        {
                            anchorElement.InnerHtml.AppendHtml(card.Title);
                        }
                        else
                        {
                            anchorElement.InnerHtml.Append(card.Title.ToHtmlString());
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
                            tprSectionCardTitle.InnerHtml.Append(card.Title.ToHtmlString());
                        }
                    }

                    tprSectionCardBody.InnerHtml.AppendHtml(tprSectionCardTitle);
                }

                if (card.Content is not null && !string.IsNullOrWhiteSpace(card.Content.ToHtmlString()))
                {
                    var tprSectionCardContent = new TagBuilder("div");
                    tprSectionCardContent.MergeCssClass("tpr-section-card__content");
                    if (card.ContentAttributes != null) { tprSectionCardContent.MergeAttributes(card.ContentAttributes); }

                    if (card.ContentAllowHtml)
                    {
                        tprSectionCardContent.InnerHtml.AppendHtml(card.Content);
                    }
                    else
                    {
                        var tprSectionCardPara = new TagBuilder("p");
                        tprSectionCardPara.MergeCssClass("govuk-body");
                        tprSectionCardPara.InnerHtml.Append(card.Content.ToHtmlString());
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
