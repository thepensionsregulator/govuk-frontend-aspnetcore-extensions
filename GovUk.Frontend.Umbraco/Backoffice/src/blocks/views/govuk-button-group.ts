import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType, UmbBlockValueType } from '@umbraco-cms/backoffice/block';
import { UmbBlockListLayoutModel } from '@umbraco-cms/backoffice/block-list';
import { PACKAGE_VERSION } from '../../package-version.generated';

interface IGovUkButtonGroupContent extends UmbBlockDataType {
    buttons: UmbBlockValueType<UmbBlockListLayoutModel> | null;
}

interface IGovUkButtonGroupSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-button-group')
export class GovUkButtonGroupView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkButtonGroupContent;

    @property({ attribute: false })
    settings?: IGovUkButtonGroupSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        let blocks = "No blocks.";    
        if (this.content?.buttons?.contentData?.length === 1) { blocks = "1 block." }
        if ((this.content?.buttons?.contentData?.length || 0) > 1) { blocks = `${this.content?.buttons?.contentData?.length} blocks.` }
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view ${ this.settings?.cssClasses}">
            <h2 class="govuk-heading-s">Button group</h2>
            <p class="backoffice-additional-blocks">${ blocks }</p>
        </a>
        `;
    }
}

export default GovUkButtonGroupView;