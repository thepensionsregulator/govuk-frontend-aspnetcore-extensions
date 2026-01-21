import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { UmbMediaItemRepository } from '@umbraco-cms/backoffice/media';
import { UMB_DOCUMENT_PROPERTY_DATASET_CONTEXT, UmbDocumentItemRepository } from '@umbraco-cms/backoffice/document';
import { ILinkPickerModel } from '../types/ILinkPickerModel';
import { createDocumentBlock, renderDocument, updateNodeName } from '../helpers/document-helper';
import { ITprDocumentBlock } from '../types/ITprDocumentBlock';

interface ITprDocumentContent extends UmbBlockDataType {
    key: string;
    document: Array<ILinkPickerModel> | undefined;
    datePublished: string | undefined;
    numberOfPages: number | undefined;
    description: string | undefined;
}

interface ITprDocumentSettings extends UmbBlockDataType {
    cssClasses: string;
}

@customElement('tpr-document')
export class TprDocumentView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: ITprDocumentContent;

    @property({ attribute: false })
    settings?: ITprDocumentSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    #mediaItemRepository: UmbMediaItemRepository;
    #documentItemRepository: UmbDocumentItemRepository;
    #document: ITprDocumentBlock | null | undefined;
    #currentCulture: string | null = null;

    constructor() {
        super();

        this.#mediaItemRepository = new UmbMediaItemRepository(this);
        this.#documentItemRepository = new UmbDocumentItemRepository(this);

        this.consumeContext(UMB_DOCUMENT_PROPERTY_DATASET_CONTEXT, (context) => {
            if (!context) return;
            this.observe(context.culture, (culture) => {
                if (culture) {
                    this.#currentCulture = culture;
                }
            })
        });
    }

    override willUpdate(changedProperties: Map<string, any>) {
        // Called before render(). Convert block data into ITprDocumentBlock objects.
        if (changedProperties.has('content')) {
            this.#document = createDocumentBlock(
                this.content?.key,
                this.content?.document?.[0],
                this.content?.datePublished,
                this.content?.numberOfPages,
                this.content?.description
            );
        }
    }

    override async updated(changedProperties: Map<string, any>) {
        // Called after render(). Use this lifecycle hook to load media/content node names asynchronously.
        if (changedProperties.has('content')) {
            this.#document!.docName = await updateNodeName(
                this.#mediaItemRepository,
                this.#documentItemRepository,
                this.#document?.docLink?.unique,
                this.#document?.docName,
                this.#document?.docLink?.type,
                this.#currentCulture
            );
            this.requestUpdate(); // Force re-render after loading names
        }
    }



    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            ${ renderDocument(this.#document,false) }
        </a>`;
    }
}

export default TprDocumentView;
