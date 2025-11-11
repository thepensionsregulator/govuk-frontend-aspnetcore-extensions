import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { renderCheckboxesDivider } from '../helpers/checkboxes-helper';
interface IGovUkCheckboxesDividerContent extends UmbBlockDataType {
    text: string;
}

@customElement('govuk-checkboxes-divider')
export class GovUkCheckboxesDividerView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkCheckboxesDividerContent;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            ${renderCheckboxesDivider(this.content?.text) }
        </a>
        `;
    }
}

export default GovUkCheckboxesDividerView;