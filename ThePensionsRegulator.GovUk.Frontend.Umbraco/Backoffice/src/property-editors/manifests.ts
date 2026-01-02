export const manifests: Array<UmbExtensionManifest> = [
    {
        "type": "propertyEditorUi",
        "alias": "govukModelProperty",
        "name": "Model Property Picker Property Editor UI",
        "element": () => import('./views/govuk-model-property-picker'),
        "elementName": "govuk-model-property-picker",
        "meta": {
            "label": "Model Property Picker",
            "propertyEditorSchemaAlias": "Umbraco.Plain.String",
            "icon": "icon-brackets",
            "group": "pickers"
        }
    }
];