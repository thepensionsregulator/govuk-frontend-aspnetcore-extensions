import "@testing-library/jest-dom";
import { jest } from "@jest/globals";
import { renderConfirmedAddress } from "../../Scripts/address-lookup/address-confirmed";
import type { ConfirmedAddress } from "../../Scripts/address-lookup/types";

describe("renderConfirmedAddress", () => {
    const completeAddress: ConfirmedAddress = {
        addressLine1: "10 High Street",
        addressLine2: "Flat 2",
        addressLine3: "West End",
        postTown: "London",
        postCounty: "Greater London",
        countryId: "GB",
        postCode: "SW1A 2AA",
        uprnReference: "100000000001"
    };

    it("should render populated address fields in a paragraph separated by breaks", () => {
        const component = renderConfirmedAddress(completeAddress, jest.fn());
        const paragraph = component.querySelector("p");

        expect(paragraph).not.toBeNull();
        expect(paragraph).toHaveTextContent("10 High StreetFlat 2West EndLondonGreater LondonSW1A 2AA");
        expect(paragraph?.querySelectorAll("br")).toHaveLength(5);
    });

    it("should omit optional fields without adding blank lines", () => {
        const address: ConfirmedAddress = {
            addressLine1: "10 High Street",
            postCode: "SW1A 2AA",
            uprnReference: "100000000001"
        };

        const paragraph = renderConfirmedAddress(address, jest.fn()).querySelector("p");

        expect(paragraph).toHaveTextContent("10 High StreetSW1A 2AA");
        expect(paragraph?.querySelectorAll("br")).toHaveLength(1);
    });

    it.each([
        { countryId: "GB", countryName: "United Kingdom", expectedCountry: false },
        { countryId: "FR", countryName: "France", expectedCountry: true }
    ])("should $expectedCountry show the country name for $countryId addresses", ({ countryId, countryName, expectedCountry }) => {
        const address: ConfirmedAddress = {
            addressLine1: "10 Rue de Rivoli",
            postTown: "Paris",
            countryId,
            countryName,
            postCode: "75001",
            uprnReference: ""
        };

        const paragraph = renderConfirmedAddress(address, jest.fn()).querySelector("p");

        if (expectedCountry) {
            expect(paragraph).toHaveTextContent(countryName);
        } else {
            expect(paragraph).not.toHaveTextContent(countryName);
        }
    });

    it("should create hidden inputs with the confirmed address field IDs", () => {
        const component = renderConfirmedAddress(completeAddress, jest.fn());
        const expectedValues = {
            AddressLine1: "10 High Street",
            AddressLine2: "Flat 2",
            AddressLine3: "West End",
            PostTown: "London",
            PostCounty: "Greater London",
            CountryId: "GB",
            PostCode: "SW1A 2AA",
            UPRNReference: "100000000001"
        };

        for (const [id, value] of Object.entries(expectedValues)) {
            const input = component.querySelector<HTMLInputElement>(`#${id}`);
            expect(input).toHaveValue(value);
            expect(input).toHaveAttribute("type", "hidden");
        }
    });

    it("should omit hidden inputs for absent optional values", () => {
        const address: ConfirmedAddress = {
            addressLine1: "10 High Street",
            postCode: "SW1A 2AA",
            uprnReference: "100000000001"
        };

        const component = renderConfirmedAddress(address, jest.fn());

        expect(component.querySelector("#AddressLine2")).toBeNull();
        expect(component.querySelector("#AddressLine3")).toBeNull();
        expect(component.querySelector("#PostTown")).toBeNull();
        expect(component.querySelector("#PostCounty")).toBeNull();
        expect(component.querySelector("#CountryId")).toBeNull();
    });

    it("should report when the change address link is clicked", () => {
        const onBackToSearchRequested = jest.fn();
        const component = renderConfirmedAddress(completeAddress, onBackToSearchRequested);
        const link = component.querySelector<HTMLAnchorElement>("a")!;
        const event = new MouseEvent("click", { cancelable: true });

        link.dispatchEvent(event);

        expect(event.defaultPrevented).toBe(true);
        expect(onBackToSearchRequested).toHaveBeenCalledTimes(1);
    });
});
