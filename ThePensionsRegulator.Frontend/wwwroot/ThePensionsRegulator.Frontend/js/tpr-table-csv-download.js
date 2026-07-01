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
 * Returns true if the table is already followed by a download control, so that a
 * duplicate button is not added. This covers both a button previously added by this
 * script (idempotency) and a download button rendered server-side, such as the one
 * produced by the HTML table component (a form with the "tpr-table-download-form" class).
 * @param {HTMLTableElement} table
 * @returns {boolean}
 */
export function hasExistingDownloadButton(table) {
    const next = table.nextElementSibling;
    if (!next) return false;

    // A CSV download button already added by this script (idempotency).
    if (next.hasAttribute("data-tpr-table-csv-button")) return true;

    // A download button rendered server-side. Match by class name (tolerant of
    // additional classes) and allow the form to be the sibling itself or nested
    // within a wrapping sibling element.
    const downloadForm = next.classList.contains("tpr-table-download-form")
        ? next
        : (next.querySelector ? next.querySelector(".tpr-table-download-form") : null);
    if (downloadForm && downloadForm.querySelector("button")) return true;

    return false;
}

/**
 * Finds a server-side rendered CSV download form that follows the table, if present.
 * Returns the form element, or null if not found.
 * @param {HTMLTableElement} table
 * @returns {HTMLFormElement|null}
 */
export function findServerSideDownloadForm(table) {
    const next = table.nextElementSibling;
    if (!next) return null;

    if (next.classList.contains("tpr-table-download-form") && next.querySelector("button")) {
        return next;
    }

    if (next.querySelector) {
        const form = next.querySelector(".tpr-table-download-form");
        if (form && form.querySelector("button")) return form;
    }

    return null;
}

/**
 * Initialises CSV download buttons on all .govuk-table elements.
 * Idempotent — will not add a duplicate button if one already follows the table.
 * When a server-side download form is present, intercepts its submit and performs
 * the download client-side instead of posting to the server.
 */
export function initTableCsvDownload() {
    const buttonText = getButtonText();
    const tables = document.querySelectorAll(".govuk-table");

    for (let i = 0; i < tables.length; i++) {
        const table = tables[i];

        // Skip tables with merged cells — CSV cannot represent them reliably
        if (hasMergedCells(table)) continue;

        // Skip if a client-side download button was already added by this script (idempotency).
        const next = table.nextElementSibling;
        if (next && next.hasAttribute("data-tpr-table-csv-button")) continue;

        const caption = table.querySelector("caption");
        const fileName = sanitizeFileName(caption ? (caption.innerText || caption.textContent) : null);

        // If a server-side download form is present, intercept its submit and perform
        // the download client-side to avoid a server round-trip.
        const serverForm = findServerSideDownloadForm(table);
        if (serverForm) {
            if (!serverForm.hasAttribute("data-tpr-table-csv-intercepted")) {
                serverForm.setAttribute("data-tpr-table-csv-intercepted", "true");
                serverForm.addEventListener("submit", (e) => {
                    e.preventDefault();
                    downloadCsv(tableToCsv(table), fileName);
                });
            }
            continue;
        }

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
