import '@testing-library/jest-dom';
import { jest } from '@jest/globals';
import { getButtonText, sanitizeFileName, getCellText, escapeCsvValue, hasMergedCells, tableToCsv, initTableCsvDownload, downloadCsv } from '../wwwroot/ThePensionsRegulator.Frontend/js/tpr-table-csv-download';

beforeEach(() => {
    document.body.innerHTML = "";
    document.body.removeAttribute("data-tpr-table-csv-download-text");
});

describe("getButtonText", () => {
    test("returns default text when no data attribute is set", () => {
        expect(getButtonText()).toBe("Download table data (CSV)");
    });

    test("returns custom text from body data attribute", () => {
        document.body.setAttribute("data-tpr-table-csv-download-text", "Custom download");
        expect(getButtonText()).toBe("Custom download");
    });

    test("returns default text when data attribute is empty", () => {
        document.body.setAttribute("data-tpr-table-csv-download-text", "");
        expect(getButtonText()).toBe("Download table data (CSV)");
    });
});

describe("sanitizeFileName", () => {
    test("returns 'table-data' for null input", () => {
        expect(sanitizeFileName(null)).toBe("table-data");
    });

    test("returns 'table-data' for empty string", () => {
        expect(sanitizeFileName("")).toBe("table-data");
    });

    test("returns 'table-data' for whitespace-only input", () => {
        expect(sanitizeFileName("   ")).toBe("table-data");
    });

    test("converts to lowercase and replaces spaces with hyphens", () => {
        expect(sanitizeFileName("My Table Name")).toBe("my-table-name");
    });

    test("removes special characters", () => {
        expect(sanitizeFileName("Table: 100% (data)")).toBe("table-100-data");
    });

    test("truncates to 50 characters", () => {
        const longName = "A".repeat(60);
        expect(sanitizeFileName(longName).length).toBe(50);
    });

    test("returns 'table-data' when all characters are special", () => {
        expect(sanitizeFileName("!@#$%")).toBe("table-data");
    });
});

describe("getCellText", () => {
    test("returns trimmed text content of a cell", () => {
        const td = document.createElement("td");
        td.textContent = "  Hello World  ";
        expect(getCellText(td)).toBe("Hello World");
    });

    test("normalises internal whitespace", () => {
        const td = document.createElement("td");
        td.textContent = "Hello   World\n\tFoo";
        expect(getCellText(td)).toBe("Hello World Foo");
    });

    test("returns empty string for empty cell", () => {
        const td = document.createElement("td");
        expect(getCellText(td)).toBe("");
    });
});

describe("escapeCsvValue", () => {
    test("returns empty string for empty input", () => {
        expect(escapeCsvValue("")).toBe("");
    });

    test("returns empty string for null input", () => {
        expect(escapeCsvValue(null)).toBe("");
    });

    test("returns value unchanged when no special characters", () => {
        expect(escapeCsvValue("hello")).toBe("hello");
    });

    test("quotes value containing comma", () => {
        expect(escapeCsvValue("hello,world")).toBe('"hello,world"');
    });

    test("quotes value containing double quote and escapes it", () => {
        expect(escapeCsvValue('say "hello"')).toBe('"say ""hello"""');
    });

    test("quotes value containing newline", () => {
        expect(escapeCsvValue("line1\nline2")).toBe('"line1\nline2"');
    });

    test("quotes value containing carriage return", () => {
        expect(escapeCsvValue("line1\rline2")).toBe('"line1\rline2"');
    });

    test("prefixes formula-triggering = character", () => {
        expect(escapeCsvValue("=SUM(A1)")).toBe("'=SUM(A1)");
    });

    test("prefixes formula-triggering + character", () => {
        expect(escapeCsvValue("+44 123")).toBe("'+44 123");
    });

    test("prefixes formula-triggering - character", () => {
        expect(escapeCsvValue("-1")).toBe("'-1");
    });

    test("prefixes formula-triggering @ character", () => {
        expect(escapeCsvValue("@mention")).toBe("'@mention");
    });

    test("prefixes and quotes value with formula char and comma", () => {
        expect(escapeCsvValue("=A,B")).toBe("\"'=A,B\"");
    });
});

