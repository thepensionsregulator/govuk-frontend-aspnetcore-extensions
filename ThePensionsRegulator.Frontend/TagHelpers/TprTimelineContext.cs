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

        private int _headingLevel;
        // private string _dateSize;
        private bool _hideTail;
        private string _ariaTitle;

        public IReadOnlyList<TprTimelineItem> Tasks => _items;

        public int HeadingLevel
        {
            get
            {
                return _headingLevel;
            }
            set
            {
                _headingLevel = value;
            }
        }        
        public bool HideTail
        {
            get
            {
                return _hideTail;
            }
            set
            {
                _hideTail = value;
            }
        }

        public string AriaTitle
        {
            get
            {
                return _ariaTitle;
            }
            set
            {
                _ariaTitle = value;
            }
        }
        public TprTimelineContext()
        {
            _items = new List<TprTimelineItem>();
        }

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
