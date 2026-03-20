namespace GovUk.Frontend.Umbraco
{
    public class GovUkFrontendUmbracoOptions
    {
        /// <summary>
        /// Render a .govuk-width-container around each block, for use when the layout page does not contain .govuk-width-container.
        /// </summary>
        public bool RenderWidthContainerForBlocks { get; set; }
    }
}