describe("tableToCsv", () => {
    test("converts a simple table to CSV", () => {
        document.body.innerHTML = `
            <table class="govuk-table">
                <thead><tr><th>Name</th><th>Age</th></tr></thead>
                <tbody><tr><td>Alice</td><td>30</td></tr></tbody>
            </table>`;
        const table = document.querySelector("table");
        expect(tableToCsv(table)).toBe("Name,Age\r\nAlice,30");
    });

    test("returns empty string for table with no rows", () => {
        document.body.innerHTML = '<table class="govuk-table"></table>';
        const table = document.querySelector("table");
        expect(tableToCsv(table)).toBe("");
    });

    test("escapes CSV special characters in cell values", () => {
        document.body.innerHTML = `
            <table class="govuk-table">
                <tr><td>Hello, World</td><td>Normal</td></tr>
            </table>`;
        const table = document.querySelector("table");
        expect(tableToCsv(table)).toBe('"Hello, World",Normal');
    });
});

describe("hasMergedCells", () => {
    test("returns false for a table with no merged cells", () => {
        document.body.innerHTML = `
            <table class="govuk-table">
                <tr><td>A</td><td>B</td></tr>
            </table>`;
        expect(hasMergedCells(document.querySelector("table"))).toBe(false);
    });

    test("returns true when a cell has colspan > 1", () => {
        document.body.innerHTML = `
            <table class="govuk-table">
                <tr><td colspan="2">Merged</td></tr>
            </table>`;
        expect(hasMergedCells(document.querySelector("table"))).toBe(true);
    });

    test("returns true when a cell has rowspan > 1", () => {
        document.body.innerHTML = `
            <table class="govuk-table">
                <tr><td rowspan="2">Merged</td><td>B1</td></tr>
                <tr><td>B2</td></tr>
            </table>`;
        expect(hasMergedCells(document.querySelector("table"))).toBe(true);
    });

    test("returns false when colspan and rowspan are 1", () => {
        document.body.innerHTML = `
            <table class="govuk-table">
                <tr><td colspan="1" rowspan="1">Cell</td></tr>
            </table>`;
        expect(hasMergedCells(document.querySelector("table"))).toBe(false);
    });
});

