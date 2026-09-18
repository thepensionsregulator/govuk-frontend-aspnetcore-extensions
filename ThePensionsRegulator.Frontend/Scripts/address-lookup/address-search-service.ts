import { AddressSearchResult, AddressSearchService, FetchAddressSearchServiceOptions } from "./types";


export class FetchAddressSearchService implements AddressSearchService {
    private readonly searchEndpoint: string;
    // private readonly addressByIdEndpoint: string;
    private readonly fetchFunction: typeof globalThis.fetch;
    
    constructor(options: FetchAddressSearchServiceOptions) {
        this.searchEndpoint = options.searchEndpoint;
        // this.addressByIdEndpoint = options.addressByIdEndpoint;
        this.fetchFunction = options.fetchFunction ?? globalThis.fetch.bind(globalThis);
    }

    async searchAddress(postcode: string): Promise<AddressSearchResult[]> {
        const url = new URL(this.searchEndpoint, window.location.origin);

        url.searchParams.set("postcode", postcode);

        return await this.fetchJson<AddressSearchResult[]>(url);
    }

    private async fetchJson<T>(url: URL): Promise<T> {
        const response = await this.fetchFunction(url, {
            headers: { Accept: "application/json" },
        });

        if (!response.ok) {
            throw new Error(`Address lookup failed with status ${response.status}`);
        }

        const body = await response.json() as { results?: { DPA: AddressSearchResult }[] };

        var results =  (body.results ?? []).map(r => r.DPA);

        return results as T;
    }
}