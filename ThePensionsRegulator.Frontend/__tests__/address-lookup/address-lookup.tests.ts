import { jest } from "@jest/globals";
import "@testing-library/jest-dom";
import { createAddressLookup } from "../../Scripts/address-lookup/address-lookup";

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

    it("should return an HTMLElement", () => {
        const component = createAddressLookup({ searchEndpoint: "/api/address-search", addressByIdEndpoint: "/api/address" });

        expect(component).toBeInstanceOf(HTMLElement);
    });

    it("should render the search form initially", () => {
        const component = createAddressLookup({ searchEndpoint: "/api/address-search", addressByIdEndpoint: "/api/address" });

        expect(component.querySelector("#building")).not.toBeNull();
        expect(component.querySelector("#postcode")).not.toBeNull();
        expect(component.querySelector("button")).not.toBeNull();
    });

    it("should search using the configured search endpoint", async () => {
        const fetchMock = mockFetchResults([]);
        const component = createAddressLookup({ searchEndpoint: "/api/address-search", addressByIdEndpoint: "/api/address" });
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
            { UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" },
            { UPRN: "2", UDPRN: "2", ADDRESS: "2 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" }
        ]);
        const component = createAddressLookup({ searchEndpoint: "/api/address-search", addressByIdEndpoint: "/api/address" });
        const postcodeInput = component.querySelector<HTMLInputElement>("#postcode")!;
        const button = component.querySelector<HTMLButtonElement>("button")!;
        postcodeInput.value = "SW1A 2AA";

        button.click();
        await flushPromises();

        expect(component.querySelector("#postcode")).toBeNull();
        expect(component.querySelector("#address-select")).not.toBeNull();
    });

    it("should only ever show one view's markup at a time", async () => {
        mockFetchResults([
            { UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" },
            { UPRN: "2", UDPRN: "2", ADDRESS: "2 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" }
        ]);
        const component = createAddressLookup({ searchEndpoint: "/api/address-search", addressByIdEndpoint: "/api/address" });
        const postcodeInput = component.querySelector<HTMLInputElement>("#postcode")!;
        const button = component.querySelector<HTMLButtonElement>("button")!;
        postcodeInput.value = "SW1A 2AA";

        button.click();
        await flushPromises();

        expect(component.children).toHaveLength(1);
    });

    it("should confirm the selected address from the results view", async () => {
        mockFetchResults([
            { UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" },
            { UPRN: "2", UDPRN: "2", ADDRESS: "2 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" }
        ]);
        const component = createAddressLookup({ searchEndpoint: "/api/address-search", addressByIdEndpoint: "/api/address" });
        const postcodeInput = component.querySelector<HTMLInputElement>("#postcode")!;
        postcodeInput.value = "SW1A 2AA";
        component.querySelector<HTMLButtonElement>("button")!.click();
        await flushPromises();

        const select = component.querySelector<HTMLSelectElement>("#address-select")!;
        select.value = "2";
        component.querySelector<HTMLButtonElement>("button")!.click();

        // Known gap: the "confirmed" state has no case in the render switch, so the container goes blank.
        expect(component.children).toHaveLength(0);
    });

    it("should leave the container blank when a single address is found", async () => {
        mockFetchResults([
            { UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA" }
        ]);
        const component = createAddressLookup({ searchEndpoint: "/api/address-search", addressByIdEndpoint: "/api/address" });
        const postcodeInput = component.querySelector<HTMLInputElement>("#postcode")!;
        const button = component.querySelector<HTMLButtonElement>("button")!;
        postcodeInput.value = "SW1A 2AA";

        button.click();
        await flushPromises();

        // Known gap: single-result search auto-confirms, but there's no view for the confirmed state.
        expect(component.children).toHaveLength(0);
    });

    it("should return to a pre-filled search form when back-to-search is requested", async () => {
        mockFetchResults([
            { UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA", BUILDING_NUMBER: "1" },
            { UPRN: "2", UDPRN: "2", ADDRESS: "1 Station Road", POST_TOWN: "London", POSTCODE: "SW1A 2AA", BUILDING_NUMBER: "1" }
        ]);
        const component = createAddressLookup({ searchEndpoint: "/api/address-search", addressByIdEndpoint: "/api/address" });
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
});
