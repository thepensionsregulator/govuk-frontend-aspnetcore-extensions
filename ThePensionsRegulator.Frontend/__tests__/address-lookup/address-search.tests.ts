import { jest } from "@jest/globals";
import "@testing-library/jest-dom";
import { renderAddressSearch } from "../../Scripts/address-lookup/address-search";
import { FetchAddressSearchService } from "../../Scripts/address-lookup/address-search-service";
import type { AddressSearchCriteria, AddressSearchResult } from "../../Scripts/address-lookup/types";

describe("renderAddressSearch", () => {
    function createSearch(criteria?: AddressSearchCriteria){
        const searchService = new FetchAddressSearchService({ searchEndpoint: "https://example.com/search", addressByIdEndpoint: "https://example.com/address", fetchFunction: jest.fn(() => Promise.resolve(new Response())) });
        const searchAddress = jest.fn<(postcode: string) => Promise<AddressSearchResult[]>>()
            .mockResolvedValue([]);
        searchService.searchAddress = searchAddress;
        const onSearchSuccess = jest.fn<(results: AddressSearchResult[], criteria: AddressSearchCriteria) => void>();

        const component = renderAddressSearch({ searchService, onSearchSuccess, criteria });

        const buildingInput = component.querySelector<HTMLInputElement>("#building");
        const postcodeInput = component.querySelector<HTMLInputElement>("#postcode");
        const button = component.querySelector<HTMLButtonElement>("button");

        return { component, buildingInput, postcodeInput, button, searchOptions: searchAddress, onSearchSuccess };
    }
    
    it("should leave the inputs empty when no criteria is provided", () => {
        const { buildingInput, postcodeInput } = createSearch();

        expect(buildingInput).toHaveValue("");
        expect(postcodeInput).toHaveValue("");
    });

    it("should pre-fill the inputs from the provided criteria", () => {
        const { buildingInput, postcodeInput } = createSearch({ buildingName: "1", postcode: "SW1A 2AA" });

        expect(buildingInput).toHaveValue("1");
        expect(postcodeInput).toHaveValue("SW1A 2AA");
    });

    it("should render search inputs and a button in a fieldset", () => {
        const { component, buildingInput, postcodeInput, button } = createSearch();
        const fieldset = component.querySelector("fieldset");

        expect(component).toContainElement(fieldset);
        expect(fieldset).toContainElement(buildingInput);
        expect(fieldset).toContainElement(postcodeInput);
        expect(component).toContainElement(button);
    });

    it("should show a required error and not search when postcode is empty", () => {
        const { component, button, searchOptions } = createSearch();

        button?.click();

        expect(component.querySelector("#postcode-error")).toHaveTextContent("Error: Enter a postcode");
        expect(searchOptions).not.toHaveBeenCalled();
    });

    it("should show a format error and not search for an invalid postcode", () => {
        const { component, searchOptions: onSearch, postcodeInput, button } = createSearch();
        postcodeInput!.value = "not-a-postcode";

        button?.click();

        expect(component.querySelector("#postcode-error")).toHaveTextContent(
            "Error: Enter a valid postcode"
        );
        expect(onSearch).not.toHaveBeenCalled();
    });

    

    it("should search with a normalised postcode while preserving input text", () => {
        const { postcodeInput, button, searchOptions } = createSearch();
        postcodeInput!.value = "sw1a2aa";

        button!.click();

        expect(searchOptions).toHaveBeenCalledWith("SW1A 2AA");

        expect(postcodeInput).toHaveValue("sw1a2aa");
    });

    it("should return all addresses when building name or number is empty", async () => {
        const { postcodeInput, button, searchOptions, onSearchSuccess } = createSearch();
        const addresses = [
            { UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA", BUILDING_NUMBER: "1" },
            { UPRN: "2", UDPRN: "2", ADDRESS: "2 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA", BUILDING_NUMBER: "2" }
        ];
        searchOptions.mockResolvedValue(addresses);
        postcodeInput!.value = "SW1A 2AA";

        button!.click();
        await Promise.resolve();

        expect(onSearchSuccess).toHaveBeenCalledWith(addresses, { buildingName: "", postcode: "SW1A 2AA" });
        expect(button).not.toBeDisabled();
    });

    it("should return all addresses matching the building number", async () => {
        const { buildingInput, postcodeInput, button, searchOptions, onSearchSuccess } = createSearch();
        const addresses = [
            { UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA", BUILDING_NUMBER: "1" },
            { UPRN: "2", UDPRN: "2", ADDRESS: "1 Station Road", POST_TOWN: "London", POSTCODE: "SW1A 2AA", BUILDING_NUMBER: "1" },
            { UPRN: "3", UDPRN: "3", ADDRESS: "2 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA", BUILDING_NUMBER: "2" }
        ];
        searchOptions.mockResolvedValue(addresses);
        buildingInput!.value = "1";
        postcodeInput!.value = "SW1A 2AA";

        button!.click();
        await Promise.resolve();

        expect(onSearchSuccess).toHaveBeenCalledWith([addresses[0], addresses[1]], { buildingName: "1", postcode: "SW1A 2AA" });
        expect(button).not.toBeDisabled();
    });

    it("should show an error when no addresses match the building", async () => {
        const { buildingInput, postcodeInput, button, searchOptions, onSearchSuccess, component } = createSearch();
        searchOptions.mockResolvedValue([
            { UPRN: "1", UDPRN: "1", ADDRESS: "2 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA", BUILDING_NUMBER: "2" }
        ]);
        buildingInput!.value = "1";
        postcodeInput!.value = "SW1A 2AA";

        button!.click();
        await Promise.resolve();

        expect(component.querySelector("#address-search-fieldset-error")).toHaveTextContent(
            "Error: The address and postcode do not match"
        );
        expect(onSearchSuccess).not.toHaveBeenCalled();
        expect(button).not.toBeDisabled();
    });

    it("should show an error when address search fails", async () => {
        const { postcodeInput, button, searchOptions, onSearchSuccess, component } = createSearch();
        searchOptions.mockRejectedValue(new Error("search failed"));
        postcodeInput!.value = "SW1A 2AA";

        button!.click();
        await Promise.resolve();

        expect(component.querySelector("#address-search-fieldset-error")).toHaveTextContent(
            "Error: There was a problem searching for addresses. Please try again."
        );
        expect(onSearchSuccess).not.toHaveBeenCalled();
        expect(button).not.toBeDisabled();
    });

});