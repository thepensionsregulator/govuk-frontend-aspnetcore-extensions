import { jest } from "@jest/globals";
import "@testing-library/jest-dom";
import { createAddressSearch } from "../../Scripts/address-lookup/address-search";

describe("createAddressSearch", () => {
    function createSearch(){
        const searchOptions = jest.fn();
        const component = createAddressSearch({ onSearch: searchOptions });

        const buildingInput = component.querySelector<HTMLInputElement>("#building");
        const postcodeInput = component.querySelector<HTMLInputElement>("#postcode");
        const button = component.querySelector<HTMLButtonElement>("button");

        return { component, buildingInput, postcodeInput, button, searchOptions };
    }
    
    it("should render search inputs and a button in a fieldset", () => {
        const { component, buildingInput, postcodeInput, button } = createSearch();
        const fieldset = component.querySelector("fieldset");

        expect(component).toContainElement(fieldset);
        expect(fieldset).toContainElement(buildingInput);
        expect(fieldset).toContainElement(postcodeInput);
        expect(component).toContainElement(button);
    });

    it("show show a required error and not search when postcode is empty", () => {
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

        expect(searchOptions).toHaveBeenCalledWith({
            buildingName: undefined,
            postcode: "SW1A 2AA",
        });

        expect(postcodeInput).toHaveValue("sw1a2aa");
    });

    it("should include the optional building name in the search criteria", () => {
        const { buildingInput, postcodeInput, button, searchOptions } = createSearch();
        buildingInput!.value = "10";
        postcodeInput!.value = "sw1a2aa";

        button!.click();

        expect(searchOptions).toHaveBeenCalledWith({
            buildingName: "10",
            postcode: "SW1A 2AA",
        });
    });
});