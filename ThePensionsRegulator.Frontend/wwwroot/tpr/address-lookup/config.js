const ADDRESS_LOOKUP_CONFIG = {
    COMPONENT_SELECTOR: ".tpr-address-lookup",
    ATTRIBUTES: {
        API_ENDPOINT: "data-address-lookup-url",
        BASE: "data-address-lookup"
    },
    DATA_ATTRIBUTES: {
        BUILDING_INPUT: "building",
        POSTCODE: "postcode",
        FIND_ADDRESS: "find-address",
        CONFIRM_BUTTON: "confirm-button",
        SELECT_ADDRESS: "select-address",
        RETURN_TO_POSTCODE: "return-to-postcode",
        ADDRESS_NOT_ON_LIST: "address-not-on-list",
        LINK_LIST: "link-list",
        EDIT_ADDRESS: "edit-address"
    },

    CSS_CLASSES: {
        FORM_GROUP: "govuk-form-group",
        LABEL: "govuk-label",
        INPUT: "govuk-input",
        INPUT_WIDTH_10: "govuk-input--width-10",
        INPUT_WIDTH_20: "govuk-input--width-20",
        SELECT: "govuk-select",
        BUTTON_SECONDARY: "govuk-button govuk-button--secondary",
        LINK: "govuk-link",
        LINK_LIST: "govuk-list tpr-address-lookup__links",
        BODY: "govuk-body"
    },

    LABELS: {
        BUILDING_NAME: "Building name",
        POSTCODE: "Postcode",
        FIND_ADDRESS_BUTTON: "Find address",
        CONFIRM_ADDRESS_BUTTON: "Confirm address",
        SELECT_ADDRESS_PLACEHOLDER: ""
    },

    LINK_TEXT: {
        EDIT_ADDRESS: "Edit address",
        ENTER_ADDRESS_NOT_ON_LIST: "Enter address not on list",
        RETURN_TO_POSTCODE_SEARCH: "Return to postcode search"
    },

    ERROR_MESSAGES: {
        SELECT_REQUIRED: "Select an address",
        REQUIRED: "This field is required",
        INVALID_POSTCODE: "Enter a valid UK postcode"
    },

    FIELD_NAMES: {
        SELECT_ADDRESS: "Find-address"
    },

    VALIDATION: {
        POSTCODE_REQUIRED: true,
        SELECT_REQUIRED: true
    }
};

export { ADDRESS_LOOKUP_CONFIG };