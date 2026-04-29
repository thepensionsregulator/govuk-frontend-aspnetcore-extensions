namespace ThePensionsRegulator.Frontend
{
    public class TprFrontendOptions
    {
        public string[]? UpdateDestinationHostnames { get; set; }

        /// <summary>
        /// Enable a CSV download button below each .govuk-table element on the page.
        /// The button text defaults to "Download table data (CSV)" but can be customised by adding
        /// a data-tpr-table-csv-download-text attribute to the &lt;body&gt; element.
        /// Tables with merged cells (colspan or rowspan) are skipped.
        /// </summary>
        public bool EnableTableCsvDownload { get; set; }
    }
}
