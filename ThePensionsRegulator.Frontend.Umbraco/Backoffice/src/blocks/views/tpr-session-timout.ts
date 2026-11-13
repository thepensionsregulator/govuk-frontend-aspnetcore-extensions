import { html, customElement, LitElement, property, unsafeHTML } from '@umbraco-cms/backoffice/external/lit';
import { UmbElementMixin } from '@umbraco-cms/backoffice/element-api';
import type { UmbBlockEditorCustomViewElement, UmbBlockEditorCustomViewConfiguration } from '@umbraco-cms/backoffice/block-custom-view';
import type { UmbBlockDataType } from '@umbraco-cms/backoffice/block';
import { UmbPropertyEditorRteValueType } from '@umbraco-cms/backoffice/rte';
import { disableLinks } from '../helpers/html-helper';
import { PACKAGE_VERSION } from '../../package-version.generated';

interface ITimeoutMessage extends UmbBlockDataType {
    tprSessionTimeoutTitle: string;
    tprSessionTimeoutContent: UmbPropertyEditorRteValueType;
    tprSessionTimeoutPrimaryActionText: string;
    tprSessionTimeoutSecondaryActionText: string;
}

@customElement('tpr-session-timeout-variant')
export class TprSessionTimeoutView extends UmbElementMixin(LitElement) implements UmbBlockEditorCustomViewElement {

    @property({ attribute: false })
    content?: ITimeoutMessage;

    @property({ attribute: false })
    config?: UmbBlockEditorCustomViewConfiguration;

    override render() {
        const contentMarkup = this.content?.tprSessionTimeoutContent?.markup;

        return html`
            <link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
            <a href="${this.config?.editContentPath ?? ''}" class="backoffice-block-view">
                <div>
                    <h2 class="govuk-heading-m">
                        ${this.content?.tprSessionTimeoutTitle}
                    </h2>
                    ${contentMarkup ? html`<div class="govuk-body">${unsafeHTML(disableLinks(contentMarkup))}</div>` : null}
                    ${this.content?.tprSessionTimeoutPrimaryActionText ? html`<span class="govuk-button govuk-button--primary">${this.content.tprSessionTimeoutPrimaryActionText}</span>` : null}
                    ${this.content?.tprSessionTimeoutSecondaryActionText ? html`<div><span class="govuk-link">${this.content.tprSessionTimeoutSecondaryActionText}</span></div>` : null}
                </div>
            </a>
        `;
    }
}
export default TprSessionTimeoutView;