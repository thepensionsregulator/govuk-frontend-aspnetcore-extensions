import { html, repeat, TemplateResult, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbBlockListLayoutModel, UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS } from '@umbraco-cms/backoffice/block-list';
import { UmbBlockValueType } from '@umbraco-cms/backoffice/block';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';
import { disableLinks } from '../helpers/html-helper';

export function renderSummaryList(listItems: UmbBlockValueType<UmbBlockListLayoutModel> | undefined, cssClasses: string | undefined = undefined): TemplateResult {
        return html`
            <dl class="govuk-summary-list ${cssClasses}">
                ${repeat(listItems?.layout[UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS] || [],
                    (layout) => layout.contentKey,
                    (layout) => {
                        const block = listItems?.contentData.find(item => item.key === layout.contentKey)!;
                        const itemKey = (block.values.find(props => props.alias == "itemKey")?.value as string)?.toString();
                        const itemValue = (block.values.find(props => props.alias == "itemValue")?.value as UmbPropertyEditorRteValueType)?.markup;
                        const actions = (block.values.find(props => props.alias == "actions")?.value as UmbBlockValueType<UmbBlockListLayoutModel>);
                        return renderSummaryListItem(itemKey, itemValue, actions);
                    })}
            </dl>`;
}

export function renderSummaryListItem(itemKey: string | undefined, itemValueHtml: string | undefined, actions: UmbBlockValueType<UmbBlockListLayoutModel> | undefined):  TemplateResult {
    return html`
        <div class="govuk-summary-list__row">
            <dt class="govuk-summary-list__key">${itemKey}</dt>
            <dd class="govuk-summary-list__value">${unsafeHTML(disableLinks(itemValueHtml) || '')}</dd>
            ${actions ? html`
                <dd class="govuk-summary-list__actions">
                    <ul class="govuk-summary-list__actions-list">
                    ${repeat(actions.layout[UMB_BLOCK_LIST_PROPERTY_EDITOR_SCHEMA_ALIAS] || [],
                        (layout) => layout.contentKey,
                        (layout) => {
                            const action = actions?.contentData.find(item => item.key === layout.contentKey)!;
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