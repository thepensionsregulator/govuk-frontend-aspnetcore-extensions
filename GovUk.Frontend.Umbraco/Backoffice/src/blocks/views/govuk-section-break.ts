import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';

interface IGovUkSectionBreakSettings extends UmbBlockDataType {
    hidden: boolean;
    size: Array<string>;
    cssClasses: string;
}


@customElement('govuk-section-break')
export class GovUkSectionBreakView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    settings?: IGovUkSectionBreakSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <p class="govuk-body backoffice-section-break"><span>${this.settings?.hidden ? 'Hidden section break' : 'Section break'} (${this.settings?.size || 'Large'})</span></p>
        </a>`;
    }
}

export default GovUkSectionBreakView;