class TprAddressLookup {
    constructor(element, key) {
        this.container = element;
        this.index = key;
        this.apiEndpoint = element.getAttribute("data-address-lookup-url"); //TODO: Should data- attribute be properties on the tag helper?
        this.init();
        console.log(`Creating TPR address lookup component with id: ${this.index}`);
    }

    BUILDING_INPUT = "building";
    POSTCODE = "postcode";
    FIND_ADDRESS = "find-address";
    CONFIRM_BUTTON = "confirm-button";
    SELECT_ADDRESS = "select-address";

    JsonResults = [];

    //TODO:
    // - Customise postcode error message
    // - Customise regex for valid UK postcode?
    // - Customise field length?
    // - Customise building name label text
    // - Customise postcode label text
    // - Actually search an address
    // - Pressing 'confirm address' triggers validation if no address is selected in 'select' component
    // - Pressing 'confirm address' removes select and button, shows address as a govuk-body p tag. Populate some hidden input fields so a form can get these fields

    createGovukTextInput(labelText, dataAddressLookupValue) {
        const formGroup = document.createElement("div");
        formGroup.classList = "govuk-form-group";
        const inputId = `${dataAddressLookupValue}-${this.index}`;

        const label = document.createElement("label");
        label.classList = "govuk-label"
        label.innerText = labelText;
        label.setAttribute("for", inputId);
            
        const input = document.createElement("input");
        input.classList = "govuk-input";
        input.setAttribute("data-address-lookup", dataAddressLookupValue);
        input.setAttribute("name", dataAddressLookupValue);
        input.setAttribute("id", inputId);

        formGroup.appendChild(label);
        formGroup.appendChild(input)

        return formGroup;
    }

    init() {
        while (this.container.firstChild) {
            this.container.removeChild(this.container.firstChild);
        }

        this.JsonResults = [];

        const buildingNameGroup = this.createGovukTextInput("Building name", this.BUILDING_INPUT);
        const postcodeGroup = this.createGovukTextInput("Postcode", this.POSTCODE);
        const findAddressButton = this.createFindAddressButton();

        this.container.appendChild(buildingNameGroup);
        this.container.appendChild(postcodeGroup);
        this.container.appendChild(findAddressButton);

        const postcode = this.getComponentByDataAddressAttribute(this.POSTCODE);
        postcode.setAttribute("data-val", "true");
        postcode.setAttribute("required", "required");
        postcode.setAttribute("data-val-required", "This field is required");
        
    }

    getComponentByDataAddressAttribute(attributeValue) {
        return this.container.querySelector(`[data-address-lookup='${attributeValue}']`);
    }

    createSelect(options) {
        const div = document.createElement("div");
        div.classList = "govuk-form-group";

        const select = document.createElement("select");
        select.classList = "govuk-select";
        select.name = "Find-address";
        select.setAttribute("data-address-lookup", this.SELECT_ADDRESS);
        select.setAttribute("required", "required");
        select.setAttribute("data-val", "true");
        select.setAttribute("data-val-required", "This field is required"); //TODO: Customise this error message

        const blankOption = new Option("", "", true, true);
        blankOption.selected = "selected";
        select.appendChild(blankOption);

        options.forEach(option => {
            select.appendChild(option);
        });

        div.appendChild(select);

        return div;
    }

    createOption(value, label) {
        const option = new Option(label, value);
        return option;
    }

    createFindAddressButton() {
        const button = createGovUkSecondaryButton("Find address", this.FIND_ADDRESS);
        button.addEventListener("click", event => this.findAddressButtonOnClick(event));

        return button;
    }

    createConfirmAddressButton() {
        const button = createGovUkSecondaryButton("Confirm address", this.CONFIRM_BUTTON);
        button.addEventListener('click', (event) => this.confirmAddressOnClick(event));

        return button;
    }

