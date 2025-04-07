using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprSectionCardsContext
    {
        private readonly List<TprSectionCardContext> _cards = new();
        public IReadOnlyList<TprSectionCardContext> Cards => _cards;

        public void AddCard(TprSectionCardContext tprSectionCardsCardContext)
        {
            _cards.Add(tprSectionCardsCardContext);
        }
    }
}
