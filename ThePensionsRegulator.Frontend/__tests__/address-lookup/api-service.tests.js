import '@testing-library/jest-dom';
import { jest } from '@jest/globals';
import { AddressLookupApiService } from "../../wwwroot/tpr/address-lookup/api-service.js";

function mockFetchResponse(body) {
    return Promise.resolve({
        json: () => Promise.resolve(body)
    });
}

beforeEach(() => {
    global.fetch = jest.fn();
});

afterEach(() => {
    jest.restoreAllMocks();
});

describe("Address lookup API service", () => {
    describe("searchAddresses", () => {
        it("should call fetch with the postcode query param set", async () => {
            global.fetch.mockReturnValue(mockFetchResponse({}));

            const service = new AddressLookupApiService('/api/search', '/api/id');
            await service.searchAddresses('BN1 4DW', '');

            expect(global.fetch).toHaveBeenCalledTimes(1);
            const calledUrl = global.fetch.mock.calls[0][0].toString();
            expect(calledUrl).toContain('postcode=BN1+4DW');
        });

        it("should return an empty array if no results are returned", async () => {
            global.fetch.mockReturnValue(mockFetchResponse({results: []}));

            const service = new AddressLookupApiService('/search', '/id');
            const results = await service.searchAddresses('BN1 4DW', '');

            expect(global.fetch).toHaveBeenCalledTimes(1);
            expect(results).toEqual([]);
        });

        it("should return the DPA results in an array", async () => {
            const address1 = { UPRN: '1', ADDRESS: 'Address 1' };
            const address2 = { UPRN: '2', ADDRESS: 'Address 2' };

            const apiResponse = {
                results: [
                    { DPA: address1 },
                    { DPA: address2 }
            ]};
            global.fetch.mockReturnValue(mockFetchResponse(apiResponse));

            const service = new AddressLookupApiService('/search', '/id');
            const results = await service.searchAddresses('BN1 4DW', '');

            expect(results).toEqual([address1, address2]);
        });

        it('should filter by building number when building is numeric', async () => {
            const address1 = { BUILDING_NUMBER: '1', THOROUGHFARE_NAME: 'HIGH STREET' };
            const address2 = { BUILDING_NUMBER: '2', THOROUGHFARE_NAME: 'HIGH STREET' };

            global.fetch.mockReturnValue(mockFetchResponse({
                results: [
                    { DPA: address1 },
                    { DPA: address2 }
                ]
            }));

            const service = new AddressLookupApiService('/api/search', '/api/id');
            const results = await service.searchAddresses('BN1 4DW', '2');

            expect(results).toEqual([ address2 ]);
        });

        it('should filter by building name when building is not numeric', async () => {
            const address1 = { BUILDING_NAME: 'Rose Cottage', THOROUGHFARE_NAME: 'HIGH STREET' };
            const address2 = { BUILDING_NAME: 'Ivy House', THOROUGHFARE_NAME: 'HIGH STREET' };
            global.fetch.mockReturnValue(mockFetchResponse({
                results: [
                    { DPA: address1 },
                    { DPA: address2 }
                ]
            }));

            const service = new AddressLookupApiService('/api/search', '/api/id');
            const results = await service.searchAddresses('BN1 4DW', 'rose');

            expect(results).toEqual([ address1 ]);
        });
    })

    describe("getAddressById", () => {
        it('should call fetch with uprn query parameter', async () => {
            const UPRN = '12345';
            global.fetch.mockReturnValue(mockFetchResponse({
                results: [{ DPA: { UPRN: UPRN } }]
            }));

            const service = new AddressLookupApiService('/api/search', '/api/id');
            const results = await service.getAddressById(UPRN);

            const calledUrl = global.fetch.mock.calls[0][0].toString();
            expect(calledUrl).toContain(`uprn=${UPRN}`);
            expect(results).toEqual([{ UPRN: UPRN }]);
        });
    });
});