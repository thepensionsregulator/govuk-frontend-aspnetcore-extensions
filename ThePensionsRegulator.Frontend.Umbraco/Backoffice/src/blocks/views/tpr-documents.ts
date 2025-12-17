import { html, customElement, LitElement, property, repeat } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import { UmbBlockDataType, UmbBlockValueType } from '@umbraco-cms/backoffice/block';
import { UmbBlockListLayoutModel, UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS } from '@umbraco-cms/backoffice/block-list';
import { UmbMediaItemRepository } from '@umbraco-cms/backoffice/media';
import { UMB_DOCUMENT_PROPERTY_DATASET_CONTEXT, UmbDocumentItemRepository } from '@umbraco-cms/backoffice/document';
import { ILinkPickerModel } from '../types/ILinkPickerModel';
import { updateNodeName, createDocumentBlock, renderDocument } from '../helpers/document-helper';
import { ITprDocumentBlock } from '../types/ITprDocumentBlock';
import { PACKAGE_VERSION } from '../../package-version.generated';

interface ITprDocumentsContent extends UmbBlockDataType {
    documents: UmbBlockValueType<UmbBlockListLayoutModel>;
}

interface ITprDocumentsSettings extends UmbBlockDataType {
    cssClasses: string;
}

@customElement('tpr-documents')
export class TprDocumentsView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: ITprDocumentsContent;

    @property({ attribute: false })
    settings?: ITprDocumentsSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    #mediaItemRepository: UmbMediaItemRepository;
    #documentItemRepository: UmbDocumentItemRepository;
    #documents: ITprDocumentBlock[] = [];
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
            const docs = this.content?.documents?.layout[UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS] || [];
            this.#documents = docs.map(layout => {
                const doc = this.content?.documents?.contentData.find(item => item.key === layout.contentKey)!;
                const docLink = (doc.values.find(props => props.alias == "document")?.value as Array<ILinkPickerModel>)[0];
                const datePublished = doc.values.find(props => props.alias == "datePublished")?.value as string;
                const numberOfPages = (doc.values.find(props => props.alias == "numberOfPages")?.value as number);
                const description = (doc.values.find(props => props.alias == "description")?.value as string);

                return createDocumentBlock(doc.key, docLink, datePublished, numberOfPages, description);
            });
        }
    }

    override async updated(changedProperties: Map<string, any>) {
        // Called after render(). Use this lifecycle hook to load media/content node names asynchronously.
        if (changedProperties.has('content')) {
            for (const doc of this.#documents) {
                doc.docName = await updateNodeName(
                    this.#mediaItemRepository,
                    this.#documentItemRepository,
                    doc.docLink?.unique,
                    doc.docName,
                    doc.docLink?.type,
                    this.#currentCulture);
            }
            this.requestUpdate(); // Force re-render after loading names
        }
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <dl class="tpr-documents govuk-list ${ this.settings?.cssClasses}">
                ${repeat(this.#documents || [],
                    (doc) => doc.key,
                    (doc) => {
                        return renderDocument(doc, true);
                    }
                )}
            </dl>
        </a>`;
    }
}

export default TprDocumentsView;
