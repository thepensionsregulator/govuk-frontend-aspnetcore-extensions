import '@testing-library/jest-dom';

import { jest } from '@jest/globals';
import { AddressMapper } from "../../wwwroot/tpr/address-lookup/mapper.js";
import { ADDRESS_LOOKUP_CONFIG } from "../../wwwroot/tpr/address-lookup/config.js";

describe("Address mapper", () => {
    const mapper = new AddressMapper(ADDRESS_LOOKUP_CONFIG);
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
    });

    describe("mapFromManualUKEntry", () => {
        it("all fields are populated", () => {
            const result = mapper.mapFromManualUKEntry("10 Downing Street", "Westminster", "London", "Greater London", "SW1A 2AA");

            expect(result.addressLine1).toEqual("10 Downing Street");
            expect(result.addressLine2).toEqual("Westminster");
            expect(result.town).toEqual("London");
            expect(result.county).toEqual("Greater London");
            expect(result.postcode).toEqual("SW1A 2AA");
        });

        it("addressLine2 defaults to empty string when not provided", () => {
            const result = mapper.mapFromManualUKEntry("10 Downing Street", undefined, "London", "Greater London", "SW1A 2AA");

            expect(result.addressLine2).toEqual("");
        });

        it("county defaults to empty string when not provided", () => {
            const result = mapper.mapFromManualUKEntry("10 Downing Street", "Westminster", "London", undefined, "SW1A 2AA");

            expect(result.county).toEqual("");
        });
    });

    describe("mapFromManualInternationalEntry", () => {
        it("all fields are populated", () => {
            const result = mapper.mapFromManualInternationalEntry("1600 Pennsylvania Avenue", "Suite 1", "Washington", "DC", "United States", "20500");

            expect(result.addressLine1).toEqual("1600 Pennsylvania Avenue");
            expect(result.addressLine2).toEqual("Suite 1");
            expect(result.town).toEqual("Washington");
            expect(result.county).toEqual("DC");
            expect(result.country).toEqual("United States");
            expect(result.postcode).toEqual("20500");
        });

        it("addressLine2 defaults to empty string when not provided", () => {
            const result = mapper.mapFromManualInternationalEntry("1600 Pennsylvania Avenue", undefined, "Washington", "DC", "United States", "20500");

            expect(result.addressLine2).toEqual("");
        });

        it("region defaults to empty string when not provided", () => {
            const result = mapper.mapFromManualInternationalEntry("1600 Pennsylvania Avenue", "Suite 1", "Washington", undefined, "United States", "20500");

            expect(result.county).toEqual("");
        });
    });
});