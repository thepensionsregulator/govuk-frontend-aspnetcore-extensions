using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string SearchElement = "aside";

        public virtual TagBuilder GenerateTprSearch()
        {
            var section = new TagBuilder(SearchElement);
            section.AddCssClass("govuk-grid-row");
            return section;
        }
    }
}
