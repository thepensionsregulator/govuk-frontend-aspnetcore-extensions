import { AddressLookupApiService } from "./api-service.js"; 
import { ADDRESS_LOOKUP_CONFIG } from "./config.js";
import { AddressLookupStateMachine } from "./state-machine.js";
import { AddressLookupComponentBuilder } from "./component-builder.js";
import { AddressLookupValidator } from "./validator.js";
import { AddressMapper } from "./mapper.js";

class TprAddressLookup {
    constructor(element, key, role, primaryStateMachine) {
        this.container = element;
        this.index = key;
        this.role = role;
        this.primaryStateMachine = primaryStateMachine;
        this.searchEndpoint = element.getAttribute("data-address-lookup-search-url");
        this.idEndpoint = element.getAttribute("data-address-lookup-id-url");
        this.apiService = new AddressLookupApiService(this.searchEndpoint, this.idEndpoint);
        this.validator = new AddressLookupValidator(element, this.index);
        this.componentBuilder = new AddressLookupComponentBuilder(this.index, ADDRESS_LOOKUP_CONFIG);
        this.addressMapper = new AddressMapper(ADDRESS_LOOKUP_CONFIG);
        this.stateMachine = new AddressLookupStateMachine();
        this.stateMachine.onChange((newState, data) => this.onStateChange(newState, data));

        this.originalInputs = this.captureOriginalInputs();

        const address = this.addressMapper.mapFromInput(this.originalInputs);

        while (this.container.firstChild) {
            this.container.removeChild(this.container.firstChild);
        }

        if (this.role === "secondary") {
            this.sameAsCheckbox = this.componentBuilder.createCheckbox("Same as primary address");
            this.sameAsCheckbox.querySelector("input[type='checkbox']")
                .addEventListener("change", (event) => this.onSameAsCheckboxChange(event));
            this.container.appendChild(this.sameAsCheckbox);

            this.primaryStateMachine.onChange((newState, data) => this.onPrimaryStateChange(newState, data));
        }

        this.stateContainer = document.createElement("div");
        this.container.appendChild(this.stateContainer);

        if (this.validator.validAddress(address)) { 
            this.stateMachine.transition(AddressLookupStateMachine.STATES.CONFIRMED, { address });
        } else {
            this.stateMachine.transition(AddressLookupStateMachine.STATES.SEARCH);
        }
    }

    JsonResults = [];

    onPrimaryStateChange(newState, data) {
        if (newState === AddressLookupStateMachine.STATES.CONFIRMED && this.isSameAsChecked()) {
            this.confirmWithAddress(data.address);
        }
    }

    onStateChange(newState, data) {
        switch (newState) {
            case AddressLookupStateMachine.STATES.SEARCH:
                this.renderSearchView();
                break;
            case AddressLookupStateMachine.STATES.SELECT:
                this.renderSelectView(data.results);
                break;
            case AddressLookupStateMachine.STATES.CONFIRMED:
                this.renderConfirmedView(data.address);
                break;
            case AddressLookupStateMachine.STATES.MANNUAL_INTERNATIONAL_ENTRY:
                this.renderInternationalManualEntryView();
                break;
            case AddressLookupStateMachine.STATES.MANNUAL_UK_ENTRY:
                this.renderUKManualEntryView();
                break;
        }
    }

    captureOriginalInputs() {
        const inputs = this.container.querySelectorAll("input");
        return Array.from(inputs).map(input => ({
            name: input.getAttribute("name"),
            id: input.getAttribute("id"),
            dataAddressLookup: input.getAttribute(ADDRESS_LOOKUP_CONFIG.ATTRIBUTES.BASE),
            value: input.value
        }));
    }

