import { html, repeat, TemplateResult, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { IBlockListItem } from '../interfaces/IBlockListItem';

export function renderSummaryList(listItems: Array<IBlockListItem> | [], cssClasses: string | undefined = undefined): TemplateResult {
        return html`
            <dl class="govuk-summary-list ${cssClasses}">
                ${repeat(listItems || [],
                    (block) => block.key,
                    (block) => {
                        const itemKey = block.values.find(props => props.alias == "itemKey")?.value;
                        const itemValue = block.values.find(props => props.alias == "itemValue")?.value?.markup;
                        const actions = block.values.find(props => props.alias == "actions")?.value?.contentData;
                        return renderSummaryListItem(itemKey, itemValue, actions);
                    })}
            </dl>`;
}

export function renderSummaryListItem(itemKey: string | undefined, itemValueHtml: string | undefined, actions: Array<IBlockListItem> | null | undefined):  TemplateResult {
    return html`
        <div class="govuk-summary-list__row">
            <dt class="govuk-summary-list__key">${itemKey}</dt>
            <dd class="govuk-summary-list__value">${unsafeHTML(itemValueHtml || '')}</dd>
            ${actions ? html`
                <dd class="govuk-summary-list__actions">
                    <ul class="govuk-summary-list__actions-list">
                    ${repeat(actions || [],
                        (action: IBlockListItem) => action.key,
                        (action: IBlockListItem) => html`<li class="govuk-summary-list__actions-list-item">
                            ${renderSummaryListAction(action.values.find(props => props.alias == "text")?.value)}
                        </li>`
                    )}
                    </ul>
                </dd>` : null}
        </div>`
}

export function renderSummaryListAction(text: string | undefined): TemplateResult {
    return  html`<a class="govuk-link" href="javascript:return false">${text}</a>`;
}