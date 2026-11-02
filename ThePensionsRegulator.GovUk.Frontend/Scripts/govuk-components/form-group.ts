import { FormGroupOptions } from "./types";

export function createFormGroup(formGroupOptions: FormGroupOptions): HTMLElement {
    const div = document.createElement("div");
    div.className = "govuk-form-group";

    div.appendChild(formGroupOptions.label);
    div.appendChild(formGroupOptions.control);

    return div;
}