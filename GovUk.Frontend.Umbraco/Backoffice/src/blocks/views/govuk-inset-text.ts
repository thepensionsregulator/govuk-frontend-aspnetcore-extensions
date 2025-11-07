import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { unsafeHTML } from 'lit/directives/unsafe-html.js';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IRichTextProperty } from './IRichTextProperty';

interface IGovUkInsetTextContent extends UmbBlockDataType {
    text: IRichTextProperty;
}

interface IGovUkInsetTextSettings extends UmbBlockDataType {
    cssClasses: boolean;
}


@customElement('govuk-inset-text')
export class GovUkInsetTextView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkInsetTextContent;

    @property({ attribute: false })
    settings?: IGovUkInsetTextSettings;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="govuk-inset-text backoffice-block-view ${ this.settings?.cssClasses}" aria-label="Edit inset text component">${unsafeHTML(this.content?.text.markup) }</div>
        `;
    }
}

export default GovUkInsetTextView;