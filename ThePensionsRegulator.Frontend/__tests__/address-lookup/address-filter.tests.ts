import { filterAddressesByBuilding } from "../../Scripts/address-lookup/address-filter";
import { AddressSearchResult } from "../../Scripts/address-lookup/types";

describe("filterAddressByBuilding", () => {
    let addresses: AddressSearchResult[];
    const buildingNameUprn = "100000000001"
    const buildingNumberUprn = "100000000002";
    const subBulidingUprn = "100000000003";
    beforeEach(() => {
        addresses = [
            {
                UPRN: buildingNameUprn,
                UDPRN: "200000000001",
                ADDRESS: "The Old Bakery, High Street, London, SW1A 1AA",
                POST_TOWN: "London",
                POSTCODE: "SW1A 1AA",
                BUILDING_NAME: "The Old Bakery"
            },
            {
                UPRN: buildingNumberUprn,
                UDPRN: "200000000002",
                ADDRESS: "1 High Street, London, SW1A 1AB",
                POST_TOWN: "London",
                POSTCODE: "SW1A 1AB",
                BUILDING_NUMBER: "1"
            },
            {
                UPRN: "100000000004",
                UDPRN: "200000000004",
                ADDRESS: "1 Station Road, London, SW1A 1AD",
                POST_TOWN: "London",
                POSTCODE: "SW1A 1AD",
                BUILDING_NUMBER: "1"
            },
            {
                UPRN: subBulidingUprn,
                UDPRN: "200000000003",
                ADDRESS: "Flat 2, The Old Bakery, High Street, London, SW1A 1AC",
                POST_TOWN: "London",
                POSTCODE: "SW1A 1AC",
                SUB_BUILDING_NAME: "Flat 2"
            }
        ];
    });
    it("should return an empty array when no addresses are provided", () => {
        const emptyAddresses: AddressSearchResult[] = [];

        const result = filterAddressesByBuilding(emptyAddresses, "building name");

        expect(result).toHaveLength(0);
    });

    it("should return an empty array when no matches are provided", () => {
        const result = filterAddressesByBuilding(addresses, "Nonexistent Building");

        expect(result).toHaveLength(0);
    });

    it("should return a match if the SUB_BUILDING_NAME matches", () => {
        const result = filterAddressesByBuilding(addresses, "Flat 2");

        expect(result).toHaveLength(1);
        expect(result[0].UPRN).toBe(subBulidingUprn);
    });

    it("should return a match if the BUILDING_NAME matches", () => {
        const result = filterAddressesByBuilding(addresses, "The Old Bakery");

        expect(result).toHaveLength(1);
        expect(result[0].UPRN).toBe(buildingNameUprn);
    });

    it("should return multiple matches if provided number is in BULIDING_NUMBER", () => {
        const result = filterAddressesByBuilding(addresses, "1");

        expect(result).toHaveLength(2);
    });
});