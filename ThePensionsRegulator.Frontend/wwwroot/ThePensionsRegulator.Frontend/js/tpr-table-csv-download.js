"use strict";

const DEFAULT_BUTTON_TEXT = "Download table data (CSV)";

/**
 * Gets the button text from the body's data attribute, falling back to default.
 * @returns {string}
 */
export function getButtonText() {
    const customText = document.body.getAttribute("data-tpr-table-csv-download-text");
    return customText || DEFAULT_BUTTON_TEXT;
}

/**
 * Sanitises a string for use as a file name.
 * @param {string|null} name
 * @returns {string}
 */
export function sanitizeFileName(name) {
    if (!name) return "table-data";
    const sanitized = name.trim().replace(/[^\w\s\-]/g, "").replace(/\s+/g, "-").toLowerCase();
    if (!sanitized) return "table-data";
    return sanitized.length > 50 ? sanitized.substring(0, 50) : sanitized;
}

/**
 * Gets the visible text content of a table cell, normalising whitespace.
 * @param {HTMLTableCellElement} cell
 * @returns {string}
 */
export function getCellText(cell) {
    return (cell.innerText || cell.textContent || "").replace(/\s+/g, " ").trim();
}

/**
 * Escapes a value for safe inclusion in a CSV field per RFC 4180.
 * Also mitigates CSV injection by prefixing formula-triggering characters with a single quote.
 * @param {string} value
 * @returns {string}
 */
export function escapeCsvValue(value) {
    if (!value) return "";

    // CSV injection mitigation: prefix formula-triggering characters
    if (/^[=+\-@\t\r]/.test(value)) {
        value = "'" + value;
    }

    // RFC 4180: quote fields containing comma, double-quote, or newline
    if (/[,"\n\r]/.test(value)) {
        return '"' + value.replace(/"/g, '""') + '"';
    }
    return value;
}

/**
 * Returns true if the table has any cells with colspan or rowspan greater than 1.
 * @param {HTMLTableElement} table
 * @returns {boolean}
 */
export function hasMergedCells(table) {
    const cells = table.querySelectorAll("th, td");
    for (let i = 0; i < cells.length; i++) {
        const colspan = parseInt(cells[i].getAttribute("colspan"), 10);
        const rowspan = parseInt(cells[i].getAttribute("rowspan"), 10);
        if ((colspan && colspan > 1) || (rowspan && rowspan > 1)) {
            return true;
        }
    }
    return false;
}

/**
 * Converts a table to CSV content.
 * @param {HTMLTableElement} table
 * @returns {string}
 */
export function tableToCsv(table) {
    const rows = table.querySelectorAll("tr");
    const csvRows = [];
    for (let i = 0; i < rows.length; i++) {
        const cells = rows[i].querySelectorAll("th, td");
        const rowData = [];
        for (let j = 0; j < cells.length; j++) {
            rowData.push(escapeCsvValue(getCellText(cells[j])));
        }
        csvRows.push(rowData.join(","));
    }
    return csvRows.join("\r\n");
}

/**
 * Triggers a CSV file download in the browser.
 * @param {string} csvContent
 * @param {string} fileName
 */
export function downloadCsv(csvContent, fileName) {
    const BOM = "\uFEFF";
    const blob = new Blob([BOM + csvContent], { type: "text/csv;charset=utf-8;" });
    const url = URL.createObjectURL(blob);
    const link = document.createElement("a");
    link.setAttribute("href", url);
    link.setAttribute("download", fileName + ".csv");
    link.click();
    URL.revokeObjectURL(url);
}

/**
 * Initialises CSV download buttons on all .govuk-table elements.
 * Idempotent — will not add a duplicate button if one already follows the table.
 */
export function initTableCsvDownload() {
    const buttonText = getButtonText();
    const tables = document.querySelectorAll(".govuk-table");

    for (let i = 0; i < tables.length; i++) {
        const table = tables[i];
        const nextElement = table.nextElementSibling;

        // Skip tables with merged cells — CSV cannot represent them reliably
        if (hasMergedCells(table)) continue;

        // Skip if a CSV download button has already been added (idempotency)
        if (
            nextElement &&
            nextElement.hasAttribute("data-tpr-table-csv-button") || nextElement && nextElement.matches('form[class="tpr-table-download-form"]') && nextElement.querySelector("button")
        ) {
            continue;
        }

        const caption = table.querySelector("caption");
        const fileName = sanitizeFileName(caption ? (caption.innerText || caption.textContent) : null);

        const button = document.createElement("button");
        button.type = "button";
        button.className = "govuk-button govuk-button--secondary";
        button.setAttribute("data-module", "govuk-button");
        button.setAttribute("data-tpr-table-csv-button", "true");
        button.setAttribute("data-tpr-table-csv-filename", fileName);
        button.textContent = buttonText;

        button.addEventListener("click", () => {
            const csv = tableToCsv(table);
            downloadCsv(csv, fileName);
        });

        table.parentNode.insertBefore(button, table.nextSibling);
    }
}

if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", initTableCsvDownload);
} else {
    initTableCsvDownload();
}
