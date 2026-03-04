# TPR Documents

The 'TPR Documents' component can be used to display a list of documents.

## Umbraco example

The 'Documents' block is supported on the 'TPR Block Grid' and 'TPR Block List' data types in Umbraco. The block should look like this:

![TPR Documents block inside TPR Block Grid / TPR Block List](../images/tpr-documents-block.png)

Once you have clicked on the block, an inner custom Block List will appear that only allows for the pre-created Document type:

![TPR Documents custom inner block list](../images/tpr-documents-inner-blocklist.png)

After pressing 'Add Document' the following menu should appear, prompting you to link the document (which can be to either a media item that has already been uploaded to Umbraco, or a URL) as well as fill out any relevant information about the document. The 'Number of pages' field should only be filled out if the document you are linking to is a piece of media such as a .PDF or .DOCX file. A media file that consists of a speadsheet such as an Excel file will not display the 'Number of pages' field.

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
