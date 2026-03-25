namespace ThePensionsRegulator.Frontend
{
    public class TprFrontendOptions
    {
        public string[]? UpdateDestinationHostnames { get; set; }

        /// <summary>
        /// Enable a CSV download button on all .govuk-table elements.
        /// The button text defaults to "Download table data (CSV)" but can be customised by adding
        /// a data-tpr-table-csv-download-text attribute to the &lt;body&gt; element.
        /// Tables already inside a .tpr-table-wrapper element are skipped.
        /// </summary>
        public bool EnableTableCsvDownload { get; set; }
    }
}
