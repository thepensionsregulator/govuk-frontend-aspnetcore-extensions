import { html, customElement, LitElement, property, repeat } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType, UmbBlockValueDataPropertiesBaseType, UmbBlockDataModel } from '@umbraco-cms/backoffice/block';
import { renderAccordionSection } from '../helpers/accordion-helper';

interface IGovUkAccordionContent extends UmbBlockDataType {
    sections: UmbBlockValueDataPropertiesBaseType | null;
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

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            ${ this.content?.sections?.contentData ? html`
                <div class="govuk-accordion">
                    ${repeat(this.content.sections.contentData,
                        (section: UmbBlockDataModel) => section.key,
                        (section: UmbBlockDataModel) => {
                            const heading = (section?.values.find(props => props.alias == "heading")?.value as string);
                            const summary = (section?.values.find(props => props.alias == "summary")?.value as string);
                            const blocks = (section?.values.find(props => props.alias == "blocks")?.value as UmbBlockValueDataPropertiesBaseType)?.contentData;
                            return renderAccordionSection(heading, summary, blocks);
                        }
                    )}
                </div>` : 
                html`<p class="backoffice-additional-blocks">Accordion with no sections.</p>`
            }
        </a>`;
    }
}

export default GovUkAccordionView;