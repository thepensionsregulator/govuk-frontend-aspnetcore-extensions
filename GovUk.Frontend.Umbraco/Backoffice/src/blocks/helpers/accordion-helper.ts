import { html, TemplateResult } from '@umbraco-cms/backoffice/external/lit';
import { IBlockListItem } from '../interfaces/IBlockListItem';

export function renderAccordionSection(heading: string | undefined, summary: string | undefined, blocks: Array<IBlockListItem> | null | undefined = []): TemplateResult {
        let blocksText = "No blocks.";    
        if (blocks?.length === 1) { blocksText = "1 block." }
        if ((blocks?.length || 0) > 1) { blocksText = `${blocks?.length} blocks.` }

        return html`<div class="govuk-accordion__section govuk-accordion__section--expanded">
        <div class="govuk-accordion__section-header">
            <h2 class="govuk-accordion__section-heading">${ heading }</h2>
            ${ summary ? html`<div class="govuk-accordion__section-summary govuk-body">${ summary }</div>` : null }
        </div>
        <div class="govuk-accordion__section-content">
            <p class="backoffice-additional-blocks">${ blocksText }</p>
        </div>
    </div>`    
    }