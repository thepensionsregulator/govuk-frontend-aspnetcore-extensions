import "@testing-library/jest-dom";
import { jest } from "@jest/globals";
import { renderAddressUkManualEntry, validateUkManualEntryAddress } from "../../Scripts/address-lookup/address-uk-manual-entry.js";
import { createMockAddressLookupContent } from "../../__test-helpers__/mock-address-lookup-content.js";

function createValidValues(overrides: Partial<Parameters<typeof validateUkManualEntryAddress>[0]> = {}) {
    return {
        addressLine1: "1 High Street",
        addressLine2: "",
        addressLine3: "",
        townOrCity: "London",
        county: "",
        postcode: "SW1A 1AA",
        ...overrides
    };
}

describe("renderAddressUkManualEntry", () => {
    beforeEach(() => {
        jest.clearAllMocks();
    });
    const onAddressSubmitted = jest.fn();
    const onBackToSearchRequested = jest.fn();
    const content = createMockAddressLookupContent();
    it("renders the UK manual entry fields with the expected widths and secondary actions", () => {
        const component = renderAddressUkManualEntry({ onAddressSubmitted, onBackToSearchRequested, content });

        const addressLine1 = component.querySelector<HTMLInputElement>("#address-line-1");
        const addressLine2 = component.querySelector<HTMLInputElement>("#address-line-2");
        const addressLine3 = component.querySelector<HTMLInputElement>("#address-line-3");
        const townOrCity = component.querySelector<HTMLInputElement>("#town-or-city");
        const county = component.querySelector<HTMLInputElement>("#county");
        const postcode = component.querySelector<HTMLInputElement>("#postcode");
        const confirmButton = component.querySelector<HTMLButtonElement>("button");
        const backLink = component.querySelector<HTMLAnchorElement>("a");

        expect(addressLine1).toHaveAttribute("name", "address-line-1");
        expect(addressLine1).toHaveClass("govuk-input", "govuk-input--width-20");
        expect(addressLine2).toHaveClass("govuk-input", "govuk-input--width-20");
        expect(addressLine3).toHaveClass("govuk-input", "govuk-input--width-20");
        expect(townOrCity).toHaveClass("govuk-input", "govuk-input--width-10");
        expect(county).toHaveClass("govuk-input", "govuk-input--width-10");
        expect(postcode).toHaveClass("govuk-input", "govuk-input--width-5");

        expect(confirmButton).toHaveTextContent("Confirm address");
        expect(confirmButton).toHaveClass("govuk-button", "govuk-button--secondary");
        expect(confirmButton).toHaveAttribute("type", "button");
        expect(backLink).toHaveTextContent("Back to postcode search");
    });

    it("uses the configured confirm address label", () => {
        const customContent = {
            ...content,
            confirmAddressLabel: "Save this address"
        };
        const component = renderAddressUkManualEntry({ onAddressSubmitted, onBackToSearchRequested, content: customContent });

        expect(component.querySelector("button")).toHaveTextContent("Save this address");
    });

    it("calls the back link handler when the return link is clicked", () => {
        const component = renderAddressUkManualEntry({ onAddressSubmitted, onBackToSearchRequested, content });
        const backLink = component.querySelector<HTMLAnchorElement>("a");
        const clickEvent = new MouseEvent("click", { cancelable: true });

        backLink!.dispatchEvent(clickEvent);

        expect(clickEvent.defaultPrevented).toBe(true);
        expect(onBackToSearchRequested).toHaveBeenCalledTimes(1);
    });

    it("submits the valid address when the confirm button is clicked", () => {
        const component = renderAddressUkManualEntry({ onAddressSubmitted, onBackToSearchRequested, content });

        const addressLine1 = component.querySelector<HTMLInputElement>("#address-line-1");
        const townOrCity = component.querySelector<HTMLInputElement>("#town-or-city");
        const postcode = component.querySelector<HTMLInputElement>("#postcode");
        const confirmButton = component.querySelector<HTMLButtonElement>("button");

        addressLine1!.value = "1 High Street";
        townOrCity!.value = "london";
        postcode!.value = "sw1a 1aa";

        confirmButton!.click();

        expect(onAddressSubmitted).toHaveBeenCalledWith({
            addressLine1: "1 High Street",
            addressLine2: "",
            addressLine3: "",
            postTown: "london",
            county: "",
            postcode: "SW1A 1AA"
        });
        expect(onBackToSearchRequested).not.toHaveBeenCalled();
    });

    it("shows validation errors and does not submit when the confirm button is clicked with invalid data", () => {
        const component = renderAddressUkManualEntry({ onAddressSubmitted, onBackToSearchRequested, content });

        const addressLine1 = component.querySelector<HTMLInputElement>("#address-line-1");
        const townOrCity = component.querySelector<HTMLInputElement>("#town-or-city");
        const postcode = component.querySelector<HTMLInputElement>("#postcode");
        const confirmButton = component.querySelector<HTMLButtonElement>("button");

        addressLine1!.value = "";
        townOrCity!.value = "";
        postcode!.value = "NOTREAL";

        confirmButton!.click();

        expect(onAddressSubmitted).not.toHaveBeenCalled();
        expect(addressLine1).toHaveAttribute("aria-describedby", expect.stringContaining("address-line-1-error"));
        expect(townOrCity).toHaveAttribute("aria-describedby", expect.stringContaining("town-or-city-error"));
        expect(postcode).toHaveAttribute("aria-describedby", expect.stringContaining("postcode-error"));
        expect(component.querySelector("#address-line-1-error")).toHaveClass("govuk-error-message");
        expect(component.querySelector("#town-or-city-error")).toHaveClass("govuk-error-message");
        expect(component.querySelector("#postcode-error")).toHaveClass("govuk-error-message");
    });

    it("clears validation errors when corrected values are confirmed", () => {
        const component = renderAddressUkManualEntry({ onAddressSubmitted, onBackToSearchRequested, content });

        const addressLine1 = component.querySelector<HTMLInputElement>("#address-line-1");
        const townOrCity = component.querySelector<HTMLInputElement>("#town-or-city");
        const postcode = component.querySelector<HTMLInputElement>("#postcode");
        const confirmButton = component.querySelector<HTMLButtonElement>("button");

        confirmButton!.click();

        expect(component.querySelector("#address-line-1-error")).not.toBeNull();
        expect(component.querySelector("#town-or-city-error")).not.toBeNull();
        expect(component.querySelector("#postcode-error")).not.toBeNull();

        addressLine1!.value = "1 High Street";
        townOrCity!.value = "London";
        postcode!.value = "SW1A 1AA";

        confirmButton!.click();

        expect(onAddressSubmitted).toHaveBeenCalledTimes(1);
        expect(component.querySelector("#address-line-1-error")).toBeNull();
        expect(component.querySelector("#town-or-city-error")).toBeNull();
        expect(component.querySelector("#postcode-error")).toBeNull();
    });

    it("uses the fieldset form group as the submission-error anchor", () => {
        const component = renderAddressUkManualEntry({ onAddressSubmitted, onBackToSearchRequested, content });
        const fieldset = component.querySelector("fieldset");

        expect(fieldset?.parentElement).toHaveAttribute("data-address-lookup-submit-error-anchor");
    });
});

