import { html, customElement, LitElement, property,state } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { UMB_DOCUMENT_PROPERTY_DATASET_CONTEXT } from '@umbraco-cms/backoffice/document';

interface IGovUkPageHeadingContent extends UmbBlockDataType {
    text: string;
}

interface IGovUkPageHeadingSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-page-heading')
export class GovUkPageHeadingView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkPageHeadingContent;

    @property({ attribute: false })
    settings?: IGovUkPageHeadingSettings;

    @state()
    _nodeName?: string;

    constructor() {
        super();
       
        this.consumeContext(UMB_DOCUMENT_PROPERTY_DATASET_CONTEXT, (context) => {
            if (!context) return;

            this.observe(context.name, (nodeName) => {
                if (nodeName) {
                    this._nodeName = nodeName;
                }
	        });
        });
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <h1 class="govuk-heading-l backoffice-block-view ${ this.settings?.cssClasses }">${this.content?.text ? this.content.text : this._nodeName }</h1>
        `;
    }
}

export default GovUkPageHeadingView;