    renderSearchView() {
        this.clearContainer();

        this.JsonResults = [];

        const fieldset = this.componentBuilder.createFieldset(ADDRESS_LOOKUP_CONFIG.LABELS.SEARCH_BY_POSTCODE);
        const buildingNameGroup = this.componentBuilder.createGovukTextInput(ADDRESS_LOOKUP_CONFIG.LABELS.BUILDING_NAME, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.BUILDING_INPUT, ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.X_LARGE)
            .addMaxLengthValidation(100, ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_100)
            .build();
        const postcodeGroup = this.componentBuilder.createGovukTextInput(ADDRESS_LOOKUP_CONFIG.LABELS.POSTCODE, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.POSTCODE, ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.LARGE)
            .addRequiredValidation(ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.REQUIRED)
            .addPatternValidation(ADDRESS_LOOKUP_CONFIG.PATTERNS.POSTCODE, ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.INVALID_POSTCODE)
            .build();
        const findAddressButton = this.componentBuilder.createFindAddressButton((event) => this.findAddressButtonOnClick(event));
        const enterInternationalAddressLink = this.componentBuilder.createLink(ADDRESS_LOOKUP_CONFIG.LINK_TEXT.ENTER_INTERNATIONAL_ADDRESS, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ENTER_INTERNATIONAL_ADDRESS);
        enterInternationalAddressLink.addEventListener("click", (event) => { this.enterInternationalAddressOnClick(event); });
        const linkList = this.componentBuilder.createLinkList([ enterInternationalAddressLink ]);


        const fieldsetChildrenFormGroup = document.createElement("div");
        fieldsetChildrenFormGroup.classList.add("govuk-form-group");

        fieldsetChildrenFormGroup.appendChild(buildingNameGroup);
        fieldsetChildrenFormGroup.appendChild(postcodeGroup);
        fieldset.appendChild(fieldsetChildrenFormGroup);

        this.stateContainer.appendChild(fieldset);
        this.stateContainer.appendChild(findAddressButton);
        this.stateContainer.appendChild(linkList);

        this.validator.govuk.updateErrorSummary();
        this.validator.reparse();
    }

    renderSelectView(addressResults) {
        this.clearContainer();

        const addressOptions = addressResults.map(result => this.componentBuilder.createOption(result.UPRN, result.ADDRESS));
        const selectElement = this.componentBuilder.createAddressSelect(ADDRESS_LOOKUP_CONFIG.LABELS.CHOOSE_AN_ADDRESS, addressOptions)
            .addRequiredValidation(ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.SELECT_REQUIRED)
            .build();

        const confirmAddressButton = this.componentBuilder.createConfirmAddressButton((event) => this.confirmAddressOnClick(event));

        const enterAddressNotOnListLink = this.componentBuilder.createLink(ADDRESS_LOOKUP_CONFIG.LINK_TEXT.ENTER_ADDRESS_NOT_ON_LIST, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_NOT_ON_LIST);
        enterAddressNotOnListLink.addEventListener("click", event => { this.enterAddressNotOnListOnClick(event); });
        const enterAddressNotOnListLinkItem = this.componentBuilder.createListItem(enterAddressNotOnListLink);

        const returnToAddressLookupLink = this.componentBuilder.createLink(ADDRESS_LOOKUP_CONFIG.LINK_TEXT.RETURN_TO_POSTCODE_SEARCH, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.RETURN_TO_POSTCODE);
        returnToAddressLookupLink.addEventListener("click", (event) => this.returnToSearchOnClick(event));
        const returnToAddressLookupLinkItem = this.componentBuilder.createListItem(returnToAddressLookupLink);

        const linkList = this.componentBuilder.createLinkList([enterAddressNotOnListLinkItem, returnToAddressLookupLinkItem]);

        this.stateContainer.appendChild(selectElement);
        this.stateContainer.appendChild(confirmAddressButton);
        this.stateContainer.appendChild(linkList);

        this.validator.govuk.updateErrorSummary();
    }

    createHiddenInputsForAddress(address) {
        const addressPropertyMap = {
            [ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_1]: address.addressLine1,
            [ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_2]: address.addressLine2,
            [ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.TOWN_OR_CITY]: address.town,
            [ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.COUNTY]: address.county,
            [ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.COUNTRY]: address.country,
            [ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.POSTCODE]: address.postcode,
            [ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.UPRN]: address.UPRN
        };

        const fragment = document.createDocumentFragment();
        this.originalInputs.forEach(original => {
            const value = addressPropertyMap[original.dataAddressLookup];
            const hiddenInput = this.componentBuilder.createHiddenInput(original.name, original.id, value);
            fragment.appendChild(hiddenInput);
        });
        return fragment;
    }

