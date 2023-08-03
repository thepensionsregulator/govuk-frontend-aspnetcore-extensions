using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class PaginationViewModel
    {
        public Pagination? Page { get; set; }

        [Required(ErrorMessage = nameof(Items))]
        public string? Items { get; set; }

        public string? PageTitle { get; set; }
    }
}
