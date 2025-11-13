import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType, UmbBlockValueType } from '@umbraco-cms/backoffice/block';
import { UmbBlockListLayoutModel } from '@umbraco-cms/backoffice/block-list';
import { renderSummaryListItem } from '../helpers/summary-list-helper';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';

interface IGovUkSummaryListItemContent extends UmbBlockDataType {
    itemKey: string;
    itemValue: UmbPropertyEditorRteValueType;
    actions: UmbBlockValueType<UmbBlockListLayoutModel> | undefined;
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

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <dl class="govuk-summary-list">
                ${ renderSummaryListItem(this.content?.itemKey, this.content?.itemValue?.markup, this.content?.actions) }
            </dl>
        </a>
        `;
    }
}

export default GovUkSummaryListItemView;