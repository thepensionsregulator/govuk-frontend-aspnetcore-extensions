using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprSectionCardsContainerContext
    {
        private readonly List<TprSectionCardContext> _cards = new();
        public IReadOnlyList<TprSectionCardContext> Cards => _cards;

        public void AddCard(TprSectionCardContext tprSectionCardContext)
        {
            _cards.Add(tprSectionCardContext);
        }
    }
}
