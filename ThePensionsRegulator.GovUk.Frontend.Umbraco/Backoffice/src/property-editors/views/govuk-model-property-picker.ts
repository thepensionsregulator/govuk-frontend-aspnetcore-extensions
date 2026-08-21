import { html, customElement, property, state } from '@umbraco-cms/backoffice/external/lit';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import type { UmbPropertyEditorUiElement } from '@umbraco-cms/backoffice/property-editor';
import { umbHttpClient } from '@umbraco-cms/backoffice/http-client';
import { UMB_CONTENT_WORKSPACE_CONTEXT } from '@umbraco-cms/backoffice/content'; 
import { PACKAGE_VERSION } from '../../package-version.generated';

@customElement('govuk-model-property-picker')
export class GovUkModelPropertyPickerView extends UmbLitElement implements UmbPropertyEditorUiElement {
	constructor() {
		super();

		this.consumeContext(UMB_CONTENT_WORKSPACE_CONTEXT, (context) => {
			if (!context) return;
			this.observe(context.structure.ownerContentTypeAlias, (contentTypeAlias) => {
				if (contentTypeAlias) {
                    this.#contentTypeAlias = contentTypeAlias;
					this.#getData();
				}
			});

		}).passContextAliasMatches();
	}

	@property()
	value?: string;

	#onChange(e: InputEvent) {
		this.value = (e.target as HTMLInputElement).value;
		this.dispatchEvent(new UmbChangeEvent());
	}

	async #getData() {
		const { data } = await umbHttpClient.get<string[]>(
			{
				security: [
					{
						type: "http",
						scheme: "bearer"
					}
				],
				url: `/umbraco/management/api/v1/model-property/${this.#contentTypeAlias}`
			}
		);

		if (data) {
			const mapped = data.map(name => ({ name, value: name }));
			this._propertyNames = [{name:'',value:''}, ...mapped];
		}
	}

    #contentTypeAlias?: string;

	@state()
	private _propertyNames: Array<Option> = [];

	override render() {
		const propertyNamesWithSelected = this._propertyNames.map(option => ({
			...option,
			selected: option.value === this.value
		}));

		return html`<link rel="stylesheet" href="/css/govuk-umbraco-backoffice.css?v=${PACKAGE_VERSION}" />
				${propertyNamesWithSelected.length ? html`<uui-select .options="${propertyNamesWithSelected}" @change="${this.#onChange}" />` :
				html`<div class="govuk-umbraco-error">
						<span class="govuk-umbraco-error__icon">
							<uui-icon-registry-essential>
								<uui-icon name="alert" />
							</uui-icon-registry-essential>
						</span>
						<p>This document type is not configured for forms.</p>
						<p>Apply a [ModelType] attribute to your controller action. The properties of the class identified by [ModelType] will appear here.</p>
					</div>` }`
	}
}

export default GovUkModelPropertyPickerView;

declare global {
	interface HTMLElementTagNameMap {
		'govuk-model-property-picker': GovUkModelPropertyPickerView;
	}
}