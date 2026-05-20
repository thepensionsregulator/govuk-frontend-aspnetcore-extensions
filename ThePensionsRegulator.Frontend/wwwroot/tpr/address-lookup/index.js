import { AddressLookupApiService } from "./api-service.js"; 
import { ADDRESS_LOOKUP_CONFIG } from "./config.js";
import { AddressLookupStateMachine } from "./state-machine.js";
import { AddressLookupComponentBuilder } from "./component-builder.js";
import { AddressLookupValidator } from "./validator.js";
import { AddressMapper } from "./mapper.js";
import { PostcodeSanitiser } from "./postcode-sanitiser.js";
import { FieldDefaults } from "./field-defaults.js";
import { PostcodeNormaliser } from "./postcode-normaliser.js";
    
class TprAddressLookup {
    constructor(element, key, role, primaryStateMachine) {
        this.container = element;
        this.index = key;
        this.role = role;
        this.primaryStateMachine = primaryStateMachine;
        this.searchEndpoint = element.getAttribute("data-address-lookup-search-url");
        this.idEndpoint = element.getAttribute("data-address-lookup-id-url");
        this.checkboxLabel = role === "secondary" ? element.getAttribute(`${ADDRESS_LOOKUP_CONFIG.ATTRIBUTES.BASE}-${ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.CHECKBOX_LABEL}`) : null;
        this.apiService = new AddressLookupApiService(this.searchEndpoint, this.idEndpoint);
        this.validator = new AddressLookupValidator(element, this.index);
        this.componentBuilder = new AddressLookupComponentBuilder(this.index, ADDRESS_LOOKUP_CONFIG);
        this.addressMapper = new AddressMapper(ADDRESS_LOOKUP_CONFIG);
        this.stateMachine = new AddressLookupStateMachine();
        this.stateMachine.onChange((newState, data) => this.onStateChange(newState, data));
        this.postcodeSanitiser = new PostcodeSanitiser();
        this.originalInputs = this.captureOriginalInputs();
        this.countryOptions = this.captureCountryOptions();
        this.fieldDefaults = new FieldDefaults(this.container, this.originalInputs);
        this.postcodeNormaliser = new PostcodeNormaliser();

        const address = this.addressMapper.mapFromInput(this.originalInputs);

        while (this.container.firstChild) {
            this.container.removeChild(this.container.firstChild);
        }

        if (this.role === "secondary") {
            this.sameAsCheckbox = this.componentBuilder.createCheckbox(this.checkboxLabel);
            this.sameAsCheckbox.querySelector("input[type='checkbox']")
                .addEventListener("change", (event) => this.onSameAsCheckboxChange(event));
            this.container.appendChild(this.sameAsCheckbox);

            const primaryAddress = this.primaryStateMachine?.confirmedAddress;
            if (primaryAddress && this.addressMapper.addressesMatch(primaryAddress, address)) {
                this.sameAsCheckbox.querySelector("input[type='checkbox']").checked = true;
            }

            this.primaryStateMachine.onChange((newState, data) => this.onPrimaryStateChange(newState, data));
        }

        this.stateContainer = document.createElement("div");
        this.container.appendChild(this.stateContainer);

        this.stateContainer.addEventListener("keydown", (event) => {
            if (event.key === "Enter") {
                const activeButton = this.stateContainer.querySelector("button[type='button']");
                if (activeButton) {
                    event.preventDefault();
                    activeButton.click();
                }
            }
        });

        this.initialised = false;

        if (this.validator.validAddress(address)) { 
            this.stateMachine.transition(AddressLookupStateMachine.STATES.CONFIRMED, { address });
        } else {
            this.stateMachine.transition(AddressLookupStateMachine.STATES.SEARCH);
        }

        this.initialised = true;
    }

