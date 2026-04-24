# Table CSV download

Adds a "Download table data (CSV)" button below each `.govuk-table` on the page, allowing users to download the table data as a CSV file. This improves accessibility by providing an alternative way to consume tabular data.

## Enabling the feature

Set `EnableTableCsvDownload = true` on `TprFrontendOptions`:

```csharp
// When using ThePensionsRegulator.Frontend directly
builder.Services.AddTprFrontend(options => { options.EnableTableCsvDownload = true; });

// When using ThePensionsRegulator.Frontend.Umbraco
builder.Services.AddTprFrontendUmbraco(tprOptions => { tprOptions.EnableTableCsvDownload = true; });
```

When enabled, a `<script>` tag is added to the `TPR/BodyClosing` partial with `type="module"`.

## How it works

On `DOMContentLoaded`, the script:

1. Finds all `<table class="govuk-table">` elements not already inside a `.tpr-table-wrapper`.
2. Wraps each table in a `<div class="tpr-table-wrapper">`.
3. Adds a `<button class="govuk-button govuk-button--secondary">` below the table.
4. On button click, converts the table to CSV (handling `colspan` and `rowspan`) and triggers a file download.

### File naming

The downloaded file name is derived from the table's `<caption>` element. If there is no caption, the file is named `table-data.csv`. The name is sanitised to remove special characters and truncated to 50 characters.

### CSV format

- Follows [RFC 4180](https://datatracker.ietf.org/doc/html/rfc4180) for quoting and escaping.
- Includes a UTF-8 BOM for correct display in Excel.
- Mitigates CSV injection by prefixing formula-triggering characters (`=`, `+`, `-`, `@`, tab, carriage return) with a single quote.

## Customising button text

The default button text is **"Download table data (CSV)"**. To customise it (e.g. for Welsh language support), add a `data-tpr-table-csv-download-text` attribute to the `<body>` element:

```html
<body data-tpr-table-csv-download-text="Lawrlwytho data tabl (CSV)">
```

In Umbraco, a dictionary item `Table CSV Download Button Text` can be used to provide translations. The layout page should render the body attribute using the dictionary value.

## Merged cells

Tables with `colspan` or `rowspan` attributes are supported. Merged cells are expanded in the CSV output:

- The text appears in the first cell of the merged area.
- Remaining cells in the merged area are left empty.
