import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { ILinkPickerModel } from '../types/ILinkPickerModel';
interface ITprYouTubeVideoContent extends UmbBlockDataType {
    title: string;
    url: string;
    transcriptUrl: Array<ILinkPickerModel>;
}

interface ITprYouTubeVideoSettings extends UmbBlockDataType {
    cssClasses: string;
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

    override render() {
        const parseUrlResult = this.#tryParseVideoUrl(this.content?.url);

        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <div class="tpr-video-wrapper-no-cookies ${this.settings?.cssClasses}">
                <div class="tpr-video-wrapper-no-cookies__video-container">
                    <iframe referrerpolicy="strict-origin-when-cross-origin" src="https://www.youtube-nocookie.com/embed/${parseUrlResult.videoId}" title="${this.content?.title}"></iframe>
                </div>
                ${this.content?.transcriptUrl?.length ? html`<span class="tpr-video-wrapper-no-cookies__transcript-link">View transcript for '${this.content?.title}'</span>` : null}
             </div>
        </a>`;
    }
}

export default TprYouTubeVideoView;