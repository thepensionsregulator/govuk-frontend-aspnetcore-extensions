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
        ENTER_INTERNATIONAL_ADDRESS: "enter-international-address",
        ADDRESS_LINE_1: "address-line-1",
        ADDRESS_LINE_2: "address-line-2",
        TOWN_OR_CITY: "town-or-city",
        COUNTY: "county",
        REGION_INTERNATIONAL: "region-international",
        POSTCODE_INTERNATIONAL: "postcode-international",
        COUNTRY: "country",
        LINK_LIST: "link-list",
        UPRN: "UPRN",
        EDIT_ADDRESS: "edit-address",
        SAME_AS: "same-as"
    },

    CSS_CLASSES: {
        FORM_GROUP: "govuk-form-group",
        LABEL: "govuk-label",
        INPUT: "govuk-input",
        INPUT_WIDTH_20: "govuk-input--width-20",
        SELECT: "govuk-select",
        BUTTON_SECONDARY: "govuk-button govuk-button--secondary",
        LINK: "govuk-link",
        LINK_LIST: "govuk-list tpr-address-lookup__links",
        BODY: "govuk-body",
        FIELDSET: "govuk-fieldset",
        LEGEND: "govuk-fieldset__legend govuk-fieldset__legend--for-fieldset",
    },

    INPUT_WIDTHS: {
        XX_SMALL: "xx-small",
        X_SMALL: "x-small",
        SMALL: "small",
        MEDIUM: "medium",
        LARGE: "large",
        X_LARGE: "x-large",
        XX_LARGE: "xx-large"
    },

    LABELS: {
        BUILDING_NAME: "Building or house number",
        CHOOSE_AN_ADDRESS: "Choose an address",
        CONFIRM_ADDRESS_BUTTON: "Confirm address",
        ENTER_NEW_INTERNATIONAL_ADDRESS: "Enter new international address",
        ENTER_NEW_UK_ADDRESS: "Enter new UK address",
        POSTCODE: "Postcode",
        SEARCH_BY_POSTCODE: "Search by postcode",
        FIND_ADDRESS_BUTTON: "Find address",
        SELECT_ADDRESS_PLACEHOLDER: "",
        ADDRESS_LINE_1: "Address line 1",
        ADDRESS_LINE_2: "Address line 2",
        TOWN_OR_CITY: "Town or city",
        REGION_INTERNATIONAL: "Province/region/state (optional)",
        COUNTY: "County",
        COUNTRY: "Country",
        POSTCODE_INTERNATIONAL: "Postal code/zip code",

    },

    LINK_TEXT: {
        EDIT_ADDRESS: "Edit address",
        ENTER_ADDRESS_NOT_ON_LIST: "Enter address not on list",
        ENTER_INTERNATIONAL_ADDRESS: "Enter an international address",
        RETURN_TO_POSTCODE_SEARCH: "Return to postcode search"
    },

    ERROR_MESSAGES: {
        SELECT_REQUIRED: "Select an address",
        MAX_LENGTH_500: "Your answer must not exceed 500 characters",
        MAX_LENGTH_100: "Must be no more than 100 characters",
        MAX_LENGTH_20: "Your answer must not exceed 20 characters",
        INVALID_COUNTRY: "This is not a valid country",
        COUNTRY_NOT_UK: "This page is for international addresses",
        REQUIRED: "This field must be populated",
        INVALID_POSTCODE: "Enter a valid UK postcode",
        ADDRESS_POSTCODE_DONT_MATCH: "The address and postcode do not match",
        NO_ADDRESS_AT_POSTCODE: "There are no addresses registered at this postcode"
    },

    FIELD_NAMES: {
        SELECT_ADDRESS: "Find-address"
    },

    VALIDATION: {
        POSTCODE_REQUIRED: true,
        SELECT_REQUIRED: true
    },
    PATTERNS: {
        POSTCODE: "^\\s*[A-Za-z]{1,2}[0-9]{1,2}[A-Za-z]?[\\s\\-]*[0-9][ABDEFGHJLNPQRSTUWXYZabdefghjlnpqrstuwxyz]{2}\\s*$"
    }
};

export { ADDRESS_LOOKUP_CONFIG };