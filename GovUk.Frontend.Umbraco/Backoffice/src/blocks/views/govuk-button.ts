import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
interface IGovUkButtonContent extends UmbBlockDataType {
    text: string;
}

interface IGovUkButtonSettings extends UmbBlockDataType {
    cssClasses: string;
    styleOfButton: 'Secondary' | 'Warning' | 'Reversed';
}


@customElement('govuk-button')
export class GovUkButtonView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkButtonContent;

    @property({ attribute: false })
    settings?: IGovUkButtonSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        let buttonClass = "";
        let blockViewClass = "";
        if (this.settings?.styleOfButton.indexOf("Secondary") !== -1) { buttonClass += " govuk-button--secondary"; }
        if (this.settings?.styleOfButton.indexOf("Warning") !== -1) { buttonClass += " govuk-button--warning"; }
        if (this.settings?.styleOfButton.indexOf("Reversed") !== -1) {
            buttonClass += " govuk-button--inverse";
            blockViewClass = " backoffice-block-view-inverse";
        }

        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <div class="${ blockViewClass }">
                <button class="govuk-button ${ buttonClass} ${this.settings?.cssClasses}" type="button">${this.content?.text }</button>
            </div>
        </a>
        `;
    }
}

export default GovUkButtonView;