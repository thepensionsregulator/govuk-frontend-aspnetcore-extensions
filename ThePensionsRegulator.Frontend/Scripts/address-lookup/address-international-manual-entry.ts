import { InternationalManualEntryAddress } from "./types";
import { createButton } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/button.js";
import { createFieldset } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/fieldset.js";
import { createSelectFormGroup, createTextInputFormGroup } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/inputs.js";
import { createLink } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/link.js";
import { clearFormGroupError, showFormGroupError } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/validation.js";

export interface AddressInternationalManualEntryOptions {
    onAddressSubmitted: (address: InternationalManualEntryAddress) => void;
    onBackToSearchRequested: () => void;
    countries: Array<{ value: string; text: string }>;
}

export function renderAddressInternationalManualEntry(options: AddressInternationalManualEntryOptions) : HTMLElement {
    const div = document.createElement("div");
    const addressLine1 = createTextInputFormGroup({ labelText: 'Address line 1', input: { id: 'address-line-1', name: 'address-line-1', type: "text", width: "x-large" }});
    const addressLine2 = createTextInputFormGroup({ labelText: 'Address line 2', input: { id: 'address-line-2', name: 'address-line-2', type: "text", width: "x-large" }});
    const addressLine3 = createTextInputFormGroup({ labelText: 'Address line 3', input: { id: 'address-line-3', name: 'address-line-3', type: "text", width: "x-large" }});
    const postTown = createTextInputFormGroup({ labelText: 'Town or City', input: { id: 'post-town', name: 'post-town', type: "text", width: "x-large" }});
    const countyStateProvince = createTextInputFormGroup({ labelText: 'County / State / Province', input: { id: 'county-state-province', name: 'county-state-province', type: "text", width: "x-large" }});
    const country = createSelectFormGroup({ labelText: 'Country', select: { id: 'country', name: 'country', width: "x-large", options: [{ value: '', text: 'Select a country' }, ...options.countries] }});
    const postcode = createTextInputFormGroup({ labelText: 'Postcode', input: { id: 'postcode', name: 'postcode', type: "text", width: "x-large" }});

    const fieldset = createFieldset({ legendText: 'Enter an international address', children: [addressLine1, addressLine2, addressLine3, postTown, countyStateProvince, country, postcode]})
    
    div.appendChild(fieldset);

    const confirmButton = createButton({ labelText: 'Confirm address', type: 'submit', variant: 'secondary' });
    confirmButton.addEventListener("click", handleConfirmAddress);
    div.appendChild(confirmButton);

    const backToSearch = createLink({ labelText: "Back to postcode search" });
    backToSearch.addEventListener("click", handleBackToSearch);

    const nav = document.createElement("nav");
    nav.appendChild(backToSearch);

    div.appendChild(nav);

    function handleBackToSearch(event: Event): void {
        event.preventDefault();
        options.onBackToSearchRequested();
    }

    return div;

    function handleConfirmAddress(event: Event): void {
        event.preventDefault();
        const formGroupsByField: Record<FieldRuleSet["fieldName"], HTMLElement> = { addressLine1, addressLine2, addressLine3, postTown, countyStateProvince, countryId: country, postcode };
        Object.values(formGroupsByField).forEach(clearFormGroupError);
        const validationResult = validateInternationalUkManualEntryAddress(readValues(addressLine1, addressLine2, addressLine3, postTown, countyStateProvince, country, postcode));
        if (!validationResult.isValid) {
            validationResult.errorMessages.forEach(({ fieldName, message }) => showFormGroupError(formGroupsByField[fieldName], message));
            return;
        }

        options.onAddressSubmitted(validationResult.address);
    }
}

type FieldRuleSet = { fieldName: Exclude<keyof InternationalManualEntryAddress, "countryName">, validators: Array<{ message: string; isValid: () => boolean }> };
type ErrorMessage = { fieldName: FieldRuleSet["fieldName"], message: string };
type InternationalManualEntryValidationResult = 
    | { isValid: true, address: InternationalManualEntryAddress }
    | { isValid: false, errorMessages: ErrorMessage[] };

