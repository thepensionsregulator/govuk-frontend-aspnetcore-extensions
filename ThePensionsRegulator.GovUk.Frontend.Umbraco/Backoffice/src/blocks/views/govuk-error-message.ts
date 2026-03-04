import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { PACKAGE_VERSION } from '../../package-version.generated';
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

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <p class="govuk-error-message ${ this.settings?.cssClasses}">${this.content?.error }</p>
        </a>
        `;
    }
}

export default GovUkErrorMessageView;