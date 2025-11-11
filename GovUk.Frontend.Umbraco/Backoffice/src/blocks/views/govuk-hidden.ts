import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';

interface IGovUkHiddenSettings extends UmbBlockDataType {
    modelProperty: string;
}


@customElement('govuk-hidden')
export class GovUkHiddenView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    settings?: IGovUkHiddenSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <p class="backoffice-additional-blocks">Hidden field ${this.settings?.modelProperty ? '(bound to ' + this.settings.modelProperty + ')' : '(not bound to a property)'}</p>
        </a>
        `;
    }
}

export default GovUkHiddenView;