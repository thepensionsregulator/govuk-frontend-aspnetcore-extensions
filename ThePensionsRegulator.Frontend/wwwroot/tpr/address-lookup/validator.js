import { govuk as createGovUkValidator } from "../../../../_content/ThePensionsRegulator.GovUk.Frontend/govuk/govuk-validation.js";
class AddressLookupValidator {
    constructor(container) {
        this.container = container;
        this.form = container.closest("form");
        this.govuk = createGovUkValidator();
        this.jQueryValidator = this.#initValidator();

        this.container.addEventListener("focusout", (event) => {
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
}

export { AddressLookupValidator };