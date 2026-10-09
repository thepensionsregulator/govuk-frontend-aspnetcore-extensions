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
import { clearFormGroupError, showFormGroupError } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/validation.js";
import type { AddressLookupContent, AddressSearchCriteria, AddressSearchOptions } from "./types.js";
import { createLink } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/link.js";
import { createParagraph } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/paragraph.js";
import { submitOnEnter } from "./submit-on-enter.js";

export function renderAddressSearch(options: AddressSearchOptions): HTMLElement {
    const addressSearchWrapper = document.createElement("div");

    const buildingNameFormGroup = createTextInputFormGroup({
        labelText: options.content.buildingNameOrNumberLabel,
        input: {
            id: "building",
            name: "building",
            width: "x-large",
            value: options.criteria?.buildingName
        }
    });
    const buildingNameOrNumberInput = buildingNameFormGroup.querySelector("input")!;

    const postcodeFormGroup = createTextInputFormGroup({
        labelText: options.content.postcodeLabel,
        input:{
            id: "postcode",
            name: "postcode",
            width: "large",
            value: options.criteria?.postcode
        }
    });
    const postcodeInput = postcodeFormGroup.querySelector("input")!;

    const fieldset = createFieldsetFormGroup({legendText: options.content.searchLegend, children: [ buildingNameFormGroup, postcodeFormGroup ], id: "address-search-fieldset"});
    fieldset.setAttribute("data-address-lookup-submit-error-anchor", "true");
    addressSearchWrapper.appendChild(fieldset);
    
    const findAddressButton = createButton({labelText: options.content.findAddressButton, variant: "secondary", type: "button"});
    submitOnEnter(addressSearchWrapper, handleAddressSearch);
    findAddressButton.addEventListener("click", handleAddressSearch);
    addressSearchWrapper.appendChild(findAddressButton);

    
    const internationalEntryLink = createLink({labelText: options.content.internationalManualEntryLabel});
    internationalEntryLink.addEventListener("click", handleOnInternationalEntryClicked);
    const p = createParagraph();
    
    p.appendChild(internationalEntryLink);
    
    addressSearchWrapper.appendChild(p);

    return addressSearchWrapper;
    
    async function handleAddressSearch(): Promise<void> {
        const validationResult = readAndValidateCriteria(buildingNameOrNumberInput, postcodeInput, options.content);
        if (!validationResult.isValid) {
            showFormGroupError(postcodeFormGroup, validationResult.errorMessage);
            return;
        }
        const { buildingName, postcode: normalisedPostcode } = validationResult.criteria;
        
        clearFormGroupError(postcodeFormGroup);
        findAddressButton.disabled = true;
        
        try {
            const addresses = await options.searchService.searchAddress(normalisedPostcode);
            const matches = buildingName ? filterAddressesByBuilding(addresses, buildingName) : addresses;

            if (matches.length === 0) {
                showFormGroupError(fieldset, options.content.addressNotFoundMessage);
                return;
            }

            clearFormGroupError(fieldset);
            options.onSearchSuccess(matches, validationResult.criteria);
        } catch {
            showFormGroupError(fieldset, options.content.serviceUnavailableMessage);
        } 
        finally {
             findAddressButton.disabled = false;
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
        const errorMessage = validatePostcodeResult.error === "required" ? content.postcodeRequiredErrorMessage : content.postcodePatternErrorMessage;
        return { isValid: false, errorMessage };
    }

    return { isValid: true, criteria: { buildingName, postcode: normalisedPostcode } };
}