    onPrimaryStateChange(newState, data) {
        if (newState === AddressLookupStateMachine.STATES.CONFIRMED && this.isSameAsChecked()) {
            this.stateMachine.transition(AddressLookupStateMachine.STATES.CONFIRMED,  data);
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
            case AddressLookupStateMachine.STATES.MANUAL_INTERNATIONAL_ENTRY:
                this.renderInternationalManualEntryView();
                break;
            case AddressLookupStateMachine.STATES.MANUAL_UK_ENTRY:
                this.renderUKManualEntryView();
                break;
        }

        if (this.initialised) {
            this.focusFirstInteractiveElement();
        }
    }

    focusFirstInteractiveElement() {
        setTimeout(() => {
            const focusTarget = this.stateContainer.querySelector(
                "h4.govuk-visually-hidden[tabindex], select, input:not([type='hidden']), a.govuk-link"
            );
            if (focusTarget) {
                focusTarget.focus();
            }
        }, 200);
    }

    captureOriginalInputs() {
        const inputs = this.container.querySelectorAll("input, select");
        return Array.from(inputs).map(input => ({
            name: input.getAttribute("name"),
            id: input.getAttribute("id"),
            dataAddressLookup: input.getAttribute(ADDRESS_LOOKUP_CONFIG.ATTRIBUTES.BASE),
            value: input.value,
            selectedOptionLabel: input.tagName === "SELECT" ? input.options[input.selectedIndex]?.text || undefined : undefined,
            label: input.labels?.[0]?.textContent.trim() || undefined,
            requiredMessage: input.dataset.valRequired,
            maxLength: input.dataset.valMaxlengthMax,
            maxLengthMessage: input.dataset.valMaxlength,
            patternMessage: input.dataset.valRegex
        }));
    }

    captureCountryOptions() {
        const countrySelect = this.container.querySelector(`select[${ADDRESS_LOOKUP_CONFIG.ATTRIBUTES.BASE}="${ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.COUNTRY}"]`);
        const options = Array.from(countrySelect.querySelectorAll("option"));
        options.forEach(option => option.removeAttribute("selected"));
        return options;
    }

    renderSearchView() {
        this.clearContainer();

        const fieldset = this.componentBuilder.createFieldset(ADDRESS_LOOKUP_CONFIG.LABELS.SEARCH_BY_POSTCODE);

        const buildingKey = ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.BUILDING_INPUT;
        const buildingNameGroup = this.componentBuilder.createGovukTextInput(this.fieldDefaults.label(buildingKey), buildingKey, this.fieldDefaults.width(buildingKey))
            .addMaxLengthValidation(this.fieldDefaults.maxLength(buildingKey), this.fieldDefaults.maxLengthMessage(buildingKey))
            .build();

        const postcodeKey = ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.POSTCODE;
        const postcodeGroup = this.componentBuilder.createGovukTextInput(this.fieldDefaults.label(postcodeKey), postcodeKey, this.fieldDefaults.width(postcodeKey))
            .addRequiredValidation(this.fieldDefaults.requiredMessage(postcodeKey))
            .addMaxLengthValidation(this.fieldDefaults.maxLength(postcodeKey), this.fieldDefaults.maxLengthMessage(postcodeKey))
            .addPatternValidation(this.fieldDefaults.pattern(postcodeKey), this.fieldDefaults.patternMessage(postcodeKey))
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

        const selectKey = ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.SELECT_ADDRESS;
        const selectElement = this.componentBuilder.createGovukSelect(this.fieldDefaults.label(selectKey), selectKey, addressOptions, this.fieldDefaults.labelSize(selectKey))
            .addRequiredValidation(this.fieldDefaults.requiredMessage(selectKey))
            .build();

        const confirmAddressButton = this.componentBuilder.createConfirmAddressButton((event) => this.confirmAddressOnClick(event));

        const enterAddressNotOnListLink = this.componentBuilder.createLink(ADDRESS_LOOKUP_CONFIG.LINK_TEXT.ENTER_ADDRESS_NOT_ON_LIST, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_NOT_ON_LIST);
        enterAddressNotOnListLink.addEventListener("click", event => { this.enterAddressNotOnListOnClick(event); });

        const returnToAddressLookupLink = this.componentBuilder.createLink(ADDRESS_LOOKUP_CONFIG.LINK_TEXT.RETURN_TO_POSTCODE_SEARCH, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.RETURN_TO_POSTCODE);
        returnToAddressLookupLink.addEventListener("click", (event) => this.returnToSearchOnClick(event));

        const linkList = this.componentBuilder.createLinkList([enterAddressNotOnListLink, returnToAddressLookupLink]);

        this.stateContainer.appendChild(selectElement);
        this.stateContainer.appendChild(confirmAddressButton);
        this.stateContainer.appendChild(linkList);

        this.validator.reparse();
        this.validator.govuk.updateErrorSummary();
    }

