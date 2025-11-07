import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
interface IGovUkErrorMessageContent extends UmbBlockDataType {
    error: string;
}

interface IGovUkErrorMessageSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-error-message')
export class GovUkErrorMessageView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkErrorMessageContent;

    @property({ attribute: false })
    settings?: IGovUkErrorMessageSettings;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <p class="govuk-error-message backoffice-block-view ${ this.settings?.cssClasses }" aria-label="Edit error message component">${ this.content?.error }</p>
        `;
    }
}

export default GovUkErrorMessageView;