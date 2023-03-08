using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class SelectViewModel
    {
        public Select? Page { get; set; }

        [Required(ErrorMessage = nameof(Field1))]
        public string? Field1 { get; set; }

        [Required(ErrorMessage = nameof(Field2))]
        public string? Field2 { get; set; }
    }
}
