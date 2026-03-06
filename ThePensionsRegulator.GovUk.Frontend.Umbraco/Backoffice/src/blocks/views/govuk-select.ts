import { html, customElement, LitElement, property, repeat, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType, UmbBlockValueType } from '@umbraco-cms/backoffice/block';
import { UmbBlockListLayoutModel, UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS } from '@umbraco-cms/backoffice/block-list';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';
import { disableLinks } from '../helpers/html-helper';
import { PACKAGE_VERSION } from '../../package-version.generated';

interface IGovUkSelectContent extends UmbBlockDataType {
    label: string;
    hint: UmbPropertyEditorRteValueType;
    options: UmbBlockValueType<UmbBlockListLayoutModel> | undefined;
}

interface IGovUkSelectSettings extends UmbBlockDataType {
    cssClasses: string;
    labelIsPageHeading: boolean;
}


@customElement('govuk-select')
export class GovUkSelectView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkSelectContent;

    @property({ attribute: false })
    settings?: IGovUkSelectSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="govuk-form-group backoffice-block-view ${ this.settings?.cssClasses }">
            ${ this.settings?.labelIsPageHeading ?
            html`<h1 class="govuk-label-wrapper">
                    <label class="govuk-label govuk-label--l">${this.content?.label}</label>
                </h1>` :
            html`<label class="govuk-label">${this.content?.label}</label>` }            
            ${ this.content?.hint?.markup ? html`<div class="govuk-hint">${ unsafeHTML(disableLinks(this.content?.hint?.markup))}</div>` : null }
            <select class="govuk-select">
                ${ this?.content?.options?.contentData ? repeat(this.content?.options?.layout[UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS] || [],
                    (layout) => layout.contentKey,
                    (layout) => {
                        const opt = this.content?.options?.contentData.find(item => item.key === layout.contentKey);
                        const value = opt?.values.find(props => props.alias == "value")?.value;
                        const label = opt?.values.find(props => props.alias == "label")?.value;

                        return html`<option value="${value}">${label}</option>`
                    }
                ) : null }
            </select>
        </a>
        `;
    }
}

export default GovUkSelectView;