import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
interface IGovUkCaptionContent extends UmbBlockDataType {
    caption: string;
}

interface IGovUkCaptionSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-caption')
export class GovUkCaptionView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkCaptionContent;

    @property({ attribute: false })
    settings?: IGovUkCaptionSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <div class="govuk-caption-l ${ this.settings?.cssClasses}">${this.content?.caption }</div>
        </a>
        `;
    }
}

export default GovUkCaptionView;