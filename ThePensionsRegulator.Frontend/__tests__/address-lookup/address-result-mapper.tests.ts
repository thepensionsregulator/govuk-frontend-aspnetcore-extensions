import { mapAddressSearchResult } from "../../Scripts/address-lookup/address-result-mapper";
import type { AddressSearchResult, ConfirmedAddress } from "../../Scripts/address-lookup/types";

describe("mapAddressSearchResult", () => {
	const completeAddress: AddressSearchResult = {
		UPRN: "100000000001",
		UDPRN: "200000000001",
		ADDRESS: "The Pensions Regulator, 10 High Street, London, SW1A 2AA",
		ORGANISATION_NAME: "The Pensions Regulator",
		DEPARTMENT_NAME: "Operations",
		SUB_BUILDING_NAME: "Flat 2",
		BUILDING_NAME: "The Old House",
		BUILDING_NUMBER: "10",
		DEPENDENT_THOROUGHFARE_NAME: "Old Road",
		THOROUGHFARE_NAME: "High Street",
		DOUBLE_DEPENDENT_LOCALITY: "West End",
		DEPENDENT_LOCALITY: "Central",
		POST_TOWN: "London",
		POSTCODE: "SW1A 2AA"
	};

	it("should map DPA fields and preserve the UPRN", () => {
		const mappedAddress: ConfirmedAddress = mapAddressSearchResult(completeAddress);

		expect(mappedAddress).toEqual({
			organisationName: "The Pensions Regulator",
			departmentName: "Operations",
			subBuildingName: "Flat 2",
			buildingName: "The Old House",
			buildingNumber: "10",
			dependentThoroughfareName: "Old Road",
			thoroughfareName: "High Street",
			doubleDependentLocality: "West End",
			dependentLocality: "Central",
			postTown: "London",
			postcode: "SW1A 2AA",
			uprn: "100000000001"
		});
	});

	it.each([
		"ORGANISATION_NAME",
		"DEPARTMENT_NAME",
		"SUB_BUILDING_NAME",
		"BUILDING_NAME",
		"BUILDING_NUMBER",
		"DEPENDENT_THOROUGHFARE_NAME",
		"THOROUGHFARE_NAME",
		"DOUBLE_DEPENDENT_LOCALITY",
		"DEPENDENT_LOCALITY"
	] as const)("should map a missing optional %s to an empty string", (field) => {
		const addressWithoutOptionalField = { ...completeAddress };
		delete addressWithoutOptionalField[field];

		const mappedAddress = mapAddressSearchResult(addressWithoutOptionalField);
		const mappedField = field
			.toLowerCase()
			.replace(/_([a-z])/g, (_, character: string) => character.toUpperCase());

		expect(mappedAddress[mappedField as keyof ConfirmedAddress]).toBe("");
	});
});
