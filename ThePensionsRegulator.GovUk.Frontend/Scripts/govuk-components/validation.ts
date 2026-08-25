export function showControlError(control: HTMLInputElement | HTMLSelectElement, errorMessage: string): void {
    const formGroup = control.closest(".govuk-form-group");
    if (!formGroup){
        return;
    }
    formGroup.classList.add("govuk-form-group--error");
    
    if (control instanceof HTMLInputElement) {
        control.classList.add("govuk-input--error");
    }else if (control instanceof HTMLSelectElement) {
        control.classList.add("govuk-select--error");
    }

    let errorMessageElement = formGroup.querySelector<HTMLElement>(".govuk-error-message");
    if (!errorMessageElement){ 
        errorMessageElement = document.createElement("p");
        errorMessageElement.className = "govuk-error-message";
        errorMessageElement.id = `${control.id}-error`;
        
        formGroup.insertBefore(errorMessageElement, control);
    }

    setErrorMessage(errorMessageElement, errorMessage);

    const errorId = `${control.id}-error`;
    const describedBy = control.getAttribute("aria-describedby")?.split(" ") ?? [];
    if(!describedBy.includes(errorId)){
        describedBy.push(errorId);
        control.setAttribute("aria-describedby", describedBy.join(" "));    
    }
}

function setErrorMessage(errorMessageElement: HTMLElement, errorMessage: string): void {
    errorMessageElement.replaceChildren();

    const errorPrefix = document.createElement("span");
    errorPrefix.className = "govuk-visually-hidden";
    errorPrefix.textContent = "Error: ";
    
    errorMessageElement.appendChild(errorPrefix);
    errorMessageElement.appendChild(document.createTextNode(errorMessage));
}

export function clearControlError(control: HTMLInputElement | HTMLSelectElement): void {
    const formGroup = control.closest(".govuk-form-group.govuk-form-group--error");
    if (!formGroup) {
        return;
    }

    formGroup.classList.remove("govuk-form-group--error");
    control.classList.remove("govuk-input--error", "govuk-select--error");

    const errorMessageElement = formGroup.querySelector(".govuk-error-message");
    if (errorMessageElement) {
        errorMessageElement.remove();
    }

    const errorId = `${control.id}-error`;
    const describedBy = control.getAttribute("aria-describedby")?.split(" ") ?? [];
    const updatedDescribedBy = describedBy.filter(id => id !== errorId);
    if (updatedDescribedBy.length > 0) {
        control.setAttribute("aria-describedby", updatedDescribedBy.join(" "));
    } else {
        control.removeAttribute("aria-describedby");
    }
}