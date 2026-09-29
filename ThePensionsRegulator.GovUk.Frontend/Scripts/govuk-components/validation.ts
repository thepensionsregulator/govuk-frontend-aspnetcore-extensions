import { ensureErrorSummary, updateErrorSummary, updateTitle } from "./error-summary.js";

type ValidatableElement = 
    | HTMLInputElement 
    | HTMLSelectElement 
    | HTMLFieldSetElement;

function getValidatableElement(formGroup: HTMLElement) : ValidatableElement | null {
    for (const child of formGroup.children) {
        if (
            child instanceof HTMLInputElement || 
            child instanceof HTMLSelectElement || 
            child instanceof HTMLFieldSetElement
        ) {
            return child;
        }
    }
    return null;
}

export function showFormGroupError(formGroup: HTMLElement, errorMessage: string): void {
    formGroup.classList.add("govuk-form-group--error");

    const validatableElement = getValidatableElement(formGroup);
    if (validatableElement instanceof HTMLFieldSetElement) {
        showFieldsetError(formGroup, validatableElement, errorMessage);
    }else{
        showControlError(formGroup, validatableElement as HTMLInputElement | HTMLSelectElement, errorMessage);
    }
}


function showControlError(formGroup: HTMLElement, control: HTMLInputElement | HTMLSelectElement, errorMessage: string): void {
    if (control instanceof HTMLInputElement) {
        control.classList.add("govuk-input--error");
    }else if (control instanceof HTMLSelectElement) {
        control.classList.add("govuk-select--error");
    }

    const errorId = `${control.id}-error`;
    let errorMessageElement = formGroup.querySelector<HTMLElement>(`#${errorId}`);
    if (!errorMessageElement){ 
        errorMessageElement = document.createElement("p");
        errorMessageElement.className = "govuk-error-message";
        errorMessageElement.id = errorId;
        
        control.insertAdjacentElement("beforebegin", errorMessageElement);
    }

    setErrorMessage(errorMessageElement, errorMessage);
    addDescriptionId(control, errorId);
    setFieldsetFormGroupError(control);

    ensureErrorSummary();
    updateErrorSummary();
    updateTitle();
}

function showFieldsetError(formGroup: HTMLElement, fieldset: HTMLFieldSetElement, errorMessage: string): void {
    const errorId = `${fieldset.id}-error`;
    formGroup.classList.add("govuk-form-group--error");

    let errorMessageElement = formGroup.querySelector<HTMLElement>(`#${errorId}`);
    if (!errorMessageElement) {
        errorMessageElement = document.createElement("p");
        errorMessageElement.className = "govuk-error-message";
        errorMessageElement.id = errorId;

        const legend = fieldset.querySelector("legend");
        legend?.insertAdjacentElement("afterend", errorMessageElement);
    }

    setErrorMessage(errorMessageElement, errorMessage);
    addDescriptionId(fieldset, errorId);

    ensureErrorSummary();
    updateErrorSummary();
    updateTitle();
}

function setErrorMessage(errorMessageElement: HTMLElement, errorMessage: string): void {
    errorMessageElement.replaceChildren();

    const errorPrefix = document.createElement("span");
    errorPrefix.className = "govuk-visually-hidden";
    errorPrefix.textContent = "Error: ";
    
    errorMessageElement.appendChild(errorPrefix);
    errorMessageElement.appendChild(document.createTextNode(errorMessage));
}

function addDescriptionId(element: HTMLElement, descriptionId: string): void {
    const describedBy = element
        .getAttribute("aria-describedby")
        ?.split(" ")
        .filter(Boolean) ?? [];

    if (!describedBy.includes(descriptionId)) {
        describedBy.push(descriptionId);
        element.setAttribute("aria-describedby", describedBy.join(" "));
    }
}

export function clearFormGroupError(formGroup: HTMLElement): void {
    const validatableElement = getValidatableElement(formGroup);

    if (validatableElement instanceof HTMLFieldSetElement) {
        clearFieldsetError(formGroup, validatableElement);
    } else {
        clearControlError(formGroup, validatableElement as HTMLInputElement | HTMLSelectElement);
    }
}

function clearFieldsetError(formGroup: HTMLElement, fieldset: HTMLFieldSetElement): void {
    formGroup.classList.remove("govuk-form-group--error");
    fieldset.classList.remove("govuk-fieldset--error");

    removeErrorMessage(formGroup, fieldset.id);
    removeDescriptionId(fieldset, fieldset.id + "-error");

    updateErrorSummary();
    updateTitle();
}

function clearControlError(formGroup: HTMLElement, control: HTMLInputElement | HTMLSelectElement): void {
    formGroup.classList.remove("govuk-form-group--error");
    control.classList.remove("govuk-input--error", "govuk-select--error");

    removeErrorMessage(formGroup, control.id);

    removeDescriptionId(control, control.id + "-error");
    clearFieldsetFormGroupError(control);

    updateErrorSummary();
    updateTitle();
}

function removeErrorMessage(formGroup: HTMLElement, validatableElementId: string): void {
    const errorMessageElement = formGroup.querySelector(`#${validatableElementId}-error`);
    if (errorMessageElement) {
        errorMessageElement.remove();
    }
}

function setFieldsetFormGroupError(control: HTMLInputElement | HTMLSelectElement): void {
    const fieldset = control.closest("fieldset");
    const fieldsetFormGroup = fieldset?.parentElement;

    if (fieldsetFormGroup?.classList.contains("govuk-form-group")) {
        fieldsetFormGroup.classList.add("govuk-form-group--error");
    }
}

function clearFieldsetFormGroupError(control: HTMLInputElement | HTMLSelectElement): void {
    const fieldset = control.closest("fieldset");
    const fieldsetFormGroup = fieldset?.parentElement;

    if (
        fieldsetFormGroup?.classList.contains("govuk-form-group") &&
        !fieldset?.querySelector(".govuk-form-group--error")
    ) {
        fieldsetFormGroup.classList.remove("govuk-form-group--error");
    }
}

function removeDescriptionId(element: HTMLElement, descriptionId: string): void {
    const describedBy = element.getAttribute("aria-describedby")?.split(" ") ?? [];
    const updatedDescribedBy = describedBy.filter(id => id !== descriptionId);
    if (updatedDescribedBy.length > 0) {
        element.setAttribute("aria-describedby", updatedDescribedBy.join(" "));
    } else {
        element.removeAttribute("aria-describedby");
    }
}