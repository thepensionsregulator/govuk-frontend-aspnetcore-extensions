import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';

interface IGovUkPaginationContent extends UmbBlockDataType {
}

interface IGovUkPaginationSettings extends UmbBlockDataType {
    cssClasses: string;
    nextPageLabel: string;
}


@customElement('govuk-pagination')
export class GovUkPaginationView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkPaginationContent;

    @property({ attribute: false })
    settings?: IGovUkPaginationSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editSettingsPath ?? ''}" class="backoffice-block-view">
            <nav class="govuk-pagination--block govuk-pagination ${ this.settings?.cssClasses}">
                <div class="govuk-pagination__next">
                    <span class="govuk-link govuk-pagination__link">
                        <svg class="govuk-pagination__icon govuk-pagination__icon--next" focusable="false" height="13" viewBox="0 0 15 13" width="15" xmlns="http://www.w3.org/2000/svg">
                            <path d="m8.107-0.0078125-1.4136 1.414 4.2926 4.293h-12.986v2h12.896l-4.1855 3.9766 1.377 1.4492 6.7441-6.4062-6.7246-6.7266z"></path>
                        </svg> 
                        <span class="govuk-pagination__link-title">Next</span><span class="govuk-visually-hidden">:</span>
                        <span class="govuk-pagination__link-label">${ this.settings?.nextPageLabel || '{{page}} of {{total}}' }</span>
                     </span>
                </div>
            </nav>
        </a>`;
    }
}

export default GovUkPaginationView;