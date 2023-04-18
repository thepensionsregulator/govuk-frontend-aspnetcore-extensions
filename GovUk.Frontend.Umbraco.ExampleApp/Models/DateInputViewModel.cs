using System;
using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class DateInputViewModel
    {
        public DateInput? Page { get; set; }

        [Required(ErrorMessage = nameof(Field1))]
        public DateOnly? Field1 { get; set; }

        [Required(ErrorMessage = nameof(Field2))]
        public DateTime? Field2 { get; set; }
    }
}
