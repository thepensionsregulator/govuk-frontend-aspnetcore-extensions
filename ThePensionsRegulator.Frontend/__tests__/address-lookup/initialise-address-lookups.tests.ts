import { jest } from "@jest/globals";
import type { AddressLookupInstance } from "../../Scripts/address-lookup/types";

const createAddressLookup = jest.fn();

jest.unstable_mockModule("../../Scripts/address-lookup/address-lookup.js", () => ({
    createAddressLookup
}));

const { initialiseAddressLookups: initiliseAddressLookups } = await import("../../Scripts/address-lookup/initialise-address-lookups.js");

const contentMappings: ReadonlyArray<readonly [string, string]> = [
    ["addressLookupLegend", "addressLookupLegend"],
    ["addressLookupHint", "addressLookupHint"],
    ["noJsLegend", "addressLookupNoJsLegend"],
    ["searchLegend", "addressLookupSearchLegend"],
    ["addressLine1Label", "addressLookupAddressLine-1Label"],
    ["addressLine1RequiredErrorMessage", "addressLookupAddressLine-1RequiredErrorMessage"],
    ["addressLine1MaxLengthErrorMessage", "addressLookupAddressLine-1MaxLengthErrorMessage"],
    ["addressLine2Label", "addressLookupAddressLine-2Label"],
    ["addressLine2MaxLengthErrorMessage", "addressLookupAddressLine-2MaxLengthErrorMessage"],
    ["addressLine3Label", "addressLookupAddressLine-3Label"],
    ["addressLine3MaxLengthErrorMessage", "addressLookupAddressLine-3MaxLengthErrorMessage"],
    ["postTownLabel", "addressLookupPostTownLabel"],
    ["postTownRequiredErrorMessage", "addressLookupPostTownRequiredErrorMessage"],
    ["postTownMaxLengthErrorMessage", "addressLookupPostTownMaxLengthErrorMessage"],
    ["countyLabel", "addressLookupCountyLabel"],
    ["countyMaxLengthErrorMessage", "addressLookupCountyMaxLengthErrorMessage"],
    ["countyStateProvinceLabel", "addressLookupCountyStateProvinceLabel"],
    ["countyStateProvinceMaxLengthErrorMessage", "addressLookupCountyStateProvinceMaxLengthErrorMessage"],
    ["postcodeLabel", "addressLookupPostcodeLabel"],
    ["postcodeRequiredErrorMessage", "addressLookupPostcodeRequiredErrorMessage"],
    ["postcodeMaxLengthErrorMessage", "addressLookupPostcodeMaxLengthErrorMessage"],
    ["postcodePatternErrorMessage", "addressLookupPostcodePatternErrorMessage"],
    ["countryLabel", "addressLookupCountryLabel"],
    ["countryRequiredErrorMessage", "addressLookupCountryRequiredErrorMessage"],
    ["buildingNameOrNumberLabel", "addressLookupBuildingNameOrNumberLabel"],
    ["findAddressButton", "addressLookupFindAddressButton"],
    ["addressNotFoundMessage", "addressLookupAddressNotFoundMessage"],
    ["serviceUnavailableMessage", "addressLookupServiceUnavailableMessage"],
    ["searchNotConfirmedErrorMessage", "addressLookupSearchNotConfirmedErrorMessage"],
    ["addressSelectLabel", "addressLookupAddressSelectLabel"],
    ["confirmAddressLabel", "addressLookupConfirmAddressLabel"],
    ["backToPostcodeSearchLabel", "addressLookupBackToPostcodeSearchLabel"],
    ["addressSelectErrorMessage", "addressLookupAddressSelectErrorMessage"],
    ["resultsNotConfirmedErrorMessage", "addressLookupResultsNotConfirmedErrorMessage"],
    ["ukManualEntryLinkLabel", "addressLookupUkManualEntryLinkLabel"],
    ["ukManualEntryLegend", "addressLookupUkManualEntryLegend"],
    ["ukManualEntryNotConfirmedErrorMessage", "addressLookupUkManualEntryNotConfirmedErrorMessage"],
    ["internationalManualEntryLabel", "addressLookupInternationalManualEntryLabel"],
    ["internationalManualEntryLegend", "addressLookupInternationalManualEntryLegend"],
    ["internationalManualEntryNotConfirmedErrorMessage", "addressLookupInternationalManualEntryNotConfirmedErrorMessage"],
    ["changeAddressLabel", "addressLookupChangeAddressLabel"]
];