    confirmAddressOnClick(event) {
        event.preventDefault();
        const selectInput = this.getComponentByDataAddressAttribute(this.SELECT_ADDRESS);

        const form = this.container.closest("form");
        if (form) {
            const validator = $(form).validate();
            const isValid = validator.element(selectInput);

            if (!isValid) {
                return;
            }
        }

        console.log(selectInput);
        const selectedUPRN = selectInput.value;
        console.log(`Selected UPRN: ${selectedUPRN}`);
        selectInput.remove();

        const confirmAddressbutton = this.getComponentByDataAddressAttribute(this.CONFIRM_BUTTON);
        confirmAddressbutton.remove();

        console.log(this.JsonResults);
        const matchedDPA = this.JsonResults.find(x => x.DPA.UPRN == selectedUPRN);
        const address = matchedDPA.DPA.ADDRESS;
        const addressReplaced = address.replaceAll(",", "<br/>");

        const selectedAddressElement = document.createElement("p");
        selectedAddressElement.className = "govuk-body";
        selectedAddressElement.innerHTML = addressReplaced;

        this.container.appendChild(selectedAddressElement);

    }

    returnToSearchOnClick(event) {
        event.preventDefault();
        this.init();
    }

    async findAddressButtonOnClick(event) {
        event.preventDefault();

        const postcodeInput = this.getComponentByDataAddressAttribute(this.POSTCODE);
        const form = this.container.closest("form");
        if (form) {
            const validator = $(form).validate();
            const isValid = validator.element(postcodeInput);

            if (!isValid) {
                return;
            }
        }

        const buildingInput = this.getComponentByDataAddressAttribute(this.BUILDING_INPUT);
        const findAddressButton = this.getComponentByDataAddressAttribute(this.FIND_ADDRESS);

        const postcode = postcodeInput.value;
        const building = buildingInput.value;

        postcodeInput.parentNode.remove();
        buildingInput.parentNode.remove();
        findAddressButton.remove();

        this.JsonResults = [];

        try {
            const result = (await fetchJson(this.apiEndpoint));
            console.log(result);
            this.JsonResults = result.results;
            console.log(this.JsonResults);
        } catch (error) {
            console.error("Failed to search content:", error);
        }

        // filter results,
        // get the building input, if this is just


        const options = []
        this.JsonResults.forEach(result => {
            options.push(this.createOption(result.DPA.UPRN, result.DPA.ADDRESS));
        });

        const select = this.createSelect(options);

        this.container.appendChild(select);

        const confirmAddressbutton = this.createConfirmAddressButton();
        this.container.appendChild(confirmAddressbutton);

        const enterAddressNotOnListLink = createLink("Enter address not on list", "address-not-on-list");
        const returnToAddressLookupLink = createLink("Return to postcode search", "return-to-postcode");
        returnToAddressLookupLink.addEventListener("click", (event) => this.returnToSearchOnClick(event));

        const enterAddressListItem = createListItem(enterAddressNotOnListLink);
        const returnToAddressListItem =  createListItem(returnToAddressLookupLink);


        const linkList = createUnorderedList()
        linkList.appendChild(enterAddressListItem);
        linkList.appendChild(returnToAddressListItem);

        this.container.appendChild(linkList);

    }
}

document.addEventListener("DOMContentLoaded", function () {
    console.log("TPR Address Lookup script loaded");
    const addressLookupComponents = document.querySelectorAll(".tpr-address-lookup");
    addressLookupComponents.forEach((element, key) => {
        new TprAddressLookup(element, key);
    });

    //TODO: Find the submit button for the whole form, this should be the primary button. Each page should only have one primary button.
    // - If this button is clicked and the user has not confirmed an address, error the fieldset 

});

async function fetchJson(url) {
    const response = await fetch(url, { headers: { 'Content-Type': 'application/json' } });
    return response.json();
}

function createGovUkSecondaryButton(label, dataAddressLookupValue){
    const button = document.createElement("button");
    button.setAttribute("data-address-lookup", dataAddressLookupValue);
    button.classList = "govuk-button govuk-button--secondary";
    button.innerText = label;
    button.setAttribute("type", "submit");

    return button;
}

function createLink(label, dataAddressLookupValue) {
    const link = document.createElement("a");
    link.setAttribute("data-address-lookup", dataAddressLookupValue);
    link.classList = "govuk-link";
    link.innerText = label;

    return link;
}

function createUnorderedList() {
    const ul = document.createElement("ul");
    ul.classList = "govuk-list tpr-address-lookup__links";

    return ul;
}

function createListItem(innerHTML) {
    const li = document.createElement("li");
    li.appendChild(innerHTML);

    return li;
}