class InputBuilder {
    constructor(formGroup) {
        this.formGroup = formGroup;
        this.input = formGroup.querySelector("input") || formGroup.querySelector("select");
    }

    addRequiredValidation(errorMessage) {
        this.input.setAttribute("data-val", "true");
        this.input.setAttribute("required", "required");
        this.input.setAttribute("data-val-required", errorMessage);

        return this;
    }

    addMaxLengthValidation(maxLength, errorMessage) {
        this.input.setAttribute("data-val", "true");
        this.input.setAttribute("data-val-maxlength-max", maxLength);
        this.input.setAttribute("maxlength", maxLength);
        this.input.setAttribute("data-val-maxlength", errorMessage);

        return this;
    }

    addPatternValidation(pattern, errorMessage) {
        this.input.setAttribute("data-val", "true");
        this.input.setAttribute("data-val-regex", errorMessage);
        this.input.setAttribute("data-val-regex-pattern", pattern);
        this.input.setAttribute("pattern", pattern);

        return this;
    }

    build() {
        return this.formGroup;
    }
}

export { InputBuilder };