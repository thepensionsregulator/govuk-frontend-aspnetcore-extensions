import { html, customElement, LitElement, property, state, unsafeHTML, repeat } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import { UmbBlockDataType, UmbBlockValueType } from '@umbraco-cms/backoffice/block';
import { UmbBlockListLayoutModel, UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS } from '@umbraco-cms/backoffice/block-list';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';
import { UMB_DOCUMENT_PROPERTY_DATASET_CONTEXT } from '@umbraco-cms/backoffice/document';
import { renderRadiosDivider, renderRadioButton } from '../helpers/radios-helper';
import { disableLinks } from '../helpers/html-helper';
import { PACKAGE_VERSION } from '../../package-version.generated';

interface IGovUkRadiosContent extends UmbBlockDataType {
    fieldsetBlocks: UmbBlockValueType<UmbBlockListLayoutModel>;
    radioButtons: UmbBlockValueType<UmbBlockListLayoutModel>;
    legend: string;
    hint: UmbPropertyEditorRteValueType;
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

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

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
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="govuk-form-group backoffice-block-view">
            <fieldset class="govuk-fieldset govuk-radios__fieldset ${ this.settings?.cssClasses}">
                <legend class="govuk-fieldset__legend ${legendClass}">${this.settings?.legendIsPageHeading ? html`<h1 class="govuk-fieldset__heading">${legend}</h1>` : legend }</legend>
                <p class="backoffice-additional-blocks">${blocksText}</p>
                <div class="govuk-form-group">
                    ${this.content?.hint?.markup ? html`<div class="govuk-hint">${unsafeHTML(disableLinks(this.content?.hint?.markup))}</div>` : null}
                    <div class="govuk-radios ${horizontalLayout ? "govuk-radios--inline" : null}">
                        ${ repeat(this.content?.radioButtons?.layout[UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS] || [],
                            (layout) => layout.contentKey,
                            (layout) => {
                                const radio = this.content?.radioButtons?.contentData.find(item => item.key === layout.contentKey)!;
                                const govukRadiosDivider = "5bbc1a49-49b7-4119-b6ad-c113226d92e0";
                                if (radio.contentTypeKey === govukRadiosDivider) {
                                    const text = (radio?.values.find(props => props.alias == "text")?.value as string);
                                    return renderRadiosDivider(text);
                                } else {
                                    const label = (radio?.values.find(props => props.alias == "label")?.value as string);
                                    const value = (radio?.values.find(props => props.alias == "value")?.value as string);
                                    const hint = (radio?.values.find(props => props.alias == "hint")?.value as UmbPropertyEditorRteValueType);
                                    const conditionalBlocks = (radio?.values.find(props => props.alias == "conditionalBlocks")?.value as UmbBlockValueType<UmbBlockListLayoutModel>);
                                    return renderRadioButton(label, value, hint?.markup, conditionalBlocks?.contentData, !horizontalLayout)
                                }
                            }
                        )}
                    </div>
                </div>
            </fieldset>
        </a>
        `;
    }
}

export default GovUkRadiosView;