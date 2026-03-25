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
 * Converts a table to CSV content, expanding merged cells (colspan/rowspan).
 * @param {HTMLTableElement} table
 * @returns {string}
 */
export function tableToCsv(table) {
    const rows = table.querySelectorAll("tr");
    if (rows.length === 0) return "";

    // Build a grid to handle colspan and rowspan
    const grid = [];
    for (let i = 0; i < rows.length; i++) {
        if (!grid[i]) grid[i] = [];
        const cells = rows[i].querySelectorAll("th, td");
        let colIndex = 0;

        for (let j = 0; j < cells.length; j++) {
            // Find the next available column
            while (grid[i][colIndex] !== undefined) colIndex++;

            const cell = cells[j];
            const text = getCellText(cell);
            const colspan = parseInt(cell.getAttribute("colspan"), 10) || 1;
            const rowspan = parseInt(cell.getAttribute("rowspan"), 10) || 1;

            // Fill the grid for the colspan/rowspan area
            for (let r = 0; r < rowspan; r++) {
                for (let c = 0; c < colspan; c++) {
                    if (!grid[i + r]) grid[i + r] = [];
                    // Only the first cell gets the text; spanned cells are empty
                    grid[i + r][colIndex + c] = (r === 0 && c === 0) ? text : "";
                }
            }
            colIndex += colspan;
        }
    }

    // Convert grid to CSV
    const csvRows = [];
    for (let i = 0; i < grid.length; i++) {
        const row = grid[i] || [];
        const values = [];
        for (let j = 0; j < row.length; j++) {
            values.push(escapeCsvValue(row[j] !== undefined ? row[j] : ""));
        }
        csvRows.push(values.join(","));
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
    link.style.display = "none";
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url);
}

/**
 * Initialises CSV download buttons on all .govuk-table elements
 * that are not already inside a .tpr-table-wrapper.
 */
export function initTableCsvDownload() {
    const buttonText = getButtonText();
    const tables = document.querySelectorAll(".govuk-table");

    for (let i = 0; i < tables.length; i++) {
        const table = tables[i];

        // Skip tables already wrapped
        if (table.closest(".tpr-table-wrapper")) continue;

        const caption = table.querySelector("caption");
        const fileName = sanitizeFileName(caption ? (caption.innerText || caption.textContent) : null);

        const wrapper = document.createElement("div");
        wrapper.className = "tpr-table-wrapper";
        table.parentNode.insertBefore(wrapper, table);
        wrapper.appendChild(table);

        const button = document.createElement("button");
        button.type = "button";
        button.className = "govuk-button govuk-button--secondary";
        button.setAttribute("data-module", "govuk-button");
        button.textContent = buttonText;

        button.addEventListener("click", () => {
            const csv = tableToCsv(table);
            downloadCsv(csv, fileName);
        });

        wrapper.appendChild(button);
    }
}

if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", initTableCsvDownload);
} else {
    initTableCsvDownload();
}
