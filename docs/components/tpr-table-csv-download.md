# TPR table CSV download

Adds a CSV download button below each `.govuk-table` element on the page. This is a TPR accessibility feature that allows users to download table data in CSV format, which can be easier to read in a spreadsheet application than a complex HTML table on a small screen.

## How it works

When the script runs on `DOMContentLoaded`, it finds all elements with the `.govuk-table` class and inserts a "Download table data (CSV)" button after each one. Clicking the button generates a CSV file from the table data and triggers a download.

Tables with merged cells (`colspan` or `rowspan` greater than 1) are automatically skipped, because merged cells cannot be reliably represented in CSV format.

The script is idempotent — if it runs more than once, it will not create duplicate buttons.

## CSV standard

The generated CSV conforms to [RFC 4180](https://www.rfc-editor.org/rfc/rfc4180) and the [UK government recommended open standard for tabular data](https://www.gov.uk/government/publications/recommended-open-standards-for-government/tabular-data-standard):

- Fields are separated by commas
- Fields containing commas, double quotes, or line breaks are enclosed in double quotes
- Double quotes within a field are escaped by doubling them
- Lines are terminated with CRLF (`\r\n`)
- The file is encoded as UTF-8 with a byte order mark (BOM)

Values beginning with `=`, `+`, `-`, or `@` are prefixed with a single quote (`'`) to mitigate CSV formula injection when opened in spreadsheet applications.

## Client-side support

Include the following script to enable the feature. This is included by default when referencing `<partial name="TPR/BodyClosing" />` in your layout.

```html
<script src="/_content/ThePensionsRegulator.Frontend/tpr/tpr-table-csv-download.js" type="module"></script>
```

The script is lightweight and is a no-op if there are no `.govuk-table` elements on the page.

## Customising button text

The default button text is "Download table data (CSV)". To customise it, add a `data-tpr-table-csv-download-text` attribute to the `<body>` element:

```html
<body data-tpr-table-csv-download-text="Download as CSV">
```

## File name

The downloaded file name is derived from the table's `<caption>` element text, sanitised to remove special characters. If no caption is present, the file is named `table-data.csv`.

## Configuration

Add the `EnableTableCsvDownload` property to `TprFrontendOptions` when registering services:

```csharp
services.AddTprFrontend(
    tprOptions =>
    {
        tprOptions.EnableTableCsvDownload = true;
    }
);
```

## Example

Given this table:

```html
<table class="govuk-table">
  <caption>Quarterly results</caption>
  <thead>
    <tr>
      <th scope="col">Quarter</th>
      <th scope="col">Revenue</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>Q1</td>
      <td>£1,000</td>
    </tr>
    <tr>
      <td>Q2</td>
      <td>£1,500</td>
    </tr>
  </tbody>
</table>
```

The script will insert a secondary GOV.UK button after the table. Clicking it downloads `quarterly-results.csv` with this content:

```
Quarter,Revenue
Q1,"£1,000"
Q2,"£1,500"
```

## Tables that are skipped

The following tables will **not** get a download button:

- Tables without the `.govuk-table` class
- Tables containing cells with `colspan` or `rowspan` greater than 1 (merged cells)
