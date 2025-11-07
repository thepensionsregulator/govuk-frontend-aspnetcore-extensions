export const manifests: Array<UmbExtensionManifest> = [
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukCaption',
        name: "Block editor custom view for 'Caption' blocks",
        element: () => import('./blocks/views/govuk-caption'),
        forContentTypeAlias: 'govukCaption'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukDetails',
        name: "Block editor custom view for 'Details' blocks",
        element: () => import('./blocks/views/govuk-details'),
        forContentTypeAlias: 'govukDetails'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukInsetText',
        name: "Block editor custom view for 'Inset text' blocks",
        element: () => import('./blocks/views/govuk-inset-text'),
        forContentTypeAlias: 'govukInsetText'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukPageHeading',
        name: "Block editor custom view for 'Page heading' blocks",
        element: () => import('./blocks/views/govuk-page-heading'),
        forContentTypeAlias: 'govukPageHeading'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukTypography',
        name: "Block editor custom view for 'Text' blocks",
        element: () => import('./blocks/views/govuk-typography'),
        forContentTypeAlias: 'govukTypography'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukWarningText',
        name: "Block editor custom view for 'Warning text' blocks",
        element: () => import('./blocks/views/govuk-warning-text'),
        forContentTypeAlias: 'govukWarningText'
    }
];