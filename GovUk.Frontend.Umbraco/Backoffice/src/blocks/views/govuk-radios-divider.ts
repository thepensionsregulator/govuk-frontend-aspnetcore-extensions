import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { renderRadiosDivider } from '../helpers/radios-helper';
interface IGovUkRadiosDividerContent extends UmbBlockDataType {
    text: string;
}

@customElement('govuk-radios-divider')
export class GovUkRadiosDividerView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkRadiosDividerContent;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="backoffice-block-view">
            ${renderRadiosDivider(this.content?.text) }
        </div>
        `;
    }
}

export default GovUkRadiosDividerView;