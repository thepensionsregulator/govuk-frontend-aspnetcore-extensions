import { normalisePostcode } from './postcode-normaliser.js';
import { sanitisePostcode } from './postcode-sanitiser.js';
import { validatePostcode } from './postcode-validator.js';
import { runActionOnEnter } from './run-action-on-enter.js';
import type { AddressLookupContent, UkManualEntryAddress } from './types.js';
import { createButton } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/button.js';
import { createFieldsetFormGroup } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/fieldset.js';
import { createTextInputFormGroup } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/inputs.js';
import { createLink } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/link.js';
import { clearFormGroupError, showFormGroupError, focusFirstInvalidControl } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/validation.js';

export interface AddressUkManualEntryOptions {
    content: AddressLookupContent
    onAddressSubmitted: (address: UkManualEntryAddress) => void;
    onBackToSearchRequested: () => void;
}

export function renderAddressUkManualEntry(options: AddressUkManualEntryOptions): HTMLElement {    
    const content = options.content;
    const div = document.createElement('div');
    const addressLine1 = createTextInputFormGroup({ labelText: 'Address line 1', input: { id: 'address-line-1', name: 'address-line-1', type: "text", width: "x-large", attributes: { autocomplete: "address-line1"} } });
    const addressLine2 = createTextInputFormGroup({ labelText: 'Address line 2 (optional)', input: { id: 'address-line-2', name: 'address-line-2', type: "text", width: "x-large", attributes: { autocomplete: "address-line2"} } });
    const addressLine3 = createTextInputFormGroup({ labelText: 'Address line 3 (optional)', input: { id: 'address-line-3', name: 'address-line-3', type: "text", width: "x-large", attributes: { autocomplete: "address-line3"} } });
    const townOrCity = createTextInputFormGroup({ labelText: 'Town or city', input: { id: 'town-or-city', name: 'town-or-city', type: "text", width: "large", attributes: { autocomplete: "address-level2"} } });
    const county = createTextInputFormGroup({ labelText: 'County (optional)', input: { id: 'county', name: 'county', type: "text", width: "large", attributes: { autocomplete: "address-level1"} } });
    const postcode = createTextInputFormGroup({ labelText: 'Postcode', input: { id: 'postcode', name: 'postcode', type: "text", width: "large", attributes: { autocomplete: "postal-code"} } });

    const fieldsetFormGroup = createFieldsetFormGroup({
        id: "address-uk-manual-entry-fieldset",
        legendText: content.i18n.ukManualEntryLegend,
        children: [addressLine1, addressLine2, addressLine3, townOrCity, county, postcode]
    });
    fieldsetFormGroup.setAttribute("data-address-lookup-submit-error-anchor", "true");
    div.appendChild(fieldsetFormGroup);

    const confirmButton = createButton({ labelText: content.i18n.confirmAddressLabel, type: 'button', variant: 'secondary' });
    runActionOnEnter(div, () => confirmButton.click());
    confirmButton.addEventListener("click", handleConfirmAddress);
    div.appendChild(confirmButton);

    const backToSearchNav = document.createElement("nav");
    div.appendChild(backToSearchNav);
    const backToSearch = createLink({ labelText: content.i18n.backToPostcodeSearchLabel});
    backToSearch.addEventListener("click", handleBackToSearch);
    backToSearchNav.appendChild(backToSearch);

    function handleBackToSearch(event: Event): void {
        event.preventDefault();
        options.onBackToSearchRequested();
    }

    return div;
    
    function handleConfirmAddress(event: Event) {
        event.preventDefault();
        const formGroupsByField: Record<keyof UkManualEntryValues, HTMLElement> = { addressLine1, addressLine2, addressLine3, townOrCity, county, postcode };
        Object.values(formGroupsByField).forEach(clearFormGroupError);
        const validationResult = validateUkManualEntryAddress(readValues(addressLine1, addressLine2, addressLine3, townOrCity, county, postcode), content);
        if (!validationResult.isValid) {
            validationResult.errorMessages.forEach(({ fieldName, message }) => showFormGroupError(formGroupsByField[fieldName], message));
            const form = fieldsetFormGroup.closest("form");
            if (form) { 
                focusFirstInvalidControl(form);
            }
            return;
        }
        
        options.onAddressSubmitted(validationResult.address);
    }
}

