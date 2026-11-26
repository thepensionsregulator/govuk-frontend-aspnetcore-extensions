using System;
using System.Collections.Generic;
using System.Text;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.Models
{
    public class TprTimelineModel
    {
        public List<TprTimelineItem> TimelineItems { get; set; } = [];
    }
}
