import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
interface IGovUkLinkContent extends UmbBlockDataType {
    text: string;
}

interface IGovUkLinkSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-link')
export class GovUkLinkView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkLinkContent;

    @property({ attribute: false })
    settings?: IGovUkLinkSettings;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="backoffice-block-view" aria-label="Edit link component">
            <a href="javascript:return false" class="govuk-link ${this.settings?.cssClasses}">${this.content?.text}</a>
        </div>`;
    }
}

export default GovUkLinkView;