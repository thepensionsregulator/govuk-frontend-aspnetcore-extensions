import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType, UmbBlockValueType } from '@umbraco-cms/backoffice/block';
import { UmbBlockListLayoutModel } from '@umbraco-cms/backoffice/block-list';

interface ITprSectionCardsContent extends UmbBlockDataType {
    cards: UmbBlockValueType<UmbBlockListLayoutModel>;
}

interface ITprSectionCardsSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('tpr-section-cards')
export class TprSectionCardsView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: ITprSectionCardsContent;

    @property({ attribute: false })
    settings?: ITprSectionCardsSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        let blocksText = "No blocks.";
        if (this.content?.cards?.contentData?.length === 1) { blocksText = "1 block." }
        if ((this.content?.cards?.contentData?.length || 0) > 1) { blocksText = `${this.content?.cards.contentData.length} blocks.` }

        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <div class="${this.settings?.cssClasses}">
                <h2 class="govuk-heading-s">Section cards</h2>
                <p class="backoffice-additional-blocks">${ blocksText }</p>
            </div>
        </a>
        `;
    }
}

export default TprSectionCardsView;