import { AddressLookupContent } from './types';
import { createCheckbox, createCheckboxesFormGroup } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/inputs.js';

export function renderSameAsCheckbox(content: AddressLookupContent, checked: boolean, sameAsCheckboxOnClick?: (event: Event) => void) : HTMLElement {
    const checkbox = createCheckbox({
        name: "same-as",
        labelText: content.sameAsLabel || "Same as above",
        onClick: sameAsCheckboxOnClick
    });
    const input = checkbox.querySelector<HTMLInputElement>("input[type='checkbox']");
    const checkboxGroup = createCheckboxesFormGroup({
        checkboxes: [ checkbox ]
    });

    if (input) {
        input.checked = checked;
    }

    return checkboxGroup;
}


