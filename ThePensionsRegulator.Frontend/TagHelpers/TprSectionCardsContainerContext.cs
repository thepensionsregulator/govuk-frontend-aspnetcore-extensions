using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprSectionCardsContainerContext
    {
        private readonly List<TprSectionCardsCardContext> _cards = new();
        public IReadOnlyList<TprSectionCardsCardContext> Cards => _cards;

        public void AddCard(TprSectionCardsCardContext tprSectionCardsCardContext)
        {
            _cards.Add(tprSectionCardsCardContext);
        }
    }
}
