import { mapAddressSearchResult } from "../../Scripts/address-lookup/address-result-mapper";
import type { AddressSearchResult, ConfirmedAddress } from "../../Scripts/address-lookup/types";

describe("mapAddressSearchResult", () => {
	const baseAddress: AddressSearchResult = {
		UPRN: "100000000001",
		UDPRN: "200000000001",
		ADDRESS: "10 High Street, London, SW1A 2AA",
		POST_TOWN: "London",
		POSTCODE: "SW1A 2AA",
		THOROUGHFARE_NAME: "High Street"
	};

	it.each([
		{
			name: "a building number and thoroughfare",
			result: { BUILDING_NUMBER: "103" },
			expectedLines: ["103 High Street"]
		},
		{
			name: "a named building and thoroughfare",
			result: { BUILDING_NAME: "Telecom House" },
			expectedLines: ["Telecom House", "High Street"]
		},
		{
			name: "a named building with a sub-building",
			result: {
				ORGANISATION_NAME: "The Pensions Regulator",
				SUB_BUILDING_NAME: "Flat 2",
				BUILDING_NAME: "The Old House",
				BUILDING_NUMBER: "10",
				DEPENDENT_THOROUGHFARE_NAME: "Old Road"
			},
			expectedLines: ["The Pensions Regulator", "Flat 2, The Old House", "10 Old Road High Street"]
		},
		{
			name: "a numeric building name with a letter suffix",
			result: { BUILDING_NAME: "103a" },
			expectedLines: ["103a High Street"]
		},
		{
			name: "a building name containing a trailing range",
			result: { BUILDING_NAME: "The Hacienda 11-15" },
			expectedLines: ["The Hacienda", "11-15 High Street"]
		},
		{
			name: "a numeric building name with a sub-building",
			result: { SUB_BUILDING_NAME: "Flat 2", BUILDING_NAME: "10" },
			expectedLines: ["Flat 2", "10 High Street"]
		},
		{
			name: "a named building and building number with a sub-building",
			result: {
				SUB_BUILDING_NAME: "Flat 14",
				BUILDING_NAME: "Da Vinci House",
				BUILDING_NUMBER: "44"
			},
			expectedLines: ["Flat 14, Da Vinci House", "44 High Street"]
		},
		{
			name: "a numeric sub-building and named building",
			result: { SUB_BUILDING_NAME: "12", BUILDING_NAME: "West House" },
			expectedLines: ["12 West House"]
		},
		{
			name: "a sub-building with only a building number and thoroughfare",
			result: { SUB_BUILDING_NAME: "Flat 14", BUILDING_NUMBER: "44" },
			expectedLines: ["Flat 14, 44 High Street"]
		},
		{
			name: "an organisation with a building number and thoroughfare",
			result: { ORGANISATION_NAME: "Screwfix", BUILDING_NUMBER: "12" },
			expectedLines: ["Screwfix", "12 High Street"]
		},
		{
			name: "an organisation with a numeric building range",
			result: { ORGANISATION_NAME: "Primark Ltd", BUILDING_NAME: "1-27" },
			expectedLines: ["Primark Ltd", "1-27 High Street"]
		},
		{
			name: "a numeric sub-building and building range",
			result: { SUB_BUILDING_NAME: "618", BUILDING_NAME: "The Hacienda 11-15" },
			expectedLines: ["618 The Hacienda", "11-15 High Street"]
		},
		{
			name: "a building name containing a trailing number",
			result: { BUILDING_NAME: "The Old House 10" },
			expectedLines: ["The Old House", "10 High Street"]
		}
	])("should map $name into confirmed address lines", ({ result, expectedLines }) => {
		const mappedAddress: ConfirmedAddress = mapAddressSearchResult({ ...baseAddress, ...result });

		expect([mappedAddress.addressLine1, mappedAddress.addressLine2, mappedAddress.addressLine3]
			.filter(line => line !== "")).toEqual(expectedLines);
	});

	it("should map the shared address metadata and preserve the UPRN", () => {
		const mappedAddress = mapAddressSearchResult(baseAddress);

		expect(mappedAddress).toMatchObject({
			postTown: "London",
			postCounty: "",
			countryId: "GB",
			countryName: "United Kingdom",
			postCode: "SW1A 2AA",
			uprnReference: "100000000001"
		});
	});
});
