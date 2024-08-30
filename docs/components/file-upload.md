# File upload

For examples see [ASP.NET syntax for the File upload component](https://github.com/gunndabad/govuk-frontend-aspnetcore/blob/main/docs/components/file-upload.md).

## Umbraco

You can add a file upload component to a block grid or block list in Umbraco. For an example of this component in use, see the 'File upload' page in the Umbraco example app.

Error messages for file upload components using `[Required]` are configurable in Umbraco on the settings for the file upload component.

![File upload settings in Umbraco](/docs/images/file-upload-settings.png)


### Validating allowed file types

Use the `[AllowedFileTypes]` attribute to validate allowed file types. This uses a comparison of the binary file header against a set of known binary sequences that represents the respective file type.

Error messages for file upload components using `[AllowedFileTypes]` are configurable in Umbraco on the settings for the file upload component.

![File validation type settings in Umbraco](/docs/images/filetype-validation-settings.png)