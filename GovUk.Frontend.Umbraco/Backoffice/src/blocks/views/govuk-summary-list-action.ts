import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { renderSummaryListAction } from '../helpers/summary-list-helper';

interface IGovUkSummaryListActionContent extends UmbBlockDataType {
    text: string;
}

@customElement('govuk-summary-list-action')
export class GovUkSummaryListActionView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkSummaryListActionContent;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="backoffice-block-view">
            ${renderSummaryListAction(this.content?.text)}
        </div>`;
    }
}

export default GovUkSummaryListActionView;