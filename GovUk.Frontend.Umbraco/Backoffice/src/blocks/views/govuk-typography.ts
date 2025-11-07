import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { unsafeHTML } from 'lit/directives/unsafe-html.js';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IRichTextProperty } from './IRichTextProperty';

interface IGovUkTypographyContent extends UmbBlockDataType {
    text: IRichTextProperty;
}

interface IGovUkTypographySettings extends UmbBlockDataType {
}


@customElement('govuk-typography')
export class GovUkTypographyView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkTypographyContent;

    @property({ attribute: false })
    settings?: IGovUkTypographySettings;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="govuk-body backoffice-block-view" aria-label="Edit text component">${unsafeHTML(this.content?.text.markup) }</div>
        `;
    }
}

export default GovUkTypographyView;