import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { PACKAGE_VERSION } from '../../package-version.generated';
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

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <div class="govuk-error-summary">
                <h2 class="govuk-error-summary__title">${ this.content?.title || "There is a problem" }</h2>
                <div class="govuk-error-summary__body">
                    <ul class="govuk-error-summary__list govuk-list"><li><span class="govuk-link">Example error message</span></li></ul>
                </div>
            </div>
        </a>
        `;
    }
}

export default GovUkErrorSummaryView;