type FieldRuleSet = { fieldName: keyof UkManualEntryValues, validators: Array<{ message: string; isValid: () => boolean }>};
type ErrorMessage = { fieldName: keyof UkManualEntryValues, message: string };
type UkManualEntryValidationResult =
    | { isValid: true; address: UkManualEntryAddress }
    | { isValid: false; errorMessages: ErrorMessage[] };

export function validateUkManualEntryAddress(values: UkManualEntryValues, content: AddressLookupContent): UkManualEntryValidationResult {

    const fieldRuleSets: FieldRuleSet[] = [

        //TODO: Validation messages are too tightly coupled to the validation logic and content. Decouple for better maintainability. 
        //      A validator should define rules of type 'required' 'maxLength' 'pattern etc. A lookup function should then lookup a value in a map by content[`${fieldName}${rule}ErrorMessage`]

        { fieldName: 'addressLine1', validators: [ 
            { message: content.i18n.addressLine1RequiredErrorMessage, isValid: () => values.addressLine1.trim() !== '' },
            { message: content.i18n.addressLine1MaxLengthErrorMessage, isValid: () => values.addressLine1.trim().length <= 100 }
        ]},
        { fieldName: 'addressLine2', validators: [ { message: content.i18n.addressLine2MaxLengthErrorMessage, isValid: () => values.addressLine2.trim().length <= 100 }] },
        { fieldName: 'addressLine3', validators: [ { message: content.i18n.addressLine3MaxLengthErrorMessage, isValid: () => values.addressLine3.trim().length <= 100 }] },
        { fieldName: 'townOrCity', validators: [ 
            { message: content.i18n.postTownRequiredErrorMessage, isValid: () => values.townOrCity.trim() !== '' },
            { message: content.i18n.postTownMaxLengthErrorMessage, isValid: () => values.townOrCity.trim().length <= 100 }
        ]},
        { fieldName: 'county', validators: [ { message: content.i18n.countyMaxLengthErrorMessage, isValid: () => values.county.trim().length <= 100 }] },
        { fieldName: 'postcode', validators: [ 
            { message: content.i18n.postcodeRequiredErrorMessage, isValid: () => values.postcode.trim() !== '' },
            { message: content.i18n.postcodePatternErrorMessage, isValid: () => validatePostcode(values.postcode).isValid },
            { message: content.i18n.postcodeMaxLengthErrorMessage, isValid: () => values.postcode.trim().length <= 10 }
        ]}
    ];

    const errorMessages = fieldRuleSets
        .map(({ fieldName, validators }) => {
            const firstFailure = validators.find( v => !v.isValid());
            return firstFailure ? { fieldName: fieldName, message: firstFailure.message } : null;
        })
        .filter(x => x != null);
    

    if (errorMessages.length > 0) {
        return { isValid: false, errorMessages };
    }

    return {
        isValid: true,
        address: {
            addressLine1: values.addressLine1,
            addressLine2: values.addressLine2,
            addressLine3: values.addressLine3,
            postTown: values.townOrCity,
            county: values.county,
            postcode: values.postcode
        }
    };
}

type UkManualEntryValues = {
    addressLine1: string;
    addressLine2: string;
    addressLine3: string;
    townOrCity: string;
    county: string;
    postcode: string;
}

function readValues(addressLine1: HTMLElement, addressLine2: HTMLElement, addressLine3: HTMLElement, townOrCity: HTMLElement, county: HTMLElement, postcode: HTMLElement): UkManualEntryValues {
    const addressLine1Value = (addressLine1.querySelector('input') as HTMLInputElement).value;
    const addressLine2Value = (addressLine2.querySelector('input') as HTMLInputElement).value;
    const addressLine3Value = (addressLine3.querySelector('input') as HTMLInputElement).value;
    const townOrCityValue = (townOrCity.querySelector('input') as HTMLInputElement).value;
    const countyValue = (county.querySelector('input') as HTMLInputElement).value;
    const postcodeValue = (postcode.querySelector('input') as HTMLInputElement).value;
    const sanitisedPostcode = sanitisePostcode(postcodeValue);
    const normalisedPostcode = normalisePostcode(sanitisedPostcode);

    return {
        addressLine1: addressLine1Value,
        addressLine2: addressLine2Value,
        addressLine3: addressLine3Value,
        townOrCity: townOrCityValue,
        county: countyValue,
        postcode: normalisedPostcode
    };
}
