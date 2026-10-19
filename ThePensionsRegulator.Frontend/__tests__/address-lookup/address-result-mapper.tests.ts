import { confirmedAddressesAreEqual, mapAddressSearchResult, mapInitialAddress, mapInternationalManualEntryAddress, mapUkManualEntryAddress } from "../../Scripts/address-lookup/address-result-mapper";
import type { AddressSearchResult, ConfirmedAddress, InitialAddress, InternationalManualEntryAddress } from "../../Scripts/address-lookup/types";
import { createMockAddressLookupContent } from "../../__test-helpers__/mock-address-lookup-content";

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
			.filter(line => line !== undefined && line !== "")).toEqual(expectedLines);
	});

	it("should map the shared address metadata and preserve the UPRN", () => {
		const mappedAddress = mapAddressSearchResult(baseAddress);

		expect(mappedAddress).toMatchObject({
			postTown: "London",
			postcode: "SW1A 2AA",
			uprnReference: "100000000001"
		});
		expect(mappedAddress).not.toHaveProperty("postCounty");
		expect(mappedAddress).not.toHaveProperty("countryId");
		expect(mappedAddress).not.toHaveProperty("countryName");
	});
});

describe("mapInternationalManualEntryAddress", () => {
	it("should map all international manual-entry fields to a confirmed address", () => {
		const address: InternationalManualEntryAddress = {
			addressLine1: "123 Rue de Rivoli",
			addressLine2: "Apartment 4B",
			addressLine3: "3rd arrondissement",
			postTown: "Paris",
			countyStateProvince: "Ile-de-France",
			countryId: "FR",
			countryName: "France",
			postcode: "75001"
		};

		expect(mapInternationalManualEntryAddress(address)).toEqual({
			addressLine1: "123 Rue de Rivoli",
			addressLine2: "Apartment 4B",
			addressLine3: "3rd arrondissement",
			postTown: "Paris",
			postCounty: "Ile-de-France",
			countryId: "FR",
			countryName: "France",
			postcode: "75001"
		});
	});

	it("should omit optional international manual-entry fields when they are not supplied", () => {
		const address: InternationalManualEntryAddress = {
			addressLine1: "123 Main Street",
			postTown: "New York"
		};

		expect(mapInternationalManualEntryAddress(address)).toEqual({
			addressLine1: "123 Main Street",
			addressLine2: undefined,
			addressLine3: undefined,
			postTown: "New York",
			postCounty: undefined,
			countryId: undefined,
			countryName: undefined,
			postcode: undefined
		});
	});
});

describe("confirmedAddressesAreEqual", () => {
	const ukAddress: ConfirmedAddress = mapUkManualEntryAddress({
		addressLine1: "1 High Street",
		addressLine2: "Flat 2",
		addressLine3: "West End",
		postTown: "London",
		county: "Greater London",
		postcode: "SW1A 1AA"
	});
	const internationalAddress: ConfirmedAddress = mapInternationalManualEntryAddress({
		addressLine1: "123 Rue de Rivoli",
		postTown: "Paris",
		countyStateProvince: "Ile-de-France",
		countryId: "FR",
		countryName: "France",
		postcode: "75001"
	});

	it("matches manual UK addresses without UPRNs", () => {
		expect(confirmedAddressesAreEqual(ukAddress, { ...ukAddress })).toBe(true);
	});

	it("matches manual international addresses without UPRNs", () => {
		expect(confirmedAddressesAreEqual(internationalAddress, { ...internationalAddress })).toBe(true);
	});

	it("ignores casing, surrounding whitespace and postcode spacing", () => {
		expect(confirmedAddressesAreEqual(ukAddress, {
			...ukAddress,
			addressLine1: " 1 high street ",
			postTown: " london ",
			postcode: " sw1a1aa "
		})).toBe(true);
	});

	it("treats absent, empty and whitespace-only optional fields as equivalent", () => {
		expect(confirmedAddressesAreEqual(internationalAddress, {
			...internationalAddress,
			addressLine2: "",
			addressLine3: " "
		})).toBe(true);
	});

	it.each([
		"addressLine1", "addressLine2", "addressLine3", "postTown", "postCounty", "postcode"
	] as const)("rejects a different %s when both UPRNs are absent", field => {
		expect(confirmedAddressesAreEqual(ukAddress, { ...ukAddress, [field]: "Different" })).toBe(false);
	});

	it("rejects different international country IDs", () => {
		expect(confirmedAddressesAreEqual(internationalAddress, {
			...internationalAddress, countryId: "BE"
		})).toBe(false);
	});

	it("uses country IDs rather than country labels when supplied", () => {
		expect(confirmedAddressesAreEqual(internationalAddress, {
			...internationalAddress, countryName: "France (French Republic)"
		})).toBe(true);
	});

	it("compares country names when neither country ID is supplied", () => {
		const address = { ...internationalAddress, countryId: undefined };
		expect(confirmedAddressesAreEqual(address, { ...address, countryName: " france " })).toBe(true);
		expect(confirmedAddressesAreEqual(address, { ...address, countryName: "Belgium" })).toBe(false);
	});

	it("matches international addresses without optional postcodes or towns", () => {
		const address = { addressLine1: "123 Main Street", countryId: "FR" };
		expect(confirmedAddressesAreEqual(address, { ...address, postcode: "", postTown: "" })).toBe(true);
	});

	it.each([undefined, "", " "])("compares fields when one UPRN is %s", uprnReference => {
		const addressWithUprn = { ...ukAddress, uprnReference: "123" };
		const addressWithoutUprn = { ...ukAddress, uprnReference };
		expect(confirmedAddressesAreEqual(addressWithUprn, addressWithoutUprn)).toBe(true);
		expect(confirmedAddressesAreEqual(addressWithoutUprn, addressWithUprn)).toBe(true);
		expect(confirmedAddressesAreEqual(addressWithUprn, {
			...addressWithoutUprn, addressLine1: "2 High Street"
		})).toBe(false);
	});

	it("matches identical nonempty UPRNs even when address text differs", () => {
		expect(confirmedAddressesAreEqual(
			{ ...ukAddress, uprnReference: "123" },
			{ ...ukAddress, uprnReference: "123", addressLine1: "Different" }
		)).toBe(true);
	});

	it("rejects conflicting nonempty UPRNs even when address text matches", () => {
		expect(confirmedAddressesAreEqual(
			{ ...ukAddress, uprnReference: "123" },
			{ ...ukAddress, uprnReference: "456" }
		)).toBe(false);
	});
});

