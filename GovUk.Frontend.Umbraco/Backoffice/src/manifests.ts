export const manifests: Array<UmbExtensionManifest> = [
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukAccordion',
        name: "Block editor custom view for 'Accordion' blocks",
        element: () => import('./blocks/views/govuk-accordion'),
        forContentTypeAlias: ['govukAccordion','tprAccordion']
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukAccordionSection',
        name: "Block editor custom view for 'Accordion section' blocks",
        element: () => import('./blocks/views/govuk-accordion-section'),
        forContentTypeAlias: ['govukAccordionSection','tprAccordionSection']
    },
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
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukFieldset',
        name: "Block editor custom view for 'Fieldset' blocks",
        element: () => import('./blocks/views/govuk-fieldset'),
        forContentTypeAlias: 'govukFieldset'
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
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukLink',
        name: "Block editor custom view for 'Link' blocks",
        element: () => import('./blocks/views/govuk-link'),
        forContentTypeAlias: 'govukLink'
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
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukNotificationBanner',
        name: "Block editor custom view for 'Notification banner' blocks",
        element: () => import('./blocks/views/govuk-notification-banner'),
        forContentTypeAlias: 'govukNotificationBanner'
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
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukPagination',
        name: "Block editor custom view for 'Pagination' blocks",
        element: () => import('./blocks/views/govuk-pagination'),
        forContentTypeAlias: 'govukPagination'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukPanel',
        name: "Block editor custom view for 'Panel' blocks",
        element: () => import('./blocks/views/govuk-panel'),
        forContentTypeAlias: 'govukPanel'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukSelect',
        name: "Block editor custom view for 'Select' blocks",
        element: () => import('./blocks/views/govuk-select'),
        forContentTypeAlias: 'govukSelect'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukSummaryList',
        name: "Block editor custom view for 'Summary list' blocks",
        element: () => import('./blocks/views/govuk-summary-list'),
        forContentTypeAlias: 'govukSummaryList'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukSummaryCard',
        name: "Block editor custom view for 'Summary card' blocks",
        element: () => import('./blocks/views/govuk-summary-card'),
        forContentTypeAlias: 'govukSummaryCard'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukTask',
        name: "Block editor custom view for 'Task' blocks",
        element: () => import('./blocks/views/govuk-task'),
        forContentTypeAlias: 'govukTask'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukTaskList',
        name: "Block editor custom view for 'Task list' blocks",
        element: () => import('./blocks/views/govuk-task-list'),
        forContentTypeAlias: 'govukTaskList'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.govukTaskListSummary',
        name: "Block editor custom view for 'Task list summary' blocks",
        element: () => import('./blocks/views/govuk-task-list-summary'),
        forContentTypeAlias: 'govukTaskListSummary'
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