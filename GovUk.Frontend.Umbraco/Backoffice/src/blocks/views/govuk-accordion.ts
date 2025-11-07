import { html, customElement, LitElement, property, repeat } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IBlockListProperty } from '../interfaces/IBlockListProperty';
import { IBlockListItem } from '../interfaces/IBlockListItem';
import { renderAccordionSection } from '../helpers/accordion-helper';

interface IGovUkAccordionContent extends UmbBlockDataType {
    sections: IBlockListProperty | null;
}

interface IGovUkAccordionSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-accordion')
export class GovUkAccordionView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkAccordionContent;

    @property({ attribute: false })
    settings?: IGovUkAccordionSettings;

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="backoffice-block-view">
            ${ this.content?.sections?.contentData ? html`
                <div class="govuk-accordion">
                    ${repeat(this.content.sections.contentData,
                        (section: IBlockListItem) => section.key,
                        (section: IBlockListItem) => {
                            const heading = section?.values.find(props => props.alias == "heading")?.value;
                            const summary = section?.values.find(props => props.alias == "summary")?.value;
                            const blocks = section?.values.find(props => props.alias == "blocks")?.value?.contentData;
                            return renderAccordionSection(heading, summary, blocks);
                        }
                    )}
                </div>` : 
                html`<p class="backoffice-additional-blocks">Accordion with no sections.</p>`
            }
        </div>`;
    }
}

export default GovUkAccordionView;