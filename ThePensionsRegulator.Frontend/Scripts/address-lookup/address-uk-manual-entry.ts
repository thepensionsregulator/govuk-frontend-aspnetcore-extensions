import { normalisePostcode } from './postcode-normaliser.js';
import { sanitisePostcode } from './postcode-sanitiser.js';
import { validatePostcode } from './postcode-validator.js';
import type { UkManualEntryAddress } from './types.js';
import { createButton } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/button.js';
import { createFieldset } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/fieldset.js';
import { createTextInputFormGroup } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/inputs.js';
import { createLink } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/link.js';
import { clearFormGroupError, showFormGroupError } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/validation.js';

export interface AddressUkManualEntryOptions{
    onAddressSubmitted: (address: UkManualEntryAddress) => void;
    onBackToSearchRequested: () => void;
}

export function renderAddressUkManualEntry(options: AddressUkManualEntryOptions): HTMLElement {    
    const div = document.createElement('div');
    const addressLine1 = createTextInputFormGroup({ labelText: 'Address line 1', input: { id: 'address-line-1', name: 'address-line-1', type: "text", width: "x-large" } });
    const addressLine2 = createTextInputFormGroup({ labelText: 'Address line 2 (optional)', input: { id: 'address-line-2', name: 'address-line-2', type: "text", width: "x-large" } });
    const addressLine3 = createTextInputFormGroup({ labelText: 'Address line 3 (optional)', input: { id: 'address-line-3', name: 'address-line-3', type: "text", width: "x-large" } });
    const townOrCity = createTextInputFormGroup({ labelText: 'Town or city', input: { id: 'town-or-city', name: 'town-or-city', type: "text", width: "large" } });
    const county = createTextInputFormGroup({ labelText: 'County (optional)', input: { id: 'county', name: 'county', type: "text", width: "large" } });
    const postcode = createTextInputFormGroup({ labelText: 'Postcode', input: { id: 'postcode', name: 'postcode', type: "text", width: "medium" } });

    const fieldset = createFieldset({ legendText: 'Enter new Uk address', children: [addressLine1, addressLine2, addressLine3, townOrCity, county, postcode] });

    div.appendChild(fieldset);

    const confirmButton = createButton({ labelText: 'Confirm address', type: 'submit', variant: 'secondary' });
    confirmButton.addEventListener("click", handleConfirmAddress);
    div.appendChild(confirmButton);

    const backToSearch = createLink({ labelText: "Back to postcode search"});
    backToSearch.addEventListener("click", handleBackToSearch);
    div.appendChild(backToSearch);

    function handleBackToSearch(event: Event): void {
        event.preventDefault();
        options.onBackToSearchRequested();
    }

    return div;
    
    function handleConfirmAddress(event: Event) {
        event.preventDefault();
        const formGroupsByField: Record<keyof UkManualEntryValues, HTMLElement> = { addressLine1, addressLine2, addressLine3, townOrCity, county, postcode };
        Object.values(formGroupsByField).forEach(clearFormGroupError);
        var validationResult = validateUkManualEntryAddress(readValues(addressLine1, addressLine2, addressLine3, townOrCity, county, postcode));
        if (!validationResult.isValid) {
            validationResult.errorMessages.forEach(({ fieldName, message }) => showFormGroupError(formGroupsByField[fieldName], message));
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

export function validateUkManualEntryAddress(values: UkManualEntryValues): UkManualEntryValidationResult {

    const fieldRuleSets: FieldRuleSet[] = [
        { fieldName: 'addressLine1', validators: [ 
            { message: 'Address line 1 is required', isValid: () => values.addressLine1.trim() !== '' },
            { message: 'Address line 1 must be 100 characters or fewer', isValid: () => values.addressLine1.trim().length <= 100 } 
        ]},
        { fieldName: 'addressLine2', validators: [ { message: 'Address line 2 must be 100 characters or fewer', isValid: () => values.addressLine2.trim().length <= 100 }] },
        { fieldName: 'addressLine3', validators: [ { message: 'Address line 3 must be 100 characters or fewer', isValid: () => values.addressLine3.trim().length <= 100 }] },
        { fieldName: 'townOrCity', validators: [ 
            { message: 'Enter a post town', isValid: () => values.townOrCity.trim() !== '' },
            { message: 'Town or city is required', isValid: () => values.townOrCity.trim() !== '' }, { message: 'Town or city must be 100 characters or fewer', isValid: () => values.townOrCity.trim().length <= 100 }
        ]},
        { fieldName: 'county', validators: [ { message: 'County must be 100 characters or fewer', isValid: () => values.county.trim().length <= 100 }] },
        { fieldName: 'postcode', validators: [ 
            { message: 'Enter a postcode', isValid: () => values.postcode.trim() !== '' },
            { message: 'Enter a real postcode', isValid: () => validatePostcode(values.postcode).isValid },
            { message: 'Postcode must be 10 characters or fewer', isValid: () => values.postcode.trim().length <= 10 }
        ]}
    ];

    const errorMessages = fieldRuleSets
        .map(({ fieldName, validators }) => {
            const firstFailure = validators.find( v => !v.isValid()); // Run the validators foreach form group until one fails
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
