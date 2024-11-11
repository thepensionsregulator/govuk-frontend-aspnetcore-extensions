# Configure heading levels

The [GOV.UK Design System page on headings](https://design-system.service.gov.uk/styles/headings/) says:

> "For a question page, or pages with long headings, follow the usual hierarchy of heading levels and styles associated with them. For example, `govuk-heading-l` for an `<h1>`, followed by `govuk-heading-m` for an `<h2>` and so on. In rare cases, you might want to alter how you use the headings hierarchy to achieve a better visual balance.
> ...
> If your page has lots of long form content, start with `govuk-heading-xl` for an `<h1>`, `govuk-heading-l` for an `<h2>`, and so on."

The typography scale for headings starts at `govuk-heading-l` for an `<h1>` by default. To change the typography scale to start at `govuk-heading-xl` for an `<h1>` add the 'GOV.UK/Page settings/Page heading size' composition to your document type.

If your site has lots of long form content add it to your 'Home' document type to set it sitewide. This will make a 'Page heading size' property available on your home page.

![Page heading size setting](/docs/images/page-heading-size.png)

For the rare cases where you need to change the typography scale for a better visual balance, add the composition to the document type for a specific page to override the setting for a single page and its descendants.

Using either typography scale, an editor has the option to select the most appropriate heading size in the rich text editor. This allows them to follow the standard for the site, but also in rare cases to select a different appearance for a better visual balance, while retaining the correct heading hierarchy.

![Heading levels in the rich text editor](/docs/images/heading-levels-in-rich-text-editor.png)
