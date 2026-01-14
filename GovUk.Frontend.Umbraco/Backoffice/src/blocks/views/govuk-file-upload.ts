import { html, customElement, LitElement, property, state, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IRichTextProperty } from '../interfaces/IRichTextProperty';
import { UMB_DOCUMENT_PROPERTY_DATASET_CONTEXT } from '@umbraco-cms/backoffice/document';
interface IGovUkFileUploadContent extends UmbBlockDataType {
    label: string;
    hint: IRichTextProperty;
}

interface IGovUkFileUploadSettings extends UmbBlockDataType {
    cssClasses: string;
    labelIsPageHeading: boolean;
}


@customElement('govuk-file-upload')
export class GovUkFileUploadView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkFileUploadContent;

    @property({ attribute: false })
    settings?: IGovUkFileUploadSettings;

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
        const uniqueId = crypto.randomUUID();
        const label = (this.content?.label || "").replace("{{name}}", this._nodeName || "");


        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />

        <div class="govuk-form-group backoffice-block-view ${ this.settings?.cssClasses }">
            ${this.settings?.labelIsPageHeading ?
                html`<h1 class="govuk-label-wrapper">
                    <label class="govuk-label govuk-label--l" id="${uniqueId}-label">${label}</label>
                </h1>` :
                html`<label class="govuk-label" id="${uniqueId}-label">${label}</label>`}
            ${this.content?.hint?.markup ? html`<div class="govuk-hint" id="${uniqueId}-hint">${unsafeHTML(this.content?.hint?.markup)}</div>` : null}
            <div class="govuk-drop-zone">
                <button class="govuk-file-upload-button govuk-file-upload-button--empty" type="button" id="${uniqueId}" onclick="javascript:return false" aria-labelledby="${uniqueId}-label ${uniqueId}-comma ${uniqueId}-hint">
                    <span class="govuk-body govuk-file-upload-button__status">No file chosen</span>
                    <span class="govuk-visually-hidden" id="${uniqueId}-comma">, </span>
                    <span class="govuk-file-upload-button__pseudo-button-container">
                        <span class="govuk-button govuk-button--secondary govuk-file-upload-button__pseudo-button">Choose file</span> 
                        <span class="govuk-body govuk-file-upload-button__instruction">or drop file</span>
                    </span>
                </button>
            </div>
        </div>
        `;
    }
}

export default GovUkFileUploadView;