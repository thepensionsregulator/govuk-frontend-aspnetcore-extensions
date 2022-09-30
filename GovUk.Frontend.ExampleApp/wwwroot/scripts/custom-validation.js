const govuk = createGovUkValidator();

const validator = govuk.getValidator();
govuk.createErrorSummary();

if (validator) {
    validator.setDefaults({
        highlight: govuk.showError,
        unhighlight: govuk.removeOrUpdateError,
    });

    validator.addMethod('custom', function (value, element, params) {
        var prop1 = $("#" + params.property1);
        var prop2 = $("#" + params.property2);

        if (prop1 && prop2) {
            var sum = parseInt(prop1.val()) + parseInt(prop2.val());
            if (sum != value) {
                return false;
            }
        }

        return true;
    });

    validator.unobtrusive.adapters.add("custom", ["property1", "property2"],
        function (options) {
            options.rules['custom'] = options.params;
            options.messages['custom'] = options.message;
        }
    );
    validator.unobtrusive.parse();
}