function createLookup(isConfirmed: boolean): AddressLookupInstance {
    return {
        element: document.createElement("div"),
        isConfirmed: jest.fn(() => isConfirmed),
        showNotConfirmedError: jest.fn()
    };
}

function renderForm(id: string, lookupCount: number): HTMLFormElement {
    const form = document.createElement("form");
    form.id = id;

    for (let index = 0; index < lookupCount; index++) {
        const target = document.createElement("div");
        target.className = "tpr-address-lookup";
        form.appendChild(target);
    }

    document.body.appendChild(form);
    return form;
}

describe("initialiseAddressLookups", () => {
    beforeEach(() => {
        document.body.replaceChildren();
        createAddressLookup.mockReset();
    });

    it("prevents submission and shows an error when a lookup is unconfirmed", () => {
        const lookup = createLookup(false);
        createAddressLookup.mockReturnValue(lookup);
        const form = renderForm("address-form", 1);

        initiliseAddressLookups();

        const event = new Event("submit", { bubbles: true, cancelable: true });
        form.dispatchEvent(event);

        expect(event.defaultPrevented).toBe(true);
        expect(lookup.showNotConfirmedError).toHaveBeenCalledTimes(1);
    });

    it("allows submission when every lookup is confirmed", () => {
        const lookup = createLookup(true);
        createAddressLookup.mockReturnValue(lookup);
        const form = renderForm("address-form", 1);

        initiliseAddressLookups();

        const event = new Event("submit", { bubbles: true, cancelable: true });
        form.dispatchEvent(event);

        expect(event.defaultPrevented).toBe(false);
        expect(lookup.showNotConfirmedError).not.toHaveBeenCalled();
    });

    it.each(contentMappings)("reads %s from generated HTML", (contentProperty, datasetProperty) => {
        const lookup = createLookup(true);
        createAddressLookup.mockReturnValue(lookup);
        const target = document.createElement("div");
        target.className = "tpr-address-lookup";
        const expectedValue = `Value for ${contentProperty}`;
        target.dataset[datasetProperty] = expectedValue;
        document.body.appendChild(target);

        initiliseAddressLookups();

        expect(createAddressLookup).toHaveBeenCalledWith(expect.objectContaining({
            content: expect.objectContaining({ [contentProperty]: expectedValue })
        }));
    });

    it("reads generated countries into the lookup options", () => {
        const lookup = createLookup(true);
        createAddressLookup.mockReturnValue(lookup);
        const target = document.createElement("div");
        target.className = "tpr-address-lookup";
        target.innerHTML = `
            <select>
                <option value="44">United Kingdom</option>
                <option value="250">France</option>
            </select>
        `;
        document.body.appendChild(target);

        initiliseAddressLookups();

        expect(createAddressLookup).toHaveBeenCalledWith(expect.objectContaining({
            countries: [
                { value: "44", name: "United Kingdom" },
                { value: "250", name: "France" }
            ]
        }));
    });

    it("shows errors only for the unconfirmed lookups in a form", () => {
        const confirmedLookup = createLookup(true);
        const unconfirmedLookup = createLookup(false);
        createAddressLookup
            .mockReturnValueOnce(confirmedLookup)
            .mockReturnValueOnce(unconfirmedLookup);
        const form = renderForm("address-form", 2);

        initiliseAddressLookups();

        const event = new Event("submit", { bubbles: true, cancelable: true });
        form.dispatchEvent(event);

        expect(event.defaultPrevented).toBe(true);
        expect(confirmedLookup.showNotConfirmedError).not.toHaveBeenCalled();
        expect(unconfirmedLookup.showNotConfirmedError).toHaveBeenCalledTimes(1);
    });

    it("keeps submission handling isolated to the submitted form", () => {
        const firstLookup = createLookup(false);
        const secondLookup = createLookup(false);
        createAddressLookup
            .mockReturnValueOnce(firstLookup)
            .mockReturnValueOnce(secondLookup);
        const firstForm = renderForm("first-address-form", 1);
        renderForm("second-address-form", 1);

        initiliseAddressLookups();

        const event = new Event("submit", { bubbles: true, cancelable: true });
        firstForm.dispatchEvent(event);

        expect(event.defaultPrevented).toBe(true);
        expect(firstLookup.showNotConfirmedError).toHaveBeenCalledTimes(1);
        expect(secondLookup.showNotConfirmedError).not.toHaveBeenCalled();
    });
});