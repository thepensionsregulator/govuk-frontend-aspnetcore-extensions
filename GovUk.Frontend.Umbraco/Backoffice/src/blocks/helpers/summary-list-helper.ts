import { html, repeat, TemplateResult, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbBlockDataModel, UmbBlockValueDataPropertiesBaseType } from '@umbraco-cms/backoffice/block';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';
import { disableLinks } from '../helpers/html-helper';

export function renderSummaryList(listItems: Array<UmbBlockDataModel> | [], cssClasses: string | undefined = undefined): TemplateResult {
        return html`
            <dl class="govuk-summary-list ${cssClasses}">
                ${repeat(listItems || [],
                    (block) => block.key,
                    (block) => {
                        const itemKey = (block.values.find(props => props.alias == "itemKey")?.value as string)?.toString();
                        const itemValue = (block.values.find(props => props.alias == "itemValue")?.value as UmbPropertyEditorRteValueType)?.markup;
                        const actions = (block.values.find(props => props.alias == "actions")?.value as UmbBlockValueDataPropertiesBaseType)?.contentData;
                        return renderSummaryListItem(itemKey, itemValue, actions);
                    })}
            </dl>`;
}

export function renderSummaryListItem(itemKey: string | undefined, itemValueHtml: string | undefined, actions: Array<UmbBlockDataModel> | null | undefined):  TemplateResult {
    return html`
        <div class="govuk-summary-list__row">
            <dt class="govuk-summary-list__key">${itemKey}</dt>
            <dd class="govuk-summary-list__value">${unsafeHTML(disableLinks(itemValueHtml) || '')}</dd>
            ${actions ? html`
                <dd class="govuk-summary-list__actions">
                    <ul class="govuk-summary-list__actions-list">
                    ${repeat(actions || [],
                        (action: UmbBlockDataModel) => action.key,
                        (action: UmbBlockDataModel) => {
                            const text = (action.values.find(props => props.alias == "text")?.value as string);
                            return html`<li class="govuk-summary-list__actions-list-item">
                                ${renderSummaryListAction(text)}
                                </li>`
                            }
                    )}
                    </ul>
                </dd>` : null}
        </div>`
}

export function renderSummaryListAction(text: string | undefined): TemplateResult {
    return  html`<span class="govuk-link">${text}</span>`;
}