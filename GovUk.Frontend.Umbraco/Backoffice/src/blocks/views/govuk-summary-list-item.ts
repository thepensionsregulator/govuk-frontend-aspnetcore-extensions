import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { renderSummaryListItem } from '../helpers/summary-list-helper';
import { IBlockListProperty } from '../interfaces/IBlockListProperty';
import { IRichTextProperty } from '../interfaces/IRichTextProperty';

interface IGovUkSummaryListItemContent extends UmbBlockDataType {
    itemKey: string;
    itemValue: IRichTextProperty;
    actions: IBlockListProperty | null;
}

interface IGovUkSummaryListItemSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-summary-list-item')
export class GovUkSummaryListItemView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkSummaryListItemContent;

    @property({ attribute: false })
    settings?: IGovUkSummaryListItemSettings;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <dl class="backoffice-block-view govuk-summary-list">
            ${ renderSummaryListItem(this.content?.itemKey, this.content?.itemValue?.markup, this.content?.actions?.contentData) }
        </dl>
        `;
    }
}

export default GovUkSummaryListItemView;