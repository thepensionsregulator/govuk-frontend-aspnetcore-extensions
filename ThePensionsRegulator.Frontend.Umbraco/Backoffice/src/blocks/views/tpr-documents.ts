import { html, customElement, LitElement, property, repeat } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import { UmbBlockDataType, UmbBlockValueDataPropertiesBaseType } from '@umbraco-cms/backoffice/block';
import { UmbMediaItemRepository, UMB_MEDIA_ENTITY_TYPE } from '@umbraco-cms/backoffice/media';
import { UMB_DOCUMENT_ENTITY_TYPE, UMB_DOCUMENT_PROPERTY_DATASET_CONTEXT, UmbDocumentItemRepository } from '@umbraco-cms/backoffice/document';
import { ILinkPickerModel } from '../types/ILinkPickerModel';

interface ITprDocumentsContent extends UmbBlockDataType {
    documents: UmbBlockValueDataPropertiesBaseType;
}

interface ITprDocumentsSettings extends UmbBlockDataType {
    cssClasses: string;
}

interface ITprDocumentBlock {
    key: string;
    docName: string;
    docLink: ILinkPickerModel;
    datePublished: Date | null;
    numberOfPages: number;
    description: string;
    ext: string | undefined;
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
            const docs = this.content?.documents?.contentData || [];
            this.#documents = docs.map(doc => {
                const docLink = (doc?.values.find(props => props.alias == "document")?.value as Array<ILinkPickerModel>)[0];
                const datePublished = doc?.values.find(props => props.alias == "datePublished")?.value as string;
                const numberOfPages = (doc?.values.find(props => props.alias == "numberOfPages")?.value as number);
                const description = (doc?.values.find(props => props.alias == "description")?.value as string);
                const ext = docLink?.url?.split('.')?.pop()?.toLowerCase();

                return {
                    key: doc.key,
                    docName: docLink.name || '',
                    docLink,
                    datePublished: datePublished ? new Date(datePublished) : null,
                    numberOfPages,
                    description,
                    ext
                };
            });
        }
    }

    override async updated(changedProperties: Map<string, any>) {
        // Called after render(). Use this lifecycle hook to load media/content node names asynchronously.
        if (changedProperties.has('content')) {
            for (const doc of this.#documents) {
                if (!doc.docName && doc.docLink.type === UMB_MEDIA_ENTITY_TYPE) {
                    doc.docName = await this.#getNameForMedia(doc.docLink.unique);
                }
                else if (!doc.docName && doc.docLink.type == UMB_DOCUMENT_ENTITY_TYPE) {
                    doc.docName = await this.#getNameForDocument(doc.docLink.unique);
                }
            }
            this.requestUpdate(); // Force re-render after loading names
        }
    }

    async #getNameForMedia(unique: string) {
        const { data } = await this.#mediaItemRepository.requestItems([unique]);
        return data?.[0]?.name ?? '';
    }

    async #getNameForDocument(unique: string) {
        const { data } = await this.#documentItemRepository.requestItems([unique]);

        const currentCultureVariant = data?.[0]?.variants.find(variant => variant.culture === this.#currentCulture);
        const invariantCultureVariant = data?.[0]?.variants.find(variant => !variant.culture);
        const enGBcultureVariant = data?.[0]?.variants.find(variant => variant.culture === 'en-GB');
        const variant = currentCultureVariant || enGBcultureVariant || invariantCultureVariant;

        return variant?.name ?? '';
    }

    override render() {
        const known = ['csv', 'doc', 'docx', 'dotx', 'odt', 'pdf', 'pptx', 'rtf', 'xlst', 'xlsx'];
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            <dl class="tpr-documents govuk-list ${ this.settings?.cssClasses}">
                ${repeat(this.#documents || [],
                    (doc) => doc.key,
                    (doc) => {
                        const isKnownFileType = doc.ext && known.indexOf(doc.ext) !== -1;

                        return html`
                        <div class="tpr-document">
                            <dt>
                                <span class="govuk-link">
                                    ${doc.docName}
                                    ${isKnownFileType ? html`
                                        <br>
                                        ${doc.ext == 'csv' ? html`<span class="excel fileicon">CSV</span>` : null}
                                        ${doc.ext == 'doc' ? html`<span class="doc fileicon">Word</span>` : null}
                                        ${doc.ext == 'docx' ? html`<span class="doc fileicon">Word</span>` : null}
                                        ${doc.ext == 'dotx' ? html`<span class="doc fileicon">DOTX</span>` : null}
                                        ${doc.ext == 'odt' ? html`<span class="misc fileicon">ODT</span>` : null}
                                        ${doc.ext == 'pdf' ? html`<span class="pdf fileicon">PDF</span>` : null}
                                        ${doc.ext == 'pptx' ? html`<span class="powerpoint fileicon">PPTX</span>` : null}
                                        ${doc.ext == 'rtf' ? html`<span class="misc fileicon">RTF</span>` : null}
                                        ${doc.ext == 'xlst' ? html`<span class="excel fileicon">XLST</span>` : null}
                                        ${doc.ext == 'xlsx' ? html`<span class="excel fileicon">Excel</span>` : null}
                                        <span>--KB, </span>
                                        ${ doc.numberOfPages ? html`${doc.numberOfPages} page(s)` : null}
                                    ` : null}
                                </span>
                            </dt>
                            ${doc.datePublished ? html`<dd>Published: ${doc.datePublished.toLocaleDateString('en-GB', { year: 'numeric', month: 'long' })}</dd>` : null}
                            ${doc.description ? html`<dd>${doc.description}</dd>` : null}
                        </div>`;
                    }
                )}
            </dl>
        </a>`;
    }
}

export default TprDocumentsView;
