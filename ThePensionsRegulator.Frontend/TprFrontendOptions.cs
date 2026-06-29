namespace ThePensionsRegulator.Frontend
{
    public class TprFrontendOptions
    {
        public string[]? UpdateDestinationHostnames { get; set; }

        /// <summary>
        /// Enable a CSV download button below each .govuk-table element on the page.
        /// The button text defaults to "Download table data (CSV)" but can be customised by passing
        /// a custom text string to the TPR/BodyClosing partial.
        /// Tables with merged cells (colspan or rowspan) are skipped.
        /// </summary>
        public bool EnableTableCsvDownload { get; set; }
    }
}
