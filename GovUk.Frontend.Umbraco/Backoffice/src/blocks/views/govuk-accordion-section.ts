import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import { UmbBlockDataType, UmbBlockValueType } from '@umbraco-cms/backoffice/block';
import { UmbBlockListLayoutModel } from '@umbraco-cms/backoffice/block-list';
import { renderAccordionSection } from '../helpers/accordion-helper';

interface IGovUkAccordionSectionContent extends UmbBlockDataType {
    heading: string;
    summary: string;
    blocks: UmbBlockValueType<UmbBlockListLayoutModel> | null;
}

interface IGovUkAccordionSectionSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-accordion-section')
export class GovUkAccordionSectionView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkAccordionSectionContent;

    @property({ attribute: false })
    settings?: IGovUkAccordionSectionSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            ${renderAccordionSection(this.content?.heading, this.content?.summary, this?.content?.blocks?.contentData) }
        </a>`;
    }
}

export default GovUkAccordionSectionView;