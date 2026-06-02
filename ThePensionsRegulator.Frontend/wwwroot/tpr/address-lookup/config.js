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
        COUNTRIES: "countries",
        COUNTRY_CODE: "country-code",
        LINK_LIST: "link-list",
        UPRN: "UPRN",
        EDIT_ADDRESS: "edit-address",
        SAME_AS: "same-as",
        CHECKBOX_LABEL: "same-as-primary-checkbox-label"
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
        LEGEND: "govuk-fieldset__legend govuk-fieldset__legend--s",
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
        ADDRESS_CONFIRMED: "Confirmed address",
        CONFIRM_ADDRESS_BUTTON: "Confirm address",
        ENTER_NEW_INTERNATIONAL_ADDRESS: "Enter new international address",
        ENTER_NEW_UK_ADDRESS: "Enter new UK address",
        SEARCH_BY_POSTCODE: "Search by postcode",
        FIND_ADDRESS_BUTTON: "Find address",
    },

    LINK_TEXT: {
        EDIT_ADDRESS: "Change address",
        ENTER_ADDRESS_NOT_ON_LIST: "Enter address not on list",
        ENTER_INTERNATIONAL_ADDRESS: "Enter an international address",
        RETURN_TO_POSTCODE_SEARCH: "Return to postcode search"
    },

    ERROR_MESSAGES: {
        INVALID_COUNTRY: "This is not a valid country",
        COUNTRY_NOT_UK: "This page is for international addresses",
        ADDRESS_POSTCODE_DONT_MATCH: "The address and postcode do not match",
        NO_ADDRESS_AT_POSTCODE: "There are no addresses registered at this postcode",
        "select-confirm": "Select confirm address before saving this page",
        SELECT_FIND_ADDRESS: "Select 'find an address' and confirm your address before saving this page"
    },

    FIELD_NAMES: {
        SELECT_ADDRESS: "Find-address"
    },

    VALIDATION: {
        POSTCODE_REQUIRED: true,
        SELECT_REQUIRED: true
    },
    DEFAULTS: {
        COUNTRY: 'United Kingdom'
    },
    FIELD_DEFAULTS: {
        "address-line-1": {
            label: "Address line 1",
            width: "xx-large",
            validation: {
                required: { message: "Enter address line 1" },
                maxLength: { value: 100, message: "Address line 1 must be 100 characters or less" }
            }
        },
        "address-line-2": {
            label: "Address line 2 (optional)",
            width: "xx-large",
            validation: {
                maxLength: { value: 100, message: "Address line 2 must be 100 characters or less"}
            }
        },
        "town-or-city": {
            label: "Town or city",
            width: "x-large",
            validation: {
                required: { message: "Enter a town or city" },
                maxLength: { value: 100, message: "Town or city must be 100 characters or less" }
            }
        },
        "region-international": {
            label: "Province/region/state (optional)",
            width: "x-large",
            validation: {
                maxLength: { value: 100, message: "Province/region/state must be 100 characters or less" }
            }
        },
        "county": {
            label: "County (optional)",
            width: "x-large",
            validation: {
                maxLength: { value: 100, message: "County must be 100 characters or less" }
            }
        },
        "country-code": {
            label: "Country",
            width: "x-large",
            validation: {
                required: { message: "Enter a country" },
                maxLength: { value: 100, message: "Country must be 100 characters or less" }
            }
        },
        "postcode-international": {
            label: "Postal code/zip code",
            width: "large",
            validation: {
                required: { message: "Enter a postal code/zip code" },
                maxLength: { value: 10, message: "Postal code/zip code must be 10 characters or less" }
            }
        },
        "postcode": {
            label: "Postcode",
            width: "large",
            validation: {
                required: { message: "Enter a postcode" },
                maxLength: { value: 10, message: "Postcode must be 10 characters or less" },
                pattern: {
                    value: "^\\s*[A-Za-z]{1,2}[0-9]{1,2}[A-Za-z]?[\\s\\-]*[0-9][ABDEFGHJLNPQRSTUWXYZabdefghjlnpqrstuwxyz]{2}\\s*$",
                    message: "Enter a valid UK postcode"
                }
            }
        },
        "building": {
            label: "Building or house number (optional)",
            width: "x-large",
            validation: {
                maxLength: { value: 100, message: "Building or house number must be 100 characters or less" }
            }
        },
        "select-address": {
            label: "Choose an address",
            labelSize: "govuk-label--m",
            validation: {
                required: { message: "Select an address" }
            }
        }
    }
};

export { ADDRESS_LOOKUP_CONFIG };