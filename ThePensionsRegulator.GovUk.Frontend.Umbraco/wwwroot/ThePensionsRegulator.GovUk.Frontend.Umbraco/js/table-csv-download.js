(function () {
    "use strict";

    var DEFAULT_FALLBACK_BUTTON_TEXT = "Download table data (CSV)";

    function getButtonText() {
        var meta = document.querySelector('meta[name="table-csv-download-button-text"]');
        return (meta && meta.getAttribute("content")) || DEFAULT_FALLBACK_BUTTON_TEXT;
    }

    function sanitizeFileName(name) {
        if (!name) return "table-data";
        var sanitized = name.trim().replace(/[^\w\s\-]/g, "").replace(/\s+/g, "-").toLowerCase();
        return sanitized.length > 50 ? sanitized.substring(0, 50) : sanitized || "table-data";
    }

    function getCellText(cell) {
        return (cell.innerText || cell.textContent || "").replace(/\s+/g, " ").trim();
    }

    function escapeCsvValue(value) {
        if (!value) return "";
        if (value.indexOf(",") !== -1 || value.indexOf('"') !== -1 || value.indexOf("\n") !== -1 || value.indexOf("\r") !== -1) {
            return '"' + value.replace(/"/g, '""') + '"';
        }
        return value;
    }

    function tableToCsv(table) {
        var csv = [];
        var rows = table.querySelectorAll("tr");
        for (var i = 0; i < rows.length; i++) {
            var cells = rows[i].querySelectorAll("th, td");
            var rowValues = [];
            for (var j = 0; j < cells.length; j++) {
                rowValues.push(escapeCsvValue(getCellText(cells[j])));
            }
            csv.push(rowValues.join(","));
        }
        return csv.join("\r\n");
    }

    function downloadCsv(csvContent, fileName) {
        var BOM = "\uFEFF";
        var blob = new Blob([BOM + csvContent], { type: "text/csv;charset=utf-8;" });
        var url = URL.createObjectURL(blob);
        var link = document.createElement("a");
        link.setAttribute("href", url);
        link.setAttribute("download", fileName + ".csv");
        link.style.display = "none";
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(url);
    }

    function initTableCsvDownload() {
        var buttonText = getButtonText();
        var tables = document.querySelectorAll("table");

        for (var i = 0; i < tables.length; i++) {
            var table = tables[i];

            var caption = table.querySelector("caption");
            var fileName = sanitizeFileName(caption ? caption.innerText || caption.textContent : null);

            var wrapper = document.createElement("div");
            wrapper.className = "tpr-table-wrapper";
            table.parentNode.insertBefore(wrapper, table);
            wrapper.appendChild(table);

            var button = document.createElement("button");
            button.type = "button";
            button.className = "govuk-button govuk-button--secondary";
            button.setAttribute("data-module", "govuk-button");
            button.textContent = buttonText;

            (function (tbl, fn) {
                button.addEventListener("click", function () {
                    var csv = tableToCsv(tbl);
                    downloadCsv(csv, fn);
                });
            })(table, fileName);

            wrapper.appendChild(button);
        }
    }

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", initTableCsvDownload);
    } else {
        initTableCsvDownload();
    }
})();
