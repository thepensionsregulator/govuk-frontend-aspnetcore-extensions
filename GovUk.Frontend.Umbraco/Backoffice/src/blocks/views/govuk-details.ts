import { html, customElement, LitElement, property, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';
import { disableLinks } from '../helpers/html-helper';
import { PACKAGE_VERSION } from '../../package-version.generated';

interface IGovUkDetailsContent extends UmbBlockDataType {
    summary: string;
    text: UmbPropertyEditorRteValueType;
}

interface IGovUkDetailsSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-details')
export class GovUkDetailsView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkDetailsContent;

    @property({ attribute: false })
    settings?: IGovUkDetailsSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <details class="govuk-details ${ this.settings?.cssClasses}" open>
                <summary class="govuk-details__summary"><span class="govuk-details__summary-text">${ this.content?.summary }</span></summary>
                ${ unsafeHTML(disableLinks(this.content?.text.markup)) }
            </details>
        </a>
        `;
    }
}

export default GovUkDetailsView;