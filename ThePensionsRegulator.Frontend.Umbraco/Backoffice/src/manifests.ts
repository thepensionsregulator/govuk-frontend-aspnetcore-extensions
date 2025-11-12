export const manifests: Array<UmbExtensionManifest> = [
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.tprBackToMenu',
        name: "Block editor custom view for 'Back to menu' blocks",
        element: () => import('./blocks/views/tpr-back-to-menu'),
        forContentTypeAlias: 'tprBackToMenu'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.tprRelatedLinks',
        name: "Block editor custom view for 'Related links' blocks",
        element: () => import('./blocks/views/tpr-related-links'),
        forContentTypeAlias: 'tprRelatedLinks'
    }
];