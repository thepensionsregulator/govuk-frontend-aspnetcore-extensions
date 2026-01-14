import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IBlockListProperty } from '../interfaces/IBlockListProperty';
import { renderAccordionSection } from '../helpers/accordion-helper';

interface IGovUkAccordionSectionContent extends UmbBlockDataType {
    heading: string;
    summary: string;
    blocks: IBlockListProperty | null;
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

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="backoffice-block-view">
            ${ renderAccordionSection(this.content?.heading, this.content?.summary, this?.content?.blocks?.contentData) }
        </div>`;
    }
}

export default GovUkAccordionSectionView;