# TPR Documents

The 'TPR Documents' component can be used to display a list of documents.

## Example

```cshtml
<tpr-documents outercss="custom-documents">

    <tpr-document href="/documents/annual-report-2025.pdf" kbsize="20" pages="1" datepublished="March 2025" innercss="custom-document">
        
      <tpr-document-title>
            Annual Report 2025
      </tpr-document-title>

      <tpr-document-description>
            Annual report covering scheme performance and regulatory updates.
      </tpr-document-description>

    </tpr-document>

</tpr-documents>
```

---

## `<tpr-documents>`

Container component required to group one or more document entries.

## Attributes

| Attribute | Required | Description |
|------------|----------|-------------|
| `outercss` | No | Additional CSS classes applied to the document list container. |


## `<tpr-document>`

Represents an individual document within a document list.

## Attributes

| Attribute | Required | Description |
|------------|----------|-------------|
| `href` | Yes | URL the document title links to. |
| `kbsize` | No | File size in kilobytes (KB). Typically used for downloadable files such as PDFs or Word documents etc. |
| `pages` | No | Number of pages in the document. |
| `pages-label` | No | Label displayed alongside the page count. Useful for localisation. If attribute is not provided on tag, will default to "page(s)". |
| `published-label` | No | Label displayed before the publication date. Useful for localisation. Will only display if `datepublished` value is also provided. If attribute is not provided on tag, will default to "Published:" |
| `datepublished` | No | Publication date displayed underneath document title. |
| `innercss` | No | Additional CSS classes applied to the document item. |


## `<tpr-document-title>`

The document title displayed as the primary link text.

## `<tpr-document-description>`

Optional supporting content displayed beneath the document title.

# Umbraco example

The 'Documents' block is supported on the 'TPR Block Grid' and 'TPR Block List' data types in Umbraco. The block should look like this:

![TPR Documents block inside TPR Block Grid / TPR Block List](../images/tpr-documents-block.png)

Once you have clicked on the block, an inner custom Block List will appear that only allows for the pre-created Document type:

![TPR Documents custom inner block list](../images/tpr-documents-inner-blocklist.png)

After pressing 'Add Document' the following menu should appear, prompting you to link the document (which can be to either a media item that has already been uploaded to Umbraco, or a URL) as well as fill out any relevant information about the document. The 'Number of pages' field should only be filled out if the document you are linking to is a piece of media such as a .PDF or .DOCX file. A media file that consists of a spreadsheet such as an Excel file will not display the 'Number of pages' field.

![TPR Documents document input form](../images/tpr-documents-input-document-form.png)

After adding a couple of documents to the list, it should look like so:

![TPR Documents populated inner block list](../images/tpr-documents-populated-inner-blocklist.png)

This is what the generated HTML should look like, as well as what is displayed on the screen:

```html
<dl class="tpr-documents govuk-list">
  <div class="tpr-document">
    <dt><a class="govuk-link" href="/media/icaposfq/example-pdf-file.pdf">Example PDF file<br /><span class="pdf fileicon">PDF</span> 20KB, 1 page(s) </dt></a>
    <dd>Published: May 2025</dd>
    <dd>This is an example PDF file, uploaded for example purposes.</dd>
  </div>
  <div class="tpr-document">
    <dt><a class="govuk-link" href="/media/pjebff53/example-word-file.docx">Example Word file<br /><span class="doc fileicon">Word</span> 13KB, 1 page(s) </dt></a>
    <dd>Published: May 2025</dd>
    <dd>This is an example Word file, uploaded for example purposes.</dd>
  </div>
  <div class="tpr-document">
    <dt><a class="govuk-link" href="/media/grphyhbj/example-powerpoint-file.pptx">Example PowerPoint file<br /><span class="powerpoint fileicon">PPTX</span> 37KB, 2 page(s) </dt></a>
    <dd>Published: May 2025</dd>
    <dd>This is an example PowerPoint file, uploaded for example purposes.</dd>
  </div>
  <div class="tpr-document">
    <dt><a class="govuk-link" href="/">Home page</a></dt>
    <dd>This directs you back to the homepage for example purposes. In practice, this would link to a document or article on another page.</dd>
  </div>
</dl>
```

![TPR Documents on screen display](../images/tpr-documents-on-screen-display.png)

