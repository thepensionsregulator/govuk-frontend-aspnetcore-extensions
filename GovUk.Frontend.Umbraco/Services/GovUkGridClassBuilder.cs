﻿namespace GovUk.Frontend.Umbraco.Services
{
    public static class GovUkGridClassBuilder
    {
        public static string BuildGridRowClass(string? customClass)
        {
            string rowClass = ("govuk-grid-row " + customClass).TrimEnd();
            return rowClass;
        }
        public static string BuildGridColumnClass(string? columnSizeClass, string? fromDesktopClass, string? customClass)
        {
            if (!string.IsNullOrEmpty(columnSizeClass)) { columnSizeClass = "govuk-grid-column-" + columnSizeClass; }
            if (!string.IsNullOrEmpty(fromDesktopClass)) { fromDesktopClass = "govuk-grid-column-" + fromDesktopClass + "-from-desktop"; }
            var columnClass = (columnSizeClass + " " + fromDesktopClass).Trim();
            if (string.IsNullOrEmpty(columnClass)) { columnClass = "govuk-grid-column-two-thirds"; }
            columnClass = ("govuk-grid-column " + columnClass + " " + customClass).TrimEnd(); // .govuk-grid-column is not part of the GOV.UK design system but it's useful to be able to target any column
            return columnClass;
        }
    }
}