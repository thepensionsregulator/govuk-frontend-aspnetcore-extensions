import { html, customElement, LitElement, property, repeat } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType, UmbBlockValueType } from '@umbraco-cms/backoffice/block';
import { UmbBlockListLayoutModel, UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS } from '@umbraco-cms/backoffice/block-list';
import { renderAccordionSection } from '../helpers/accordion-helper';

interface IGovUkAccordionContent extends UmbBlockDataType {
    sections: UmbBlockValueType<UmbBlockListLayoutModel> | undefined;
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
                    ${repeat(this.content.sections.layout[UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS] || [],
                        (layout) => layout.contentKey,
                        (layout) => {
                            const section = this.content?.sections?.contentData.find(item => item.key === layout.contentKey)!;
                            const heading = (section?.values.find(props => props.alias == "heading")?.value as string);
                            const summary = (section?.values.find(props => props.alias == "summary")?.value as string);
                            const blocks = (section?.values.find(props => props.alias == "blocks")?.value as UmbBlockValueType<UmbBlockListLayoutModel>)?.contentData;
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