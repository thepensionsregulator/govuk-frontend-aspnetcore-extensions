import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { renderSummaryList } from '../helpers/summary-list-helper';
import { IBlockListProperty } from '../interfaces/IBlockListProperty';

interface IGovUkSummaryListContent extends UmbBlockDataType {
    items: IBlockListProperty | null;
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

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="backoffice-block-view">
            ${ this.content?.items?.contentData ? 
                renderSummaryList(this.content?.items?.contentData || [], this.settings?.cssClasses) :
                html`<p class="backoffice-additional-blocks">Summary list with no list items.</p>`
            }
        </div>
        `;
    }
}

export default GovUkSummaryListView;