import { html, repeat, TemplateResult, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { IBlockListItem } from '../interfaces/IBlockListItem';

export function renderSummaryList(listItems: Array<IBlockListItem> | [], cssClasses: string | undefined = undefined): TemplateResult {
        return html`
            <dl class="govuk-summary-list ${cssClasses}">
                ${repeat(listItems || [],
            (block) => block.key,
            (block) => html`
                    <div class="govuk-summary-list__row">
                        <dt class="govuk-summary-list__key">${block.values.find(props => props.alias == "itemKey")?.value}</dt>
                        <dd class="govuk-summary-list__value">${unsafeHTML(block.values.find(props => props.alias == "itemValue")?.value?.markup)}</dd>
                        ${block.values.find(props => props.alias == "actions")?.value?.contentData ? html`
                            <dd class="govuk-summary-list__actions">
                                <ul class="govuk-summary-list__actions-list">
                                ${repeat(block.values.find(props => props.alias == "actions")?.value?.contentData || [],
                (action: IBlockListItem) => action.key,
                (action: IBlockListItem) => html`<li class="govuk-summary-list__actions-list-item">
                                        <a class="govuk-link" href="javascript:return false">${action.values.find(props => props.alias == "text")?.value}</a>
                                    </li>`
            )}
                                </ul>
                            </dd>` : null}
                    </div>`)}
            </dl>`;
    }