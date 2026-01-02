import { html, customElement, LitElement, property, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';
import { disableLinks } from '../helpers/html-helper';
import { PACKAGE_VERSION } from '../../package-version.generated';

interface IGovUkTextareaContent extends UmbBlockDataType {
    label: string;
    hint: UmbPropertyEditorRteValueType;
}

interface IGovUkTextareaSettings extends UmbBlockDataType {
    cssClasses: string;
    labelIsPageHeading: boolean;
    rows: number | null;
    readOnly: boolean;
    showCharacterCount: boolean;
}


@customElement('govuk-textarea')
export class GovUkTextareaView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkTextareaContent;

    @property({ attribute: false })
    settings?: IGovUkTextareaSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        const rows = this.settings?.rows ?? 5;

        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="govuk-form-group backoffice-block-view ${this.settings?.cssClasses}">
            ${this.settings?.labelIsPageHeading ?
                html`<h1 class="govuk-label-wrapper">
                    <label class="govuk-label govuk-label--l">${this.content?.label}</label>
                </h1>` :
                html`<label class="govuk-label">${this.content?.label}</label>`}
            ${this.content?.hint?.markup ? html`<div class="govuk-hint">${unsafeHTML(disableLinks(this.content?.hint?.markup))}</div>` : null}
            ${ this.settings?.readOnly ? html`<textarea class="govuk-textarea" rows="${rows}" id="${crypto.randomUUID()}" readonly></textarea>` :
                html`<textarea class="govuk-textarea" rows="${rows}" id="${crypto.randomUUID()}"></textarea>`}
            ${this.settings?.showCharacterCount ? html`<div class="govuk-hint govuk-character-count__message govuk-character-count__status" aria-hidden="true">You have xxx characters remaining</div>` : null }
        </a>
        `
    }
}

export default GovUkTextareaView;