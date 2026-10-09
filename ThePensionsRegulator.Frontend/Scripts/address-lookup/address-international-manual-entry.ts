import { submitOnEnter } from "./submit-on-enter.js";
import { InternationalManualEntryAddress, CountryOption, AddressLookupContent } from "./types";
import { createButton } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/button.js";
import { createFieldsetFormGroup } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/fieldset.js";
import { createSelectFormGroup, createTextInputFormGroup } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/inputs.js";
import { createLink } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/link.js";
import { createParagraph } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/paragraph.js";
import { clearFormGroupError, showFormGroupError } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/validation.js";

export interface AddressInternationalManualEntryOptions {
    content: AddressLookupContent;
    countries: CountryOption[];
    onAddressSubmitted: (address: InternationalManualEntryAddress) => void;
    onBackToSearchRequested: () => void;
}

export function renderAddressInternationalManualEntry(options: AddressInternationalManualEntryOptions) : HTMLElement {
    const div = document.createElement("div");
    const addressLine1 = createTextInputFormGroup({ labelText: options.content.addressLine1Label, input: { id: 'address-line-1', name: 'address-line-1', type: "text", width: "x-large" }});
    const addressLine2 = createTextInputFormGroup({ labelText: options.content.addressLine2Label, input: { id: 'address-line-2', name: 'address-line-2', type: "text", width: "x-large" }});
    const addressLine3 = createTextInputFormGroup({ labelText: options.content.addressLine3Label, input: { id: 'address-line-3', name: 'address-line-3', type: "text", width: "x-large" }});
    const postTown = createTextInputFormGroup({ labelText: options.content.postTownLabel, input: { id: 'post-town', name: 'post-town', type: "text", width: "x-large" }});
    const countyStateProvince = createTextInputFormGroup({ labelText: options.content.countyStateProvinceLabel, input: { id: 'county-state-province', name: 'county-state-province', type: "text", width: "x-large" }});
    const country = createSelectFormGroup({ labelText: options.content.countryLabel, select: { id: 'country', name: 'country', width: "x-large", options: [{ value: '', text: options.content.countrySelectPlaceholder },  ...options.countries.map(c => ({ value: c.value, text: c.name }))] }});
    const postcode = createTextInputFormGroup({ labelText: options.content.postcodeLabel, input: { id: 'postcode', name: 'postcode', type: "text", width: "x-large" }});

    const fieldsetFormGroup = createFieldsetFormGroup({
        id: "address-international-manual-entry-fieldset",
        legendText: options.content.internationalManualEntryLegend,
        children: [addressLine1, addressLine2, addressLine3, postTown, countyStateProvince, country, postcode]
    });
    fieldsetFormGroup.setAttribute("data-address-lookup-submit-error-anchor", "true");
    div.appendChild(fieldsetFormGroup);

    const confirmButton = createButton({ labelText: options.content.confirmAddressLabel, type: 'button', variant: 'secondary' });
    submitOnEnter(div, () => confirmButton.click());
    confirmButton.addEventListener("click", handleConfirmAddress);
    div.appendChild(confirmButton);

    const backToSearch = createLink({ labelText: options.content.backToPostcodeSearchLabel });
    backToSearch.addEventListener("click", handleBackToSearch);

    const p = createParagraph();
    p.appendChild(backToSearch);

    div.appendChild(p);

    function handleBackToSearch(event: Event): void {
        event.preventDefault();
        options.onBackToSearchRequested();
    }

    return div;

    function handleConfirmAddress(event: Event): void {
        event.preventDefault();
        const formGroupsByField: Record<FieldRuleSet["fieldName"], HTMLElement> = { addressLine1, addressLine2, addressLine3, postTown, countyStateProvince, countryId: country, postcode };
        Object.values(formGroupsByField).forEach(clearFormGroupError);
        const validationResult = validateInternationalUkManualEntryAddress(readValues(addressLine1, addressLine2, addressLine3, postTown, countyStateProvince, country, postcode), options.content);
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

export function validateInternationalUkManualEntryAddress(values: InternationalManualEntryAddress, content: AddressLookupContent): InternationalManualEntryValidationResult {
    const fieldRuleSets: FieldRuleSet[] = [
        { fieldName: 'addressLine1', validators: [
            { message: content.addressLine1RequiredErrorMessage, isValid: () => values.addressLine1.trim() !== "" },
            { message: content.addressLine1MaxLengthErrorMessage, isValid: () => values.addressLine1.trim().length <= 100 }
        ] },
        { fieldName: 'addressLine2', validators: [
            { message: content.addressLine2MaxLengthErrorMessage, isValid: () => values.addressLine2 != undefined && values.addressLine2.trim().length <= 100 }
        ] },
        { fieldName: 'addressLine3', validators: [
            { message: content.addressLine3MaxLengthErrorMessage, isValid: () => values.addressLine3 != undefined && values.addressLine3.trim().length <= 100 }
        ] },
        { fieldName: 'postTown', validators: [
            { message: content.postTownRequiredErrorMessage, isValid: () => values.postTown.trim() !== "" },
            { message: content.postTownMaxLengthErrorMessage, isValid: () => values.postTown.trim().length <= 100 }
        ] },
        { fieldName: 'countyStateProvince', validators: [
            { message: content.countyStateProvinceMaxLengthErrorMessage, isValid: () => values.countyStateProvince != undefined && values.countyStateProvince.trim().length <= 100 }
        ]},
        { fieldName: 'countryId', validators: [
            { message: content.countryRequiredErrorMessage, isValid: () => values.countryId !== undefined && values.countryId.trim() !== "" }
        ]},
        { fieldName: 'postcode', validators: [
            { message: content.postcodeMaxLengthErrorMessage, isValid: () => values.postcode != undefined && values.postcode.trim().length <= 10 }
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