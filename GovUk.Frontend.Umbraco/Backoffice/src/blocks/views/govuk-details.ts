import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { unsafeHTML } from 'lit/directives/unsafe-html.js';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IRichTextProperty } from './IRichTextProperty';

interface IGovUkDetailsContent extends UmbBlockDataType {
    summary: string;
    text: IRichTextProperty;
}

interface IGovUkDetailsSettings extends UmbBlockDataType {
    cssClasses: boolean;
}


@customElement('govuk-details')
export class GovUkDetailsView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkDetailsContent;

    @property({ attribute: false })
    settings?: IGovUkDetailsSettings;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <details class="govuk-details backoffice-block-view ${ this.settings?.cssClasses}" aria-label="Edit details component">
            <summary class="govuk-details__summary"><span class="govuk-details__summary-text" aria-hidden="true">${ this.content?.summary }</span></summary>
            ${ unsafeHTML(this.content?.text.markup) }
        </details>
        `;
    }
}

export default GovUkDetailsView;