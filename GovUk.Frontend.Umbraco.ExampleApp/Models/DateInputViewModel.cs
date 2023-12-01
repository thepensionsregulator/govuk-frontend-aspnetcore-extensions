using GovUk.Frontend.AspNetCore.Extensions.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class DateInputViewModel
    {
        public DateInput? Page { get; set; }

        [Required(ErrorMessage = nameof(Field1))]
        [DateRange("2020-1-1", "2029-12-31", ErrorMessage = nameof(Field1))]
        public DateOnly? Field1 { get; set; }

        [Required(ErrorMessage = nameof(Field2))]
        public DateTime? Field2 { get; set; }
        public DateTime? Field3 { get; set; }
    }
}