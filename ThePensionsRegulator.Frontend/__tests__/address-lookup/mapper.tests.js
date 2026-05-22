import '@testing-library/jest-dom';

import { jest } from '@jest/globals';
import { AddressMapper } from "../../wwwroot/tpr/address-lookup/mapper.js";
import { ADDRESS_LOOKUP_CONFIG } from "../../wwwroot/tpr/address-lookup/config.js";

describe("Address mapper", () => {
    const options = [new Option(ADDRESS_LOOKUP_CONFIG.DEFAULTS.COUNTRY, "1"), new Option("France", "FR")];
    const mapper = new AddressMapper(ADDRESS_LOOKUP_CONFIG, options);
    describe("defaultCountryCode", () => {
        it("is set to the value of the option matching the default country label in config", () => {
            expect(mapper.defaultCountryCode).toEqual("1");
        });
    });
    describe("mapFromDpaResult", () => {
        const typicalResidentialDPAResult = {
            BUILDING_NUMBER: "103",
            THOROUGHFARE_NAME: "PRESTON ROAD",
            POST_TOWN: "BRIGHTON",
            POSTCODE: "BN1 6AF",
        };
        const residentialDPAResultWithBuildingName = {
            BUILDING_NAME: "TELECOM HOUSE",
            THOROUGHFARE_NAME: "PRESTON ROAD",
            POST_TOWN: "BRIGHTON",
            POSTCODE: "BN1 6AF",
        };
        const residentialDPAResultWithBuidingNumberInName = {
            BUILDING_NAME: "103a",
            THOROUGHFARE_NAME: "PRESTON ROAD",
            POST_TOWN: "BRIGHTON",
            POSTCODE: "BN1 6AF",
        };
        it("buiding number and thoroughfare name are combined", () => {
            const result = mapper.mapFromDpaResult(typicalResidentialDPAResult);

            expect(result.addressLine1).toEqual(`${typicalResidentialDPAResult.BUILDING_NUMBER} ${typicalResidentialDPAResult.THOROUGHFARE_NAME}`);
        });

        it("building name and thoroughfare name are combined if buiding name is a number with a letter e.g. 1a, 1b", () => {
            const result = mapper.mapFromDpaResult(residentialDPAResultWithBuidingNumberInName);

            expect(result.addressLine1).toEqual(`${residentialDPAResultWithBuidingNumberInName.BUILDING_NAME} ${residentialDPAResultWithBuidingNumberInName.THOROUGHFARE_NAME}`);
        });

        it("buiding name and thoroughfare are seperate lines if building name is not a number", () => {
            const result = mapper.mapFromDpaResult(residentialDPAResultWithBuildingName);

            expect(result.addressLine1).toEqual(residentialDPAResultWithBuildingName.BUILDING_NAME);
            expect(result.addressLine2).toEqual(residentialDPAResultWithBuildingName.THOROUGHFARE_NAME);
        });

        it("town field is populated by POST_TOWN from DPA result", () => {
            const result = mapper.mapFromDpaResult(typicalResidentialDPAResult);

            expect(result.town).toMatch(typicalResidentialDPAResult.POST_TOWN);
        });

        it("postcode is populated by POSTCODE from DPA result", () => {
            const result = mapper.mapFromDpaResult(typicalResidentialDPAResult);

            expect(result.postcode).toMatch(typicalResidentialDPAResult.POSTCODE);
        });

        it("county is not populated as DPA result does not contain a county", () => {
            const result = mapper.mapFromDpaResult(typicalResidentialDPAResult);

            expect(result.county).toMatch('');
        });

        it("country is always the default value", () => {
            const result = mapper.mapFromDpaResult(typicalResidentialDPAResult);

            expect(result.country).toMatch(ADDRESS_LOOKUP_CONFIG.DEFAULTS.COUNTRY);
        });

        const subBuildingBuildingNameBuildingNumberDPAResult = {
            "SUB_BUILDING_NAME": "FLAT 14",
            "BUILDING_NAME": "DA VINCI HOUSE",
            "BUILDING_NUMBER": "44",
            "THOROUGHFARE_NAME": "SAFFRON HILL",
            "POST_TOWN": "LONDON",
            "POSTCODE": "EC1N 8FH",
        };
        it("SUB_BUILDING_NAME and BUILDING_NAME are combined, seperated by a comma", () => {
            const result = mapper.mapFromDpaResult(subBuildingBuildingNameBuildingNumberDPAResult);

            expect(result.addressLine1).toMatch(`${subBuildingBuildingNameBuildingNumberDPAResult.SUB_BUILDING_NAME}, ${subBuildingBuildingNameBuildingNumberDPAResult.BUILDING_NAME}`);
        });

        it("BUIDING_NUMBER and THOROUGHFARE_NAME are combined even when there is a building name", () => {
            const result = mapper.mapFromDpaResult(subBuildingBuildingNameBuildingNumberDPAResult);

            expect(result.addressLine2).toMatch(`${subBuildingBuildingNameBuildingNumberDPAResult.BUILDING_NUMBER} ${subBuildingBuildingNameBuildingNumberDPAResult.THOROUGHFARE_NAME}`);
        });

        const organisationNameWithBuildingNumberDPAResult = {
            "ORGANISATION_NAME": "SCREWFIX",
            "BUILDING_NUMBER": "12",
            "THOROUGHFARE_NAME": "VALLEY ROAD",
            "POST_TOWN": "PLYMOUTH",
            "POSTCODE": "PL7 1RF",
        };

        it("organisation name is addressLine1 if DPA result provides one", () => {
            const result = mapper.mapFromDpaResult(organisationNameWithBuildingNumberDPAResult);

            expect(result.addressLine1).toMatch(organisationNameWithBuildingNumberDPAResult.ORGANISATION_NAME);
        });

        it("address line 2 is building number and thoroughfare name when organisation name is provided", () => {
            const result = mapper.mapFromDpaResult(organisationNameWithBuildingNumberDPAResult);

            expect(result.addressLine2).toMatch(`${organisationNameWithBuildingNumberDPAResult.BUILDING_NUMBER} ${organisationNameWithBuildingNumberDPAResult.THOROUGHFARE_NAME}`);
        });

        const organistaionWithBuidingNameNumberRange = {
            "ORGANISATION_NAME": "PRIMARK LTD",
            "BUILDING_NAME": "1-27",
            "THOROUGHFARE_NAME": "CASTLE STREET",
            "POST_TOWN": "BELFAST",
            "POSTCODE": "BT1 1BL",
        };
        it("BUILDING_NAME that is a number range and THOROUGHFARE_NAME are combined", () => {
            const result = mapper.mapFromDpaResult(organistaionWithBuidingNameNumberRange);

            expect(result.addressLine1).toMatch(organistaionWithBuidingNameNumberRange.ORGANISATION_NAME);
            expect(result.addressLine2).toMatch(`${organistaionWithBuidingNameNumberRange.BUILDING_NAME} ${organistaionWithBuidingNameNumberRange.THOROUGHFARE_NAME}`);
            expect(result.town).toMatch(organistaionWithBuidingNameNumberRange.POST_TOWN);
            expect(result.postcode).toMatch(organistaionWithBuidingNameNumberRange.POSTCODE);
        });

        const buildingNumberRangeInBuildingName = {
            "SUB_BUILDING_NAME": "618",
            "BUILDING_NAME": "THE HACIENDA 11-15",
            "THOROUGHFARE_NAME": "WHITWORTH STREET WEST",
            "POST_TOWN": "MANCHESTER",
            "POSTCODE": "M1 5DD",
        };
        it("BUIDING_NAME containing a building number range should be combined with THOROUGHFARE_NAME", () => {
            const result = mapper.mapFromDpaResult(buildingNumberRangeInBuildingName);

            expect(result.addressLine1).toMatch(`${buildingNumberRangeInBuildingName.SUB_BUILDING_NAME} THE HACIENDA`);
            expect(result.addressLine2).toMatch(`11-15 ${buildingNumberRangeInBuildingName.THOROUGHFARE_NAME}`);
            expect(result.town).toMatch(buildingNumberRangeInBuildingName.POST_TOWN);
            expect(result.postcode).toMatch(buildingNumberRangeInBuildingName.POSTCODE);
        });
        const uprnDpaResult = {
            "UPRN": "1234567",
            "SUB_BUILDING_NAME": "618",
            "BUILDING_NAME": "THE HACIENDA 11-15",
            "THOROUGHFARE_NAME": "WHITWORTH STREET WEST",
            "POST_TOWN": "MANCHESTER",
            "POSTCODE": "M1 5DD",
        };

        it("result contains UPRN", () => {
            const result = mapper.mapFromDpaResult(uprnDpaResult);

            expect(result.UPRN).toMatch(uprnDpaResult.UPRN);
        });
    });

    describe("mapFromManualEntry", () => {
        it("all fields are populated and county field exists when country is United Kingdom", () => {
            const result = mapper.mapFromManualEntry("10 Downing Street", "Westminster", "London", "Greater London", "SW1A 2AA", ADDRESS_LOOKUP_CONFIG.DEFAULTS.COUNTRY, "1");

            expect(result.addressLine1).toEqual("10 Downing Street");
            expect(result.addressLine2).toEqual("Westminster");
            expect(result.town).toEqual("London");
            expect(result.county).toEqual("Greater London");
            expect(result.postcode).toEqual("SW1A 2AA");
            expect(result.country).toEqual("United Kingdom");
            expect(result.countryCode).toEqual("1");

            expect(result).not.toHaveProperty("region");
        });

        it("all fields are populated and region field exists when country is not United Kingdom", () => {
            const result = mapper.mapFromManualEntry("10 Rue de Rivoli", "", "Paris", "Île-de-France", "75001", "France", "FR");

            expect(result.addressLine1).toEqual("10 Rue de Rivoli");
            expect(result.addressLine2).toEqual("");
            expect(result.town).toEqual("Paris");
            expect(result.region).toEqual("Île-de-France");
            expect(result.postcode).toEqual("75001");
            expect(result.country).toEqual("France");
            expect(result.countryCode).toEqual("FR");
            expect(result).not.toHaveProperty("county");
        }); 



        it("addressLine2 defaults to empty string when not provided", () => {
            const result = mapper.mapFromManualEntry("10 Downing Street", undefined, "London", "Greater London", "SW1A 2AA", "United Kingdom", "1");

            expect(result.addressLine2).toEqual("");
        });

        it("county defaults to empty string when not provided", () => {
            const result = mapper.mapFromManualEntry("10 Downing Street", "Westminster", "London", undefined, "SW1A 2AA", "United Kingdom", "1");

            expect(result.county).toEqual("");
        });

        it("countryCode defaults to empty string when not provided", () => {
            const result = mapper.mapFromManualEntry("10 Downing Street", "Westminster", "London", "Greater London", "SW1A 2AA", "United Kingdom", undefined);
        });
    });

    describe("addressesMatch", () => {
        const address = { addressLine1: "10 Downing Street", addressLine2: "Westminster", town: "London", county: "Greater London", country: "England", postcode: "SW1A 2AA" };

        it("returns true for identical addresses", () => {
            expect(mapper.addressesMatch(address, { ...address })).toBe(true);
        });

        it("returns false when addressLine1 differs", () => {
            expect(mapper.addressesMatch(address, { ...address, addressLine1: "11 Downing Street" })).toBe(false);
        });

        it("returns false when addressLine2 differs", () => {
            expect(mapper.addressesMatch(address, { ...address, addressLine2: "Whitehall" })).toBe(false);
        });

        it("returns false when town differs", () => {
            expect(mapper.addressesMatch(address, { ...address, town: "Manchester" })).toBe(false);
        });

        it("returns false when county differs", () => {
            expect(mapper.addressesMatch(address, { ...address, county: "West Midlands" })).toBe(false);
        });

        it("returns false when country differs", () => {
            expect(mapper.addressesMatch(address, { ...address, country: "Wales" })).toBe(false);
        });

        it("returns false when postcode differs", () => {
            expect(mapper.addressesMatch(address, { ...address, postcode: "SW1A 2AB" })).toBe(false);
        });

        it("treats undefined and empty string as equal", () => {
            const a = { addressLine1: "10 Downing Street", addressLine2: undefined, town: "London", county: "", country: undefined, postcode: "SW1A 2AA" };
            const b = { addressLine1: "10 Downing Street", addressLine2: "", town: "London", county: undefined, country: "", postcode: "SW1A 2AA" };

            expect(mapper.addressesMatch(a, b)).toBe(true);
        });
    });

    describe("mapFromInputs", () => {
        const originalInputs = [
            {
                "name": "BillingAddressLine1",
                "id": "BillingAddressLine1",
                "dataAddressLookup": "address-line-1",
                "value": "Telecom House"
            },
            {
                "name": "BillingAddressLine2",
                "id": "BillingAddressLine2",
                "dataAddressLookup": "address-line-2",
                "value": "125-135 Preston Road"
            },
            {
                "name": "BillingSomethingReallyRandom",
                "id": "BillingSomethingReallyRandom",
                "dataAddressLookup": "town-or-city",
                "value": "Brighton"
            },
            {
                "name": "BillingCounty",
                "id": "BillingCounty",
                "dataAddressLookup": "county",
                "value": ""
            },
            {
                "name": "BillingPostcode",
                "id": "BillingPostcode",
                "dataAddressLookup": "postcode",
                "value": ""
            },
            {
                "name": "BillingPostcode",
                "id": "BillingPostcode",
                "dataAddressLookup": "postcode-international",
                "value": "BN1 6AF"
            },
            {
                "name": "BillingUPRN",
                "id": "BillingUPRN",
                "dataAddressLookup": "UPRN",
                "value": "22275623"
            },
            {
                "name": "BillingCountryCode",
                "id": "BillingCountryCode",
                "dataAddressLookup": "country-code",
                "value": "1",
                "selectedOptionLabel": "United Kingdom"
            }
        ]

        it("maps input values to address fields based on data attributes", () => {
            const result = mapper.mapFromInput(originalInputs);

            expect(result.addressLine1).toEqual("Telecom House");
            expect(result.addressLine2).toEqual("125-135 Preston Road");
            expect(result.town).toEqual("Brighton");
            expect(result.county).toEqual("");
            expect(result.country).toEqual("United Kingdom");
            expect(result.countryCode).toEqual("1");
            expect(result.postcode).toEqual("BN1 6AF");
            expect(result.UPRN).toEqual("22275623");
        });
    });
});