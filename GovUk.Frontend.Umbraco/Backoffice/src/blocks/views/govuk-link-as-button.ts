import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
interface IGovUkLinkAsButtonContent extends UmbBlockDataType {
    text: string;
}

interface IGovUkLinkAsButtonSettings extends UmbBlockDataType {
    cssClasses: string;
    isStartButton: boolean;
    styleOfButton: 'Secondary' | 'Warning' | 'Reversed';
}


@customElement('govuk-link-as-button')
export class GovUkLinkAsButtonView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkLinkAsButtonContent;

    @property({ attribute: false })
    settings?: IGovUkLinkAsButtonSettings;

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
            blockViewClass = "backoffice-block-view-inverse";
        }

        if (this.settings?.isStartButton) {
            return html`
            <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
            <div class="backoffice-block-view" aria-label="Edit link styled as button component">
                <div class="${ blockViewClass }">
                    <a href="javascript:return false" role="button" draggable="false" class="govuk-button govuk-button--start ${buttonClass} ${this.settings?.cssClasses}">
                      ${this.content?.text }
                      <svg class="govuk-button__start-icon" xmlns="http://www.w3.org/2000/svg" width="17.5" height="19" viewBox="0 0 33 40" aria-hidden="true" focusable="false">
                        <path fill="currentColor" d="M0 0h13l20 20-20 20H0l20-20z" />
                      </svg>
                    </a>
                </div>
            </div>`;
        }
        else {
            return html`
            <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
            <div class="backoffice-block-view" aria-label="Edit link styled as button component">
                <div class="${ blockViewClass }">
                    <a class="govuk-button ${buttonClass} ${this.settings?.cssClasses}" role="button" aria-hidden="true">${this.content?.text}</a>
                </div>
            </div>`;
        }
    }
}

export default GovUkLinkAsButtonView;