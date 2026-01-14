import { html, customElement, LitElement, property, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IRichTextProperty } from '../interfaces/IRichTextProperty';
import { IBlockListProperty } from "../interfaces/IBlockListProperty";

interface IGovUkCheckboxContent extends UmbBlockDataType {
    label: string;
    value: string;
    hint: IRichTextProperty;
    conditionalBlocks: IBlockListProperty;
}

interface IGovUkCheckboxSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-checkbox')
export class GovUkCheckboxView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkCheckboxContent;

    @property({ attribute: false })
    settings?: IGovUkCheckboxSettings;

    constructor() {
        super();
    }

    override render() {

        let conditionalBlocksText = "No conditional blocks.";
        if (this.content?.conditionalBlocks?.contentData?.length === 1) { conditionalBlocksText = "1 conditional block." }
        if ((this.content?.conditionalBlocks?.contentData?.length || 0) > 1) { conditionalBlocksText = `${this.content?.conditionalBlocks.contentData.length} conditional blocks.` }

        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="govuk-form-group backoffice-block-view">
            <div class="govuk-checkboxes ${ this.settings?.cssClasses}">
                <div class="govuk-checkboxes__item">
                    <input class="govuk-checkboxes__input" type="checkbox" value="${this.content?.value}" />
                    <label class="govuk-checkboxes__label govuk-label">${this.content?.label}</label>
                    ${this.content?.hint?.markup ? html`<div class="govuk-checkboxes__hint govuk-hint">${unsafeHTML(this.content?.hint?.markup)}</div>` : null}
                </div>
                <div class="govuk-checkboxes__conditional">
                    <div class="govuk-form-group">
                        <p class="backoffice-additional-blocks">${conditionalBlocksText}</p>
                    </div> 
                </div>
            </div>
        </div>
        `;
    }
}

export default GovUkCheckboxView;