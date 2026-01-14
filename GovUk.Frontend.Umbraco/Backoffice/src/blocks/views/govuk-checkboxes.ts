import { html, customElement, LitElement, property, unsafeHTML, state, repeat } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IRichTextProperty } from '../interfaces/IRichTextProperty';
import { IBlockListProperty } from "../interfaces/IBlockListProperty";
import { UMB_DOCUMENT_PROPERTY_DATASET_CONTEXT } from '@umbraco-cms/backoffice/document';
import { renderCheckboxesDivider, renderCheckbox } from '../helpers/checkboxes-helper';

interface IGovUkCheckboxesContent extends UmbBlockDataType {
    legend: string;
    hint: IRichTextProperty;
    fieldsetBlocks: IBlockListProperty;
    checkboxes: IBlockListProperty;
}

interface IGovUkCheckboxesSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-checkboxes')
export class GovUkCheckboxesView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkCheckboxesContent;

    @property({ attribute: false })
    settings?: IGovUkCheckboxesSettings;

    @state()
    _nodeName?: string;

    constructor() {
        super();

        this.consumeContext(UMB_DOCUMENT_PROPERTY_DATASET_CONTEXT, (context) => {
            if (!context) return;

            this.observe(context.name, (nodeName) => {
                if (nodeName) {
                    this._nodeName = nodeName;
                }
            });
        });
    }

    override render() {
        let blocksText = "No blocks.";
        if (this.content?.fieldsetBlocks?.contentData?.length === 1) { blocksText = "1 block." }
        if ((this.content?.fieldsetBlocks?.contentData?.length || 0) > 1) { blocksText = `${this.content?.fieldsetBlocks.contentData.length} blocks.` }

        const legend = (this.content?.legend || "").replace("{{name}}", this._nodeName || "");
        const legendClass = this.settings?.legendIsPageHeading ? 'govuk-fieldset__legend--l' : 'govuk-fieldset__legend--for-field';

        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="govuk-form-group backoffice-block-view">
            <fieldset class="govuk-fieldset govuk-checkboxes__fieldset ${ this.settings?.cssClasses}">
                <legend class="govuk-fieldset__legend ${legendClass}">${this.settings?.legendIsPageHeading ? html`<h1 class="govuk-fieldset__heading">${legend}</h1>` : legend }</legend>
                <div class="govuk-form-group">
                    <p class="backoffice-additional-blocks">${blocksText}</p>
                    ${this.content?.hint?.markup ? html`<div class="govuk-hint">${unsafeHTML(this.content?.hint?.markup)}</div>` : null}
                    <div class="govuk-checkboxes">
                        ${repeat(this.content?.checkboxes?.contentData || [],
                            (checkbox) => checkbox.key,
                            (checkbox) => {
                                const govukCheckboxesDivider = "ee99c671-93dc-4d08-9933-e61e50dc234a";
                                if (checkbox.contentTypeKey === govukCheckboxesDivider) {
                                    const text = checkbox?.values.find(props => props.alias == "text")?.value;
                                    return renderCheckboxesDivider(text);
                                } else {
                                    const label = checkbox?.values.find(props => props.alias == "label")?.value;
                                    const value = checkbox?.values.find(props => props.alias == "value")?.value;
                                    const hint = checkbox?.values.find(props => props.alias == "hint")?.value;
                                    const conditionalBlocks = checkbox?.values.find(props => props.alias == "conditionalBlocks")?.value;
                                    return renderCheckbox(label, value, hint?.markup, conditionalBlocks?.contentData)
                                }
                            }
                        )}
                    </div>
                </div>
            </fieldset>
        </div>
        `;
    }
}

export default GovUkCheckboxesView;