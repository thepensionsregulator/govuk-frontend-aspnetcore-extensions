export const manifests: Array<UmbExtensionManifest> = [
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukButton',
        name: "Block editor custom view for 'Button' blocks",
        element: () => import('./blocks/views/govuk-button'),
        forContentTypeAlias: 'govukButton'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukButtonGroup',
        name: "Block editor custom view for 'Button group' blocks",
        element: () => import('./blocks/views/govuk-button-group'),
        forContentTypeAlias: 'govukButtonGroup'
    },
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
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukErrorMessage',
        name: "Block editor custom view for 'Error message' blocks",
        element: () => import('./blocks/views/govuk-error-message'),
        forContentTypeAlias: 'govukErrorMessage'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukErrorSummary',
        name: "Block editor custom view for 'Error summary' blocks",
        element: () => import('./blocks/views/govuk-error-summary'),
        forContentTypeAlias: 'govukErrorSummary'
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
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukLinkAsButton',
        name: "Block editor custom view for 'Link, styled as a button' blocks",
        element: () => import('./blocks/views/govuk-link-as-button'),
        forContentTypeAlias: 'govukLinkAsButton'
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