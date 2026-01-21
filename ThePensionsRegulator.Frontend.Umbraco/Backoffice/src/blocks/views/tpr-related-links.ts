import { html, customElement, LitElement, property, repeat } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { ILinkPickerModel } from '../types/ILinkPickerModel';

interface ITprRelatedLinksContent extends UmbBlockDataType {
    heading: string;
    links: Array<ILinkPickerModel>;
}

interface ITprRelatedLinksSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('tpr-related-links')
export class TprRelatedLinksView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: ITprRelatedLinksContent;

    @property({ attribute: false })
    settings?: ITprRelatedLinksSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <div class="tpr-related-links ${ this.settings?.cssClasses}">
                <h2 class="govuk-heading-m">${ this.content?.heading ? this.content.heading : 'Related' }</h2>
                <ul class="govuk-list">
                    ${repeat(this.content?.links || [],
                        (item) => item.name,
                        (item) => {
                            return html`
                            <li>
                                <span class="govuk-link">${item.name}</span>
                            </li>
                    `})}
                </ul>
            </div>
        </a>`;
    }
}

export default TprRelatedLinksView;