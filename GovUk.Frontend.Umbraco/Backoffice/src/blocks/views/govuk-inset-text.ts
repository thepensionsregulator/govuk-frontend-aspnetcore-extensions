import { html, customElement, LitElement, property, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';
import { disableLinks } from '../helpers/html-helper';
import { PACKAGE_VERSION } from '../../package-version.generated';

interface IGovUkInsetTextContent extends UmbBlockDataType {
    text: UmbPropertyEditorRteValueType;
}

interface IGovUkInsetTextSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-inset-text')
export class GovUkInsetTextView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkInsetTextContent;

    @property({ attribute: false })
    settings?: IGovUkInsetTextSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="govuk-inset-text backoffice-block-view ${ this.settings?.cssClasses}">${unsafeHTML(disableLinks(this.content?.text.markup)) }</a>
        `;
    }
}

export default GovUkInsetTextView;