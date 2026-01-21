import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType, UmbBlockValueType } from '@umbraco-cms/backoffice/block';
import { UmbBlockListLayoutModel } from '@umbraco-cms/backoffice/block-list';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';
import { renderRadioButton } from '../helpers/radios-helper';

interface IGovUkRadioContent extends UmbBlockDataType {
    conditionalBlocks: UmbBlockValueType<UmbBlockListLayoutModel>;
    hint: UmbPropertyEditorRteValueType;
    label: string;
    value: string;
}

interface IGovUkRadioSettings extends UmbBlockDataType {
    cssClasses: string;
}

@customElement('govuk-radio')
export class GovUkRadioView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkRadioContent;

    @property({ attribute: false })
    settings?: IGovUkRadioSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;
    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            ${renderRadioButton(this.content?.label, this.content?.value, this.content?.hint?.markup, this.content?.conditionalBlocks?.contentData, true) }
        </a>
        `;
    }
}

export default GovUkRadioView;