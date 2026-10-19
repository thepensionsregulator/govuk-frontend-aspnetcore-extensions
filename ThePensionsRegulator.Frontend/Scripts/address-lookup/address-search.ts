import { createFieldsetFormGroup } from
    "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/fieldset.js";

import { createTextInputFormGroup } from
    "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/inputs.js";

import { createButton } from 
    "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/button.js";

import { sanitisePostcode } from "./postcode-sanitiser.js";
import { normalisePostcode } from "./postcode-normaliser.js";
import { validatePostcode } from "./postcode-validator.js";
import { filterAddressesByBuilding } from "./address-filter.js";
import { clearFormGroupError, focusFirstInvalidControl, showFormGroupError } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/validation.js";
import type { AddressLookupContent, AddressSearchCriteria, AddressSearchOptions } from "./types.js";
import { createLink } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/link.js";
import { runActionOnEnter } from "./run-action-on-enter.js";
import { createParagraph } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/paragraph.js";

export function renderAddressSearch(options: AddressSearchOptions): HTMLElement {
    const addressSearchWrapper = document.createElement("div");

    const buildingNameFormGroup = createTextInputFormGroup({
        labelText: options.content.i18n.buildingNameOrNumberLabel,
        input: {
            id: "building",
            name: "building",
            width: "x-large",
            attributes: {
                autocomplete: "address-line1"
            },
            value: options.criteria?.buildingName
        }
    });
    const buildingNameOrNumberInput = buildingNameFormGroup.querySelector("input")!;

    const postcodeFormGroup = createTextInputFormGroup({
        labelText: options.content.i18n.postcodeLabel,
        input:{
            id: "postcode",
            name: "postcode",
            width: "large",
            attributes: {
                autocomplete: "postal-code"
            },
            value: options.criteria?.postcode
        }
    });
    const postcodeInput = postcodeFormGroup.querySelector("input")!;

    const fieldset = createFieldsetFormGroup({legendText: options.content.i18n.searchLegend, children: [ buildingNameFormGroup, postcodeFormGroup ], id: "address-search-fieldset"});
    fieldset.setAttribute("data-address-lookup-submit-error-anchor", "true");
    addressSearchWrapper.appendChild(fieldset);
    
    const findAddressButton = createButton({labelText: options.content.i18n.findAddressButton, variant: "secondary", type: "button"});
    runActionOnEnter(addressSearchWrapper, handleAddressSearch);
    findAddressButton.addEventListener("click", handleAddressSearch);
    addressSearchWrapper.appendChild(findAddressButton);

    
    const internationalEntryLink = createLink({labelText: options.content.i18n.internationalManualEntryLabel});
    internationalEntryLink.addEventListener("click", handleOnInternationalEntryClicked);
    const p = createParagraph();
    
    p.appendChild(internationalEntryLink);
    
    addressSearchWrapper.appendChild(p);

    return addressSearchWrapper;
    
    async function handleAddressSearch(): Promise<void> {
        if (findAddressButton.disabled) { return; }
        const validationResult = readAndValidateCriteria(buildingNameOrNumberInput, postcodeInput, options.content);
        if (!validationResult.isValid) {
            showFormGroupError(postcodeFormGroup, validationResult.errorMessage);
            const form = fieldset.closest("form");
             if (form) {
                 focusFirstInvalidControl(form);
             }
            return;
        }
        const { buildingName, postcode: normalisedPostcode } = validationResult.criteria;
        
        clearFormGroupError(postcodeFormGroup);
        findAddressButton.disabled = true;
        
        try {
            const addresses = await options.searchService.searchAddress(normalisedPostcode);
            const matches = buildingName ? filterAddressesByBuilding(addresses, buildingName) : addresses;

            if (matches.length === 0) {
                showFormGroupError(fieldset, options.content.i18n.addressNotFoundMessage);
                return;
            }

            clearFormGroupError(fieldset);
            options.onSearchSuccess(matches, validationResult.criteria);
        } catch {
            showFormGroupError(fieldset, options.content.i18n.serviceUnavailableMessage);
        } 
        finally {
             findAddressButton.disabled = false;
             const form = fieldset.closest("form");
             if (form) {
                 focusFirstInvalidControl(form);
             }
        }
    } 

    function handleOnInternationalEntryClicked(event: Event): void{
        event.preventDefault();
        options.onInternationalEntryRequested();
    }
}

type CriteriaValidationResult =
    | { isValid: true; criteria: AddressSearchCriteria }
    | { isValid: false; errorMessage: string };

function readAndValidateCriteria(buildingNameInput: HTMLInputElement, postcodeInput: HTMLInputElement, content: AddressLookupContent): CriteriaValidationResult {
    const buildingName = buildingNameInput.value.trim();
    const sanitisedPostcode = sanitisePostcode(postcodeInput.value);
    const normalisedPostcode = normalisePostcode(sanitisedPostcode);

    const validatePostcodeResult = validatePostcode(normalisedPostcode);
    if (!validatePostcodeResult.isValid) {
        const errorMessage = validatePostcodeResult.error === "required" ? content.i18n.postcodeRequiredErrorMessage : content.i18n.postcodePatternErrorMessage;
        return { isValid: false, errorMessage };
    }

    return { isValid: true, criteria: { buildingName, postcode: normalisedPostcode } };
}
