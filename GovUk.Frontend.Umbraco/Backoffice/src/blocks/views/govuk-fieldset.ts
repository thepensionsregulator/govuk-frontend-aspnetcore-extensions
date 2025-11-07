import { html, customElement, LitElement, property, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IBlockListProperty } from '../interfaces/IBlockListProperty';
import { UMB_DOCUMENT_PROPERTY_DATASET_CONTEXT } from '@umbraco-cms/backoffice/document';

interface IGovUkFieldsetContent extends UmbBlockDataType {
    legend: string;
    blocks: IBlockListProperty | null;
}

interface IGovUkFieldsetSettings extends UmbBlockDataType {
    legendIsPageHeading: boolean;
    cssClasses: string;
}


@customElement('govuk-fieldset')
export class GovUkFieldsetView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkFieldsetContent;

    @property({ attribute: false })
    settings?: IGovUkFieldsetSettings;
    
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
        let blocks = "No blocks.";    
        if (this.content?.blocks?.contentData?.length === 1) { blocks = "1 block." }
        if ((this.content?.blocks?.contentData?.length || 0) > 1) { blocks = `${this.content?.blocks?.contentData?.length} blocks.` }

        const legend = (this.content?.legend || "").replace("{{name}}", this._nodeName || "");
        const legendClass = this.settings?.legendIsPageHeading ? 'govuk-fieldset__legend--l' : 'govuk-fieldset__legend--for-fieldset';

        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="backoffice-block-view">
            <fieldset class="govuk-fieldset ${ this.settings?.cssClasses}">
                <legend class="govuk-fieldset__legend ${legendClass}">${ this.settings?.legendIsPageHeading ? html `<h1 class="govuk-fieldset__heading">${ legend }</h1>`: legend }</legend>
                <p class="backoffice-additional-blocks">${ blocks }</p>
            </fieldset>
        </div>
        `;
    }
}

export default GovUkFieldsetView;