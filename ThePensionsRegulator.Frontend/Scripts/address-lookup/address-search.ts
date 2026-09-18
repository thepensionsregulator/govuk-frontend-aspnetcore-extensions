import { createFieldsetFormGroup } from
    "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/fieldset.js";

import { createTextInputFormGroup } from
    "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/inputs.js";

import { createButton } from 
    "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/button.js";

import { sanitisePostcode } from "./postcode-sanitiser.js";
import { normalisePostcode } from "./postcode-normaliser.js";
import { validatePostcode } from "./postcode-validator.js";
import { clearFormGroupError, showFormGroupError } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/validation.js";
import type { AddressSearchOptions } from "./types.js";

export function createAddressSearch(options: AddressSearchOptions): HTMLElement{
    const addressSearchWrapper = document.createElement("div");

    const buildingNameFormGroup = createTextInputFormGroup({
        labelText: "Building name or number",
        input: {
            id: "building",
            name: "building",
            width: "x-large"
        }
    });
    const buildingNameOrNumberInput = buildingNameFormGroup.querySelector("input");

    const postcodeFormGroup = createTextInputFormGroup({
        labelText: "Postcode",
        input:{
            id: "postcode",
            name: "postcode",
            width: "large"
        }
    });
    const postcodeInput = postcodeFormGroup.querySelector("input");


    const fieldset = createFieldsetFormGroup({legendText: "Search by postcode", children: [ buildingNameFormGroup, postcodeFormGroup ], id: "address-search-fieldset"});
    const findAddressButton = createButton({labelText: "Find address", variant: "secondary", type: "button"});
    findAddressButton.addEventListener("click", handleAddressSearch);
    addressSearchWrapper.appendChild(fieldset);
    addressSearchWrapper.appendChild(findAddressButton);

    return addressSearchWrapper;
    
    function handleAddressSearch(): void {
        const buildingName = buildingNameOrNumberInput!.value.trim();
        const sanitisedPostcode = sanitisePostcode(postcodeInput!.value);
        const normalisedPostcode = normalisePostcode(sanitisedPostcode);

        const validatePostcodeResult = validatePostcode(normalisedPostcode); 
        if (validatePostcodeResult.isValid === true) {
            clearFormGroupError(postcodeFormGroup);
            options.onSearch({buildingName: buildingName || undefined, postcode: normalisedPostcode || undefined});
        } else {
            if (validatePostcodeResult.error === "required"){
                showFormGroupError(postcodeFormGroup, "Enter a postcode");
            } else if (validatePostcodeResult.error === "invalid-format") {
                showFormGroupError(postcodeFormGroup, "Enter a valid postcode");
            }
        }
    }
}