import { jest } from "@jest/globals";
import type { AddressLookupInstance, AddressLookupOptions } from "../../Scripts/address-lookup/types";
import { AddressLookupStateMachine } from "../../Scripts/address-lookup/address-lookup-state-machine";
import { createMockAddressLookupContent } from "../../__test-helpers__/mock-address-lookup-content";

const createAddressLookup = jest.fn<(options: AddressLookupOptions) => AddressLookupInstance>();

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
    ["countrySelectPlaceholder", "addressLookupCountrySelectPlaceholder"],
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
    ["changeAddressLabel", "addressLookupChangeAddressLabel"],
    ["sameAsLabel", "addressLookupSameAsLabel"]
];

function createLookup(isConfirmed: boolean): AddressLookupInstance {
    return {
        element: document.createElement("div"),
        stateMachine: new AddressLookupStateMachine(createMockAddressLookupContent()),
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

    it("passes the same-form primary state machine and configured settings to a secondary", () => {
        const primary = createLookup(true);
        const secondary = createLookup(false);
        createAddressLookup.mockReturnValueOnce(primary).mockReturnValueOnce(secondary);
        const form = renderForm("address-form", 2);
        const targets = form.querySelectorAll<HTMLElement>(".tpr-address-lookup");
        targets[0].dataset.addressLookupRole = "Primary";
        targets[1].dataset.addressLookupRole = "Secondary";
        targets[1].dataset.addressLookupSearchUrl = "/api/secondary-search";

        initiliseAddressLookups();

        expect(createAddressLookup.mock.calls[0][0].settings).toEqual({ searchEndpoint: "", role: "Primary" });
        expect(createAddressLookup.mock.calls[1][0]).toMatchObject({
            settings: { searchEndpoint: "/api/secondary-search", role: "Secondary" },
            stateMachine: primary.stateMachine
        });
    });

    it("uses each form's own primary when both forms contain primary and secondary lookups", () => {
        const firstPrimary = createLookup(true);
        const secondPrimary = createLookup(true);
        createAddressLookup.mockReturnValueOnce(firstPrimary).mockReturnValueOnce(createLookup(false))
            .mockReturnValueOnce(secondPrimary).mockReturnValueOnce(createLookup(false));
        for (const id of ["first-form", "second-form"]) {
            const form = renderForm(id, 2);
            form.querySelectorAll<HTMLElement>(".tpr-address-lookup")[1].dataset.addressLookupRole = "Secondary";
        }

        initiliseAddressLookups();

        expect(createAddressLookup.mock.calls[1][0].stateMachine).toBe(firstPrimary.stateMachine);
        expect(createAddressLookup.mock.calls[3][0].stateMachine).toBe(secondPrimary.stateMachine);
    });


    it("reads named fields, trims initial values and resolves the selected country name", () => {
        createAddressLookup.mockReturnValue(createLookup(true));
        const form = renderForm("address-form", 1);
        form.querySelector(".tpr-address-lookup")!.innerHTML = `
            <input data-address-field="addressLine1" name="Office.Line1" value="  1 High Street  ">
            <input data-address-field="addressLine2" name="Office.Line2" value="   ">
            <input data-address-field="unknown" name="ignored" value="ignored">
            <input data-address-field name="empty-key" value="ignored">
            <select data-address-field="countryId" name="Office.Country">
                <option value="">Choose a country</option>
                <option value="not-a-number">Invalid entry</option>
                <option value="44" selected>United Kingdom</option>
            </select>`;

        initiliseAddressLookups();

        const options = createAddressLookup.mock.calls[0][0];
        expect(options.fields).toEqual({
            addressLine1: { name: "Office.Line1", value: "1 High Street" },
            addressLine2: { name: "Office.Line2", value: "" },
            countryId: { name: "Office.Country", value: "44" }
        });
        expect(options.initialAddress).toMatchObject({
            addressLine1: "1 High Street", addressLine2: undefined, countryId: "44", countryName: "United Kingdom"
        });
        expect(options.countries).toEqual([{ value: "44", name: "United Kingdom" }]);
    });

    it("replaces only the lookup view, including when no form contains the lookup", () => {
        const lookup = createLookup(false);
        createAddressLookup.mockReturnValue(lookup);
        const target = document.createElement("div");
        target.className = "tpr-address-lookup";
        target.innerHTML = '<p>Keep this content</p><div class="tpr-address-lookup-view"><span>Old view</span></div>';
        document.body.appendChild(target);

        initiliseAddressLookups();

        expect(target.querySelector(".tpr-address-lookup-view")!.children).toHaveLength(1);
        expect(target.querySelector(".tpr-address-lookup-view")!.firstElementChild).toBe(lookup.element);
        expect(target.querySelector("p")!.textContent).toBe("Keep this content");
        expect(lookup.showNotConfirmedError).not.toHaveBeenCalled();
    });

    it("allows a later submission after an unconfirmed lookup becomes confirmed", () => {
        const lookup = createLookup(false);
        let confirmed = false;
        lookup.isConfirmed = () => confirmed;
        createAddressLookup.mockReturnValue(lookup);
        const form = renderForm("address-form", 1);
        initiliseAddressLookups();
        const firstSubmit = new Event("submit", { cancelable: true });
        form.dispatchEvent(firstSubmit);
        expect(firstSubmit.defaultPrevented).toBe(true);

        confirmed = true;
        const secondSubmit = new Event("submit", { cancelable: true });
        form.dispatchEvent(secondSubmit);

        expect(secondSubmit.defaultPrevented).toBe(false);
        expect(lookup.showNotConfirmedError).toHaveBeenCalledTimes(1);
    });
});