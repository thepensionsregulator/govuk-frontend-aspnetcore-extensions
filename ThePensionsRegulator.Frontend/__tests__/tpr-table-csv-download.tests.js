import "@testing-library/jest-dom";

import {
  DEFAULT_BUTTON_TEXT,
  getButtonText,
  escapeCsvValue,
  hasMergedCells,
  tableToCsv,
  sanitizeFileName,
  initTableCsvDownload,
} from "../wwwroot/tpr/tpr-table-csv-download";

const createTable = ({
  className = "govuk-table",
  caption = "Test table",
  headers = ["Name", "Age"],
  rows = [
    ["Alice", "30"],
    ["Bob", "25"],
  ],
  mergedCells = false,
} = {}) => {
  const headerHtml = headers
    .map((h) => `<th scope="col">${h}</th>`)
    .join("");
  const rowHtml = rows
    .map((r) => `<tr>${r.map((c) => `<td>${c}</td>`).join("")}</tr>`)
    .join("");

  if (mergedCells) {
    return `<table class="${className}">
      ${caption ? `<caption>${caption}</caption>` : ""}
      <thead><tr>${headerHtml}</tr></thead>
      <tbody>
        <tr><td colspan="2">Merged cell</td></tr>
        ${rowHtml}
      </tbody>
    </table>`;
  }

  return `<table class="${className}">
    ${caption ? `<caption>${caption}</caption>` : ""}
    <thead><tr>${headerHtml}</tr></thead>
    <tbody>${rowHtml}</tbody>
  </table>`;
};

describe("getButtonText", () => {
  afterEach(() => {
    delete document.body.dataset.tprTableCsvDownloadText;
  });

  it("should return default button text when no data attribute is set", () => {
    expect(getButtonText()).toBe(DEFAULT_BUTTON_TEXT);
  });

  it("should return custom button text from data attribute on body", () => {
    document.body.dataset.tprTableCsvDownloadText = "Download CSV";
    expect(getButtonText()).toBe("Download CSV");
  });
});

describe("escapeCsvValue", () => {
  it("should return empty string for null or undefined", () => {
    expect(escapeCsvValue(null)).toBe("");
    expect(escapeCsvValue(undefined)).toBe("");
    expect(escapeCsvValue("")).toBe("");
  });

  it("should return plain value when no special characters", () => {
    expect(escapeCsvValue("Hello")).toBe("Hello");
  });

  it("should wrap value in quotes when it contains a comma", () => {
    expect(escapeCsvValue("Hello, World")).toBe('"Hello, World"');
  });

  it("should escape double quotes by doubling them", () => {
    expect(escapeCsvValue('He said "hi"')).toBe('"He said ""hi"""');
  });

  it("should wrap value in quotes when it contains a newline", () => {
    expect(escapeCsvValue("Line1\nLine2")).toBe('"Line1\nLine2"');
  });

  it("should wrap value in quotes when it contains a carriage return", () => {
    expect(escapeCsvValue("Line1\rLine2")).toBe('"Line1\rLine2"');
  });

  it("should prefix values starting with = to mitigate CSV injection", () => {
    expect(escapeCsvValue("=SUM(A1:A2)")).toBe("'=SUM(A1:A2)");
  });

  it("should prefix values starting with + to mitigate CSV injection", () => {
    expect(escapeCsvValue("+cmd|' /C calc'!A0")).toBe(
      "'+cmd|' /C calc'!A0"
    );
  });

  it("should both prefix and quote-wrap when injection char and comma present", () => {
    expect(escapeCsvValue("=A,B")).toBe("\"'=A,B\"");
  });

  it("should prefix values starting with - to mitigate CSV injection", () => {
    expect(escapeCsvValue("-1+1")).toBe("'-1+1");
  });

  it("should prefix values starting with @ to mitigate CSV injection", () => {
    expect(escapeCsvValue("@SUM(A1)")).toBe("'@SUM(A1)");
  });
});

describe("hasMergedCells", () => {
  it("should return false for a table with no merged cells", () => {
    document.body.innerHTML = createTable();
    const table = document.querySelector("table");
    expect(hasMergedCells(table)).toBe(false);
  });

  it("should return true for a table with colspan > 1", () => {
    document.body.innerHTML = createTable({ mergedCells: true });
    const table = document.querySelector("table");
    expect(hasMergedCells(table)).toBe(true);
  });

  it("should return true for a table with rowspan > 1", () => {
    document.body.innerHTML = `<table class="govuk-table">
      <tbody>
        <tr><td rowspan="2">Merged</td><td>Cell</td></tr>
        <tr><td>Cell</td></tr>
      </tbody>
    </table>`;
    const table = document.querySelector("table");
    expect(hasMergedCells(table)).toBe(true);
  });

  it("should return false when colspan and rowspan are 1", () => {
    document.body.innerHTML = `<table class="govuk-table">
      <tbody>
        <tr><td colspan="1" rowspan="1">Cell</td></tr>
      </tbody>
    </table>`;
    const table = document.querySelector("table");
    expect(hasMergedCells(table)).toBe(false);
  });
});

