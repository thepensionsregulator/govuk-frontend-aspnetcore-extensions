import { html, customElement, LitElement, property } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { PACKAGE_VERSION } from '../../package-version.generated';

interface IGovUkTaskListSummaryContent extends UmbBlockDataType {
    incompleteStatus: string;
    tracker: string;
}

interface IGovUkTaskListSummarySettings extends UmbBlockDataType {
    cssClasses: string;
}


@customElement('govuk-task-list-summary')
export class GovUkTaskListSummaryView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: IGovUkTaskListSummaryContent;

    @property({ attribute: false })
    settings?: IGovUkTaskListSummarySettings;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    constructor() {
        super();
    }

    override render() {
        return html`
        <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
        <a href="${this.config?.editContentPath ?? ''}" class="govuk-task-list-summary backoffice-block-view ${ this.settings?.cssClasses}">
            <h2 class="govuk-task-list-summary__heading govuk-heading-s">${ this.content?.incompleteStatus || 'Tasks incomplete' }</h2>
            <p class="govuk-task-list-summary__tracker govuk-body">${ this.content?.tracker || "You've completed 1 of 2 tasks." }</p>
        </a>
        `;
    }
}

export default GovUkTaskListSummaryView;