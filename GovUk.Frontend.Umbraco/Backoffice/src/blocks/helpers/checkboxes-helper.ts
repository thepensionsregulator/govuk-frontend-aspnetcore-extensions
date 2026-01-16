import { html, TemplateResult, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbBlockDataModel } from "@umbraco-cms/backoffice/block";
import { disableLinks } from '../helpers/html-helper';

export function renderCheckboxesDivider(text:string | undefined): TemplateResult {
    return html`<div class="govuk-checkboxes__divider">${text || 'or'}</div>`;
}

export function renderCheckbox(
    label: string | undefined,
    value: string | undefined,
    hintHtml: string | undefined,
    conditionalBlocks: Array<UmbBlockDataModel> | null | undefined = []
): TemplateResult {

    let conditionalBlocksText = "No conditional blocks.";
    if (conditionalBlocks?.length === 1) { conditionalBlocksText = "1 conditional block." }
    if ((conditionalBlocks?.length || 0) > 1) { conditionalBlocksText = `${conditionalBlocks?.length} conditional blocks.` }

    return html`<div class="govuk-checkboxes__item">
                    <input class="govuk-checkboxes__input" type="checkbox" value="${value}">
                    <label class="govuk-checkboxes__label govuk-label">${label}</label>
                    ${hintHtml ? html`<div class="govuk-checkboxes__hint govuk-hint">${unsafeHTML(disableLinks(hintHtml))}</div>` : null}
                </div>
                <div class="govuk-checkboxes__conditional">
                    <div class="govuk-form-group">
                        <p class="backoffice-additional-blocks">${conditionalBlocksText}</p>
                    </div> 
                </div>`;
}