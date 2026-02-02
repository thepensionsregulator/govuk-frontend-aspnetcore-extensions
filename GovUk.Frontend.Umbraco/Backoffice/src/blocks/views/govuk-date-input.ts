import { html, customElement, LitElement, property, unsafeHTML, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import { UmbBlockDataType, UmbBlockValueType } from '@umbraco-cms/backoffice/block';
import { UmbBlockListLayoutModel } from '@umbraco-cms/backoffice/block-list';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';
import { UMB_DOCUMENT_PROPERTY_DATASET_CONTEXT } from '@umbraco-cms/backoffice/document';
import { disableLinks } from '../helpers/html-helper';
import { PACKAGE_VERSION } from '../../package-version.generated';

interface IGovUkDateInputContent extends UmbBlockDataType {
    fieldsetBlocks: UmbBlockValueType<UmbBlockListLayoutModel>;
    legend: string;
    hint: UmbPropertyEditorRteValueType;
}

interface IGovUkDateInputSettings extends UmbBlockDataType {
    cssClasses: string;
    showDay: boolean;
    showYear: boolean;
    legendIsPageHeading: boolean;
    readOnly: boolean;
}


@customElement('govuk-date-input')
export class GovUkDateInputView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkDateInputContent;

    @property({ attribute: false })
    settings?: IGovUkDateInputSettings;

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

        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="govuk-form-group backoffice-block-view">
            <fieldset class="govuk-fieldset govuk-date-input__fieldset ${ this.settings?.cssClasses}">
                <legend class="govuk-fieldset__legend ${legendClass}">${this.settings?.legendIsPageHeading ? html`<h1 class="govuk-fieldset__heading">${legend}</h1>` : legend }</legend>
                <p class="backoffice-additional-blocks">${blocksText}</p>
                <div class="govuk-form-group">
                    ${this.content?.hint?.markup ? html`<div class="govuk-hint">${unsafeHTML(disableLinks(this.content?.hint?.markup))}</div>` : null}
                    <div class="govuk-date-input">
                        ${this.settings?.showDay !== false ? html`<div class="govuk-date-input__item">
                            <div class="govuk-form-group">
                                <label class="govuk-label govuk-date-input__label">Day</label>
                                ${ this.settings?.readOnly ?
                                    html`<input class="govuk-date-input__input govuk-input govuk-input--width-2" inputmode="numeric" type="text" id="${crypto.randomUUID()}" readonly />` :
                                    html`<input class="govuk-date-input__input govuk-input govuk-input--width-2" inputmode="numeric" type="text" id="${crypto.randomUUID()}" />` }
                            </div>
                        </div>` : null}
                        <div class="govuk-date-input__item">
                            <div class="govuk-form-group">
                                <label class="govuk-label govuk-date-input__label">Month</label>
                                ${ this.settings?.readOnly ?
                                    html`<input class="govuk-date-input__input govuk-input govuk-input--width-2" inputmode="numeric" type="text" id="${crypto.randomUUID()}" readonly />` :
                                    html`<input class="govuk-date-input__input govuk-input govuk-input--width-2" inputmode="numeric" type="text" id="${crypto.randomUUID()}" />` }
                            </div>
                        </div>
                        ${this.settings?.showYear !== false ? html`<div class="govuk-date-input__item">
                            <div class="govuk-form-group">
                                <label class="govuk-label govuk-date-input__label">Year</label>
                                ${ this.settings?.readOnly ?
                                    html`<input class="govuk-date-input__input govuk-input govuk-input--width-4" inputmode="numeric" type="text" id="${crypto.randomUUID()}" readonly />` :
                                    html`<input class="govuk-date-input__input govuk-input govuk-input--width-4" inputmode="numeric" type="text" id="${crypto.randomUUID()}" />` }
                            </div>
                        </div>` : null}
                    </div>
                </div>
            </fieldset>
        </a>
        `;
    }
}

export default GovUkDateInputView;