    createHiddenInputsForAddress(address) {
        const addressPropertyMap = {
            [ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_1]: address.addressLine1,
            [ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_2]: address.addressLine2,
            [ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.TOWN_OR_CITY]: address.town,
            [ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.COUNTY]: address.county,
            [ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.COUNTRY]: address.country,
            [ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.COUNTRY_CODE]: address.countryCode,
            [ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.POSTCODE_INTERNATIONAL]: address.postcode,
            [ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.UPRN]: address.UPRN
        };

        //TODO: Why is county not being mapped to the correct input (or any input) for UK manual entry?!!!


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

        const heading = document.createElement("h4");
        heading.classList.add("govuk-visually-hidden");
        heading.setAttribute("tabindex", "-1");
        heading.innerText = ADDRESS_LOOKUP_CONFIG.LABELS.ADDRESS_CONFIRMED;

        let postcode = address.postcode;

        if (address.country == ADDRESS_LOOKUP_CONFIG.DEFAULTS.COUNTRY) {
            postcode = this.postcodeNormaliser.normalise(postcode);
        }

        const fullAddress = [
            address.organisationName || "",
            address.addressLine1,
            address.addressLine2,
            address.town,
            address.county || address.region,
            postcode,
            address.country,
            address.countryCode].filter(Boolean);

        const confirmedAddress = this.componentBuilder.createConfirmedAddressParagraph(fullAddress, address.postcode, address.countryCode || '');

        let linkList = null;
        if (!this.isSameAsChecked()) {
            const editLink = this.componentBuilder.createLink(ADDRESS_LOOKUP_CONFIG.LINK_TEXT.EDIT_ADDRESS, ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.EDIT_ADDRESS);
            editLink.addEventListener("click", (event) => this.returnToSearchOnClick(event));
            linkList = this.componentBuilder.createLinkList([editLink]);
        }


        const hiddenInputs = this.createHiddenInputsForAddress(address);

        this.stateContainer.appendChild(heading);
        this.stateContainer.appendChild(confirmedAddress);
        this.stateContainer.appendChild(hiddenInputs);

        if (linkList) {
            this.stateContainer.appendChild(linkList);
        }

        this.validator.reparse();
        this.validator.govuk.updateErrorSummary();
    }

