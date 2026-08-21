import '@testing-library/jest-dom';
import { jest } from '@jest/globals';
import { getButtonText, sanitizeFileName, getCellText, escapeCsvValue, hasMergedCells, tableToCsv, initTableCsvDownload, downloadCsv, hasExistingDownloadButton, findServerSideDownloadForm } from '../wwwroot/ThePensionsRegulator.Frontend/js/tpr-table-csv-download';

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

    test("expands a colspan into empty trailing fields", () => {
        document.body.innerHTML = `
            <table class="govuk-table">
                <tr><td colspan="2">Merged</td></tr>
                <tr><td>A</td><td>B</td></tr>
            </table>`;
        const table = document.querySelector("table");
        expect(tableToCsv(table)).toBe("Merged,\r\nA,B");
    });

    test("expands a rowspan by carrying an empty field into following rows", () => {
        document.body.innerHTML = `
            <table class="govuk-table">
                <tr><td rowspan="2">R</td><td>B1</td></tr>
                <tr><td>B2</td></tr>
            </table>`;
        const table = document.querySelector("table");
        expect(tableToCsv(table)).toBe("R,B1\r\n,B2");
    });

    test("expands a rowspan in the last column", () => {
        document.body.innerHTML = `
            <table class="govuk-table">
                <tr><td>A1</td><td rowspan="2">B</td></tr>
                <tr><td>A2</td></tr>
            </table>`;
        const table = document.querySelector("table");
        expect(tableToCsv(table)).toBe("A1,B\r\nA2,");
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

    test("should not create a button on tables with an exsisting hardcoded button", () => {
        document.body.innerHTML = `
            <table class="govuk-table">
                 <thead><tr><th>Name</th><th>Age</th></tr></thead>
                 <tbody><tr><td>Alice</td><td>30</td></tr></tbody>
            </table> 
            <form action="/example" class="tpr-table-download-form">
                <input type="hidden">
                 <input type="hidden">
                 <input type="hidden">
                 <button class="govuk-button">Existing Button</button>
            </form>`
        initTableCsvDownload();
        const buttons = document.querySelectorAll(
            'button'
        );
        expect(buttons.length).toBe(1);
    });

    test("does not add a button when the hardcoded download form has additional classes", () => {
        document.body.innerHTML = `
            <table class="govuk-table">
                <thead><tr><th>Name</th><th>Age</th></tr></thead>
                <tbody><tr><td>Alice</td><td>30</td></tr></tbody>
            </table>
            <form action="/api/table/download-csv" method="post" class="tpr-table-download-form govuk-!-margin-top-3">
                <input type="hidden" name="tableHtml">
                <button class="govuk-button">Download table data (CSV)</button>
            </form>`;
        initTableCsvDownload();
        const buttons = document.querySelectorAll('button');
        expect(buttons.length).toBe(1);
        expect(document.querySelector('button[data-tpr-table-csv-button="true"]')).not.toBeInTheDocument();
    });

    test("does not add a button when the hardcoded download form is nested in a wrapping sibling", () => {
        document.body.innerHTML = `
            <table class="govuk-table">
                <thead><tr><th>Name</th><th>Age</th></tr></thead>
                <tbody><tr><td>Alice</td><td>30</td></tr></tbody>
            </table>
            <div class="download-wrapper">
                <form action="/api/table/download-csv" method="post" class="tpr-table-download-form">
                    <button class="govuk-button">Download table data (CSV)</button>
                </form>
            </div>`;
        initTableCsvDownload();
        expect(document.querySelectorAll('button').length).toBe(1);
    });

    test("still adds a button when a following form is not a download form", () => {
        document.body.innerHTML = `
            <table class="govuk-table">
                <thead><tr><th>Name</th></tr></thead>
                <tbody><tr><td>Alice</td></tr></tbody>
            </table>
            <form action="/search" class="search-form">
                <button class="govuk-button">Search</button>
            </form>`;
        initTableCsvDownload();
        expect(document.querySelector('button[data-tpr-table-csv-button="true"]')).toBeInTheDocument();
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

    test("adds a button to tables with merged cells", () => {
        document.body.innerHTML = `
            <div>
                <table class="govuk-table">
                    <tr><td colspan="2">Merged</td></tr>
                    <tr><td>A</td><td>B</td></tr>
                </table>
            </div>`;
        initTableCsvDownload();
        expect(document.querySelector('button[data-tpr-table-csv-button="true"]')).toBeInTheDocument();
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

describe("hasExistingDownloadButton", () => {
    test("returns false when there is no following element", () => {
        document.body.innerHTML = `<table class="govuk-table"><tr><td>Data</td></tr></table>`;
        const table = document.querySelector(".govuk-table");
        expect(hasExistingDownloadButton(table)).toBe(false);
    });

    test("returns true when followed by a script-added button", () => {
        document.body.innerHTML = `
            <table class="govuk-table"><tr><td>Data</td></tr></table>
            <button data-tpr-table-csv-button="true">Download table data (CSV)</button>`;
        const table = document.querySelector(".govuk-table");
        expect(hasExistingDownloadButton(table)).toBe(true);
    });

    test("returns true when followed by a hardcoded download form with a button", () => {
        document.body.innerHTML = `
            <table class="govuk-table"><tr><td>Data</td></tr></table>
            <form class="tpr-table-download-form"><button>Download</button></form>`;
        const table = document.querySelector(".govuk-table");
        expect(hasExistingDownloadButton(table)).toBe(true);
    });

    test("returns true when the download form has additional classes", () => {
        document.body.innerHTML = `
            <table class="govuk-table"><tr><td>Data</td></tr></table>
            <form class="tpr-table-download-form govuk-!-margin-top-3"><button>Download</button></form>`;
        const table = document.querySelector(".govuk-table");
        expect(hasExistingDownloadButton(table)).toBe(true);
    });

    test("returns false when the download form has no button", () => {
        document.body.innerHTML = `
            <table class="govuk-table"><tr><td>Data</td></tr></table>
            <form class="tpr-table-download-form"><input type="hidden"></form>`;
        const table = document.querySelector(".govuk-table");
        expect(hasExistingDownloadButton(table)).toBe(false);
    });

    test("returns false when followed by an unrelated form", () => {
        document.body.innerHTML = `
            <table class="govuk-table"><tr><td>Data</td></tr></table>
            <form class="search-form"><button>Search</button></form>`;
        const table = document.querySelector(".govuk-table");
        expect(hasExistingDownloadButton(table)).toBe(false);
    });

    test("returns true when the download form is nested in a wrapping sibling", () => {
        document.body.innerHTML = `
            <table class="govuk-table"><tr><td>Data</td></tr></table>
            <div class="download-wrapper">
                <form class="tpr-table-download-form"><button>Download</button></form>
            </div>`;
        const table = document.querySelector(".govuk-table");
        expect(hasExistingDownloadButton(table)).toBe(true);
    });
});

describe("findServerSideDownloadForm", () => {
    test("returns null when there is no following element", () => {
        document.body.innerHTML = `<table class="govuk-table"><tr><td>Data</td></tr></table>`;
        const table = document.querySelector(".govuk-table");
        expect(findServerSideDownloadForm(table)).toBeNull();
    });

    test("returns null when followed by an unrelated form", () => {
        document.body.innerHTML = `
            <table class="govuk-table"><tr><td>Data</td></tr></table>
            <form class="search-form"><button>Search</button></form>`;
        const table = document.querySelector(".govuk-table");
        expect(findServerSideDownloadForm(table)).toBeNull();
    });

    test("returns null when followed by a script-added button (not a form)", () => {
        document.body.innerHTML = `
            <table class="govuk-table"><tr><td>Data</td></tr></table>
            <button data-tpr-table-csv-button="true">Download table data (CSV)</button>`;
        const table = document.querySelector(".govuk-table");
        expect(findServerSideDownloadForm(table)).toBeNull();
    });

    test("returns the form when directly followed by a tpr-table-download-form", () => {
        document.body.innerHTML = `
            <table class="govuk-table"><tr><td>Data</td></tr></table>
            <form class="tpr-table-download-form"><button>Download</button></form>`;
        const table = document.querySelector(".govuk-table");
        const form = document.querySelector(".tpr-table-download-form");
        expect(findServerSideDownloadForm(table)).toBe(form);
    });

    test("returns the form when it has additional classes", () => {
        document.body.innerHTML = `
            <table class="govuk-table"><tr><td>Data</td></tr></table>
            <form class="tpr-table-download-form govuk-!-margin-top-3"><button>Download</button></form>`;
        const table = document.querySelector(".govuk-table");
        const form = document.querySelector(".tpr-table-download-form");
        expect(findServerSideDownloadForm(table)).toBe(form);
    });

    test("returns the form when nested in a wrapping sibling", () => {
        document.body.innerHTML = `
            <table class="govuk-table"><tr><td>Data</td></tr></table>
            <div class="download-wrapper">
                <form class="tpr-table-download-form"><button>Download</button></form>
            </div>`;
        const table = document.querySelector(".govuk-table");
        const form = document.querySelector(".tpr-table-download-form");
        expect(findServerSideDownloadForm(table)).toBe(form);
    });

    test("returns null when the form has no button", () => {
        document.body.innerHTML = `
            <table class="govuk-table"><tr><td>Data</td></tr></table>
            <form class="tpr-table-download-form"><input type="hidden"></form>`;
        const table = document.querySelector(".govuk-table");
        expect(findServerSideDownloadForm(table)).toBeNull();
    });
});

describe("initTableCsvDownload — server-side form interception", () => {
    function mockDownload() {
        const mockUrl = "blob:mock-url";
        URL.createObjectURL = jest.fn(() => mockUrl);
        URL.revokeObjectURL = jest.fn();
        const clickSpy = jest.fn();
        const originalCreateElement = document.createElement.bind(document);
        jest.spyOn(document, "createElement").mockImplementation((tag) => {
            const el = originalCreateElement(tag);
            if (tag === "a") el.click = clickSpy;
            return el;
        });
        return { clickSpy };
    }

    afterEach(() => {
        jest.restoreAllMocks();
    });

    test("intercepts server-side form submit and downloads CSV client-side", () => {
        document.body.innerHTML = `
            <table class="govuk-table">
                <thead><tr><th>Name</th><th>Age</th></tr></thead>
                <tbody><tr><td>Alice</td><td>30</td></tr></tbody>
            </table>
            <form method="post" action="/api/table/download-csv" class="tpr-table-download-form">
                <input type="hidden" name="tableHtml" value="...">
                <input type="hidden" name="fileName" value="table-data">
                <input type="hidden" name="__RequestVerificationToken" value="token">
                <button type="submit" class="govuk-button govuk-button--secondary">Download table data (CSV)</button>
            </form>`;

        initTableCsvDownload();
        const { clickSpy } = mockDownload();

        const form = document.querySelector(".tpr-table-download-form");
        form.dispatchEvent(new Event("submit", { bubbles: true, cancelable: true }));

        expect(URL.createObjectURL).toHaveBeenCalled();
        expect(clickSpy).toHaveBeenCalled();
    });

    test("does not add an extra button when a server-side form is present", () => {
        document.body.innerHTML = `
            <table class="govuk-table">
                <thead><tr><th>Name</th></tr></thead>
                <tbody><tr><td>Alice</td></tr></tbody>
            </table>
            <form method="post" action="/api/table/download-csv" class="tpr-table-download-form">
                <button type="submit" class="govuk-button govuk-button--secondary">Download table data (CSV)</button>
            </form>`;

        initTableCsvDownload();

        expect(document.querySelectorAll("button").length).toBe(1);
        expect(document.querySelector('button[data-tpr-table-csv-button="true"]')).not.toBeInTheDocument();
    });

    test("is idempotent — calling init twice does not attach duplicate submit listeners", () => {
        document.body.innerHTML = `
            <table class="govuk-table">
                <tr><td>Data</td></tr>
            </table>
            <form method="post" action="/api/table/download-csv" class="tpr-table-download-form">
                <button type="submit" class="govuk-button">Download</button>
            </form>`;

        initTableCsvDownload();
        initTableCsvDownload();

        const { clickSpy } = mockDownload();

        const form = document.querySelector(".tpr-table-download-form");
        form.dispatchEvent(new Event("submit", { bubbles: true, cancelable: true }));

        // createObjectURL should be called exactly once (not twice if listener was added twice)
        expect(URL.createObjectURL).toHaveBeenCalledTimes(1);
    });
});
