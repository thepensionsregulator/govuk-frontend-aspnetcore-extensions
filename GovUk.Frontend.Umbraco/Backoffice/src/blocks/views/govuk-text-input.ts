import { html, customElement, LitElement, property, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IRichTextProperty } from '../interfaces/IRichTextProperty';
interface IGovUkTextInputContent extends UmbBlockDataType {
    label: string;
    hint: IRichTextProperty;
}

interface IGovUkTextInputSettings extends UmbBlockDataType {
    cssClasses: string;
    labelIsPageHeading: boolean;
    prefix: string;
    suffix: string;
    textInputWidth: string;
    readOnly: boolean;
}


@customElement('govuk-text-input')
export class GovUkTextInputView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkTextInputContent;

    @property({ attribute: false })
    settings?: IGovUkTextInputSettings;

    constructor() {
        super();
    }

    override render() {

        let inputClass = "";
        switch (String(this.settings?.textInputWidth)) {
            case "xx-small":
                inputClass = "govuk-input--width-2";
                break;
            case "x-small":
                inputClass = "govuk-input--width-3";
                break;
            case "small":
                inputClass = "govuk-input--width-4";
                break;
            case "medium":
                inputClass = "govuk-input--width-5";
                break;
            case "large":
                inputClass = "govuk-input--width-10";
                break;
            case "x-large":
                inputClass = "govuk-input--width-20";
                break;
            case "xx-large":
                inputClass = "govuk-input--width-30";
                break;
        }

        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="govuk-form-group backoffice-block-view ${this.settings?.cssClasses}">
            ${this.settings?.labelIsPageHeading ?
                html`<h1 class="govuk-label-wrapper">
                    <label class="govuk-label govuk-label--l">${this.content?.label}</label>
                </h1>` :
                html`<label class="govuk-label">${this.content?.label}</label>`}
            ${this.content?.hint?.markup ? html`<div class="govuk-hint">${unsafeHTML(this.content?.hint?.markup)}</div>` : null}
            <div class="govuk-input__wrapper">
                ${this.settings?.prefix ? html`<div class="govuk-input__prefix" aria-hidden="true">${this.settings?.prefix}</div>` : null }
                ${ this.settings?.readOnly ? html`<input class="govuk-input ${inputClass}" type="text" id="${crypto.randomUUID()}" readonly />` :
                html`<input class="govuk-input ${inputClass}" type="text" id="${crypto.randomUUID()}" />` }
                ${this.settings?.suffix ? html`<div class="govuk-input__suffix" aria-hidden="true">${this.settings?.suffix}</div>` : null }
            </div>
        </div>`
    }
}

export default GovUkTextInputView;