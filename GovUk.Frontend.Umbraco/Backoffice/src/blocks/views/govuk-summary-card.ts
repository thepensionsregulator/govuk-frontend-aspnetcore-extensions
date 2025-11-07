import { html, customElement, LitElement, property, repeat } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IBlockListProperty } from '../interfaces/IBlockListProperty';
import { renderSummaryList } from '../helpers/summary-list-helper';

interface IGovUkSummaryCardContent extends UmbBlockDataType {
    cardTitle: string;
    cardActions: IBlockListProperty | null;
    summaryListItems: IBlockListProperty | null;
}

interface IGovUkSummaryCardSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-summary-card')
export class GovUkSummaryCardView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkSummaryCardContent;

    @property({ attribute: false })
    settings?: IGovUkSummaryCardSettings;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="backoffice-block-view">
            <div class="govuk-summary-card ${ this.settings?.cssClasses}">
                <div class="govuk-summary-card__title-wrapper"><h2 class="govuk-summary-card__title">${this.content?.cardTitle}</h2>
                    ${this.content?.cardActions?.contentData ? html`<div class="govuk-summary-card__actions">
                        ${repeat(this.content?.cardActions?.contentData || [],
                            (action) => action.key,
                            (action) => html`<div class="govuk-summary-card__action">
                                    <a class="govuk-link" href="javascript:return false">${action.values.find(props => props.alias == "text")?.value}</a>
                                </div>`)}
                            </div>` : null }                
                </div>
                ${ this.content?.summaryListItems?.contentData ? 
                    html`<div class="govuk-summary-card__content">${ renderSummaryList(this.content?.summaryListItems?.contentData || []) }</div>` :
                    html`<p class="backoffice-additional-blocks">Summary card with no list items.</p>` }
            </div>
        </div>
        `;
    }
}

export default GovUkSummaryCardView;