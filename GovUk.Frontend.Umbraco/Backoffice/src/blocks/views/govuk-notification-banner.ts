import { html, customElement, LitElement, property, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import { UmbBlockDataType, UmbBlockValueType } from '@umbraco-cms/backoffice/block';
import { UmbBlockListLayoutModel } from '@umbraco-cms/backoffice/block-list';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';
import { disableLinks } from '../helpers/html-helper';
import { PACKAGE_VERSION } from '../../package-version.generated';

interface IGovUkNotificationBannerContent extends UmbBlockDataType {
    heading: UmbPropertyEditorRteValueType;
    text: UmbPropertyEditorRteValueType;
    blocks: UmbBlockValueType<UmbBlockListLayoutModel> | undefined;
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

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

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
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <div class="${bannerClass} ${ this.settings?.cssClasses }">
                <div class="govuk-notification-banner__header">
                    ${ this.settings?.title ? html`<h2 class="govuk-notification-banner__title" id="govuk-notification-banner-title">${this.settings?.title}</h2>` : null }
                    ${ (!(this.settings?.type === 'Success') && !(this.settings?.title)) ? html`<h2 class="govuk-notification-banner__title" id="govuk-notification-banner-title">Important</h2>` : null }
                    ${ this.settings?.type === 'Success' && !(this.settings?.title) ? html`<h2 class="govuk-notification-banner__title" id="govuk-notification-banner-title">Success</h2>` : null }
                </div>
                <div class="govuk-notification-banner__content" aria-hidden="true">
                    <h3 class="govuk-notification-banner__heading">${ html`${unsafeHTML(disableLinks(this.content?.heading?.markup)) }` }</h3>
                    ${ this.content?.text?.markup ? html`<p class="govuk-body">${unsafeHTML(disableLinks(this.content.text?.markup)) }</p>` : null }
                    <p class="backoffice-additional-blocks">${ blocks }</p>
                </div>
            </div>
        </a>
        `;
    }
}

export default GovUkNotificationBannerView;