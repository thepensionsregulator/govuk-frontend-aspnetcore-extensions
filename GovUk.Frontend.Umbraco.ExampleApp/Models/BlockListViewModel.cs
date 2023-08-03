using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class BlockListViewModel
    {
        public BlockList? Page { get; set; }

        [Required(ErrorMessage = nameof(Field1))]
        public string? Field1 { get; set; }
    }
}
