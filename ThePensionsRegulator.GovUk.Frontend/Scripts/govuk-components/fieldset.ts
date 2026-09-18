import { FieldsetFormGroupOptions, FieldsetOptions } from "./types";

export function createFieldset(fieldsetOptions: FieldsetOptions): HTMLFieldSetElement {
    const fieldset = document.createElement("fieldset");
    fieldset.className = "govuk-fieldset";

    if (fieldsetOptions.attributes) {
        for (const [name, value] of Object.entries(fieldsetOptions.attributes)) {
            fieldset.setAttribute(name, value);
        }
    }
    
    const legend = document.createElement("legend");
    legend.className = "govuk-fieldset__legend";
    legend.textContent = fieldsetOptions.legendText;
    
    if (fieldsetOptions.legendSize) {
        legend.classList.add(legendSizeClassMap[fieldsetOptions.legendSize]);
    }

    fieldset.appendChild(legend);

    fieldsetOptions.children.forEach(child => {
        fieldset.appendChild(child);
    });

    return fieldset;
}

export function createFieldsetFormGroup(fieldsetOptions: FieldsetFormGroupOptions): HTMLElement {
    const formGroup = document.createElement("div");
    formGroup.className = "govuk-form-group";

    const fieldset = createFieldset(fieldsetOptions);
    fieldset.id = fieldsetOptions.id;
    formGroup.appendChild(fieldset);

    return formGroup;
}

const legendSizeClassMap: Record<string, string> = {
    "small": "govuk-fieldset__legend--s",
    "medium": "govuk-fieldset__legend--m",
    "large": "govuk-fieldset__legend--l",
    "x-large": "govuk-fieldset__legend--xl"
};