export function validateInternationalUkManualEntryAddress(values: InternationalManualEntryAddress): InternationalManualEntryValidationResult {
    const fieldRuleSets: FieldRuleSet[] = [
        { fieldName: 'addressLine1', validators: [
            { message: "Address line 1 is required", isValid: () => values.addressLine1.trim() !== "" },
            { message: "Address line 1 must be 100 characters or fewer", isValid: () => values.addressLine1.trim().length <= 100 }
        ] },
        { fieldName: 'addressLine2', validators: [
            { message: "Address line 2 must be 100 characters or fewer", isValid: () => values.addressLine2 != undefined && values.addressLine2.trim().length <= 100 }
        ] },
        { fieldName: 'addressLine3', validators: [
            { message: "Address line 3 must be 100 characters or fewer", isValid: () => values.addressLine3 != undefined && values.addressLine3.trim().length <= 100 }
        ] },
        { fieldName: 'postTown', validators: [
            { message: "Post town is required", isValid: () => values.postTown.trim() !== "" },
            { message: "Post town must be 100 characters or fewer", isValid: () => values.postTown.trim().length <= 100 }
        ] },
        { fieldName: 'countyStateProvince', validators: [
            { message: "County/State/Province must be 100 characters or fewer", isValid: () => values.countyStateProvince != undefined && values.countyStateProvince.trim().length <= 100 }
        ]},
        { fieldName: 'countryId', validators: [
            { message: "Country is required", isValid: () => values.countryId !== undefined && values.countryId.trim() !== "" }
        ]},
        { fieldName: 'postcode', validators: [
            { message: "Postcode must be 10 characters or fewer", isValid: () => values.postcode != undefined && values.postcode.trim().length <= 10 }
        ]}
    ];

    const errorMessages: ErrorMessage[] = fieldRuleSets
        .map(({ fieldName, validators }) => {
            const firstFailure = validators.find(v => !v.isValid());
            return firstFailure ? { fieldName: fieldName, message: firstFailure.message } : null;
        })
        .filter(x => x != null);
    
    if (errorMessages.length > 0) {
        return { isValid: false, errorMessages };
    }

    return { isValid: true, address: {
        addressLine1: values.addressLine1,
        addressLine2: values.addressLine2,
        addressLine3: values.addressLine3,
        postTown: values.postTown,
        countyStateProvince: values.countyStateProvince,
        countryId: values.countryId,
        countryName: values.countryName,
        postcode: values.postcode
    }};
}

function readValues(addressLine1: HTMLElement, addressLine2: HTMLElement, addressLine3: HTMLElement, postTown: HTMLElement, countyStateProvince: HTMLElement, country: HTMLElement, postcode: HTMLElement) : InternationalManualEntryAddress {
    const addressLine1Value = (addressLine1.querySelector('input') as HTMLInputElement).value;
    const addressLine2Value = (addressLine2.querySelector('input') as HTMLInputElement).value;
    const addressLine3Value = (addressLine3.querySelector('input') as HTMLInputElement).value;
    const postTownValue = (postTown.querySelector('input') as HTMLInputElement).value;
    const countyStateProvinceValue = (countyStateProvince.querySelector('input') as HTMLInputElement).value;
    const countrySelect = country.querySelector('select') as HTMLSelectElement;
    const postcodeValue = (postcode.querySelector('input') as HTMLInputElement).value;

    return {
        addressLine1: addressLine1Value,
        addressLine2: addressLine2Value,
        addressLine3: addressLine3Value,
        postTown: postTownValue,
        countyStateProvince: countyStateProvinceValue,
        countryId: countrySelect.value,
        countryName: countrySelect.selectedOptions[0]?.text ?? '',
        postcode: postcodeValue
    };
}