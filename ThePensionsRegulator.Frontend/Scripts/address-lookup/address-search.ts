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
import type { AddressSearchCriteria, AddressSearchOptions } from "./types.js";

export function renderAddressSearch(options: AddressSearchOptions): HTMLElement {
    const addressSearchWrapper = document.createElement("div");

    const buildingNameFormGroup = createTextInputFormGroup({
        labelText: "Building name or number",
        input: {
            id: "building",
            name: "building",
            width: "x-large",
            value: options.criteria?.buildingName
        }
    });
    const buildingNameOrNumberInput = buildingNameFormGroup.querySelector("input")!;

    const postcodeFormGroup = createTextInputFormGroup({
        labelText: "Postcode",
        input:{
            id: "postcode",
            name: "postcode",
            width: "large",
            value: options.criteria?.postcode
        }
    });
    const postcodeInput = postcodeFormGroup.querySelector("input")!;

    const fieldset = createFieldsetFormGroup({legendText: "Search by postcode", children: [ buildingNameFormGroup, postcodeFormGroup ], id: "address-search-fieldset"});
    addressSearchWrapper.appendChild(fieldset);
    
    const findAddressButton = createButton({labelText: "Find address", variant: "secondary", type: "button"});
    findAddressButton.addEventListener("click", handleAddressSearch);
    addressSearchWrapper.appendChild(findAddressButton);

    return addressSearchWrapper;
    
    async function handleAddressSearch(): Promise<void> {
        const validationResult = readAndValidateCriteria(buildingNameOrNumberInput, postcodeInput);
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
                showFormGroupError(fieldset, "The address and postcode do not match");
                return;
            }

            clearFormGroupError(fieldset);
            options.onSearchSuccess(matches, validationResult.criteria);
        } catch {
            showFormGroupError(fieldset, "There was a problem searching for addresses. Please try again.");
        } 
        finally {
             findAddressButton.disabled = false;
        }
    } 
}

type CriteriaValidationResult =
    | { isValid: true; criteria: AddressSearchCriteria }
    | { isValid: false; errorMessage: string };

function readAndValidateCriteria(buildingNameInput: HTMLInputElement, postcodeInput: HTMLInputElement): CriteriaValidationResult {
    const buildingName = buildingNameInput.value.trim();
    const sanitisedPostcode = sanitisePostcode(postcodeInput.value);
    const normalisedPostcode = normalisePostcode(sanitisedPostcode);

    const validatePostcodeResult = validatePostcode(normalisedPostcode);
    if (!validatePostcodeResult.isValid) {
        const errorMessage = validatePostcodeResult.error === "required" ? "Enter a postcode" : "Enter a valid postcode";
        return { isValid: false, errorMessage };
    }

    return { isValid: true, criteria: { buildingName, postcode: normalisedPostcode } };
}
