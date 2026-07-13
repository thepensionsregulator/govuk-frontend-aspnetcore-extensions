import { govuk as createGovUkValidator } from "../../../../_content/ThePensionsRegulator.GovUk.Frontend/govuk/govuk-validation.js";
class AddressLookupValidator {
    constructor(container, index) {
        this.container = container;
        this.form = container.closest("form");
        this.govuk = createGovUkValidator();
        this.jQueryValidator = this.#initValidator();
        this.index = index;
        this.container.addEventListener("focusout", (event) => {
            const nextFocusedElement = event.relatedTarget;

            const skipsBlurValidation = !!nextFocusedElement?.closest("button[data-address-lookup], button[type='submit'], input[type='submit'], a[data-address-lookup='return-to-postcode'], a[data-address-lookup='address-not-on-list']");
            if (skipsBlurValidation) {
                return;
            }

            const element = event.target;
            if (element.matches("[data-val='true']")) {
                this.validateOnBlur(element);
            }
        });
    }

    #initValidator() {
        let validator = $(this.form).data("validator");

        if (validator) {
            validator.settings.highlight = this.govuk.showError;
            validator.settings.unhighlight = this.govuk.removeOrUpdateError;
        } else {
            validator = $(this.form).validate({
                highlight: this.govuk.showError,
                unhighlight: this.govuk.removeOrUpdateError
            });
        }

        return validator;
    }

    reparse() {
        if (!this.form) return;
        $(this.form).removeData("validator").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse($(this.form));
        this.jQueryValidator = this.#initValidator();
    }

    clearErrors() {
        if (!this.jQueryValidator) return;

        const elementsWithErrors = this.container.querySelectorAll("[data-val='true']");
        elementsWithErrors.forEach(element => {
            this.govuk.removeOrUpdateError(element);
        });
    }

    validateOnBlur(element) {
        const isValid = this.jQueryValidator.element(element);
        if (isValid) {
            this.govuk.removeOrUpdateError(element);
        } else {
            this.govuk.updateError(element, this.jQueryValidator.errorMap[element.name]);
        }
    }

    validate(element) {
        if (!this.form) return true;
        const isValid = this.jQueryValidator.element(element);
        if (!isValid) {
            this.govuk.updateError(element, this.jQueryValidator.errorMap[element.name]);
        }
        return isValid;
    }

    validateMultiple(elements) {
        if (!this.form) return true;
        let allValid = true;
        elements.forEach(element => {
            const isValid = this.jQueryValidator.element(element);
            if (!isValid) {
                this.govuk.updateError(element, this.jQueryValidator.errorMap[element.name]);
                allValid = false;
            }
        });

        return allValid;
    }

    validAddress(address) {
        return !!(address.addressLine1 && address.town && address.postcode);
    }

    removeCustomFieldsetError(fieldset) {
        const formGroup = fieldset.parentElement?.classList.contains("govuk-form-group") ? fieldset.parentElement : null;
        if (formGroup) {
            //formGroup.classList.remove("govuk-form-group--error");
            const errorElement = fieldset.querySelector(".fieldset-error-message");
            if (errorElement) {
                errorElement.remove();
            }
        }
    }

    addOrUpdateCustomFieldsetError(fieldset, message) {
        let formGroup = fieldset.parentElement?.classList.contains("govuk-form-group") ? fieldset.parentElement : null;

        if (!formGroup) {
            formGroup = document.createElement("div");
            formGroup.classList.add("govuk-form-group", "govuk-form-group--error");

            fieldset.parentNode.insertBefore(formGroup, fieldset);
            formGroup.appendChild(fieldset);
        }

        let errorElement = fieldset.querySelector(".fieldset-error-message");
        if (!errorElement) {
            const errorParaSpan = document.createElement("span");
            errorParaSpan.classList.add("govuk-visually-hidden");
            errorParaSpan.innerText = "Error: ";

            errorElement = document.createElement("p");
            errorElement.classList.add("govuk-error-message", "fieldset-error-message");

            const firstInput = fieldset.querySelector("input");
            const errorElementId = `${firstInput.id}-error`
            errorElement.id = errorElementId;

            errorElement.appendChild(errorParaSpan);

            const legend = fieldset.querySelector("legend");
            if (legend) {
                legend.insertAdjacentElement("afterend", errorElement);
            }
            fieldset.setAttribute("aria-describedby", errorElementId);
        }

        const hiddenSpan = errorElement.querySelector(".govuk-visually-hidden");
        if (hiddenSpan.nextSibling) {
            hiddenSpan.nextSibling.remove();
        }

        if (message) {
            errorElement.appendChild(document.createTextNode(message));
        }

        this.govuk.updateErrorSummary();
    }

    addSelectError(selectElement, message) {
        const formgroup = this.govuk.formGroupForElement(selectElement);
        formgroup.classList.add("govuk-form-group--error");

        selectElement.classList.add("govuk-select--error");

        this.govuk.updateError(selectElement, message);
    }

    focusInvalid() {
        const validator = $(this.form).data("validator");
        if (validator) {
            validator.focusInvalid();
        }
    }
}

export { AddressLookupValidator };