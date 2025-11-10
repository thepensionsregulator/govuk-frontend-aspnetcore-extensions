import { html, customElement, LitElement, property, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IBlockListProperty } from '../interfaces/IBlockListProperty';
import { IRichTextProperty } from '../interfaces/IRichTextProperty';

interface IGovUkNotificationBannerContent extends UmbBlockDataType {
    heading: IRichTextProperty;
    text: IRichTextProperty;
    blocks: IBlockListProperty | null;
}

interface IGovUkNotificationBannerSettings extends UmbBlockDataType {
    cssClasses: string;
    type: 'Default' | 'Success';
    title: string;
}


@customElement('govuk-notification-banner')
export class GovUkNotificationBannerView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkNotificationBannerContent;

    @property({ attribute: false })
    settings?: IGovUkNotificationBannerSettings;

    constructor() {
        super();
    }

    override render() {
        let blocks = "No blocks.";    
        if (this.content?.blocks?.contentData?.length === 1) { blocks = "1 block." }
        if ((this.content?.blocks?.contentData?.length || 0) > 1) { blocks = `${this.content?.blocks?.contentData?.length} blocks.` }

        let bannerClass = 'govuk-notification-banner';
        if (this.settings?.type === 'Success') {
            bannerClass += " govuk-notification-banner--success"
        }

        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="backoffice-block-view">
            <div class="${bannerClass} ${ this.settings?.cssClasses }">
                <div class="govuk-notification-banner__header">
                    ${ this.settings?.title ? html`<h2 class="govuk-notification-banner__title" id="govuk-notification-banner-title">${this.settings?.title}</h2>` : null }
                    ${ (!(this.settings?.type === 'Success') && !(this.settings?.title)) ? html`<h2 class="govuk-notification-banner__title" id="govuk-notification-banner-title">Important</h2>` : null }
                    ${ this.settings?.type === 'Success' && !(this.settings?.title) ? html`<h2 class="govuk-notification-banner__title" id="govuk-notification-banner-title">Success</h2>` : null }
                </div>
                <div class="govuk-notification-banner__content" aria-hidden="true">
                    <h3 class="govuk-notification-banner__heading">${ html`${unsafeHTML(this.content?.heading.markup) }` }</h3>
                    ${ this.content?.text?.markup ? html`<p class="govuk-body">${unsafeHTML(this.content.text.markup) }</p>` : null }
                    <p class="backoffice-additional-blocks">${ blocks }</p>
                </div>
            </div>
        </div>
        `;
    }
}

export default GovUkNotificationBannerView;