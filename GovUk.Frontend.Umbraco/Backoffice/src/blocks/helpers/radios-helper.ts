import { html, TemplateResult, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { IBlockListItem } from "../interfaces/IBlockListItem";
import { disableLinks } from '../helpers/html-helper';

export function renderRadiosDivider(text:string | undefined): TemplateResult {
    return html`<div class="govuk-radios__divider">${text || 'or'}</div>`;
}

export function renderRadioButton(
    label: string | undefined,
    value: string | undefined,
    hintHtml: string | undefined,
    conditionalBlocks: Array<IBlockListItem> | null | undefined = [],
    renderConditionalBlocks: boolean,
): TemplateResult {

    let conditionalBlocksText = "No conditional blocks.";
    if (conditionalBlocks?.length === 1) { conditionalBlocksText = "1 conditional block." }
    if ((conditionalBlocks?.length || 0) > 1) { conditionalBlocksText = `${conditionalBlocks?.length} conditional blocks.` }

    return html`<div class="govuk-radios__item">
                    <input class="govuk-radios__input" type="radio" value="${value}">
                    <label class="govuk-radios__label govuk-label">${label}</label>
                    ${hintHtml ? html`<div class="govuk-radios__hint govuk-hint">${unsafeHTML(disableLinks(hintHtml))}</div>` : null}
                </div>
                ${renderConditionalBlocks ? html`<div class="govuk-radios__conditional">
                    <div class="govuk-form-group">
                        <p class="backoffice-additional-blocks">${conditionalBlocksText}</p>
                    </div> 
                </div>` : null}`
}