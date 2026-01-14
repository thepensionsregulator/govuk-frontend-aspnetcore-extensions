import { html, customElement, LitElement, property, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IRichTextProperty } from '../interfaces/IRichTextProperty';

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
        <div class="govuk-body backoffice-block-view">${unsafeHTML(this.content?.text.markup) }</div>
        `;
    }
}

export default GovUkTypographyView;