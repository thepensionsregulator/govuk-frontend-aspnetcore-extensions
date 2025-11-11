import { html, customElement, LitElement, property, repeat } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { IBlockListProperty } from '../interfaces/IBlockListProperty';
import { renderTask } from '../helpers/task-list-helper';

interface IGovUkTaskListContent extends UmbBlockDataType {
    tasks: IBlockListProperty | null;
}

interface IGovUkTaskListSettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-task-list')
export class GovUkTaskListView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkTaskListContent;

    @property({ attribute: false })
    settings?: IGovUkTaskListSettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            ${ this.content?.tasks?.contentData ? 
                html`<ul class="govuk-task-list ${this.settings?.cssClasses}">
                    ${ repeat(this.content?.tasks?.contentData,
                        (task) => task.key,
                        (task, index) => {
                            const taskName= task?.values.find(props => props.alias == "taskName")?.value;
                            const hint = task?.values.find(props => props.alias == "hint")?.value;

                            const settings = this.content?.tasks?.settingsData[index];
                            const status = settings?.values.find(props => props.alias == "status")?.value;
                            const cssClasses = settings?.values.find(props => props.alias == "cssClasses")?.value;

                            return renderTask(true, taskName, hint?.markup, status, cssClasses);
                        }
                    )}
                    </ul>`
                    : html`<p class="backoffice-additional-blocks">Task list with no tasks.</li>` }
        </a>
        `;
    }
}

export default GovUkTaskListView;