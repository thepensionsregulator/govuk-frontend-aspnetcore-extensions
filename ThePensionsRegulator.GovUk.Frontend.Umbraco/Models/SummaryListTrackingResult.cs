using System;
using System.Collections.Generic;
using System.Text;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Models
{
    public class SummaryListTrackingResult
    {
        public static SummaryListTrackingResult Empty { get; } = new(new HashSet<int>());
        public IReadOnlySet<int> NewItemIndexs { get; }
        public SummaryListTrackingResult(IReadOnlySet<int> newItemIndexs) {
            NewItemIndexs = newItemIndexs;
        }
        public bool IsNew(int index) => NewItemIndexs.Contains(index);
    }
}