describe("initTableCsvDownload", () => {
    test("inserts a CSV download button immediately after each .govuk-table", () => {
        document.body.innerHTML = `
            <div>
                <table class="govuk-table">
                    <tr><td>Data</td></tr>
                </table>
            </div>`;
        initTableCsvDownload();
        const table = document.querySelector(".govuk-table");
        const button = table.nextElementSibling;
        expect(button).not.toBeNull();
        expect(button.tagName).toBe("BUTTON");
        expect(button).toHaveAttribute("data-tpr-table-csv-button", "true");
        expect(button).toHaveTextContent("Download table data (CSV)");
        expect(button).toHaveClass("govuk-button", "govuk-button--secondary");
        expect(button).toHaveAttribute("data-module", "govuk-button");
        expect(button.type).toBe("button");
    });

    test("does not add a button to tables without .govuk-table class", () => {
        document.body.innerHTML = `
            <div>
                <table>
                    <tr><td>Data</td></tr>
                </table>
            </div>`;
        initTableCsvDownload();
        expect(document.querySelector('button[data-tpr-table-csv-button="true"]')).not.toBeInTheDocument();
    });

    test("is idempotent — does not add a duplicate button when run twice", () => {
        document.body.innerHTML = `
            <div>
                <table class="govuk-table">
                    <tr><td>Data</td></tr>
                </table>
            </div>`;
        initTableCsvDownload();
        initTableCsvDownload();
        const buttons = document.querySelectorAll('button[data-tpr-table-csv-button="true"]');
        expect(buttons.length).toBe(1);
    });

    test("uses custom button text from body data attribute", () => {
        document.body.setAttribute("data-tpr-table-csv-download-text", "Lawrlwytho data tabl (CSV)");
        document.body.innerHTML = `
            <table class="govuk-table">
                <tr><td>Data</td></tr>
            </table>`;
        // Re-set the attribute after innerHTML clears it
        document.body.setAttribute("data-tpr-table-csv-download-text", "Lawrlwytho data tabl (CSV)");
        initTableCsvDownload();
        const button = document.querySelector("button");
        expect(button).toHaveTextContent("Lawrlwytho data tabl (CSV)");
    });

    test("uses caption text for file name attribute", () => {
        document.body.innerHTML = `
            <div>
                <table class="govuk-table">
                    <caption>Quarterly Results</caption>
                    <tr><td>Data</td></tr>
                </table>
            </div>`;
        initTableCsvDownload();
        const button = document.querySelector("button");
        expect(button).toHaveAttribute("data-tpr-table-csv-filename", "quarterly-results");
    });

    test("skips tables with merged cells", () => {
        document.body.innerHTML = `
            <div>
                <table class="govuk-table">
                    <tr><td colspan="2">Merged</td></tr>
                    <tr><td>A</td><td>B</td></tr>
                </table>
            </div>`;
        initTableCsvDownload();
        expect(document.querySelector('button[data-tpr-table-csv-button="true"]')).not.toBeInTheDocument();
    });

    test("handles multiple .govuk-table elements", () => {
        document.body.innerHTML = `
            <div>
                <table class="govuk-table"><tr><td>Table 1</td></tr></table>
                <table class="govuk-table"><tr><td>Table 2</td></tr></table>
            </div>`;
        initTableCsvDownload();
        const buttons = document.querySelectorAll('button[data-tpr-table-csv-button="true"]');
        expect(buttons.length).toBe(2);
    });

    test("button click triggers CSV download", () => {
        document.body.innerHTML = `
            <div>
                <table class="govuk-table">
                    <caption>Test Table</caption>
                    <tr><th>Col</th></tr>
                    <tr><td>Val</td></tr>
                </table>
            </div>`;
        initTableCsvDownload();

        // Mock URL.createObjectURL and URL.revokeObjectURL
        const mockUrl = "blob:mock-url";
        const originalCreateObjectURL = URL.createObjectURL;
        const originalRevokeObjectURL = URL.revokeObjectURL;
        URL.createObjectURL = jest.fn(() => mockUrl);
        URL.revokeObjectURL = jest.fn();

        // Mock link click
        const clickSpy = jest.fn();
        const originalCreateElement = document.createElement.bind(document);
        jest.spyOn(document, "createElement").mockImplementation((tag) => {
            const el = originalCreateElement(tag);
            if (tag === "a") {
                el.click = clickSpy;
            }
            return el;
        });

        const button = document.querySelector("button");
        button.click();

        expect(URL.createObjectURL).toHaveBeenCalled();
        expect(clickSpy).toHaveBeenCalled();
        expect(URL.revokeObjectURL).toHaveBeenCalledWith(mockUrl);

        // Restore
        URL.createObjectURL = originalCreateObjectURL;
        URL.revokeObjectURL = originalRevokeObjectURL;
        document.createElement.mockRestore();
    });
});

describe("downloadCsv", () => {
    test("creates blob with BOM and triggers download", () => {
        const mockUrl = "blob:test-url";
        const originalCreateObjectURL = URL.createObjectURL;
        const originalRevokeObjectURL = URL.revokeObjectURL;
        URL.createObjectURL = jest.fn(() => mockUrl);
        URL.revokeObjectURL = jest.fn();

        const clickSpy = jest.fn();
        const originalCreateElement = document.createElement.bind(document);
        jest.spyOn(document, "createElement").mockImplementation((tag) => {
            const el = originalCreateElement(tag);
            if (tag === "a") {
                el.click = clickSpy;
            }
            return el;
        });

        downloadCsv("Name,Age\r\nAlice,30", "test-file");

        expect(URL.createObjectURL).toHaveBeenCalled();
        const blob = URL.createObjectURL.mock.calls[0][0];
        expect(blob).toBeInstanceOf(Blob);
        expect(blob.type).toBe("text/csv;charset=utf-8;");
        expect(clickSpy).toHaveBeenCalled();
        expect(URL.revokeObjectURL).toHaveBeenCalledWith(mockUrl);

        // Restore
        URL.createObjectURL = originalCreateObjectURL;
        URL.revokeObjectURL = originalRevokeObjectURL;
        document.createElement.mockRestore();
    });
});
