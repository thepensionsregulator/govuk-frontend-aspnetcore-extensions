import { html, customElement, LitElement, property, repeat } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { renderTask } from '../helpers/task-list-helper';
import { IRichTextProperty } from '../interfaces/IRichTextProperty';

interface IGovUkTaskContent extends UmbBlockDataType {
    taskName: string;
    hint: IRichTextProperty | null;
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

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <div class="backoffice-block-view">
            ${ renderTask(true, this.content?.taskName, this.content?.hint?.markup, this.settings?.status, this.settings?.cssClasses) }
        </div>
        `;
    }
}

export default GovUkTaskView;