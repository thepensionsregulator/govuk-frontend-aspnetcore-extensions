using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.GovUk.Frontend;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    public class TprTimelineContext
    {
        private readonly List<TprTimelineItem> _items = [];

        public IReadOnlyList<TprTimelineItem> Tasks => _items;

        public int HeadingLevel { get; set; }
        public bool HideTail { get; set; }
        public string? AriaTitle { get; set; }

        public void AddItem(TprTimelineItem item)
        {
            Guard.ArgumentNotNull(nameof(item), item);

            _items.Add(item);
        }

        public void ThrowIfIncomplete()
        {
            if (Tasks.Count < 1)
            {
                throw ExceptionHelper.AChildElementMustBeProvided(TprTimelineItemTagHelper.TagName);
            }
        }

    }
}
