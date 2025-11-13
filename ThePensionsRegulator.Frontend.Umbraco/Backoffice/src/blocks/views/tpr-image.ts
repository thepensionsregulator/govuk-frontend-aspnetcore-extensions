import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { UmbMediaItemRepository, UmbMediaUrlRepository, UmbMediaItemModel, UmbMediaUrlModel, UmbMediaPickerPropertyValueEntry } from '@umbraco-cms/backoffice/media';

interface ITprImageContent extends UmbBlockDataType {
    image: Array<UmbMediaPickerPropertyValueEntry>;
}

interface ITprImageSettings extends UmbBlockDataType {
    cssClasses: string;
    imageSize: string;
    spaceAfter: boolean;
    decorativeImage: boolean;
}

@customElement('tpr-image')
export class TprImageView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: ITprImageContent;

    @property({ attribute: false })
    settings?: ITprImageSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    #mediaItemRepository: UmbMediaItemRepository;
    #mediaUrlRepository: UmbMediaUrlRepository;
    #image: UmbMediaItemModel | undefined;
    #imageUrl: UmbMediaUrlModel | undefined;

    constructor() {
        super();

        this.#mediaItemRepository = new UmbMediaItemRepository(this);
        this.#mediaUrlRepository = new UmbMediaUrlRepository(this);
    }

    override async updated(changedProperties: Map<string, any>) {
        // Called after render(). Use this lifecycle hook to load media node asynchronously.
        if (changedProperties.has('content')) {
            if (this.content?.image?.[0].mediaKey) {
                const itemResult = await this.#mediaItemRepository.requestItems([this.content?.image?.[0].mediaKey]);
                if (itemResult.data && itemResult.data.length > 0) {
                    this.#image = itemResult.data[0];
                }
                
                const urlResult = await this.#mediaUrlRepository.requestItems([this.content?.image?.[0].mediaKey]);
                if (urlResult.data && urlResult.data.length > 0) {
                    this.#imageUrl = urlResult.data[0];
                }

                this.requestUpdate(); // Force re-render
            }
        }
    }



    override render() {
        let cssClasses = `tpr-image ${this.settings?.cssClasses}`;
        if (this.settings?.imageSize !== "Retain original size") {
            cssClasses += ' tpr-image--fit-container';
        }
        if (!this.settings?.spaceAfter) {
            cssClasses += ' tpr-image--no-space-after'; 
        }

        let altText = this.#image?.name;
        if (this.settings?.decorativeImage) {
            altText = '';
        }

        const imageExtensions = ['jpg', 'png', 'webp', 'gif', 'jpeg', 'svg'];
        const displayImage = imageExtensions.find(ext => ext === this.#imageUrl?.extension?.toLowerCase()) && !this.#image?.isTrashed;

        return html`
            <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
            <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
                ${displayImage ? html`<img src="${this.#imageUrl?.url}" alt="${altText}" class="${cssClasses}" />` : null}
                <p class="govuk-body backoffice-image-label ${this.#image?.isTrashed ? 'is-trashed' : null}">
                    <uui-icon-registry-essential><uui-icon name="${this.#image?.mediaType.icon}" /></uui-icon-registry-essential>
                    ${this.#image?.isTrashed ? html`<umb-localize key="mediaPicker_trashed">Trashed</umb-localize>:` : null}
                    ${this.#image?.name}
                </p>
            </a>`;
    }
}

export default TprImageView;
