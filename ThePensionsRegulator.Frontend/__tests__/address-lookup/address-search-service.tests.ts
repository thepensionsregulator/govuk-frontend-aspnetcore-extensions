import { jest } from "@jest/globals";
import { FetchAddressSearchService } from "../../Scripts/address-lookup/address-search-service";
import type { AddressSearchResult } from "../../Scripts/address-lookup/types";

describe("FetchAddressSearchService", () => {
    const address: AddressSearchResult = {
        UPRN: "1",
        UDPRN: "1",
        ADDRESS: "1 High Street, London, SW1A 2AA",
        POST_TOWN: "London",
        POSTCODE: "SW1A 2AA"
    };

    function createService(fetchFunction: typeof globalThis.fetch) {
        return new FetchAddressSearchService({
            searchEndpoint: "/api/address-search",
            addressByIdEndpoint: "/api/address",
            fetchFunction
        });
    }

    // Stub rather than a real Response, since jsdom doesn't provide the Fetch API globals.
    function createFetchResponse(body: unknown, ok = true): Response {
        return { ok, status: ok ? 200 : 500, json: () => Promise.resolve(body) } as Response;
    }

    it("should request the search endpoint with the postcode as a query parameter", async () => {
        const fetchFunction = jest.fn<typeof globalThis.fetch>()
            .mockResolvedValue(createFetchResponse({ results: [] }));
        const service = createService(fetchFunction);

        await service.searchAddress("SW1A 2AA");

        const [requestedUrl] = fetchFunction.mock.calls[0] as [URL, RequestInit];
        expect(requestedUrl.pathname).toBe("/api/address-search");
        expect(requestedUrl.searchParams.get("postcode")).toBe("SW1A 2AA");
    });

    it("should request JSON", async () => {
        const fetchFunction = jest.fn<typeof globalThis.fetch>()
            .mockResolvedValue(createFetchResponse({ results: [] }));
        const service = createService(fetchFunction);

        await service.searchAddress("SW1A 2AA");

        const [, requestInit] = fetchFunction.mock.calls[0] as [URL, RequestInit];
        expect(requestInit.headers).toEqual({ Accept: "application/json" });
    });

    it("should unwrap the DPA envelope for each result", async () => {
        const fetchFunction = jest.fn<typeof globalThis.fetch>()
            .mockResolvedValue(createFetchResponse({ results: [{ DPA: address }] }));
        const service = createService(fetchFunction);

        const results = await service.searchAddress("SW1A 2AA");

        expect(results).toEqual([address]);
    });

    it("should return an empty array when the response has no results", async () => {
        const fetchFunction = jest.fn<typeof globalThis.fetch>()
            .mockResolvedValue(createFetchResponse({}));
        const service = createService(fetchFunction);

        const results = await service.searchAddress("SW1A 2AA");

        expect(results).toEqual([]);
    });

    it("should throw with the response status when the request fails", async () => {
        const fetchFunction = jest.fn<typeof globalThis.fetch>()
            .mockResolvedValue(createFetchResponse(null, false));
        const service = createService(fetchFunction);

        await expect(service.searchAddress("SW1A 2AA")).rejects.toThrow(
            "Address lookup failed with status 500"
        );
    });
});
