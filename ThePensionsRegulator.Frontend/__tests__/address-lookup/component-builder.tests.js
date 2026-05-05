import '@testing-library/jest-dom';
import { jest } from '@jest/globals';
import { AddressLookupComponentBuilder } from "../../wwwroot/tpr/address-lookup/component-builder.js";
import { ADDRESS_LOOKUP_CONFIG } from "../../wwwroot/tpr/address-lookup/config.js";

describe("AddressLookupComponentBuilder", () => {
    let builder;
    const testIndex = 0;

    beforeEach(() => {
        builder = new AddressLookupComponentBuilder(testIndex, ADDRESS_LOOKUP_CONFIG);
        document.body.innerHTML = '';
    });

    describe("createGovukTextInput", () => {
        it("should create a form group with correct CSS class", () => {
            const inputBuilder = builder.createGovukTextInput("Test Label", "test-field", "medium");
            const formGroup = inputBuilder.build();

            expect(formGroup.classList.contains("govuk-form-group")).toBe(true);
        });

        it("should create a label with correct text and attributes", () => {
            const labelText = "Test Label";
            const dataValue = "test-field";
            const inputBuilder = builder.createGovukTextInput(labelText, dataValue, "medium");
            const formGroup = inputBuilder.build();
            const label = formGroup.querySelector("label");

            expect(label).not.toBeNull();
            expect(label.innerText).toBe(labelText);
            expect(label.getAttribute("for")).toBe(`${dataValue}-${testIndex}`);
            expect(label.classList.contains("govuk-label")).toBe(true);
        });

        it("should create an input with correct attributes", () => {
            const dataValue = "test-field";
            const inputBuilder = builder.createGovukTextInput("Test Label", dataValue, "medium");
            const formGroup = inputBuilder.build();
            const input = formGroup.querySelector("input");

            expect(input).not.toBeNull();
            expect(input.getAttribute("data-address-lookup")).toBe(dataValue);
            expect(input.getAttribute("name")).toBe(dataValue);
            expect(input.getAttribute("id")).toBe(`${dataValue}-${testIndex}`);
            expect(input.classList.contains("govuk-input")).toBe(true);
        });

        it.each([
            ["xx-small", "govuk-input--width-2"],
            ["x-small", "govuk-input--width-3"],
            ["small", "govuk-input--width-4"],
            ["medium", "govuk-input--width-5"],
            ["large", "govuk-input--width-10"],
            ["x-large", "govuk-input--width-20"],
            ["xx-large", "govuk-input--width-30"]
        ])("should apply correct width class for '%s' input width", (inputWidth, expectedClass) => {
            const inputBuilder = builder.createGovukTextInput("Test Label", "test-field", inputWidth);
            const formGroup = inputBuilder.build();
            const input = formGroup.querySelector("input");

            expect(input.classList.contains(expectedClass)).toBe(true);
        });

        it("should not apply width class when input width is not specified", () => {
            const inputBuilder = builder.createGovukTextInput("Test Label", "test-field", "invalid-width");
            const formGroup = inputBuilder.build();
            const input = formGroup.querySelector("input");

            expect(input.className).toBe("govuk-input ");
        });

        it("should return an InputBuilder instance", () => {
            const inputBuilder = builder.createGovukTextInput("Test Label", "test-field", "medium");

            expect(inputBuilder.constructor.name).toBe("InputBuilder");
        });
    });

    describe("createFieldset", () => {
        it("should create a fieldset with correct CSS class", () => {
            const fieldset = builder.createFieldset("Test Legend");

            expect(fieldset.tagName).toBe("FIELDSET");
            expect(fieldset.classList.contains("govuk-fieldset")).toBe(true);
        });

        it("should create a legend with correct text and CSS classes", () => {
            const legendText = "Test Legend";
            const fieldset = builder.createFieldset(legendText);
            const legend = fieldset.querySelector("legend");

            expect(legend).not.toBeNull();
            expect(legend.innerText).toBe(legendText);
            expect(legend.classList.contains("govuk-fieldset__legend")).toBe(true);
            expect(legend.classList.contains("govuk-fieldset__legend--s")).toBe(true);
        });
    });

    describe("createConfirmedAddressParagraph", () => {
        it("should create a paragraph with correct CSS class", () => {
            const paragraph = builder.createConfirmedAddressParagraph(["Line 1"], "BN1 4DW");

            expect(paragraph.tagName).toBe("P");
            expect(paragraph.classList.contains("govuk-body")).toBe(true);
        });

        it("should add each address line with a line break", () => {
            const address = ["Line 1", "Line 2", "Brighton"];
            const paragraph = builder.createConfirmedAddressParagraph(address, "BN1 4DW");

            const brElements = paragraph.querySelectorAll("br");
            expect(brElements.length).toBe(3);
        });

        it("should convert address lines to title case except postcode", () => {
            const address = ["UPPER CASE STREET", "BN1 4DW"];
            const paragraph = builder.createConfirmedAddressParagraph(address, "BN1 4DW");

            expect(paragraph.textContent).toContain("Upper Case Street");
            expect(paragraph.textContent).toContain("BN1 4DW");
        });

        it("should contain the default country", () => {
            const address = ["Line 1", "United Kingdom", "BN1 4DW"];
            const paragraph = builder.createConfirmedAddressParagraph(address, "BN1 4DW");

            expect(paragraph.textContent).toContain("United Kingdom");
        });

        it("should trim whitespace from address lines", () => {
            const address = ["  Line 1  ", "  Line 2  "];
            const paragraph = builder.createConfirmedAddressParagraph(address, "BN1 4DW");

            expect(paragraph.textContent).toContain("Line 1");
            expect(paragraph.textContent).toContain("Line 2");
        });
    });

    describe("createFindAddressButton", () => {
        it("should create a button with correct label", () => {
            const onClick = jest.fn();
            const button = builder.createFindAddressButton(onClick);

            expect(button.innerText).toBe(ADDRESS_LOOKUP_CONFIG.LABELS.FIND_ADDRESS_BUTTON);
        });

        it("should create a button with correct data attribute", () => {
            const onClick = jest.fn();
            const button = builder.createFindAddressButton(onClick);

            expect(button.getAttribute("data-address-lookup")).toBe(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.FIND_ADDRESS);
        });

        it("should attach click event listener", () => {
            const onClick = jest.fn();
            const button = builder.createFindAddressButton(onClick);

            button.click();

            expect(onClick).toHaveBeenCalledTimes(1);
        });

        it("should pass event to onClick handler", () => {
            const onClick = jest.fn();
            const button = builder.createFindAddressButton(onClick);

            button.click();

            expect(onClick).toHaveBeenCalledWith(expect.any(MouseEvent));
        });
    });

    describe("createConfirmAddressButton", () => {
        it("should create a button with correct label", () => {
            const onClick = jest.fn();
            const button = builder.createConfirmAddressButton(onClick);

            expect(button.innerText).toBe(ADDRESS_LOOKUP_CONFIG.LABELS.CONFIRM_ADDRESS_BUTTON);
        });

        it("should create a button with correct data attribute", () => {
            const onClick = jest.fn();
            const button = builder.createConfirmAddressButton(onClick);

            expect(button.getAttribute("data-address-lookup")).toBe(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.CONFIRM_BUTTON);
        });

        it("should attach click event listener", () => {
            const onClick = jest.fn();
            const button = builder.createConfirmAddressButton(onClick);

            button.click();

            expect(onClick).toHaveBeenCalledTimes(1);
        });

        it("should pass event to onClick handler", () => {
            const onClick = jest.fn();
            const button = builder.createConfirmAddressButton(onClick);

            button.click();

            expect(onClick).toHaveBeenCalledWith(expect.any(MouseEvent));
        });
    });

    describe("createAddressLookupButton", () => {
        it("should create a button element", () => {
            const button = builder.createAddressLookupButton("Test Button", "test-value");

            expect(button.tagName).toBe("BUTTON");
        });

        it("should set the button label", () => {
            const label = "Test Button";
            const button = builder.createAddressLookupButton(label, "test-value");

            expect(button.innerText).toBe(label);
        });

        it("should set the data attribute", () => {
            const dataValue = "test-value";
            const button = builder.createAddressLookupButton("Test Button", dataValue);

            expect(button.getAttribute("data-address-lookup")).toBe(dataValue);
        });

        it("should have secondary button CSS classes", () => {
            const button = builder.createAddressLookupButton("Test Button", "test-value");

            expect(button.classList.contains("govuk-button")).toBe(true);
            expect(button.classList.contains("govuk-button--secondary")).toBe(true);
        });

        it("should have type button", () => {
            const button = builder.createAddressLookupButton("Test Button", "test-value");

            expect(button.getAttribute("type")).toBe("button");
        });
    });

    describe("createLink", () => {
        it("should create an anchor element", () => {
            const link = builder.createLink("Test Link", "test-value");

            expect(link.tagName).toBe("A");
        });

        it("should set the link text", () => {
            const linkText = "Test Link";
            const link = builder.createLink(linkText, "test-value");

            expect(link.innerText).toBe(linkText);
        });

        it("should set the data attribute", () => {
            const dataValue = "test-value";
            const link = builder.createLink("Test Link", dataValue);

            expect(link.getAttribute("data-address-lookup")).toBe(dataValue);
        });

        it("should have govuk-link CSS class", () => {
            const link = builder.createLink("Test Link", "test-value");

            expect(link.classList.contains("govuk-link")).toBe(true);
        });

        it("should have href set to #", () => {
            const link = builder.createLink("Test Link", "test-value");

            expect(link.getAttribute("href")).toBe("#");
        });
    });

    describe("createLinkList", () => {
        it("should create an unordered list element", () => {
            const linkList = builder.createLinkList([]);

            expect(linkList.tagName).toBe("UL");
        });

        it("should set the data attribute", () => {
            const linkList = builder.createLinkList([]);

            expect(linkList.getAttribute("data-address-lookup")).toBe(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.LINK_LIST);
        });

        it("should have correct CSS classes", () => {
            const linkList = builder.createLinkList([]);

            expect(linkList.className).toBe("govuk-list tpr-address-lookup__links");
        });

        it("should handle empty links array", () => {
            const linkList = builder.createLinkList([]);

            expect(linkList.children.length).toBe(0);
        });

        it("should handle undefined links", () => {
            const linkList = builder.createLinkList(undefined);

            expect(linkList.children.length).toBe(0);
        });

        it("should create list items for each link", () => {
            const link1 = builder.createLink("Link 1", "link-1");
            const link2 = builder.createLink("Link 2", "link-2");
            const linkList = builder.createLinkList([link1, link2]);

            expect(linkList.children.length).toBe(2);
            expect(linkList.children[0].tagName).toBe("LI");
            expect(linkList.children[1].tagName).toBe("LI");
        });
    });

    describe("createListItem", () => {
        it("should create a list item element", () => {
            const content = document.createElement("span");
            const listItem = builder.createListItem(content);

            expect(listItem.tagName).toBe("LI");
        });

        it("should append the inner HTML to the list item", () => {
            const content = document.createElement("a");
            content.innerText = "Test Content";
            const listItem = builder.createListItem(content);

            expect(listItem.children.length).toBe(1);
            expect(listItem.querySelector("a")).toBe(content);
        });
    });

    describe("createGovukSelect", () => {
        it("should create a form group with correct CSS class", () => {
            const inputBuilder = builder.createGovukSelect("Choose address", "select", []);
            const formGroup = inputBuilder.build();

            expect(formGroup.classList.contains("govuk-form-group")).toBe(true);
        });

        it("should create a label with correct text and attributes", () => {
            const labelText = "Choose address";
            const dataValue = "select-address";
            const inputBuilder = builder.createGovukSelect(labelText, dataValue, []);
            const formGroup = inputBuilder.build();
            const label = formGroup.querySelector("label");

            expect(label).not.toBeNull();
            expect(label.innerText).toBe(labelText);
            expect(label.getAttribute("for")).toBe(`${dataValue}-${testIndex}`);
            expect(label.classList.contains("govuk-label")).toBe(true);
        });

        it("should not add a label size class when labelSize is not provided", () => {
            const inputBuilder = builder.createGovukSelect("Choose address", "select", []);
            const formGroup = inputBuilder.build();
            const label = formGroup.querySelector("label");

            expect(label.classList.contains("govuk-label")).toBe(true);
            expect(label.classList.length).toBe(1);
        });

        it("should add the label size class when labelSize is provided", () => {
            const inputBuilder = builder.createGovukSelect("Choose address", "select", [], "govuk-label--l");
            const formGroup = inputBuilder.build();
            const label = formGroup.querySelector("label");

            expect(label.classList.contains("govuk-label")).toBe(true);
            expect(label.classList.contains("govuk-label--l")).toBe(true);
        });

        it.each([
            ["govuk-label--s"],
            ["govuk-label--m"],
            ["govuk-label--l"],
            ["govuk-label--xl"]
        ])("should apply '%s' label size class when provided", (labelSize) => {
            const inputBuilder = builder.createGovukSelect("Label", "select", [], labelSize);
            const formGroup = inputBuilder.build();
            const label = formGroup.querySelector("label");

            expect(label.classList.contains(labelSize)).toBe(true);
        });

        it("should create a select element with correct attributes", () => {
            const dataValue = "select-address";
            const inputBuilder = builder.createGovukSelect("Choose address", dataValue, []);
            const formGroup = inputBuilder.build();
            const select = formGroup.querySelector("select");

            expect(select).not.toBeNull();
            expect(select.getAttribute("data-address-lookup")).toBe(dataValue);
            expect(select.getAttribute("data-address-lookup")).toBe(ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.SELECT_ADDRESS);
            expect(select.classList.contains("govuk-select")).toBe(true);
            expect(select.name).toBe(`${dataValue}-${testIndex}`);
            expect(select.id).toBe(`${dataValue}-${testIndex}`);
        });

        it("should create a blank option as the first option if options doesnt provide one", () => {
            const option = builder.createOption("1", "Address 1");
            const inputBuilder = builder.createGovukSelect("Choose address", "select", [ option ]);
            const formGroup = inputBuilder.build();
            const select = formGroup.querySelector("select");

            expect(select.options.length).toBeGreaterThan(0);
            expect(select.options[0].value).toBe("");
            expect(select.options[0].text).toBe("");
            expect(select.options[0].selected).toBe(true);
        });

        it("should handle empty address options array", () => {
            const inputBuilder = builder.createGovukSelect("Choose address", "select", []);
            const formGroup = inputBuilder.build();
            const select = formGroup.querySelector("select");

            expect(select.options.length).toBe(0);
        });

        it("should handle undefined address options", () => {
            const inputBuilder = builder.createGovukSelect("Choose address", "select", undefined);
            const formGroup = inputBuilder.build();
            const select = formGroup.querySelector("select");

            expect(select.options.length).toBe(0);
        });

        it("should add all provided address options", () => {
            const option1 = builder.createOption("1", "Address 1");
            const option2 = builder.createOption("2", "Address 2");
            const inputBuilder = builder.createGovukSelect("Choose address", "select", [option1, option2]);
            const formGroup = inputBuilder.build();
            const select = formGroup.querySelector("select");

            expect(select.options.length).toBe(3);
            expect(select.options[1].value).toBe("1");
            expect(select.options[1].text).toBe("Address 1");
            expect(select.options[2].value).toBe("2");
            expect(select.options[2].text).toBe("Address 2");
        });

        it("should return an InputBuilder instance", () => {
            const inputBuilder = builder.createGovukSelect("Choose address", "select", []);

            expect(inputBuilder.constructor.name).toBe("InputBuilder");
        });
    });

    describe("createOption", () => {
        it("should create an option element", () => {
            const option = builder.createOption("test-value", "Test Label");

            expect(option.tagName).toBe("OPTION");
        });

        it("should set the option value", () => {
            const value = "test-value";
            const option = builder.createOption(value, "Test Label");

            expect(option.value).toBe(value);
        });

        it("should set the option text", () => {
            const label = "Test Label";
            const option = builder.createOption("test-value", label);

            expect(option.text).toBe(label);
        });
    });

    describe("createHiddenInput", () => {
        it("should create an input element", () => {
            const input = builder.createHiddenInput("test-name", "test-id", "test-value");

            expect(input.tagName).toBe("INPUT");
        });

        it("should set type to hidden", () => {
            const input = builder.createHiddenInput("test-name", "test-id", "test-value");

            expect(input.getAttribute("type")).toBe("hidden");
        });

        it("should set the name attribute", () => {
            const name = "test-name";
            const input = builder.createHiddenInput(name, "test-id", "test-value");

            expect(input.getAttribute("name")).toBe(name);
        });

        it("should set the id attribute", () => {
            const id = "test-id";
            const input = builder.createHiddenInput("test-name", id, "test-value");

            expect(input.getAttribute("id")).toBe(id);
        });

        it("should set the value", () => {
            const value = "test-value";
            const input = builder.createHiddenInput("test-name", "test-id", value);

            expect(input.value).toBe(value);
        });

        it("should set empty string when value is undefined", () => {
            const input = builder.createHiddenInput("test-name", "test-id", undefined);

            expect(input.value).toBe("");
        });

        it("should set empty string when value is not provided", () => {
            const input = builder.createHiddenInput("test-name", "test-id");

            expect(input.value).toBe("");
        });
    });

    describe("createCheckbox", () => {
        it("should create a form group with correct CSS class", () => {
            const formGroup = builder.createCheckbox("Test Label");

            expect(formGroup.classList.contains("govuk-form-group")).toBe(true);
        });

        it("should create a checkboxes container with correct CSS class", () => {
            const formGroup = builder.createCheckbox("Test Label");
            const checkboxes = formGroup.querySelector(".govuk-checkboxes");

            expect(checkboxes).not.toBeNull();
        });

        it("should create a checkboxes item with correct CSS class", () => {
            const formGroup = builder.createCheckbox("Test Label");
            const checkboxesItem = formGroup.querySelector(".govuk-checkboxes__item");

            expect(checkboxesItem).not.toBeNull();
        });

        it("should create a checkbox input with correct attributes", () => {
            const formGroup = builder.createCheckbox("Test Label");
            const checkbox = formGroup.querySelector("input[type='checkbox']");

            expect(checkbox).not.toBeNull();
            expect(checkbox.getAttribute("type")).toBe("checkbox");
            expect(checkbox.getAttribute("name")).toBe(`${ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.SAME_AS}-${testIndex}`);
            expect(checkbox.getAttribute("id")).toBe(`${ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.SAME_AS}-${testIndex}`);
            expect(checkbox.classList.contains("govuk-checkboxes__input")).toBe(true);
        });

        it("should create a label with correct text and attributes", () => {
            const labelText = "Test Label";
            const formGroup = builder.createCheckbox(labelText);
            const label = formGroup.querySelector("label");

            expect(label).not.toBeNull();
            expect(label.innerText).toBe(labelText);
            expect(label.getAttribute("for")).toBe(`${ADDRESS_LOOKUP_CONFIG.DATA_ATTRIBUTES.SAME_AS}-${testIndex}`);
            expect(label.classList.contains("govuk-checkboxes__label")).toBe(true);
            expect(label.classList.contains("govuk-label")).toBe(true);
        });

        it("should structure elements correctly with checkbox before label", () => {
            const formGroup = builder.createCheckbox("Test Label");
            const checkboxesItem = formGroup.querySelector(".govuk-checkboxes__item");
            const children = Array.from(checkboxesItem.children);

            expect(children[0].tagName).toBe("INPUT");
            expect(children[1].tagName).toBe("LABEL");
        });
    });
});
