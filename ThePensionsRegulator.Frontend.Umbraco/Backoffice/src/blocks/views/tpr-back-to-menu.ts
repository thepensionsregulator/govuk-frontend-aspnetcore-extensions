import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { PACKAGE_VERSION } from '../../package-version.generated';
interface ITprBackToMenuContent extends UmbBlockDataType {
    text: string;
}

interface ITprBackToMenuSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('tpr-back-to-menu')
export class TprBackToMenuView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: ITprBackToMenuContent;

    @property({ attribute: false })
    settings?: ITprBackToMenuSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <p class="tpr-back-to-menu govuk-body ${ this.settings?.cssClasses}">
                <span class="govuk-link">${this.content?.text }</span>
            </p>
        </a>
        `;
    }
}

export default TprBackToMenuView;