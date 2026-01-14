import { html, customElement, LitElement, property, state, unsafeHTML, repeat } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IRichTextProperty } from '../interfaces/IRichTextProperty';
import { IBlockListProperty } from "../interfaces/IBlockListProperty";
import { UMB_DOCUMENT_PROPERTY_DATASET_CONTEXT } from '@umbraco-cms/backoffice/document';
import { renderRadiosDivider, renderRadioButton } from '../helpers/radios-helper';

interface IGovUkRadiosContent extends UmbBlockDataType {
    fieldsetBlocks: IBlockListProperty;
    radioButtons: IBlockListProperty;
    legend: string;
    hint: IRichTextProperty;
}

interface IGovUkRadiosSettings extends UmbBlockDataType {
    cssClasses: string;
    legendIsPageHeading: boolean;
    layout: 'Vertical' | 'Horizontal';
}


@customElement('govuk-radios')
export class GovUkRadiosView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkRadiosContent;

    @property({ attribute: false })
    settings?: IGovUkRadiosSettings;

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
        if ((this.content?.fieldsetBlocks?.contentData?.length || 0) > 1) { blocksText = `${this.content?.fieldsetBlocks?.contentData.length} blocks.` }

        const legend = (this.content?.legend || "").replace("{{name}}", this._nodeName || "");
        const legendClass = this.settings?.legendIsPageHeading ? 'govuk-fieldset__legend--l' : 'govuk-fieldset__legend--for-field';
        const horizontalLayout = this.settings?.layout === 'Horizontal';

        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="govuk-form-group backoffice-block-view">
            <fieldset class="govuk-fieldset govuk-radios__fieldset ${ this.settings?.cssClasses}">
                <legend class="govuk-fieldset__legend ${legendClass}">${this.settings?.legendIsPageHeading ? html`<h1 class="govuk-fieldset__heading">${legend}</h1>` : legend }</legend>
                <p class="backoffice-additional-blocks">${blocksText}</p>
                <div class="govuk-form-group">
                    ${this.content?.hint?.markup ? html`<div class="govuk-hint">${unsafeHTML(this.content?.hint?.markup)}</div>` : null}
                    <div class="govuk-radios ${horizontalLayout ? "govuk-radios--inline" : null}">
                        ${ repeat(this.content?.radioButtons?.contentData || [],
                            (radio) => radio.key,
                            (radio) => {
                                const govukRadiosDivider = "5bbc1a49-49b7-4119-b6ad-c113226d92e0";
                                if (radio.contentTypeKey === govukRadiosDivider) {
                                    const text = radio?.values.find(props => props.alias == "text")?.value;
                                    return renderRadiosDivider(text);
                                } else {
                                    const label = radio?.values.find(props => props.alias == "label")?.value;
                                    const value = radio?.values.find(props => props.alias == "value")?.value;
                                    const hint = radio?.values.find(props => props.alias == "hint")?.value;
                                    const conditionalBlocks = radio?.values.find(props => props.alias == "conditionalBlocks")?.value;
                                    return renderRadioButton(label, value, hint?.markup, conditionalBlocks?.contentData, !horizontalLayout)
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

export default GovUkRadiosView;