    renderConfirmedView(address) {
        this.clearContainer();

        const fullAddress = [
            address.organisationName,
            address.addressLine1,
            address.addressLine2,
            address.town,
            address.county || address.region,
            address.country,
            address.postcode].filter(Boolean);

        const confirmedAddress = this.componentBuilder.createConfirmedAddressParagraph(fullAddress, address.postcode);

        let linkList = null;
        if (!this.isSameAsChecked()) {
            const editLink = this.componentBuilder.createLink(ADDRESS_LOOKUP_CONFIG.LINK_TEXT.EDIT_ADDRESS, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.EDIT_ADDRESS);
            editLink.addEventListener("click", (event) => this.returnToSearchOnClick(event));
            const editAddressListItem = this.componentBuilder.createListItem(editLink);
            linkList = this.componentBuilder.createLinkList([editAddressListItem]);
        }


        const hiddenInputs = this.createHiddenInputsForAddress(address);

        this.stateContainer.appendChild(confirmedAddress);
        this.stateContainer.appendChild(hiddenInputs);

        if (linkList) {
            this.stateContainer.appendChild(linkList);
        }


        this.validator.govuk.updateErrorSummary();
    }

    renderInternationalManualEntryView() {
        this.clearContainer();

        const fieldset = this.componentBuilder.createFieldset(ADDRESS_LOOKUP_CONFIG.LABELS.ENTER_NEW_INTERNATIONAL_ADDRESS);

        const addressLine1Input = this.componentBuilder.createGovukTextInput(ADDRESS_LOOKUP_CONFIG.LABELS.ADDRESS_LINE_1, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_1, ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.XX_LARGE)
            .addRequiredValidation(ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.REQUIRED)
            .addMaxLengthValidation(500, ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500)
            .build();
            

        const addressLine2Input = this.componentBuilder.createGovukTextInput(ADDRESS_LOOKUP_CONFIG.LABELS.ADDRESS_LINE_2, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_2, ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.XX_LARGE)
            .addMaxLengthValidation(500, ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500)
            .build();

        const townOrCityInput = this.componentBuilder.createGovukTextInput(ADDRESS_LOOKUP_CONFIG.LABELS.TOWN_OR_CITY, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.TOWN_OR_CITY, ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.X_LARGE)
            .addRequiredValidation(ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.REQUIRED)
            .addMaxLengthValidation(500, ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500)
            .build();

        const regionInput = this.componentBuilder.createGovukTextInput(ADDRESS_LOOKUP_CONFIG.LABELS.REGION_INTERNATIONAL, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.REGION_INTERNATIONAL, ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.X_LARGE)
            .addMaxLengthValidation(500, ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500)
            .build();

        const countryInput = this.componentBuilder.createGovukTextInput(ADDRESS_LOOKUP_CONFIG.LABELS.COUNTRY, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.COUNTRY, ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.X_LARGE)
            .addRequiredValidation(ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.REQUIRED)
            .addMaxLengthValidation(500, ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500)
            .build();

        const postcodeInput = this.componentBuilder.createGovukTextInput(ADDRESS_LOOKUP_CONFIG.LABELS.POSTCODE_INTERNATIONAL, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.POSTCODE_INTERNATIONAL, ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.LARGE)
            .addRequiredValidation(ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.REQUIRED)
            .addMaxLengthValidation(20, ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_20)
            .build();

        const confirmAddressButton = this.componentBuilder.createConfirmAddressButton((event) => this.confirmManualInternationalAddressOnClick(event));
        const returnToAddressLookupLink = this.componentBuilder.createLink(ADDRESS_LOOKUP_CONFIG.LINK_TEXT.RETURN_TO_POSTCODE_SEARCH, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.RETURN_TO_POSTCODE);
        returnToAddressLookupLink.addEventListener('click', (event) => { this.returnToSearchOnClick(event); });
        const linkList = this.componentBuilder.createLinkList([returnToAddressLookupLink]);

        fieldset.appendChild(addressLine1Input);
        fieldset.appendChild(addressLine2Input);
        fieldset.appendChild(townOrCityInput);
        fieldset.appendChild(regionInput);
        fieldset.appendChild(countryInput);
        fieldset.appendChild(postcodeInput);

        this.stateContainer.appendChild(fieldset);
        this.stateContainer.appendChild(confirmAddressButton);
        this.stateContainer.appendChild(linkList);

        this.validator.govuk.updateErrorSummary();
    }

