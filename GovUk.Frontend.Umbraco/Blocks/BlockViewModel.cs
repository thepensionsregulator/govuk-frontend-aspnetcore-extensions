using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;

namespace GovUk.Frontend.Umbraco.Blocks
{
    /// <summary>
    /// Settings for rendering an individual block in a block grid or block list.
    /// </summary>
    public class BlockViewModel
    {
        public required IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> Block { get; set; }
        public bool HasGridAreas { get; set; }
        public bool IsSameAsPrevious { get; set; }
        public bool IsSameAsNext { get; set; }
        public required string RowClasses { get; set; }
        public required string ColumnClasses { get; set; }
        public string? FieldsetErrorClasses { get; set; }
    }
}
