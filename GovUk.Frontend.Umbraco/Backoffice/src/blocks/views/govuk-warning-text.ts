import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { unsafeHTML } from 'lit/directives/unsafe-html.js';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IRichTextProperty } from './IRichTextProperty';

interface IGovUkWarningTextContent extends UmbBlockDataType {
    iconFallbackText: string;
    text: IRichTextProperty;
}

interface IGovUkWarningTextSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-warning-text')
export class GovUkWarningTextView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkWarningTextContent;

    @property({ attribute: false })
    settings?: IGovUkWarningTextSettings;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="govuk-warning-text backoffice-block-view ${ this.settings?.cssClasses}">
            <span aria-hidden="true" class="govuk-warning-text__icon">!</span>
            <strong class="govuk-warning-text__text" aria-hidden="true">
                <span class="govuk-visually-hidden">${ this.content?.iconFallbackText || "Warning" }</span>
                ${ unsafeHTML(this.content?.text.markup) }
            </strong>
        </div>
        `;
    }
}

export default GovUkWarningTextView;