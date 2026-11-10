using System;
using System.Collections.Generic;
using System.Text;

namespace ThePensionsRegulator.Frontend.Models
{
    public class TprSessionTimeOutModel
    {
        public virtual string? Title { get; set; }
        public virtual string? Content { get; set; }
        public virtual bool DisplayPrimaryAction { get; set; }
        public virtual bool DisplaySecondaryAction { get; set; }
    }
}
