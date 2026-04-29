import { html, customElement, LitElement, property, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';
import { ILinkPickerModel } from '../types/ILinkPickerModel';
import { disableLinks } from '../helpers/html-helper';
import { PACKAGE_VERSION } from '../../package-version.generated';
interface ITprYouTubeVideoContent extends UmbBlockDataType {
    title: string;
    description: UmbPropertyEditorRteValueType;
    url: string;
    transcriptUrl: Array<ILinkPickerModel>;
}

interface ITprYouTubeVideoSettings extends UmbBlockDataType {
    cssClasses: string;
    headingClass: string;
    headingLevel: string;
}


@customElement('tpr-youtube-video')
export class TprYouTubeVideoView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: ITprYouTubeVideoContent;

    @property({ attribute: false })
    settings?: ITprYouTubeVideoSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    #tryParseVideoUrl(urlToParse: string | null | undefined): { success: boolean; videoId: string | null } {
        if (!urlToParse || urlToParse.trim() === '') {
            return { success: false, videoId: null };
        }

        const regex = /(\/embed\/|\/watch\?v=|youtu\.be\/)(?<VideoId>[A-Z0-9_-]+)/i;
        const match = urlToParse.match(regex);

        if (!match) {
            return { success: false, videoId: null };
        }

        const videoId = match.groups?.['VideoId'] ?? null;
        return { success: !!videoId, videoId };
    }

    #renderHeading(title: string | undefined, headingClass: string | undefined, headingLevel: string | undefined) {
        if (!title) return null;

        const cssClass = `${headingClass ?? 'govuk-heading-m'} tpr-video-wrapper-no-cookies__heading`;

        switch (headingLevel) {
            case 'Heading 1':
                return html`<h1 class="${cssClass}">${title}</h1>`;
            case 'Heading 3':
                return html`<h3 class="${cssClass}">${title}</h3>`;
            case 'Heading 4':
                return html`<h4 class="${cssClass}">${title}</h4>`;
            case 'Heading 5':
                return html`<h5 class="${cssClass}">${title}</h5>`;
            case 'Heading 6':
                return html`<h6 class="${cssClass}">${title}</h6>`;
            default:
                return html`<h2 class="${cssClass}">${title}</h2>`;
        }
    }

    override render() {
        const parseUrlResult = this.#tryParseVideoUrl(this.content?.url);

        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <div class="tpr-video-wrapper-no-cookies ${this.settings?.cssClasses}">
                ${this.#renderHeading(this.content?.title, this.settings?.headingClass, this.settings?.headingLevel)}
                ${this.content?.description?.markup ? html`<div class="govuk-body tpr-video-wrapper-no-cookies__description">${unsafeHTML(disableLinks(this.content?.description?.markup))}</div>` : null}
                <div class="tpr-video-wrapper-no-cookies__video-container">
                    <iframe referrerpolicy="strict-origin-when-cross-origin" src="https://www.youtube-nocookie.com/embed/${parseUrlResult.videoId}" title="${this.content?.title}"></iframe>
                </div>
                ${this.content?.transcriptUrl?.length ? html`<div class="tpr-video-wrapper-no-cookies__transcript-container"><span class="tpr-video-wrapper-no-cookies__transcript-link">Read transcript: ${this.content?.title}</span></div>` : null}
             </div>
        </a>`;
    }
}

export default TprYouTubeVideoView;