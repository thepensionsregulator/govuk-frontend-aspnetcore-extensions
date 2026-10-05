namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Models
{
    public class SummaryListTrackingResult
    {
        public static SummaryListTrackingResult Empty { get; } = new(new HashSet<int>());
        public IReadOnlySet<int> NewItemIndexes { get; }
        public SummaryListTrackingResult(IReadOnlySet<int> newItemIndexs)
        {
            NewItemIndexes = newItemIndexs;
        }
        public bool IsNew(int index) => NewItemIndexes.Contains(index);
    }
}
