using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        public virtual TagBuilder GenerateTprSectionCards(TprSectionCards tprSectionCards)
        {
            var ulTag = new TagBuilder("ul");
            if (tprSectionCards.ContainerAttributes != null) { ulTag.MergeAttributes(tprSectionCards.ContainerAttributes); }
            ulTag.AddCssClass("tpr-sectioncards-container govuk-list ");

            foreach (var card in tprSectionCards.Cards)
            {
                var tprSectionCard = new TagBuilder("li");
                if (card.CardAttributes != null) { tprSectionCard.MergeAttributes(card.CardAttributes); }
                tprSectionCard.AddCssClass("tpr-sectioncards");

                var tprSectionCardBody = new TagBuilder("div");
                tprSectionCardBody.AddCssClass("tpr-sectioncards__body");

                if (card.Title != null)
                {
                    var tprSectionCardTitle = new TagBuilder("h2");
                    if (card.TitleAttributes != null) { tprSectionCardTitle.MergeAttributes(card.TitleAttributes); }
                    tprSectionCardTitle.AddCssClass("tpr-sectioncards__title govuk-link");

                    if (!string.IsNullOrEmpty(card.TitleUrl))
                    {
                        var anchorElement = new TagBuilder("a");
                        anchorElement.Attributes.Add("href", card.TitleUrl);

                        if(!string.IsNullOrWhiteSpace(card.TitleTarget))
                        {
                            anchorElement.Attributes.Add("target",card.TitleTarget);
                        }

                        if (card.TitleAllowHtml)
                        {
                            anchorElement.InnerHtml.AppendHtml(card.Title);
                        }
                        else
                        {
                            anchorElement.InnerHtml.Append(card.Title.ToString()!);
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
                            tprSectionCardTitle.InnerHtml.Append(card.Title.ToString()!);
                        }
                    }

                    tprSectionCardBody.InnerHtml.AppendHtml(tprSectionCardTitle);
                }

                if (card.Content != null)
                {
                    var tprSectionCardContent = new TagBuilder("p");
                    if (card.ContentAttributes != null) { tprSectionCardContent.MergeAttributes(card.ContentAttributes); }

                    if (card.ContentAllowHtml)
                    {
                        tprSectionCardContent.InnerHtml.AppendHtml(card.Content);
                    }
                    else
                    {
                        tprSectionCardContent.InnerHtml.Append(card.Content.ToString()!);
                    }

                    tprSectionCardBody.InnerHtml.AppendHtml(tprSectionCardContent);
                }

                tprSectionCard.InnerHtml.AppendHtml(tprSectionCardBody);
                ulTag.InnerHtml.AppendHtml(tprSectionCard);
            }

            return ulTag;
        }
    }
}
