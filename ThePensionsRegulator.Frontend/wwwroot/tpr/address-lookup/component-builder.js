import { toSentenceCase } from "./utils.js";
import { InputBuilder } from "./input-builder.js";

class AddressLookupComponentBuilder {
    constructor(index, config) {
        this.index = index;
        this.config = config;
    }

    createGovukTextInput(labelText, dataAddressLookupValue, inputWidth) {
        const formGroup = document.createElement("div");
        formGroup.classList = `${this.config.CSS_CLASSES.FORM_GROUP}`;
        const inputId = `${dataAddressLookupValue}-${this.index}`;

        const label = document.createElement("label");
        label.classList = this.config.CSS_CLASSES.LABEL;
        label.innerText = labelText;
        label.setAttribute("for", inputId);

        const widthClass = this.#getWidthCssClass(inputWidth);
        const input = document.createElement("input");
        input.classList = `${this.config.CSS_CLASSES.INPUT} ${widthClass}`;
        input.setAttribute(this.config.ATTRIBUTES.BASE, dataAddressLookupValue);
        input.setAttribute("name", dataAddressLookupValue);
        input.setAttribute("id", inputId);

        formGroup.appendChild(label);
        formGroup.appendChild(input);

        return new InputBuilder(formGroup);
    }


    createFieldset(legendText) {
        const fieldset = document.createElement("fieldset");
        fieldset.classList = this.config.CSS_CLASSES.FIELDSET;

        const legend = document.createElement("legend");
        legend.classList = this.config.CSS_CLASSES.LEGEND;
        legend.innerText = legendText;

        fieldset.appendChild(legend);

        return fieldset;    
    }

    createConfirmedAddressParagraph(address, postcode) {
        const selectedAddressParagraph = document.createElement("p");
        selectedAddressParagraph.classList = this.config.CSS_CLASSES.BODY;
        const addressArray = address.split(",");

        addressArray.forEach((addressLine) => {
            addressLine = addressLine.trim();
            if (addressLine === postcode) {
                selectedAddressParagraph.appendChild(document.createTextNode(addressLine));
            } else {
                const sentanceCase = toSentenceCase(addressLine);
                selectedAddressParagraph.appendChild(document.createTextNode(sentanceCase));
            }
            selectedAddressParagraph.appendChild(document.createElement("br"));
        });

        return selectedAddressParagraph;
    }

    createFindAddressButton(onClick) {
        const button = this.createAddressLookupButton(this.config.LABELS.FIND_ADDRESS_BUTTON, this.config.DATA_ATTRIBUTES.FIND_ADDRESS);
        button.addEventListener("click", event => onClick(event));
        return button;
    }

    createConfirmAddressButton(onClick) {
        const button = this.createAddressLookupButton(this.config.LABELS.CONFIRM_ADDRESS_BUTTON, this.config.DATA_ATTRIBUTES.CONFIRM_BUTTON);
        button.addEventListener("click", event => onClick(event));
        return button;
    }

    createAddressLookupButton(label, dataAddressLookupValue) {
        const button = document.createElement('button');
        button.setAttribute(this.config.ATTRIBUTES.BASE, dataAddressLookupValue);
        button.className = this.config.CSS_CLASSES.BUTTON_SECONDARY;
        button.innerText = label;
        button.setAttribute("type", "submit");

        return button;
    }

    createLink(linkText, dataAddressLookupValue) {
        const link = document.createElement('a');
        link.setAttribute(this.config.ATTRIBUTES.BASE, dataAddressLookupValue);
        link.classList = this.config.CSS_CLASSES.LINK;
        link.innerText = linkText;

        return link;
    }

    createLinkList(links) {
        const linkList = document.createElement('ul');
        linkList.setAttribute(this.config.ATTRIBUTES.BASE, this.config.DATA_ATTRIBUTES.LINK_LIST);
        linkList.className = this.config.CSS_CLASSES.LINK_LIST;
        if (links !== undefined && links.length !== 0) {
            links.forEach(link => {
                linkList.appendChild(link);
            });
        }

        return linkList;
    }

    createListItem(innerHtml) {
        const listItem = document.createElement('li');
        listItem.appendChild(innerHtml);
        return listItem;
    }

    createAddressSelect(labelText, addressOptions) {
        const formGroupContainer = document.createElement("div");
        formGroupContainer.classList = this.config.CSS_CLASSES.FORM_GROUP;

        const inputId = `${this.config.FIELD_NAMES.SELECT_ADDRESS}-${this.index}`;
        const label = document.createElement("label");
        label.classList = `${this.config.CSS_CLASSES.LABEL} govuk-label--l`;
        label.innerText = labelText;
        label.setAttribute("for", inputId);

        const select = document.createElement("select");
        select.setAttribute(this.config.ATTRIBUTES.BASE, this.config.DATA_ATTRIBUTES.SELECT_ADDRESS);
        select.classList = this.config.CSS_CLASSES.SELECT;
        select.name = inputId;
        select.id = inputId;

        const blankOption = new Option("", "", true, true);
        blankOption.selected = "selected";
        select.appendChild(blankOption);

        if (addressOptions !== undefined && addressOptions.length !== 0) {
            addressOptions.forEach(addressOption => {
                select.appendChild(addressOption);
            });
        }

        formGroupContainer.appendChild(label);
        formGroupContainer.appendChild(select);

        return new InputBuilder(formGroupContainer);
    }

    createOption(value, label) {
        const option = new Option(label, value);
        return option;
    }

    #getWidthCssClass(inputWidth) {
        let inputClass = "";
        switch (inputWidth) {
            case "xx-small":
                inputClass = "govuk-input--width-2";
                break;
            case "x-small":
                inputClass = "govuk-input--width-3";
                break;
            case "small":
                inputClass = "govuk-input--width-4";
                break;
            case "medium":
                inputClass = "govuk-input--width-5";
                break;
            case "large":
                inputClass = "govuk-input--width-10";
                break;
            case "x-large":
                inputClass = "govuk-input--width-20";
                break;
            case "xx-large":
                inputClass = "govuk-input--width-30";
                break;
            default:
                break;
        }

        return inputClass;
    }
}

export { AddressLookupComponentBuilder };