    renderUKManualEntryView() {
        this.clearContainer();

        const fieldset = this.componentBuilder.createFieldset(ADDRESS_LOOKUP_CONFIG.LABELS.ENTER_NEW_UK_ADDRESS);

        const addressLine1Input = this.componentBuilder.createGovukTextInput(ADDRESS_LOOKUP_CONFIG.LABELS.ADDRESS_LINE_1, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_1, ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.XX_LARGE)
            .addRequiredValidation(ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.REQUIRED)
            .addMaxLengthValidation(500, ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500)
            .build();

        const addressLine2Input = this.componentBuilder.createGovukTextInput(ADDRESS_LOOKUP_CONFIG.LABELS.ADDRESS_LINE_2, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_2, ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.XX_LARGE)
            .addMaxLengthValidation(500, ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500)
            .build();

        const townOrCityInput = this.componentBuilder.createGovukTextInput(ADDRESS_LOOKUP_CONFIG.LABELS.TOWN_OR_CITY, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.TOWN_OR_CITY, ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.X_LARGE)
            .addRequiredValidation(ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.REQUIRED)
            .addMaxLengthValidation(500, ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500)
            .build();

        const countyInput = this.componentBuilder.createGovukTextInput(ADDRESS_LOOKUP_CONFIG.LABELS.COUNTY, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.COUNTY, ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.X_LARGE)
            .addMaxLengthValidation(500, ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_500)
            .build();

        const postcodeInput = this.componentBuilder.createGovukTextInput(ADDRESS_LOOKUP_CONFIG.LABELS.POSTCODE, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.POSTCODE, ADDRESS_LOOKUP_CONFIG.INPUT_WIDTHS.X_LARGE)
            .addRequiredValidation(ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.REQUIRED)
            .addMaxLengthValidation(20, ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.MAX_LENGTH_20)
            .addPatternValidation(ADDRESS_LOOKUP_CONFIG.PATTERNS.POSTCODE, ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.INVALID_POSTCODE)
            .build();


        const confirmAddressButton = this.componentBuilder.createConfirmAddressButton((event) => this.confirmManualUKAddressOnClick(event));
        const returnToAddressLookupLink = this.componentBuilder.createLink(ADDRESS_LOOKUP_CONFIG.LINK_TEXT.RETURN_TO_POSTCODE_SEARCH, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.RETURN_TO_POSTCODE);
        returnToAddressLookupLink.addEventListener("click", (event) => this.returnToSearchOnClick(event));
        const linkList = this.componentBuilder.createLinkList([returnToAddressLookupLink]);

        fieldset.appendChild(addressLine1Input);
        fieldset.appendChild(addressLine2Input);
        fieldset.appendChild(townOrCityInput);
        fieldset.appendChild(countyInput);
        fieldset.appendChild(postcodeInput);

        this.stateContainer.appendChild(fieldset);
        this.stateContainer.appendChild(confirmAddressButton);
        this.stateContainer.appendChild(linkList);

        this.validator.reparse();
        this.validator.govuk.updateErrorSummary();
    }

    getComponentByDataAddressAttribute(attributeValue) {
        return this.stateContainer.querySelector(`[data-address-lookup='${attributeValue}']`);
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

        var address = this.addressMapper.mapFromManualInternationalEntry(addressLine1Input.value, addressLine2Input.value, townOrCityInput.value, regionInput.value, countryInput.value, postcodeInput.value);
        this.stateMachine.transition(AddressLookupStateMachine.STATES.CONFIRMED, { address });
    }

