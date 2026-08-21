import { html, TemplateResult, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { disableLinks } from '../helpers/html-helper';

function toKebabCase(str: string): string {
    return "govuk-task-list__status--" + str
        .replace(/([a-z])([A-Z])/g, '$1-$2') // insert hyphen between camelCase
        .replace(/[\s_]+/g, '-')             // replace spaces and underscores with hyphen
        .toLowerCase();
}

export function renderTask(
    isInAList: boolean,
    taskName: string | null | undefined,
    hintHtml: string | null | undefined,
    status: string | null | undefined,
    cssClasses: string | null | undefined
): TemplateResult {
    const containerClass = "govuk-task-list__item--with-link govuk-task-list__item";
    const children = html`
        <div class="govuk-task-list__name-and-hint">
            <span class="govuk-task-list__link govuk-link" href="javascript:return false">${taskName}</span>
            ${hintHtml ? html`<div class="govuk-task-list__hint">${unsafeHTML(disableLinks(hintHtml))}</div>` : null}
        </div>
        ${status ? html`<div class="govuk-task-list__status ${toKebabCase(String(status))}">
            <strong class="govuk-tag">${status}</strong>
        </div>` : null}
    `;
    return isInAList
        ? html`<li class="${containerClass} ${cssClasses}">${children}</li>`
        : html`<div class="${containerClass} ${cssClasses}">${children}</div>`;
}