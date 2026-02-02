import { html, customElement, LitElement, property, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';
import { disableLinks } from '../helpers/html-helper';
import { PACKAGE_VERSION } from '../../package-version.generated';

interface IGovUkWarningTextContent extends UmbBlockDataType {
    iconFallbackText: string;
    text: UmbPropertyEditorRteValueType;
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

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="govuk-warning-text backoffice-block-view ${ this.settings?.cssClasses}">
            <span aria-hidden="true" class="govuk-warning-text__icon">!</span>
            <strong class="govuk-warning-text__text" aria-hidden="true">
                <span class="govuk-visually-hidden">${ this.content?.iconFallbackText || "Warning" }</span>
                ${ unsafeHTML(disableLinks(this.content?.text.markup)) }
            </strong>
        </a>
        `;
    }
}

export default GovUkWarningTextView;