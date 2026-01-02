import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { renderTask } from '../helpers/task-list-helper';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';
import { PACKAGE_VERSION } from '../../package-version.generated';

interface IGovUkTaskContent extends UmbBlockDataType {
    taskName: string;
    hint: UmbPropertyEditorRteValueType | null;
}

interface IGovUkTaskSettings extends UmbBlockDataType {
    cssClasses: string;
    status: string | null;
}


@customElement('govuk-task')
export class GovUkTaskView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkTaskContent;

    @property({ attribute: false })
    settings?: IGovUkTaskSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            ${ renderTask(true, this.content?.taskName, this.content?.hint?.markup, this.settings?.status, this.settings?.cssClasses) }
        </a>
        `;
    }
}

export default GovUkTaskView;