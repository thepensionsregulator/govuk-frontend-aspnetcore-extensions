export const manifests: Array<UmbExtensionManifest> = [
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukPageHeading',
        name: 'Block editor custom view for Page Heading blocks',
        element: () => import('./blocks/views/govuk-page-heading'),
        forContentTypeAlias: 'govukPageHeading'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukCaption',
        name: 'Block editor custom view for Caption blocks',
        element: () => import('./blocks/views/govuk-caption'),
        forContentTypeAlias: 'govukCaption'
    }
];