describe("mapInitialAddress", () => {
	const content = createMockAddressLookupContent();

	it("maps a complete initial address with the UK country ID as a UK address", () => {
		const initialAddress: InitialAddress = {
			addressLine1: "1 High Street",
			postTown: "London",
			countryId: "GB",
			countryName: "United Kingdom",
			postcode: "SW1A 1AA",
			uprnReference: "123"
		};

		expect(mapInitialAddress(content, initialAddress, "GB")).toEqual({
			addressLine1: "1 High Street",
			addressLine2: undefined,
			addressLine3: undefined,
			postTown: "London",
			postCounty: undefined,
			countryId: "GB",
			countryName: "United Kingdom",
			postcode: "SW1A 1AA",
			uprnReference: "123"
		});
	});

	it("validates an initial address with a different country ID as international", () => {
		const initialAddress: InitialAddress = {
			addressLine1: "123 Rue de Rivoli",
			postTown: "Paris",
			countryId: "FR",
			countryName: "France",
			postcode: "75001"
		};

		expect(mapInitialAddress(content, initialAddress, "GB")).toMatchObject({
			addressLine1: "123 Rue de Rivoli",
			postTown: "Paris",
			countryId: "FR",
			countryName: "France",
			postcode: "75001"
		});
	});

	it.each<InitialAddress | undefined>([
		undefined,
		{},
		{ addressLine1: "" },
		{ addressLine1: "   ", postTown: "London", postcode: "SW1A 1AA" },
		{ addressLine1: "1 High Street", postTown: "London", postcode: "INVALID" },
		{ addressLine1: "1 High Street", postcode: "SW1A 1AA" },
		{ addressLine1: "X".repeat(101), countryId: "FR", countryName: "France" }
	])("does not confirm an absent or invalid initial address: %j", address => {
		expect(mapInitialAddress(content, address, "GB")).toBeUndefined();
	});

	it("validates and normalises a UK address without a UPRN", () => {
		expect(mapInitialAddress(content, {
			addressLine1: "1 High Street", addressLine2: "Flat 2", addressLine3: "West End",
			postTown: "London", postCounty: "Greater London", postcode: " sw1a-1aa ", countryId: "GB"
		}, "GB")).toEqual({
			addressLine1: "1 High Street", addressLine2: "Flat 2", addressLine3: "West End",
			postTown: "London", postCounty: "Greater London", postcode: "SW1A 1AA",
			countryId: "GB", countryName: undefined, uprnReference: undefined
		});
	});

	it("preserves optional fields on a UPRN-backed initial UK address", () => {
		expect(mapInitialAddress(content, {
			addressLine1: "1 High Street", addressLine2: "Flat 2", addressLine3: "West End",
			postTown: "London", postCounty: "Greater London", postcode: "sw1a1aa", uprnReference: "123"
		}, "GB")).toMatchObject({
			addressLine2: "Flat 2", addressLine3: "West End", postCounty: "Greater London",
			postcode: "SW1A 1AA", uprnReference: "123"
		});
	});

	it("accepts an international address without an optional postcode", () => {
		expect(mapInitialAddress(content, {
			addressLine1: "123 Main Street", postTown: "Paris", countryId: "FR", countryName: "France"
		}, "GB")).toMatchObject({
			addressLine1: "123 Main Street", countryId: "FR", countryName: "France",
			postTown: "Paris", postcode: undefined
		});
	});
});

describe("mapUkManualEntryAddress", () => {
	it("maps optional lines and county without adding country or UPRN metadata", () => {
		expect(mapUkManualEntryAddress({
			addressLine1: "1 High Street", addressLine2: "Flat 2", addressLine3: "West End",
			postTown: "London", county: "Greater London", postcode: "SW1A 1AA"
		})).toEqual({
			addressLine1: "1 High Street", addressLine2: "Flat 2", addressLine3: "West End",
			postTown: "London", postCounty: "Greater London", postcode: "SW1A 1AA"
		});
	});

	it("leaves missing optional UK fields undefined", () => {
		expect(mapUkManualEntryAddress({ addressLine1: "1 High Street", postTown: "London", postcode: "SW1A 1AA" }))
			.toEqual({
				addressLine1: "1 High Street", addressLine2: undefined, addressLine3: undefined,
				postTown: "London", postCounty: undefined, postcode: "SW1A 1AA"
			});
	});
});