    renderInternationalManualEntryView() {
        this.clearContainer();

        const fieldset = this.componentBuilder.createFieldset(ADDRESS_LOOKUP_CONFIG.LABELS.ENTER_NEW_INTERNATIONAL_ADDRESS);

        const key = ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_1;
        const addressLine1Input = this.componentBuilder.createGovukTextInput(this.fieldDefaults.label(key), key, this.fieldDefaults.width(key))
            .addRequiredValidation(this.fieldDefaults.requiredMessage(key))
            .addMaxLengthValidation(this.fieldDefaults.maxLength(key), this.fieldDefaults.maxLengthMessage(key))
            .build();
            
        const addressLine2Key = ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_2;
        const addressLine2Input = this.componentBuilder.createGovukTextInput(this.fieldDefaults.label(addressLine2Key), addressLine2Key, this.fieldDefaults.width(addressLine2Key))
            .addMaxLengthValidation(this.fieldDefaults.maxLength(addressLine2Key), this.fieldDefaults.maxLengthMessage(addressLine2Key))
            .build();

        const townOrCityKey = ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.TOWN_OR_CITY;
        const townOrCityInput = this.componentBuilder.createGovukTextInput(this.fieldDefaults.label(townOrCityKey), townOrCityKey, this.fieldDefaults.width(townOrCityKey))
            .addRequiredValidation(this.fieldDefaults.requiredMessage(townOrCityKey))
            .addMaxLengthValidation(this.fieldDefaults.maxLength(townOrCityKey), this.fieldDefaults.maxLengthMessage(townOrCityKey))
            .build();

        const regionKey = ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.REGION_INTERNATIONAL;
        const regionInput = this.componentBuilder.createGovukTextInput(this.fieldDefaults.label(regionKey), regionKey, this.fieldDefaults.width(regionKey))
            .addMaxLengthValidation(this.fieldDefaults.maxLength(regionKey), this.fieldDefaults.maxLengthMessage(regionKey))
            .build();

        const countryKey = ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.COUNTRY;
        const countryInput = this.componentBuilder.createGovukSelect(this.fieldDefaults.label(countryKey), countryKey, this.countryOptions)
            .addRequiredValidation(this.fieldDefaults.requiredMessage(countryKey))
            .addMaxLengthValidation(this.fieldDefaults.maxLength(countryKey), this.fieldDefaults.maxLengthMessage(countryKey))
            .build();

        const postcodeInternationalKey = ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.POSTCODE_INTERNATIONAL;
        const postcodeInternationalInput = this.componentBuilder.createGovukTextInput(this.fieldDefaults.label(postcodeInternationalKey), postcodeInternationalKey, this.fieldDefaults.width(postcodeInternationalKey))
            .addRequiredValidation(this.fieldDefaults.requiredMessage(postcodeInternationalKey))
            .addMaxLengthValidation(this.fieldDefaults.maxLength(postcodeInternationalKey), this.fieldDefaults.maxLengthMessage(postcodeInternationalKey))
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
        fieldset.appendChild(postcodeInternationalInput);

        this.stateContainer.appendChild(fieldset);
        this.stateContainer.appendChild(confirmAddressButton);
        this.stateContainer.appendChild(linkList);

        this.validator.reparse();
        this.validator.govuk.updateErrorSummary();
    }

