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

1. Finds all `<table class="govuk-table">` elements on the page.
2. Inserts a `<button class="govuk-button govuk-button--secondary">` immediately after each table.
3. On button click, converts the table to CSV and triggers a file download.

The script is idempotent — if it runs more than once, it will not create duplicate buttons.

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

Tables containing cells with `colspan` or `rowspan` greater than 1 are skipped — no download button is added — because merged cells cannot be reliably represented in CSV.
