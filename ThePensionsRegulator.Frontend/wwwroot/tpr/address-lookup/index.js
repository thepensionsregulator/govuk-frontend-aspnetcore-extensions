import { AddressLookupApiService } from "./api-service.js"; 
import { ADDRESS_LOOKUP_CONFIG } from "./config.js";
import { AddressLookupStateMachine } from "./state-machine.js";
import { AddressLookupComponentBuilder } from "./component-builder.js";
import { AddressLookupValidator } from "./validator.js";
class TprAddressLookup {
    constructor(element, key) {
        this.container = element;
        this.index = key;
        this.searchEndpoint = element.getAttribute("data-address-lookup-search-url");
        this.idEndpoint = element.getAttribute("data-address-lookup-id-url");
        this.stateMachine = new AddressLookupStateMachine();
        this.stateMachine.onChange((newState, data) => this.onStateChange(newState, data));
        this.apiService = new AddressLookupApiService(this.searchEndpoint, this.idEndpoint);
        this.validator = new AddressLookupValidator(element);
        this.componentBuilder = new AddressLookupComponentBuilder(this.index, ADDRESS_LOOKUP_CONFIG);

        this.renderSearchView();
        console.log(`Creating TPR address lookup component with id: ${this.index}`);
    }

    JsonResults = [];

    onStateChange(newState, data) {
        switch (newState) {
            case AddressLookupStateMachine.STATES.SEARCH:
                this.renderSearchView();
                break;
            case AddressLookupStateMachine.STATES.SELECT:
                this.renderSelectView(data.results);
                break;
            case AddressLookupStateMachine.STATES.CONFIRMED:
                this.renderConfirmedView(data);
                break;
            case AddressLookupStateMachine.STATES.MANNUAL_INTERNATIONAL_ENTRY:
                this.renderInternationalManualEntryView();
                break;
            case AddressLookupStateMachine.STATES.MANNUAL_UK_ENTRY:
                this.renderUKManualEntryView();
                break;
        }
    }

    renderSearchView() {
        this.clearContainer();

        this.JsonResults = [];

        const buildingNameGroup = this.componentBuilder.createGovukTextInput(ADDRESS_LOOKUP_CONFIG.LABELS.BUILDING_NAME, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.BUILDING_INPUT, ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.X_LARGE);
        const postcodeGroup = this.componentBuilder.createPostcodeTextInput();
        const findAddressButton = this.componentBuilder.createFindAddressButton((event) => this.findAddressButtonOnClick(event));
        const enterInternationalAddressLink = this.componentBuilder.createLink(ADDRESS_LOOKUP_CONFIG.LINK_TEXT.ENTER_INTERNATIONAL_ADDRESS, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ENTER_INTERNATIONAL_ADDRESS);
        enterInternationalAddressLink.addEventListener("click", (event) => { this.enterInternationalAddressOnClick(event); });
        const linkList = this.componentBuilder.createLinkList([ enterInternationalAddressLink ]);

        this.container.appendChild(buildingNameGroup);
        this.container.appendChild(postcodeGroup);
        this.container.appendChild(findAddressButton);
        this.container.appendChild(linkList);
    }

    renderSelectView(addressResults) {
        this.clearContainer();

        const addressOptions = addressResults.map(result => this.componentBuilder.createOption(result.DPA.UPRN, result.DPA.ADDRESS));
        const selectElement = this.componentBuilder.createAddressSelect(addressOptions);

        const confirmAddressButton = this.componentBuilder.createConfirmAddressButton((event) => this.confirmAddressOnClick(event));

        const enterAddressNotOnListLink = this.componentBuilder.createLink(ADDRESS_LOOKUP_CONFIG.LINK_TEXT.ENTER_ADDRESS_NOT_ON_LIST, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_NOT_ON_LIST);
        enterAddressNotOnListLink.addEventListener("click", event => { this.enterAddressNotOnListOnClick(event); });
        const returnToAddressLookupLink = this.componentBuilder.createLink(ADDRESS_LOOKUP_CONFIG.LINK_TEXT.RETURN_TO_POSTCODE_SEARCH, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.RETURN_TO_POSTCODE);
        returnToAddressLookupLink.addEventListener("click", (event) => this.returnToSearchOnClick(event));

        const linkList = this.componentBuilder.createLinkList([enterAddressNotOnListLink, returnToAddressLookupLink]);

        this.container.appendChild(selectElement);
        this.container.appendChild(confirmAddressButton);
        this.container.appendChild(linkList);
    }

