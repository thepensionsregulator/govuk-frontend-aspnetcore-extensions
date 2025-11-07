import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';

interface IGovUkButtonGroupBlockList {
    contentData: Array<any>;
}

interface IGovUkButtonGroupContent extends UmbBlockDataType {
    buttons: IGovUkButtonGroupBlockList | null;
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

    constructor() {
        super();
    }

    override render() {
        let blocks = "No blocks.";    
        if (this.content?.buttons?.contentData?.length === 1) { blocks = "1 block." }
        if ((this.content?.buttons?.contentData?.length || 0) > 1) { blocks = `${this.content?.buttons?.contentData?.length} blocks.` }
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="backoffice-block-view ${ this.settings?.cssClasses}" aria-label="Edit button group component">
            <h2 class="govuk-heading-s" aria-hidden="true">Button group</h2>
            <p class="backoffice-additional-blocks" aria-hidden="true">${ blocks }</p>
        </div>
        `;
    }
}

export default GovUkButtonGroupView;