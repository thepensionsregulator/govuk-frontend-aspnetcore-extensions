import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType, UmbBlockValueDataPropertiesBaseType } from '@umbraco-cms/backoffice/block';
import { renderSummaryList } from '../helpers/summary-list-helper';

interface IGovUkSummaryListContent extends UmbBlockDataType {
    items: UmbBlockValueDataPropertiesBaseType | null;
}

interface IGovUkSummaryListSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-summary-list')
export class GovUkSummaryListView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkSummaryListContent;

    @property({ attribute: false })
    settings?: IGovUkSummaryListSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            ${ this.content?.items?.contentData ? 
                renderSummaryList(this.content?.items?.contentData || [], this.settings?.cssClasses) :
                html`<p class="backoffice-additional-blocks">Summary list with no list items.</p>`
            }
        </a>
        `;
    }
}

export default GovUkSummaryListView;