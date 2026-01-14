import { html, customElement, LitElement, property, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IRichTextProperty } from '../interfaces/IRichTextProperty';

interface IGovUkPanelContent extends UmbBlockDataType {
    panelHeading: string;
    panelText: IRichTextProperty;
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

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="govuk-panel--confirmation govuk-panel backoffice-block-view ${ this.settings?.cssClasses}">
            ${this?.content?.panelHeading ? html`<h1 class="govuk-panel__title">${ this.content?.panelHeading }</h1>` : null }
            ${this.content?.panelText.markup ? html`<div class="govuk-panel__body">${ unsafeHTML(this.content?.panelText.markup) }</div>` : null }
        </div>
        <div class="govuk-caption-l backoffice-block-view">${ this.content?.caption }</div>
        `;
    }
}

export default GovUkPanelView;