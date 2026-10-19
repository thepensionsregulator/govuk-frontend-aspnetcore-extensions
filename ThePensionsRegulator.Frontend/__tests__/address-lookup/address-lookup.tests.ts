import { jest } from "@jest/globals";
import "@testing-library/jest-dom";
import { createAddressLookup } from "../../Scripts/address-lookup/address-lookup";
import type { AddressFields, AddressLookupSettings, InitialAddress } from "../../Scripts/address-lookup/types";
import type { AddressLookupEvent } from "../../Scripts/address-lookup/address-lookup-state-machine";
import { createMockAddressLookupContent } from "../../__test-helpers__/mock-address-lookup-content";

describe("createAddressLookup", () => {
    let originalFetch: typeof globalThis.fetch;

    beforeEach(() => {
        originalFetch = globalThis.fetch;
        // jsdom doesn't provide a fetch global at all, so give every test a harmless default.
        globalThis.fetch = jest.fn<typeof globalThis.fetch>().mockResolvedValue({
            ok: true,
            json: () => Promise.resolve({ results: [] })
        } as Response);
    });

    afterEach(() => {
        globalThis.fetch = originalFetch;
    });

    // Stub rather than a real Response, since jsdom doesn't provide the Fetch API globals.
    function mockFetchResults(addresses: Record<string, unknown>[]): jest.Mock<typeof globalThis.fetch> {
        const fetchMock = jest.fn<typeof globalThis.fetch>().mockResolvedValue({
            ok: true,
            json: () => Promise.resolve({ results: addresses.map(address => ({ DPA: address })) })
        } as Response);
        globalThis.fetch = fetchMock;
        return fetchMock as unknown as jest.Mock<typeof globalThis.fetch>;
    }

    // Flushes all pending microtasks (fetch -> response.json() -> service -> view -> dispatch),
    // however deep the awaited chain, unlike a fixed number of Promise.resolve() hops.
    function flushPromises(): Promise<void> {
        return new Promise(resolve => setTimeout(resolve, 0));
    }

    const fields: AddressFields = {
        addressLine1: { name: "addressLine1", value: "" },
        addressLine2: { name: "addressLine2", value: "" },
        addressLine3: { name: "addressLine3", value: "" },
        postTown: { name: "postTown", value: "" },
        postCounty: { name: "postCounty", value: "" },
        countryId: { name: "countryId", value: "" },
        postcode: { name: "postcode", value: "" },
        uprnReference: { name: "uprnReference", value: "" }
    };
    const content = createMockAddressLookupContent();
    const settings: AddressLookupSettings = { searchEndpoint: "/api/address-search", role: "Primary" };
    const countries = [
        { value: "GB", name: "United Kingdom" },
        { value: "FR", name: "France" }
    ];
    it("should return an HTMLElement", () => {
        const addressLookupInstance = createAddressLookup({ settings, fields, content });

        expect(addressLookupInstance.element).toBeInstanceOf(HTMLElement);
    });

    it("should render the search form initially", () => {
        const addressLookupInstance = createAddressLookup({ settings, fields, content });
        const component = addressLookupInstance.element;

        expect(component.querySelector("#building")).not.toBeNull();
        expect(component.querySelector("#postcode")).not.toBeNull();
        expect(component.querySelector("button")).not.toBeNull();
    });

    it("should show the search not-confirmed error", () => {
        const addressLookupInstance = createAddressLookup({ settings, fields, content });

        addressLookupInstance.showNotConfirmedError();

        expect(addressLookupInstance.element.querySelector("#address-search-fieldset-error"))
            .toHaveTextContent(`Error: ${content.searchNotConfirmedErrorMessage}`);
    });

    it("should use the UK country option to validate a pre-filled UK address", () => {
        const addressLookupInstance = createAddressLookup({
            settings,
            fields,
            content,
            countries,
            initialAddress: {
                addressLine1: "1 High Street",
                postTown: "London",
                countryId: "GB",
                countryName: "United Kingdom",
                postcode: "NOTREAL"
            }
        });

        expect(addressLookupInstance.isConfirmed()).toBe(false);
        expect(addressLookupInstance.element.querySelector("#postcode")).not.toBeNull();
    });

    it("should initialize a pre-filled international address as confirmed", () => {
        const addressLookupInstance = createAddressLookup({
            settings,
            fields,
            content,
            countries,
            initialAddress: {
                addressLine1: "123 Rue de Rivoli",
                postTown: "Paris",
                countryId: "FR",
                countryName: "France",
                postcode: "75001"
            }
        });

        expect(addressLookupInstance.isConfirmed()).toBe(true);
        expect(addressLookupInstance.element.querySelector("p")).toHaveTextContent("France");
    });

    it("should search using the configured search endpoint", async () => {
        const fetchMock = mockFetchResults([]);
        const addressLookupInstance = createAddressLookup({ settings, fields, content });
        const component = addressLookupInstance.element;
        const postcodeInput = component.querySelector<HTMLInputElement>("#postcode")!;
        const button = component.querySelector<HTMLButtonElement>("button")!;
        postcodeInput.value = "SW1A 2AA";

        button.click();
        await flushPromises();

        const [requestedUrl] = fetchMock.mock.calls[0] as unknown as [URL, RequestInit];
        expect(requestedUrl.pathname).toBe("/api/address-search");
        expect(requestedUrl.searchParams.get("postcode")).toBe("SW1A 2AA");
    });

    it("should render the results view when multiple addresses are found", async () => {
        mockFetchResults([
            { UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", BUILDING_NUMBER: "1", THOROUGHFARE_NAME: "High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" },
            { UPRN: "2", UDPRN: "2", ADDRESS: "2 High Street", BUILDING_NUMBER: "2", THOROUGHFARE_NAME: "High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" }
        ]);
        const addressLookupInstance = createAddressLookup({ settings, fields, content });
        const component = addressLookupInstance.element;
        const postcodeInput = component.querySelector<HTMLInputElement>("#postcode")!;
        const button = component.querySelector<HTMLButtonElement>("button")!;
        postcodeInput.value = "SW1A 2AA";

        button.click();
        await flushPromises();

        expect(component.querySelector("#postcode")).toBeNull();
        expect(component.querySelector("#address-select")).not.toBeNull();
        expect(addressLookupInstance.isConfirmed()).toBe(false);
    });

    it("should only ever show one view's markup at a time", async () => {
        mockFetchResults([
            { UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" },
            { UPRN: "2", UDPRN: "2", ADDRESS: "2 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" }
        ]);
        const addressLookupInstance = createAddressLookup({ settings, fields, content });
        const component = addressLookupInstance.element;
        const postcodeInput = component.querySelector<HTMLInputElement>("#postcode")!;
        const button = component.querySelector<HTMLButtonElement>("button")!;
        postcodeInput.value = "SW1A 2AA";

        button.click();
        await flushPromises();

        expect(component.children).toHaveLength(1);
    });

    it("should confirm the selected address from the results view", async () => {
        mockFetchResults([
            { UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", BUILDING_NUMBER: "1", THOROUGHFARE_NAME: "High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" },
            { UPRN: "2", UDPRN: "2", ADDRESS: "2 High Street", BUILDING_NUMBER: "2", THOROUGHFARE_NAME: "High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" }
        ]);
        const addressLookupInstance = createAddressLookup({ settings, fields, content });
        const component = addressLookupInstance.element;
        const postcodeInput = component.querySelector<HTMLInputElement>("#postcode")!;
        postcodeInput.value = "SW1A 2AA";
        component.querySelector<HTMLButtonElement>("button")!.click();
        await flushPromises();

        const select = component.querySelector<HTMLSelectElement>("#address-select")!;
        select.value = "2";
        component.querySelector<HTMLButtonElement>("button")!.click();

        expect(component.querySelector("p")).toHaveTextContent("2 High StreetLondonSW1A 2AA");
        expect(component.querySelector<HTMLInputElement>("#addressLine1")).toHaveValue("2 High Street");
        expect(component.querySelector<HTMLInputElement>("#postcode")).toHaveValue("SW1A 2AA");
        expect(component.querySelector<HTMLInputElement>("#uprnReference")).toHaveValue("2");
                expect(addressLookupInstance.isConfirmed()).toBe(true);
    });

    it("should render the confirmed view when a single address is found", async () => {
        mockFetchResults([
            { UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", BUILDING_NUMBER: "1", THOROUGHFARE_NAME: "High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" }
        ]);
        const addressLookupInstance = createAddressLookup({ settings, fields, content });
        const component = addressLookupInstance.element;
        const postcodeInput = component.querySelector<HTMLInputElement>("#postcode")!;
        const button = component.querySelector<HTMLButtonElement>("button")!;
        postcodeInput.value = "SW1A 2AA";

        button.click();
        await flushPromises();

        expect(component.querySelector("p")).toHaveTextContent("1 High StreetLondonSW1A 2AA");
        expect(component.querySelector<HTMLInputElement>("#addressLine1")).toHaveValue("1 High Street");
        expect(component.querySelector<HTMLInputElement>("#postcode")).toHaveValue("SW1A 2AA");
        expect(component.querySelector<HTMLInputElement>("#uprnReference")).toHaveValue("1");
    });

    it("should return to a pre-filled search form when back-to-search is requested", async () => {
        mockFetchResults([
            { UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA", BUILDING_NUMBER: "1" },
            { UPRN: "2", UDPRN: "2", ADDRESS: "1 Station Road", POST_TOWN: "London", POSTCODE: "SW1A 2AA", BUILDING_NUMBER: "1" }
        ]);
        const addressLookupInstance = createAddressLookup({ settings, fields, content });
        const component = addressLookupInstance.element;
        const buildingInput = component.querySelector<HTMLInputElement>("#building")!;
        const postcodeInput = component.querySelector<HTMLInputElement>("#postcode")!;
        buildingInput.value = "1";
        postcodeInput.value = "SW1A 2AA";
        component.querySelector<HTMLButtonElement>("button")!.click();
        await flushPromises();

        component.querySelector<HTMLAnchorElement>("nav a")!.click();

        expect(component.querySelector<HTMLInputElement>("#building")).toHaveValue("1");
        expect(component.querySelector<HTMLInputElement>("#postcode")).toHaveValue("SW1A 2AA");
    });

    it("should remove the results submission error from the summary when returning to search", async () => {
        const originalTitle = document.title;
        const main = document.createElement("main");
        document.body.appendChild(main);
        mockFetchResults([
            { UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" },
            { UPRN: "2", UDPRN: "2", ADDRESS: "2 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" }
        ]);
        const addressLookupInstance = createAddressLookup({ settings, fields, content });
        const component = addressLookupInstance.element;
        main.appendChild(component);
        try {
            component.querySelector<HTMLInputElement>("#postcode")!.value = "SW1A 2AA";
            component.querySelector<HTMLButtonElement>("button")!.click();
            await flushPromises();

            addressLookupInstance.showNotConfirmedError();
            expect(main.querySelector(".govuk-error-summary__list"))
                .toHaveTextContent(content.resultsNotConfirmedErrorMessage);

            component.querySelector<HTMLAnchorElement>("nav a")!.click();

            expect(main.querySelector(".govuk-error-summary__list")).not.toHaveTextContent(
                content.resultsNotConfirmedErrorMessage
            );
        } finally {
            main.remove();
            document.title = originalTitle;
        }
    });

    it("should confirm an international address with its country name and ID", () => {
        const addressLookupInstance = createAddressLookup({ settings, fields, content, countries });
        const component = addressLookupInstance.element;

        component.querySelector<HTMLAnchorElement>("a")!.click();

        component.querySelector<HTMLInputElement>("#address-line-1")!.value = "123 Rue de Rivoli";
        component.querySelector<HTMLInputElement>("#post-town")!.value = "Paris";
        component.querySelector<HTMLSelectElement>("#country")!.value = "FR";
        component.querySelector<HTMLButtonElement>("button")!.click();

        expect(component.querySelector("p")).toHaveTextContent("France");
        expect(component.querySelector<HTMLInputElement>("#countryId")).toHaveValue("FR");
    });

    const initialAddress: InitialAddress = {
        addressLine1: "1 High Street", postTown: "London", postcode: "SW1A 2AA", countryId: "GB"
    };

    function createLookupPair(primaryAddress?: InitialAddress, secondaryAddress?: InitialAddress) {
        const primary = createAddressLookup({ settings, fields, content, countries, initialAddress: primaryAddress });
        const secondary = createAddressLookup({
            settings: { ...settings, role: "Secondary" }, fields, content, countries,
            initialAddress: secondaryAddress, stateMachine: primary.stateMachine
        });
        const checkbox = secondary.element.querySelector<HTMLInputElement>("input[type='checkbox']")!;
        return { primary, secondary, checkbox };
    }

    it("automatically links equal initial addresses and hides the change-address action", () => {
        const { secondary, checkbox } = createLookupPair(initialAddress, initialAddress);

        expect(checkbox).toBeChecked();
        expect(secondary.stateMachine.getState()).toMatchObject({ status: "confirmed", sameAsPrimary: true });
        expect(secondary.element.querySelector("button")).toBeNull();
        expect(secondary.element.querySelector("#addressLine1")).toHaveValue("1 High Street");
        secondary.showNotConfirmedError();
        expect(secondary.element.querySelector(".govuk-error-message")).toBeNull();
    });

    it("leaves unequal initial addresses independent until the checkbox is clicked", () => {
        const { secondary, checkbox } = createLookupPair(initialAddress, { ...initialAddress, addressLine1: "2 High Street" });
        expect(checkbox).not.toBeChecked();
        expect(secondary.element.querySelector("#addressLine1")).toHaveValue("2 High Street");
        expect(secondary.element.querySelector("button")).toHaveTextContent(content.changeAddressLabel);

        checkbox.click();

        expect(checkbox).toBeChecked();
        expect(secondary.element.querySelector("#addressLine1")).toHaveValue("1 High Street");
        expect(secondary.element.querySelector("button")).toBeNull();
    });

    it("copies the confirmed primary and keeps hidden fields synchronised with later updates", () => {
        const { primary, secondary, checkbox } = createLookupPair(initialAddress);

        checkbox.click();
        expect(secondary.isConfirmed()).toBe(true);
        expect(secondary.element.querySelector("#addressLine1")).toHaveValue("1 High Street");

        primary.element.querySelector<HTMLButtonElement>("button")!.click();
        primary.stateMachine.dispatch({ status: "search-succeeded", criteria: { postcode: "M1 1AA" }, addresses: [{
            UPRN: "42", UDPRN: "42", ADDRESS: "42 Market Street", BUILDING_NUMBER: "42",
            THOROUGHFARE_NAME: "Market Street", POST_TOWN: "Manchester", POSTCODE: "M1 1AA"
        }] });

        expect(checkbox).toBeChecked();
        expect(secondary.element.querySelector("#addressLine1")).toHaveValue("42 Market Street");
        expect(secondary.element.querySelector("#postTown")).toHaveValue("Manchester");
        expect(secondary.element.querySelector("#postcode")).toHaveValue("M1 1AA");
        expect(secondary.element.querySelector("#uprnReference")).toHaveValue("42");
        expect(secondary.element.querySelector("button")).toBeNull();
    });

    it("waits for primary confirmation when checked before either address is confirmed", () => {
        const { primary, secondary, checkbox } = createLookupPair();

        checkbox.click();
        expect(checkbox).toBeChecked();
        expect(secondary.isConfirmed()).toBe(false);
        primary.stateMachine.dispatch({ status: "uk-manual-entry-requested" });
        primary.stateMachine.dispatch({ status: "uk-manual-address-submitted", address: {
            addressLine1: "3 High Street", postTown: "London", postcode: "SW1A 2AA"
        } });

        expect(secondary.isConfirmed()).toBe(true);
        expect(secondary.element.querySelector("#addressLine1")).toHaveValue("3 High Street");
        expect(secondary.element.querySelector("button")).toBeNull();
    });

    it("unlinks when unchecked, restores the change action and ignores later primary changes", () => {
        const { primary, secondary, checkbox } = createLookupPair(initialAddress, initialAddress);

        checkbox.click();
        expect(checkbox).not.toBeChecked();
        expect(secondary.element.querySelector("button")).toHaveTextContent(content.changeAddressLabel);
        primary.stateMachine.dispatch({ status: "international-manual-entry-requested" });
        primary.stateMachine.dispatch({ status: "international-manual-address-submitted", address: {
            addressLine1: "10 Rue de Rivoli", postTown: "Paris", countryId: "FR", countryName: "France"
        } });

        expect(secondary.element.querySelector("#addressLine1")).toHaveValue("1 High Street");
        secondary.element.querySelector<HTMLButtonElement>("button")!.click();
        expect(secondary.isConfirmed()).toBe(false);
        expect(secondary.element.querySelector("#building")).not.toBeNull();
    });

    it.each(["Primary", "Secondary"] as const)("omits same-as without an eligible primary for role %s", role => {
        const primary = createAddressLookup({ settings, fields, content });
        const lookup = createAddressLookup({
            settings: { ...settings, role }, fields, content,
            stateMachine: role === "Primary" ? primary.stateMachine : undefined
        });

        expect(lookup.element.querySelector("input[type='checkbox']")).toBeNull();
    });

    it.each([
        { status: "uk-manual-entry-requested", message: content.ukManualEntryNotConfirmedErrorMessage },
        { status: "international-manual-entry-requested", message: content.internationalManualEntryNotConfirmedErrorMessage }
    ] as const)("shows and clears the submission error for $status", ({ status, message }) => {
        const lookup = createAddressLookup({ settings, fields, content, countries });
        lookup.stateMachine.dispatch({ status } as AddressLookupEvent);

        lookup.showNotConfirmedError();
        expect(lookup.element.querySelector(".govuk-error-message")).toHaveTextContent(message);
        lookup.element.querySelector<HTMLAnchorElement>("a")!.click();

        expect(lookup.stateMachine.getState()).toMatchObject({ status: "search" });
        expect(lookup.element.querySelector(".govuk-error-message")).toBeNull();
    });

    it("confirms a UK manual address entered from the results view", async () => {
        mockFetchResults([
            { UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" },
            { UPRN: "2", UDPRN: "2", ADDRESS: "2 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" }
        ]);
        const lookup = createAddressLookup({ settings, fields, content });
        lookup.element.querySelector<HTMLInputElement>("#postcode")!.value = "SW1A 2AA";
        lookup.element.querySelector<HTMLButtonElement>("button")!.click();
        await flushPromises();
        const manualEntryLink = Array.from(lookup.element.querySelectorAll<HTMLAnchorElement>("a"))
            .find(link => link.textContent === content.ukManualEntryLinkLabel)!;

        manualEntryLink.click();
        expect(lookup.stateMachine.getState()).toEqual({ status: "uk-manual-entry" });
        lookup.element.querySelector<HTMLInputElement>("#address-line-1")!.value = "4 High Street";
        lookup.element.querySelector<HTMLInputElement>("#town-or-city")!.value = "London";
        lookup.element.querySelector<HTMLInputElement>("#postcode")!.value = "SW1A 2AA";
        lookup.element.querySelector<HTMLButtonElement>("button")!.click();

        expect(lookup.isConfirmed()).toBe(true);
        expect(lookup.element.querySelector("#addressLine1")).toHaveValue("4 High Street");
    });
});
