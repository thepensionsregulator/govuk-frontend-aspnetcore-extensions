# Allow a TPR component as a child of a GOV.UK component

Many GOV.UK components allow child components to be added in a block grid or block list. However, TPR components and styling are packaged separately to GOV.UK components so that they remain an optional add-on to our implementation of the GOV.UK Design System. GOV.UK components only allow other GOV.UK components. This means that a GOV.UK component cannot be configured to allow a TPR component as a child.

The solution to this is to create a TPR version of the GOV.UK component. For example, we support both GOV.UK and TPR versions of the [Accordion component](https://design-system.service.gov.uk/components/accordion/). The TPR version allows images, which do not feature in the GOV.UK Design System.

1. Start by ensuring your configuration is up-to-date:

   - in git, pull the latest `develop` branch
   - [run the Umbraco example app](/docs/umbraco/run-example-application.md)
   - in the Umbraco backoffice select 'Settings > uSync > Everything > Import'.

2. Copy the document types:

   - In the Umbraco backoffice in 'Settings > Document Types' copy the document type(s) for the component from the 'GOV.UK' folder to the 'TPR' folder. Make sure you include any settings or child document types. Even if they're exact duplicates now, this gives us the freedom to add extra settings later. For example, copy 'Accordion settings', 'Accordion section' and 'Accordion section settings' as well as 'Accordion', but you don't need to copy all the other GOV.UK components allowed within an 'Accordion section'.
   - Go to the 'Settings > Document Types > TPR' folder and update the alias for each copied document type. The alias must begin with `tpr` instead of `govuk`. For example `govukAccordion` becomes `tprAccordion`. Remove `(copy)` from the document type name.

3. Copy the data types:

   - In the Umbraco backoffice in 'Settings > Data Types > GOV.UK' copy the data type(s) for any block grids or block lists that allow child components to be added to your component from the 'GOV.UK' folder to the 'TPR' folder. Rename the copy to begin with `TPR` instead of `GOV.UK`, and remove `(copy)` from the name. For example, copy 'GOV.UK Accordion block list' to 'TPR Accordion block list'.
   - Update the new TPR data type(s) to allow the additional child component(s) you need.
   - On the 'TPR Block grid' and 'TPR Block list' data types remove the GOV.UK component and replace it with the new TPR version of the component. Select your new document types and data types, but otherwise configure all settings as before.

4. Create a view in `ThePensionsRegulator.Frontend.Umbraco\Views\Shared\TPR`. The name of the view must match the document type alias (for example `TPRAccordion.cshtml`). The `TprPartialViewPathProvider` class configures any components with an alias starting with `tpr` to look for a view in this folder. To avoid duplicating code the view can reference the GOV.UK version:

   ```razor
   @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
   <partial name="GOVUK/GovUkAccordion" />
   ```

5. In the Umbraco backoffice select 'Settings > Models Builder > Generate models'.
6. In the Umbraco backoffice select 'Settings > uSync > Everything > Export > Clean Export'. Run this twice as it doesn't always export everything the first time.
7. Add an example of the allowed component on a page in the example app.
8. Increment the version in `Solution Items/Directory.Build.props` and create a pull request.
