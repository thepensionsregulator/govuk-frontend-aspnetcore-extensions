using GovUk.Frontend.AspNetCore.Extensions.TagHelpers;
using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    public class TprTimelineContext
    {
        private readonly List<TprTimelineItem> _items;

        public TprTimelineContext()
        {
            _items = new List<TprTimelineItem>();
        }

        public string DateSize;

        public IReadOnlyList<TprTimelineItem> Tasks => _items;

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