    confirmManualUKAddressOnClick(event) {
        event.preventDefault();

        const inputsToValidate = [];
        const addressLine1Input = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_1);
        const addressLine2Input = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_2);
        const townOrCityInput = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.TOWN_OR_CITY);
        const countyInput = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.COUNTY);
        const postcodeInput = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.POSTCODE);

        const isValid = this.validator.validateMultiple([addressLine1Input, addressLine2Input, townOrCityInput, countyInput, postcodeInput]);
        if (!isValid) {
            return;
        }

        var address = this.addressMapper.mapFromManualUKEntry(addressLine1Input.value, addressLine2Input.value, townOrCityInput.value, countyInput.value, postcodeInput.value);
        this.stateMachine.transition(AddressLookupStateMachine.STATES.CONFIRMED, { address });
    }

    async confirmAddressOnClick(event) {
        event.preventDefault();

        const selectInput = this.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.SELECT_ADDRESS);
        const isValid = this.validator.validate(selectInput);
        if (!isValid) {
            return;
        }

        const selectedUPRN = selectInput.value;
        const addressResult = await this.apiService.getAddressById(selectedUPRN);
        const address = this.addressMapper.mapFromDpaResult(addressResult[0]);
        this.stateMachine.transition(AddressLookupStateMachine.STATES.CONFIRMED, { address });
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
        this.validator.clearErrors();
        while (this.stateContainer.firstChild) {
            this.stateContainer.removeChild(this.stateContainer.firstChild);
        }
    }

    onSameAsCheckboxChange(event) {
        if (event.target.checked) {
            const primaryAddress = this.primaryStateMachine?.confirmedAddress;
            if (!primaryAddress) {
                return;
            }
            this.confirmWithAddress(primaryAddress);
        } else {
            //TODO: I think I just want to show the edit link
            //this.stateMachine.transition(AddressLookupStateMachine.STATES.SEARCH);
        }
    }

    confirmWithAddress(address) {
        this.stateMachine.transition(AddressLookupStateMachine.STATES.CONFIRMED, { address });
    }

    isSameAsChecked() {
        if (!this.sameAsCheckbox) {
            return false;
        }
        const checkbox = this.sameAsCheckbox.querySelector("input[type='checkbox']");
        return checkbox && checkbox.checked;
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

        if (searchResults.length === 1) {
            const address = this.addressMapper.mapFromDpaResult(searchResults[0]);
            this.stateMachine.transition(AddressLookupStateMachine.STATES.CONFIRMED, { address });
        } else if (searchResults.length > 1) {
            this.stateMachine.transition(AddressLookupStateMachine.STATES.SELECT, { results: searchResults });
        } else {
            const fieldset = this.stateContainer.querySelector("fieldset");
            let errorMessage = "";
            if (building && postcode) {
                errorMessage = ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.ADDRESS_POSTCODE_DONT_MATCH;
            } else {
                errorMessage = ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.NO_ADDRESS_AT_POSTCODE;
            }

            this.validator.addOrUpdateCustomFieldsetError(fieldset, errorMessage);
        }
    }
}


function submitOnClick(event, addressLookupObjects) {
    const submittableStates = [
        AddressLookupStateMachine.STATES.CONFIRMED
    ];


    addressLookupObjects.forEach((addressLookup) => {

        if (!submittableStates.includes(addressLookup.stateMachine.currentState)) {
            if (addressLookup.stateMachine.currentState === AddressLookupStateMachine.STATES.SELECT) {
                const select = addressLookup.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.SELECT_ADDRESS);
                addressLookup.validator.addSelectError(select, "Select confirm address before saving this page");
            } else {
                const fieldset = addressLookup.container.querySelector("fieldset");
                const errorMessage = addressLookup.stateMachine.currentState === AddressLookupStateMachine.STATES.SEARCH
                    ? "Select 'find an address' and confrim your address before saving this page"
                    : "Select confirm address before submitting this page";
                addressLookup.validator.addOrUpdateCustomFieldsetError(fieldset, errorMessage);
            }

            event.preventDefault();
        }
    });
}

document.addEventListener("DOMContentLoaded", function () {
    const addressLookupComponents = document.querySelectorAll(ADDRESS_LOOKUP_CONFIG.COMPONENT_SELECTOR);
    const addressLookupObjects = [];
    let primaryStateMachine = null;
    addressLookupComponents.forEach((element, key) => {
        const role = element.getAttribute("data-address-lookup-role") || "primary";
        let lookup = null;
        if (role === "primary") {
            lookup = new TprAddressLookup(element, key, role);
            primaryStateMachine = lookup.stateMachine;
        } else {
            lookup = new TprAddressLookup(element, key, role, primaryStateMachine);
        }
        addressLookupObjects.push(lookup);
    });

    const formSubmitButton = document.querySelector("button.govuk-button:not(.govuk-button--secondary)[type='submit']");
    formSubmitButton.addEventListener("click", (event) => { submitOnClick(event, addressLookupObjects); })
});