    renderUKManualEntryView() {
        this.clearContainer();

        const fieldset = this.componentBuilder.createFieldset(ADDRESS_LOOKUP_CONFIG.LABELS.ENTER_NEW_UK_ADDRESS);

        const addressLine1Key = ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_1;
        const addressLine1Input = this.componentBuilder.createGovukTextInput(this.fieldDefaults.label(addressLine1Key), addressLine1Key, this.fieldDefaults.width(addressLine1Key))
            .addRequiredValidation(this.fieldDefaults.requiredMessage(addressLine1Key))
            .addMaxLengthValidation(this.fieldDefaults.maxLength(addressLine1Key), this.fieldDefaults.maxLengthMessage(addressLine1Key))
            .build();

        const addressLine2Key = ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.ADDRESS_LINE_2;
        const addressLine2Input = this.componentBuilder.createGovukTextInput(this.fieldDefaults.label(addressLine2Key), addressLine2Key, this.fieldDefaults.width(addressLine2Key))
            .addMaxLengthValidation(this.fieldDefaults.maxLength(addressLine2Key), this.fieldDefaults.maxLengthMessage(addressLine2Key))
            .build();

        const townOrCityKey = ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.TOWN_OR_CITY;
        const townOrCityInput = this.componentBuilder.createGovukTextInput(this.fieldDefaults.label(townOrCityKey), townOrCityKey, this.fieldDefaults.width(townOrCityKey))
            .addRequiredValidation(this.fieldDefaults.requiredMessage(townOrCityKey))
            .addMaxLengthValidation(this.fieldDefaults.maxLength(townOrCityKey), this.fieldDefaults.maxLengthMessage(townOrCityKey))
            .build();

        const countyKey = ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.COUNTY;
        const countyInput = this.componentBuilder.createGovukTextInput(this.fieldDefaults.label(countyKey), countyKey, this.fieldDefaults.width(countyKey))
            .addMaxLengthValidation(this.fieldDefaults.maxLength(countyKey), this.fieldDefaults.maxLengthMessage(countyKey))
            .build();

        const postcodeKey = ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.POSTCODE;
        const postcodeInput = this.componentBuilder.createGovukTextInput(this.fieldDefaults.label(postcodeKey), postcodeKey, this.fieldDefaults.width(postcodeKey))
            .addRequiredValidation(this.fieldDefaults.requiredMessage(postcodeKey))
            .addMaxLengthValidation(this.fieldDefaults.maxLength(postcodeKey), this.fieldDefaults.maxLengthMessage(postcodeKey))
            .addPatternValidation(this.fieldDefaults.pattern(postcodeKey), this.fieldDefaults.patternMessage(postcodeKey))
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

        const selectedCountry = countryInput.selectedOptions[0];
        const selectedCountryText = selectedCountry.text;

        const address = this.addressMapper.mapFromManualEntry(addressLine1Input.value, addressLine2Input.value, townOrCityInput.value, regionInput.value, postcodeInput.value, selectedCountryText, countryInput.value);
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

        const normalisedPostcode = this.postcodeNormaliser.normalise(postcodeInput.value);
        const ukOption = this.countryOptions.find(option => option.text === ADDRESS_LOOKUP_CONFIG.DEFAULTS.COUNTRY);
        const countryName = ukOption ? ukOption.text : ADDRESS_LOOKUP_CONFIG.DEFAULTS.COUNTRY;
        const countryCode = ukOption ? ukOption.value : "";

        const address = this.addressMapper.mapFromManualEntry(addressLine1Input.value, addressLine2Input.value, townOrCityInput.value, countyInput.value, normalisedPostcode, countryName, countryCode);

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
        this.stateMachine.transition(AddressLookupStateMachine.STATES.MANUAL_INTERNATIONAL_ENTRY);
    }

    enterAddressNotOnListOnClick(event) {
        event.preventDefault();
        this.stateMachine.transition(AddressLookupStateMachine.STATES.MANUAL_UK_ENTRY);
    }

    clearContainer() {
        this.validator.clearErrors();
        while (this.stateContainer.firstChild) {
            this.stateContainer.removeChild(this.stateContainer.firstChild);
        }
    }

    onSameAsCheckboxChange(event) {
        const address = this.primaryStateMachine?.confirmedAddress;
        if (!address) {
            return;
        }

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

        let postcode = postcodeInput.value;
        const building = buildingInput.value;

        postcode = this.postcodeSanitiser.sanitise(postcode);

        const searchResults = await this.apiService.searchAddresses(postcode, building);

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

    // Validate non-address-lookup fields on the form
    const form = event.target.closest("form");
    if (form) {
        const isFormValid = $(form).valid();
        if (!isFormValid) {
            event.preventDefault();
        }
    }

    addressLookupObjects.forEach((addressLookup) => {

        if (!submittableStates.includes(addressLookup.stateMachine.currentState)) {
            if (addressLookup.stateMachine.currentState === AddressLookupStateMachine.STATES.SELECT) {
                const select = addressLookup.getComponentByDataAddressAttribute(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.SELECT_ADDRESS);
                addressLookup.validator.addSelectError(select, ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.SELECT_CONFIRM);
            } else {
                const fieldset = addressLookup.container.querySelector("fieldset");
                const errorMessage = addressLookup.stateMachine.currentState === AddressLookupStateMachine.STATES.SEARCH
                    ? ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.SELECT_FIND_ADDRESS
                    : ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.SELECT_CONFIRM;
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
    if (formSubmitButton != null) {
        formSubmitButton.addEventListener("click", (event) => { submitOnClick(event, addressLookupObjects); })
    }
});


export { submitOnClick };