describe("tableToCsv", () => {
  it("should generate CSV from a simple table", () => {
    document.body.innerHTML = createTable();
    const table = document.querySelector("table");
    const csv = tableToCsv(table);
    const lines = csv.split("\r\n");
    expect(lines.length).toBe(3);
    expect(lines[0]).toBe("Name,Age");
    expect(lines[1]).toBe("Alice,30");
    expect(lines[2]).toBe("Bob,25");
  });

  it("should use CRLF line endings per RFC 4180", () => {
    document.body.innerHTML = createTable({
      headers: ["A"],
      rows: [["1"]],
    });
    const table = document.querySelector("table");
    const csv = tableToCsv(table);
    expect(csv).toContain("\r\n");
    expect(csv).toBe("A\r\n1");
  });

  it("should escape values containing commas", () => {
    document.body.innerHTML = createTable({
      headers: ["Name"],
      rows: [["Smith, John"]],
    });
    const table = document.querySelector("table");
    const csv = tableToCsv(table);
    expect(csv).toContain('"Smith, John"');
  });
});

describe("sanitizeFileName", () => {
  it('should return "table-data" for null or empty input', () => {
    expect(sanitizeFileName(null)).toBe("table-data");
    expect(sanitizeFileName("")).toBe("table-data");
    expect(sanitizeFileName("   ")).toBe("table-data");
  });

  it("should convert spaces to hyphens and lowercase", () => {
    expect(sanitizeFileName("My Table Name")).toBe("my-table-name");
  });

  it("should remove special characters", () => {
    expect(sanitizeFileName("Table (2023/24)")).toBe("table-202324");
  });

  it("should truncate to 100 characters", () => {
    const longName = "a".repeat(150);
    expect(sanitizeFileName(longName).length).toBe(100);
  });
});

describe("initTableCsvDownload", () => {
    afterEach(() => {
        document.body.innerHTML = "";
        delete document.body.dataset.tprTableCsvDownloadText;
    });

    it("should add a download button after each .govuk-table", () => {
        document.body.innerHTML = createTable();
        initTableCsvDownload();
        const button = document.querySelector(
            'button[data-tpr-table-csv-button="true"]'
        );
        expect(button).not.toBeNull();
        expect(button.textContent).toBe(DEFAULT_BUTTON_TEXT);
        expect(button.className).toBe("govuk-button govuk-button--secondary");
    });

    it("should not add a button to non-.govuk-table tables", () => {
        document.body.innerHTML = createTable({ className: "other-table" });
        initTableCsvDownload();
        const button = document.querySelector(
            'button[data-tpr-table-csv-button="true"]'
        );
        expect(button).toBeNull();
    });

    it("should not add a button to tables with merged cells", () => {
        document.body.innerHTML = createTable({ mergedCells: true });
        initTableCsvDownload();
        const button = document.querySelector(
            'button[data-tpr-table-csv-button="true"]'
        );
        expect(button).toBeNull();
    });

    it("should not create duplicate buttons when called twice", () => {
        document.body.innerHTML = createTable();
        initTableCsvDownload();
        initTableCsvDownload();
        const buttons = document.querySelectorAll(
            'button[data-tpr-table-csv-button="true"]'
        );
        expect(buttons.length).toBe(1);
    });

    it("should use custom button text from data attribute", () => {
        document.body.dataset.tprTableCsvDownloadText = "Custom text";
        document.body.innerHTML = createTable();
        initTableCsvDownload();
        const button = document.querySelector(
            'button[data-tpr-table-csv-button="true"]'
        );
        expect(button.textContent).toBe("Custom text");
    });

    it("should handle multiple tables on the page", () => {
        document.body.innerHTML = createTable({ caption: "Table 1" }) +
            createTable({ caption: "Table 2" });
        initTableCsvDownload();
        const buttons = document.querySelectorAll(
            'button[data-tpr-table-csv-button="true"]'
        );
        expect(buttons.length).toBe(2);
    });

    it("should place the button immediately after the table", () => {
        document.body.innerHTML = `<div>${createTable()}</div>`;
        initTableCsvDownload();
        const table = document.querySelector(".govuk-table");
        const button = table.nextElementSibling;
        expect(button).not.toBeNull();
        expect(button.hasAttribute("data-tpr-table-csv-button")).toBe(true);
    });

    it("should derive the file name from the table caption", () => {
        document.body.innerHTML = createTable({ caption: "My Report 2024" });
        initTableCsvDownload();
        const button = document.querySelector(
            'button[data-tpr-table-csv-button="true"]'
        );
        expect(button).not.toBeNull();
        expect(button.getAttribute("data-tpr-table-csv-filename")).toBe(
            "my-report-2024"
        );
    });

    it("should skip tables with merged cells but still process simple tables", () => {
        document.body.innerHTML =
            createTable({ mergedCells: true, caption: "Merged" }) +
            createTable({ caption: "Simple" });
        initTableCsvDownload();
        const buttons = document.querySelectorAll(
            'button[data-tpr-table-csv-button="true"]'
        );
        expect(buttons.length).toBe(1);
    });

    it("should create only one button on tables with an exsisting hardcoded button", () => {
    document.body.innerHTML = `${createTable()} <form action="/example" name="tableHtml"><button class="govuk-button">Existing Button</button></form>`
    initTableCsvDownload();
    const buttons = document.querySelectorAll(
        'button'
    );
    expect(buttons.length).toBe(1);
});
});


