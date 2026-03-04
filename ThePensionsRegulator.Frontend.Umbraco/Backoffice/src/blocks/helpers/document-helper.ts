import { html, TemplateResult } from '@umbraco-cms/backoffice/external/lit';
import { UMB_MEDIA_ENTITY_TYPE, UmbMediaItemRepository } from '@umbraco-cms/backoffice/media';
import { UMB_DOCUMENT_ENTITY_TYPE, UmbDocumentItemRepository } from '@umbraco-cms/backoffice/document';
import { ITprDocumentBlock } from '../types/ITprDocumentBlock';
import { ILinkPickerModel } from '../types/ILinkPickerModel';

export function createDocumentBlock(
    key: string | undefined,
    doc: ILinkPickerModel | undefined,
    datePublished: string | undefined,
    numberOfPages: number | undefined,
    description: string | undefined): ITprDocumentBlock {
    return {
        key: key || '',
        docName: doc?.name || '',
        docLink: doc,
        datePublished: datePublished ? new Date(datePublished) : null,
        numberOfPages: numberOfPages,
        description: description,
        ext: doc?.url?.split('.')?.pop()?.toLowerCase()
    }
}

export async function updateNodeName(
    mediaItemRepository: UmbMediaItemRepository,
    documentItemRepository: UmbDocumentItemRepository, 
    unique: string | undefined,
    name: string | undefined,
    linkType: string | undefined,
    currentCulture: string | null): Promise<string | undefined>
{
    if (!name && linkType === UMB_MEDIA_ENTITY_TYPE) {
        name = await getNameForMedia(mediaItemRepository, unique);
    }
    else if (!name && linkType == UMB_DOCUMENT_ENTITY_TYPE) {
        name = await getNameForDocument(documentItemRepository, unique, currentCulture);
    }
    return name;
}

async function getNameForMedia(mediaItemRepository: UmbMediaItemRepository, unique: string | undefined) {
    if (!unique) return '';
    const { data } = await mediaItemRepository.requestItems([unique]);
    return data?.[0]?.name ?? '';
}

async function getNameForDocument(documentItemRepository: UmbDocumentItemRepository, unique: string | undefined, currentCulture: string | null) {
    if (!unique) return '';
    const { data } = await documentItemRepository.requestItems([unique]);

    const currentCultureVariant = data?.[0]?.variants.find(variant => variant.culture === currentCulture);
    const invariantCultureVariant = data?.[0]?.variants.find(variant => !variant.culture);
    const enGBcultureVariant = data?.[0]?.variants.find(variant => variant.culture === 'en-GB');
    const variant = currentCultureVariant || enGBcultureVariant || invariantCultureVariant;

    return variant?.name ?? '';
}

export function renderDocument(doc: ITprDocumentBlock | null | undefined, isInList: boolean): TemplateResult {

    const datePublished = doc?.datePublished?.toLocaleDateString('en-GB', { year: 'numeric', month: 'long' });
    const bodyClass = isInList ? "" : "govuk-body";

            
    return html`<div class="tpr-document ${bodyClass}">
        ${isInList ?
            html`<dt>${renderDocumentLink(doc)}</dt>
                ${datePublished ? html`<dd>Published: ${datePublished}</dd>` : null}
                ${doc?.description ? html`<dd>${doc?.description}</dd>` : null}` :
            html`<div>${renderDocumentLink(doc)}</div>
                ${datePublished ? html`<div>Published: ${datePublished}</div>` : null}
                ${doc?.description ? html`<div>${doc?.description}</div>` : null}`}
        </div>`
}
function renderDocumentLink(doc: ITprDocumentBlock | null | undefined): TemplateResult {
    const known = ['csv', 'doc', 'docx', 'dotx', 'odt', 'pdf', 'pptx', 'rtf', 'xlst', 'xlsx'];
    const isKnownFileType = doc?.ext && known.indexOf(doc?.ext) !== -1;

    return html`<span class="govuk-link">
        ${doc?.docName}
        ${isKnownFileType ? html`
            <br>
            ${doc?.ext == 'csv' ? html`<span class="excel fileicon">CSV</span>` : null}
            ${doc?.ext == 'doc' ? html`<span class="doc fileicon">Word</span>` : null}
            ${doc?.ext == 'docx' ? html`<span class="doc fileicon">Word</span>` : null}
            ${doc?.ext == 'dotx' ? html`<span class="doc fileicon">DOTX</span>` : null}
            ${doc?.ext == 'odt' ? html`<span class="misc fileicon">ODT</span>` : null}
            ${doc?.ext == 'pdf' ? html`<span class="pdf fileicon">PDF</span>` : null}
            ${doc?.ext == 'pptx' ? html`<span class="powerpoint fileicon">PPTX</span>` : null}
            ${doc?.ext == 'rtf' ? html`<span class="misc fileicon">RTF</span>` : null}
            ${doc?.ext == 'xlst' ? html`<span class="excel fileicon">XLST</span>` : null}
            ${doc?.ext == 'xlsx' ? html`<span class="excel fileicon">Excel</span>` : null}
            <span>--KB, </span>
            ${doc?.numberOfPages ? html`${doc?.numberOfPages} page(s)` : null}
        ` : null}
    </span>`;
}