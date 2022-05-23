﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.ExampleApp.Models
{
    public class CheckboxesViewModel
    {
        public Checkboxes Page { get; set; }

        [Required(ErrorMessage = nameof(Field1))]
        public List<string> Field1 { get; set; }

        [RegularExpression("[0-9]+", ErrorMessage = nameof(Field2))]
        public string Field2 { get; set; }
    }
}