import { html, customElement, LitElement, property, repeat } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { ILinkPickerModel } from '../types/ILinkPickerModel';
interface ITprSearchResultsContent extends UmbBlockDataType {
    heading: string;
    footerLinks: Array<ILinkPickerModel>;
}

interface ITprSearchResultsSettings extends UmbBlockDataType {
    cssClasses: string;
    headingClass: string;
    headingLevel: string;
}


@customElement('tpr-search-results')
export class TprSearchResultsView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: ITprSearchResultsContent;

    @property({ attribute: false })
    settings?: ITprSearchResultsSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    #renderHeading(heading: string | undefined, headingClass: string | undefined, headingLevel: string | undefined) {
        headingClass = `${headingClass} tpr-search-results__heading`;
        heading = heading || 'Search Q&As';

        switch (headingLevel) {
            case 'Heading 3':
                return html`<h3 class="${headingClass}">${heading}</h3>`;
            case 'Heading 4':
                return html`<h4 class="${headingClass}">${heading}</h4>`;
            case 'Heading 5':
                return html`<h5 class="${headingClass}">${heading}</h5>`;
            case 'Heading 6':
                return html`<h6 class="${headingClass}">${heading}</h6>`;
            default:
                return html`<h2 class="${headingClass}">${heading}</h2>`;
        }
    }

    override render() {
        const inputId = crypto.randomUUID();
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view js-enabled">
            <aside class="tpr-search-results ${this.settings?.cssClasses}">
                ${this.#renderHeading(this.content?.heading, this.settings?.headingClass, this.settings?.headingLevel?.[0])}
                <form class="tpr-search-results__form">
                    <div class="govuk-form-group">
                        <label class="govuk-label govuk-visually-hidden" for="${inputId}">{{block.data.heading}}</label>
                        <div class="tpr-search-results__input-group">
                            <input class="govuk-input" type="text" id="${inputId}">
                            <div class="govuk-button-group">
                                <button class="govuk-button" type="submit">Ask</button>
                                <button class="govuk-button govuk-button--secondary" type="button">Clear</button>
                            </div>
                        </div>
                    </div>
                </form>
            </aside>
            <footer class="tpr-search-results__footer">
                <ul class="govuk-list tpr-search-results__links">
                    <li>
                        <span class="govuk-link">Show more questions</span>
                    </li>

                    ${repeat(this.content?.footerLinks || [],
                        (link) => link.url,
                        (link) => html`<li>
                            <span class="govuk-link">${link.name || link.url}</span>
                        </li>`)}
                </ul>
            </footer>
        </a>`;
    }
}

export default TprSearchResultsView;

