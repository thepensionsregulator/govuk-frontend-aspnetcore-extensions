class AddressLookupValidator {
    constructor(container) {
        this.form = container.closest("form");
    }

    validate(element) {
        if (!this.form) return true;
        const validator = $(this.form).validate();
        return validator.element(element);
    }

    validateMultiple(elements) {
        if (!this.form) return true;
        const validator = $(this.form).validate();
        return elements.every(element => validator.element(element));
    }
}

export { AddressLookupValidator };