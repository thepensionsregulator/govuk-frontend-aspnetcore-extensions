import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
interface IGovUkErrorSummaryContent extends UmbBlockDataType {
    title: string;
}

interface IGovUkErrorSummarySettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-error-summary')
export class GovUkErrorSummaryView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkErrorSummaryContent;

    @property({ attribute: false })
    settings?: IGovUkErrorSummarySettings;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="backoffice-block-view">
            <div class="govuk-error-summary" aria-label="Edit error summary component">
                <h2 class="govuk-error-summary__title">${ this.content?.title || "There is a problem" }</h2>
                <div class="govuk-error-summary__body">
                    <ul class="govuk-error-summary__list govuk-list"><li><a href="javascript:return false">Example error message</a></li></ul>
                </div>
            </div>
        </div>
        `;
    }
}

export default GovUkErrorSummaryView;