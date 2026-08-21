import { html, customElement, LitElement, property, repeat } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType, UmbBlockValueType } from '@umbraco-cms/backoffice/block';
import { UmbBlockListLayoutModel, UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS } from '@umbraco-cms/backoffice/block-list';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';
import { renderTask } from '../helpers/task-list-helper';
import { PACKAGE_VERSION } from '../../package-version.generated';

interface IGovUkTaskListContent extends UmbBlockDataType {
    tasks: UmbBlockValueType<UmbBlockListLayoutModel> | null;
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
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
            ${ this.content?.tasks?.contentData ? 
                html`<ul class="govuk-task-list ${this.settings?.cssClasses}">
                    ${ repeat(this.content?.tasks?.layout[UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS] || [],
                        (layout) => layout.contentKey,
                        (layout) => {
                            const task = this.content?.tasks?.contentData.find(item => item.key === layout.contentKey)!;
                            const taskName = (task?.values.find(props => props.alias == "taskName")?.value as string)?.toString();
                            const hint = (task?.values.find(props => props.alias == "hint")?.value as UmbPropertyEditorRteValueType);

                            const settings = this.content?.tasks?.settingsData.find(item => item.key === layout.settingsKey)!;
                            const status = (settings?.values.find(props => props.alias == "status")?.value as string)?.toString();
                            const cssClasses = (settings?.values.find(props => props.alias == "cssClasses")?.value as string)?.toString();

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