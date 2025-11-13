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
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.tprDocument',
        name: "Block editor custom view for 'Document' blocks",
        element: () => import('./blocks/views/tpr-document'),
        forContentTypeAlias: 'tprDocument'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.tprDocuments',
        name: "Block editor custom view for 'Documents' blocks",
        element: () => import('./blocks/views/tpr-documents'),
        forContentTypeAlias: 'tprDocuments'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.tprRelatedLinks',
        name: "Block editor custom view for 'Related links' blocks",
        element: () => import('./blocks/views/tpr-related-links'),
        forContentTypeAlias: 'tprRelatedLinks'
    },
    {
        type: 'blockEditorCustomView',
        alias: 'ThePensionsRegulator.GovUk.Frontend.Umbraco.BlockEditorViews.tprYouTubeVideo',
        name: "Block editor custom view for 'YouTube video' blocks",
        element: () => import('./blocks/views/tpr-youtube-video'),
        forContentTypeAlias: 'tprYouTubeVideo'
    }
];