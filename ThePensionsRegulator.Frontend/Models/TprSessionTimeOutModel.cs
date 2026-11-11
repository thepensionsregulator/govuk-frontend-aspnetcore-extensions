using Microsoft.AspNetCore.Html;

namespace ThePensionsRegulator.Frontend.Models
{
    public class TprSessionTimeOutModel
    {
        public virtual string? MessageKey { get; set; }
        public virtual string? Title { get; set; }
        public virtual string? Content { get; set; }
        public virtual IHtmlContent? ContentHtml { get; set; }
        public virtual bool DisplayPrimaryAction { get; set; }
        public virtual string? PrimaryActionText { get; set; }
        public virtual string? PrimaryActionUrl { get; set; }
        public virtual bool DisplaySecondaryAction { get; set; }
        public virtual string? SecondaryActionText { get; set; }
        public virtual string? SecondaryActionUrl { get; set; }
        public virtual bool EnableClientSideSessionTimeout { get; set; }
        public virtual int SessionTimeoutMilliseconds { get; set; } = 900000;
        public virtual bool ResetTimeoutCountdownOnActivity { get; set; }
    }
}