    renderConfirmedView(UPRN) {
        // renderConfirmedView should always be provided with an addres object.
        // If UK manual entry, we will have address line 1, address line 2, town, county, post code
        // If international manual entry, we will have address line 1, address line 2, town, region, country, post code
        // If from the postcode lookup API, we will need to have mapped that data to an object that fits the above format. We don't know how to do that mapping yet.

        this.clearContainer();

        const selectedAddress = this.JsonResults.find(x => x.DPA.UPRN === UPRN);
        console.log(selectedAddress);

        const confirmedAddress = this.componentBuilder.createConfirmedAddressParagraph(selectedAddress.DPA.ADDRESS, selectedAddress.DPA.POSTCODE);

        const editLink = this.componentBuilder.createLink(ADDRESS_LOOKUP_CONFIG.LINK_TEXT.EDIT_ADDRESS, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.EDIT_ADDRESS);
        editLink.addEventListener("click", (event) => this.returnToSearchOnClick(event));
        const editAddressListItem = this.componentBuilder.createListItem(editLink);
        const linkList = this.componentBuilder.createLinkList([editAddressListItem]);

        this.container.appendChild(confirmedAddress);
        this.container.appendChild(linkList);
    }

    renderInternationalManualEntryView() {
        this.clearContainer();
        const addressLine1Input = this.componentBuilder.createGovukTextInputWithValidation(
            ADDRESS_LOOKUP_CONFIG.LABELS.ADDRESS_LINE_1,
            ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_1,
            ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.XX_LARGE,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.REQUIRED,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500
        );

        const addressLine2Input = this.componentBuilder.createGovukTextInputWithValidation(
            ADDRESS_LOOKUP_CONFIG.LABELS.ADDRESS_LINE_2,
            ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_2,
            ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.XX_LARGE,
            undefined,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500
        );

        const townOrCityInput = this.componentBuilder.createGovukTextInputWithValidation(
            ADDRESS_LOOKUP_CONFIG.LABELS.TOWN_OR_CITY,
            ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.TOWN_OR_CITY,
            ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.X_LARGE,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.REQUIRED,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500
        );

        const regionInput = this.componentBuilder.createGovukTextInputWithValidation(
            ADDRESS_LOOKUP_CONFIG.LABELS.REGION_INTERNATIONAL,
            ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.REGION_INTERNATIONAL,
            ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.X_LARGE,
            undefined,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500
        );

        const countryInput = this.componentBuilder.createGovukTextInputWithValidation(
            ADDRESS_LOOKUP_CONFIG.LABELS.COUNTRY,
            ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.COUNTRY,
            ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.X_LARGE,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.REQUIRED,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500
        );

        const postcodeInput = this.componentBuilder.createGovukTextInputWithValidation(
            ADDRESS_LOOKUP_CONFIG.LABELS.POSTCODE_INTERNATIONAL,
            ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.POSTCODE_INTERNATIONAL,
            ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.LARGE,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.REQUIRED,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_20
        );

        const confirmAddressButton = this.componentBuilder.createConfirmAddressButton((event) => this.confirmManualInternationalAddressOnClick(event));
        const returnToAddressLookupLink = this.componentBuilder.createLink(ADDRESS_LOOKUP_CONFIG.LINK_TEXT.RETURN_TO_POSTCODE_SEARCH, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.RETURN_TO_POSTCODE);
        const linkList = this.componentBuilder.createLinkList([returnToAddressLookupLink]);

        this.container.appendChild(addressLine1Input);
        this.container.appendChild(addressLine2Input);
        this.container.appendChild(townOrCityInput);
        this.container.appendChild(regionInput);
        this.container.appendChild(countryInput);
        this.container.appendChild(postcodeInput);
        this.container.appendChild(confirmAddressButton);
        this.container.appendChild(linkList);
    }

    renderUKManualEntryView() {
        this.clearContainer();

        const addressLine1Input = this.componentBuilder.createGovukTextInputWithValidation(
            ADDRESS_LOOKUP_CONFIG.LABELS.ADDRESS_LINE_1,
            ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_1,
            ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.XX_LARGE,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.REQUIRED,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500
        );

        const addressLine2Input = this.componentBuilder.createGovukTextInputWithValidation(
            ADDRESS_LOOKUP_CONFIG.LABELS.ADDRESS_LINE_2,
            ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_2,
            ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.XX_LARGE,
            undefined,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500
        );

        const townOrCityInput = this.componentBuilder.createGovukTextInputWithValidation(
            ADDRESS_LOOKUP_CONFIG.LABELS.TOWN_OR_CITY,
            ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.TOWN_OR_CITY,
            ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.X_LARGE,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.REQUIRED,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500
        );

        const countyInput = this.componentBuilder.createGovukTextInputWithValidation(
            ADDRESS_LOOKUP_CONFIG.LABELS.COUNTRY,
            ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.COUNTRY,
            ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.X_LARGE,
            undefined,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500
        );

        //TODO: Better validation of what a UK postcode is, should use a regex
        const postcodeInput = this.componentBuilder.createGovukTextInputWithValidation(
            ADDRESS_LOOKUP_CONFIG.LABELS.COUNTRY,
            ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.COUNTRY,
            ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.X_LARGE,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.REQUIRED,
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_20
        );

        const confirmAddressButton = this.componentBuilder.createConfirmAddressButton((event) => this.confirmManualUKAddressOnClick(event));
        const returnToAddressLookupLink = this.componentBuilder.createLink(ADDRESS_LOOKUP_CONFIG.LINK_TEXT.RETURN_TO_POSTCODE_SEARCH, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.RETURN_TO_POSTCODE);
        const linkList = this.componentBuilder.createLinkList([returnToAddressLookupLink]);

        this.container.appendChild(addressLine1Input);
        this.container.appendChild(addressLine2Input);
        this.container.appendChild(townOrCityInput);
        this.container.appendChild(countyInput);
        this.container.appendChild(postcodeInput);
        this.container.appendChild(confirmAddressButton);
        this.container.appendChild(linkList);
    }

