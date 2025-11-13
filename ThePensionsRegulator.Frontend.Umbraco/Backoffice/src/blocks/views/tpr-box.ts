import { html, customElement, LitElement, property, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { UmbBlockGridTypeModel } from '@umbraco-cms/backoffice/block-grid';


interface IColourPickerValue {
    label: string;
}
interface ITprBoxSettings extends UmbBlockDataType {
    styleOfBox: string;
    backgroundColour: IColourPickerValue;
    cssClasses: string;
}

@customElement('tpr-box')
export class TprBoxView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: any;

    @property({ attribute: false })
    settings?: ITprBoxSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    @state()
    blockType?: UmbBlockGridTypeModel;

    constructor() {
        super();
    }

    override render() {
        console.log(this.blockType) 

        const cssClasses = this.settings?.styleOfBox === 'Bordered' ? ' tpr-box--bordered' : (this.settings?.backgroundColour.label === 'Blue' ? ' tpr-box--blue' : null);
        const isNestedBox = this.blockType?.contentElementTypeKey == "2e831668-9e36-44d9-95f4-de209f9a35d0";

        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a class="backoffice-block-header ${isNestedBox ? 'for-tpr-nested-box' : 'for-tpr-box'}" href="${this.config?.editSettingsPath}">
			<uui-icon-registry-essential>
                <uui-icon name="icon-checkbox-empty" aria-hidden="true" />
            </uui-icon-registry-essential>
			<span class="content-type">${isNestedBox ? 'Nested box' : 'Box'}</span>
        </a>
        <div class="tpr-box ${cssClasses} ${this.settings?.cssClasses}">
            <umb-block-grid-areas-container areas="${this.blockType?.areas ?? []}" areaGridColumns="${this.blockType?.areaGridColumns}"/>
        </div>`;
    }
}

export default TprBoxView;