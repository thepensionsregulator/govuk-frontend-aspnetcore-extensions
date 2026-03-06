using Umbraco.Cms.Core.Models;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Models
{
    /// <summary>
    /// An action on a GOV.UK 'Summary list item' or 'Summary card' component.
    /// </summary>
    public class SummaryListAction
    {
        public SummaryListAction(Link link, string linkText)
        {
            Link = link;
            LinkText = linkText;
        }

        public Link Link { get; }
        public string LinkText { get; }
    }
}
