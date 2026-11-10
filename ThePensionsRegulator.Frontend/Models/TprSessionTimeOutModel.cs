namespace ThePensionsRegulator.Frontend.Models
{
    public class TprSessionTimeOutModel
    {
        public virtual string? Title { get; set; }
        public virtual string? Content { get; set; }
        public virtual bool DisplayPrimaryAction { get; set; }
        public virtual string? PrimaryActionText { get; set; }
        public virtual string? PrimaryActionUrl { get; set; }
        public virtual bool DisplaySecondaryAction { get; set; }
        public virtual string? SecondaryActionText { get; set; }
        public virtual string? SecondaryActionUrl { get; set; }
    }
}
