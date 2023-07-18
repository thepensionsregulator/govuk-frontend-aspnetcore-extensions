using System.ComponentModel.DataAnnotations;
using ThePensionsRegulator.Umbraco.BlockLists;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class PaginationViewModel
    {
        public Pagination? Page { get; set; }

        public OverridableBlockListModel Blocks { get; set; } = new();

        [Required(ErrorMessage = nameof(Items))]
        public string? Items { get; set; }

        public string? PageTitle { get; set; }
    }
}
