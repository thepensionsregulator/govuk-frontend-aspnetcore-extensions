/**
 * Adds a CSV download button below each .govuk-table element on the page.
 *
 * Button text defaults to "Download table data (CSV)" and can be customised
 * by setting a data-tpr-table-csv-download-text attribute on the <body> element.
 *
 * Tables with merged cells (colspan or rowspan > 1) are skipped because they
 * cannot be reliably represented as CSV.
 *
 * Generated CSV conforms to RFC 4180 (comma-separated, double-quote escaping,
 * CRLF line endings) and the UK government tabular data standard.
 *
 * Values that start with =, +, -, or @ are prefixed with a single quote to
 * mitigate CSV formula injection when opened in spreadsheet applications.
 */

const DEFAULT_BUTTON_TEXT = "Download table data (CSV)";

function getButtonText() {
  const customText = document.body.dataset.tprTableCsvDownloadText;
  return customText || DEFAULT_BUTTON_TEXT;
}

function escapeCsvValue(value) {
  if (!value) return "";

  // Mitigate CSV formula injection
  if (/^[=+\-@]/.test(value)) {
    value = "'" + value;
  }

  if (
    value.indexOf(",") !== -1 ||
    value.indexOf('"') !== -1 ||
    value.indexOf("\n") !== -1 ||
    value.indexOf("\r") !== -1
  ) {
    return '"' + value.replace(/"/g, '""') + '"';
  }
  return value;
}

function hasMergedCells(table) {
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

function tableToCsv(table) {
  const rows = table.querySelectorAll("tr");
  const csvRows = [];
  for (let i = 0; i < rows.length; i++) {
    const cells = rows[i].querySelectorAll("th, td");
    const rowData = [];
    for (let j = 0; j < cells.length; j++) {
      const text = (cells[j].innerText || cells[j].textContent || "").trim();
      rowData.push(escapeCsvValue(text));
    }
    csvRows.push(rowData.join(","));
  }
  return csvRows.join("\r\n");
}

function sanitizeFileName(name) {
  if (!name || !name.trim()) return "table-data";
  return name
    .trim()
    .replace(/[^a-zA-Z0-9 _-]/g, "")
    .replace(/\s+/g, "-")
    .substring(0, 100)
    .toLowerCase();
}

function downloadCsv(csvContent, fileName) {
  const BOM = "\uFEFF";
  const blob = new Blob([BOM + csvContent], {
    type: "text/csv;charset=utf-8;",
  });
  const url = URL.createObjectURL(blob);
  const link = document.createElement("a");
  link.setAttribute("href", url);
  link.setAttribute("download", fileName + ".csv");
  link.click();
  URL.revokeObjectURL(url);
}

function initTableCsvDownload() {
  const buttonText = getButtonText();
    const tables = document.querySelectorAll(".govuk-table");

  for (let i = 0; i < tables.length; i++) {
    const table = tables[i];
      const nextElement = table.nextElementSibling;

    // Skip tables with merged cells — CSV cannot represent them reliably
    if (hasMergedCells(table)) {
      continue;
    }

      // Skip if a CSV download button has already been added (idempotency)
    if (
      nextElement &&
        nextElement.hasAttribute("data-tpr-table-csv-button") || nextElement && nextElement.matches('form[name="tableHtml"]') && nextElement.querySelector("button")
    ) {
      continue;
    }

    const caption = table.querySelector("caption");
    const fileName = sanitizeFileName(
      caption ? caption.innerText || caption.textContent : null
    );

    const button = document.createElement("button");
    button.type = "button";
    button.className = "govuk-button govuk-button--secondary";
    button.setAttribute("data-module", "govuk-button");
    button.setAttribute("data-tpr-table-csv-button", "true");
    button.setAttribute("data-tpr-table-csv-filename", fileName);
    button.textContent = buttonText;

    button.addEventListener("click", function () {
      const csv = tableToCsv(table);
      downloadCsv(csv, fileName);
    });

    table.parentNode.insertBefore(button, table.nextSibling);
  }
}

document.addEventListener("DOMContentLoaded", initTableCsvDownload);

export {
  DEFAULT_BUTTON_TEXT,
  getButtonText,
  escapeCsvValue,
  hasMergedCells,
  tableToCsv,
  sanitizeFileName,
  downloadCsv,
  initTableCsvDownload,
};