    getComponentByDataAddressAttribute(attributeValue) {
        return this.container.querySelector(`[data-address-lookup='${attributeValue}']`);
    }

    confirmManualInternationalAddressOnClick(event) {
        event.preventDefault();

        const inputsToValidate = [];
        const addressLine1Input = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_1);
        const addressLine2Input = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_2);
        const townOrCityInput = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.TOWN_OR_CITY);
        const regionInput = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.REGION_INTERNATIONAL);
        const countryInput = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.COUNTRY);
        const postcodeInput = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.POSTCODE_INTERNATIONAL);
        inputsToValidate.push(addressLine1Input, addressLine2Input, townOrCityInput, regionInput, countryInput, postcodeInput);

        const isValid = this.validator.validateMultiple(inputsToValidate);
        if (!isValid) {
            return;
        }

        // Build an address object to pass to the confirmed state.
        this.stateMachine.transition(AddressLookupStateMachine.STATES.CONFIRMED, {});
    }

    confirmManualUKAddressOnClick(event) {
        event.preventDefault();

        const inputsToValidate = [];
        const addressLine1Input = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_1);
        const addressLine2Input = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_2);
        const townOrCityInput = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.TOWN_OR_CITY);
        const county = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.COUNTY);
        const postcode = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.POSTCODE);

        const isValid = this.validator.validateMultiple([addressLine1Input, addressLine2Input, townOrCityInput, county, postcode]);
        if (!isValid) {
            return;
        }

        // Build an address object to pass to the confirmed state.
        this.stateMachine.transition(AddressLookupStateMachine.STATES.CONFIRMED, {});
    }

    confirmAddressOnClick(event) {
        event.preventDefault();

        const selectInput = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.SELECT_ADDRESS);
        const isValid = this.validator.validate(selectInput);
        if (!isValid) {
            return;
        }

        const selectedUPRN = selectInput.value;

        // Build an address object to pass to the confirmed state.
        this.stateMachine.transition(AddressLookupStateMachine.STATES.CONFIRMED, selectedUPRN);
    }

    returnToSearchOnClick(event) {
        event.preventDefault();
        this.stateMachine.transition(AddressLookupStateMachine.STATES.SEARCH);
    }

    enterInternationalAddressOnClick(event) {
        event.preventDefault();
        this.stateMachine.transition(AddressLookupStateMachine.STATES.MANNUAL_INTERNATIONAL_ENTRY);
    }

    enterAddressNotOnListOnClick(event) {
        event.preventDefault();
        this.stateMachine.transition(AddressLookupStateMachine.STATES.MANNUAL_UK_ENTRY);
    }

    clearContainer() {
        while (this.container.firstChild) {
            this.container.removeChild(this.container.firstChild);
        }
    }

    async findAddressButtonOnClick(event) {
        event.preventDefault();

        const buildingInput = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.BUILDING_INPUT);
        const postcodeInput = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.POSTCODE);

        const valid = this.validator.validateMultiple([postcodeInput, buildingInput]);
        if (!valid) {
            return;
        }

        const postcode = postcodeInput.value;
        const building = buildingInput.value;

        const searchResults = await this.apiService.searchAddresses(postcode, building);
        this.JsonResults = searchResults.results;

        if (searchResults.results.length === 1) {
            this.stateMachine.transition(AddressLookupStateMachine.STATES.CONFIRMED, { address: searchResults.results[0].DPA });
        } else if (searchResults.results.length > 1) {
            this.stateMachine.transition(AddressLookupStateMachine.STATES.SELECT, { results: searchResults.results });
        } else {
            // no results found, show error message? Confirm with UX designer
        }
    }
}

document.addEventListener("DOMContentLoaded", function () {
    console.log("TPR Address Lookup script loaded");
    const addressLookupComponents = document.querySelectorAll(ADDRESS_LOOKUP_CONFIG.COMPONENT_SELECTOR);
    addressLookupComponents.forEach((element, key) => {
        new TprAddressLookup(element, key);
    });
});
