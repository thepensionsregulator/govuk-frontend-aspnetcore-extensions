using System;

namespace GovUk.Frontend.Umbraco.Typography
{
    [Obsolete("TypographyOptions is deprecated and will be removed in a future release. Use methods on the GovUkTypography class instead.")]
    public class TypographyOptions
    {
        public BackgroundType BackgroundType { get; set; } = BackgroundType.Light;
        public bool RemoveWrappingParagraphs { get; set; }
        public bool RemoveWrappingParagraph { get; set; }
    }
}