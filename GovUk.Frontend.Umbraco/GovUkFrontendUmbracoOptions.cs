namespace GovUk.Frontend.Umbraco
{
    public class GovUkFrontendUmbracoOptions
    {
        /// <summary>
        /// Render a .govuk-width-container around each block, for use when the layout page does not contain .govuk-width-container.
        /// </summary>
        public bool RenderWidthContainerForBlocks { get; set; }

        /// <summary>
        /// Enable a CSV download button on all HTML tables rendered in block grid content.
        /// The button text defaults to "Download table data (CSV)" but can be customised by adding
        /// a &lt;meta name="table-csv-download-button-text" content="your text" /&gt; tag to the page.
        /// Tables already inside a .tpr-table-wrapper element are skipped.
        /// </summary>
        public bool EnableTableCsvDownload { get; set; }
    }
}
