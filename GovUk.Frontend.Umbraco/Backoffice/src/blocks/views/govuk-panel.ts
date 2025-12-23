import { html, customElement, LitElement, property, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';
import { disableLinks } from '../helpers/html-helper';
import { PACKAGE_VERSION } from '../../package-version.generated';

interface IGovUkPanelContent extends UmbBlockDataType {
    panelHeading: string;
    panelText: UmbPropertyEditorRteValueType;
}

interface IGovUkPanelSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-panel')
export class GovUkPanelView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkPanelContent;

    @property({ attribute: false })
    settings?: IGovUkPanelSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="govuk-panel--confirmation govuk-panel backoffice-block-view ${ this.settings?.cssClasses}">
            ${this?.content?.panelHeading ? html`<h1 class="govuk-panel__title">${ this.content?.panelHeading }</h1>` : null }
            ${this.content?.panelText?.markup ? html`<div class="govuk-panel__body">${ unsafeHTML(disableLinks(this.content?.panelText?.markup)) }</div>` : null }
        </a>
        `;
    }
}

export default GovUkPanelView;