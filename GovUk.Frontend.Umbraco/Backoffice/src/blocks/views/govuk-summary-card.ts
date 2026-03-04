import { html, customElement, LitElement, property, repeat } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType, UmbBlockValueType } from '@umbraco-cms/backoffice/block';
import { UmbBlockListLayoutModel, UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS } from '@umbraco-cms/backoffice/block-list';
import { renderSummaryList } from '../helpers/summary-list-helper';
import { PACKAGE_VERSION } from '../../package-version.generated';

interface IGovUkSummaryCardContent extends UmbBlockDataType {
    cardTitle: string;
    cardActions: UmbBlockValueType<UmbBlockListLayoutModel> | undefined;
    summaryListItems: UmbBlockValueType<UmbBlockListLayoutModel> | undefined;
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

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <div class="govuk-summary-card ${ this.settings?.cssClasses}">
                <div class="govuk-summary-card__title-wrapper"><h2 class="govuk-summary-card__title">${this.content?.cardTitle}</h2>
                    ${this.content?.cardActions?.contentData ? html`<div class="govuk-summary-card__actions">
                        ${repeat(this.content?.cardActions?.layout[UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS] || [],
                            (layout) => layout.contentKey,
                            (layout) => {
                                const action = this.content?.cardActions?.contentData.find(item => item.key === layout.contentKey)!;
                                return html`<div class="govuk-summary-card__action">
                                    <a class="govuk-link" href="javascript:return false">${action.values.find(props => props.alias == "text")?.value}</a>
                                </div>`}
                            )}
                            </div>` : null }                
                </div>
                ${ this.content?.summaryListItems?.contentData ? 
                    html`<div class="govuk-summary-card__content">${ renderSummaryList(this.content?.summaryListItems) }</div>` :
                    html`<p class="backoffice-additional-blocks">Summary card with no list items.</p>` }
            </div>
        </a>
        `;
    }
}

export default GovUkSummaryCardView;