describe("validateUkManualEntryAddress", () => {
    const content = createMockAddressLookupContent();
    it("returns the mapped address when all required fields are valid", () => {
        const result = validateUkManualEntryAddress(createValidValues(), content);

        expect(result).toEqual({
            isValid: true,
            address: {
                addressLine1: "1 High Street",
                addressLine2: "",
                addressLine3: "",
                postTown: "London",
                county: "",
                postcode: "SW1A 1AA"
            }
        });
    });

    it("is valid when optional fields (address line 2, 3, county) are empty", () => {
        const result = validateUkManualEntryAddress(createValidValues({ addressLine2: "", addressLine3: "", county: "" }), content);
        expect(result.isValid).toBe(true);
    });

    it("is valid when optional fields are populated", () => {
        const result = validateUkManualEntryAddress(createValidValues({ addressLine2: "Flat 2", addressLine3: "The Mansions", county: "Greater London" }), content);
        expect(result.isValid).toBe(true);
    });

    it.each([
        ["addressLine1", { addressLine1: "" }, content.addressLine1RequiredErrorMessage],
        ["addressLine1", { addressLine1: "a".repeat(101) }, content.addressLine1MaxLengthErrorMessage],
        ["addressLine2", { addressLine2: "a".repeat(101) }, content.addressLine2MaxLengthErrorMessage],
        ["addressLine3", { addressLine3: "a".repeat(101) }, content.addressLine3MaxLengthErrorMessage],
        ["townOrCity", { townOrCity: "" }, content.postTownRequiredErrorMessage],
        ["townOrCity", { townOrCity: "a".repeat(101) }, content.postTownMaxLengthErrorMessage],
        ["county", { county: "a".repeat(101) }, content.countyMaxLengthErrorMessage],
        ["postcode", { postcode: "" }, content.postcodeRequiredErrorMessage],
        ["postcode", { postcode: "NOTREAL" }, content.postcodePatternErrorMessage]
    ] as const)("returns a %s error: %s", (fieldName, overrides, message) => {
        const result = validateUkManualEntryAddress(createValidValues(overrides), content);

        expect(result.isValid).toBe(false);
        if (!result.isValid) {
            expect(result.errorMessages).toEqual([{ fieldName, message }]);
        }
    });

    it("returns only the required-field error when the postcode is empty, not also the format or length error", () => {
        const result = validateUkManualEntryAddress(createValidValues({ postcode: "" }), content);

        expect(result.isValid).toBe(false);
        if (!result.isValid) {
            expect(result.errorMessages).toEqual([{ fieldName: "postcode", message: content.postcodeRequiredErrorMessage }]);
        }
    });

    it("returns one error per failing field when multiple fields are invalid", () => {
        const result = validateUkManualEntryAddress(createValidValues({ addressLine1: "", townOrCity: "", postcode: "" }), content);

        expect(result.isValid).toBe(false);
        if (!result.isValid) {
            expect(result.errorMessages).toEqual([
                { fieldName: "addressLine1", message: content.addressLine1RequiredErrorMessage },
                { fieldName: "townOrCity", message: content.postTownRequiredErrorMessage },
                { fieldName: "postcode", message: content.postcodeRequiredErrorMessage }
            ]);
        }
    });
});
