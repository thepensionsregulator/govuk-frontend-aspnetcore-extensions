import { normalisePostcode } from './postcode-normaliser.js';
import { sanitisePostcode } from './postcode-sanitiser.js';
import { validatePostcode } from './postcode-validator.js';
import { submitOnEnter } from './submit-on-enter.js';
import type { AddressLookupContent, UkManualEntryAddress } from './types.js';
import { createButton } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/button.js';
import { createFieldsetFormGroup } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/fieldset.js';
import { createTextInputFormGroup } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/inputs.js';
import { createLink } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/link.js';
import { createParagraph } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/paragraph.js';
import { clearFormGroupError, showFormGroupError } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/validation.js';

export interface AddressUkManualEntryOptions {
    content: AddressLookupContent
    onAddressSubmitted: (address: UkManualEntryAddress) => void;
    onBackToSearchRequested: () => void;
}

export function renderAddressUkManualEntry(options: AddressUkManualEntryOptions): HTMLElement {    
    const div = document.createElement('div');
    const addressLine1 = createTextInputFormGroup({ labelText: options.content.addressLine1Label, input: { id: 'address-line-1', name: 'address-line-1', type: "text", width: "x-large" } });
    const addressLine2 = createTextInputFormGroup({ labelText: options.content.addressLine2Label, input: { id: 'address-line-2', name: 'address-line-2', type: "text", width: "x-large" } });
    const addressLine3 = createTextInputFormGroup({ labelText: options.content.addressLine3Label, input: { id: 'address-line-3', name: 'address-line-3', type: "text", width: "x-large" } });
    const townOrCity = createTextInputFormGroup({ labelText: options.content.postTownLabel, input: { id: 'town-or-city', name: 'town-or-city', type: "text", width: "large" } });
    const county = createTextInputFormGroup({ labelText: options.content.countyLabel, input: { id: 'county', name: 'county', type: "text", width: "large" } });
    const postcode = createTextInputFormGroup({ labelText: options.content.postcodeLabel, input: { id: 'postcode', name: 'postcode', type: "text", width: "medium" } });

    const fieldsetFormGroup = createFieldsetFormGroup({
        id: "address-uk-manual-entry-fieldset",
        legendText: options.content.ukManualEntryLegend,
        children: [addressLine1, addressLine2, addressLine3, townOrCity, county, postcode]
    });
    fieldsetFormGroup.setAttribute("data-address-lookup-submit-error-anchor", "true");
    div.appendChild(fieldsetFormGroup);

    const confirmButton = createButton({ labelText: options.content.confirmAddressLabel, type: 'button', variant: 'secondary' });
    submitOnEnter(div, () => confirmButton.click());
    confirmButton.addEventListener("click", handleConfirmAddress);
    div.appendChild(confirmButton);

    const backToSearch = createLink({ labelText: options.content.backToPostcodeSearchLabel});
    backToSearch.addEventListener("click", handleBackToSearch);
    const p = createParagraph();
    p.appendChild(backToSearch);
    div.appendChild(p);

    function handleBackToSearch(event: Event): void {
        event.preventDefault();
        options.onBackToSearchRequested();
    }

    return div;
    
    function handleConfirmAddress(event: Event) {
        event.preventDefault();
        const formGroupsByField: Record<keyof UkManualEntryValues, HTMLElement> = { addressLine1, addressLine2, addressLine3, townOrCity, county, postcode };
        Object.values(formGroupsByField).forEach(clearFormGroupError);
        const validationResult = validateUkManualEntryAddress(readValues(addressLine1, addressLine2, addressLine3, townOrCity, county, postcode), options.content);
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

export function validateUkManualEntryAddress(values: UkManualEntryValues, content: AddressLookupContent): UkManualEntryValidationResult {

    const fieldRuleSets: FieldRuleSet[] = [

        //TODO: Validation messages are too tightly coupled to the validation logic and content. Decouple for better maintainability. 
        //      A validator should define rules of type 'required' 'maxLength' 'pattern etc. A lookup function should then lookup a value in a map by content[`${fieldName}${rule}ErrorMessage`]

        { fieldName: 'addressLine1', validators: [ 
            { message: content.addressLine1RequiredErrorMessage, isValid: () => values.addressLine1.trim() !== '' },
            { message: content.addressLine1MaxLengthErrorMessage, isValid: () => values.addressLine1.trim().length <= 100 } 
        ]},
        { fieldName: 'addressLine2', validators: [ { message: content.addressLine2MaxLengthErrorMessage, isValid: () => values.addressLine2.trim().length <= 100 }] },
        { fieldName: 'addressLine3', validators: [ { message: content.addressLine3MaxLengthErrorMessage, isValid: () => values.addressLine3.trim().length <= 100 }] },
        { fieldName: 'townOrCity', validators: [ 
            { message: content.postTownRequiredErrorMessage, isValid: () => values.townOrCity.trim() !== '' },
            { message: content.postTownMaxLengthErrorMessage, isValid: () => values.townOrCity.trim().length <= 100 }
        ]},
        { fieldName: 'county', validators: [ { message: content.countyMaxLengthErrorMessage, isValid: () => values.county.trim().length <= 100 }] },
        { fieldName: 'postcode', validators: [ 
            { message: content.postcodeRequiredErrorMessage, isValid: () => values.postcode.trim() !== '' },
            { message: content.postcodePatternErrorMessage, isValid: () => validatePostcode(values.postcode).isValid },
            { message: content.postcodeMaxLengthErrorMessage, isValid: () => values.postcode.trim().length <= 10 }
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
