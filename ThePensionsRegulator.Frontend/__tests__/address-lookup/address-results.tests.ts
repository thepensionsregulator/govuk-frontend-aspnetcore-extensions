import { jest } from "@jest/globals";
import "@testing-library/jest-dom";
import { createMockAddressLookupContent } from "../../__test-helpers__/mock-address-lookup-content";

import { renderAddressResults } from "../../Scripts/address-lookup/address-results";
import type { AddressSearchCriteria, AddressSearchResult } from "../../Scripts/address-lookup/types";

describe("renderAddressResults", () => {
    const criteria: AddressSearchCriteria = { buildingName: "1", postcode: "SW1A 2AA" };
    const addressOne: AddressSearchResult = {
        UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA"
    };
    const addressTwo: AddressSearchResult = {
        UPRN: "2", UDPRN: "2", ADDRESS: "2 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA"
    };


    function createResults() {
        const onAddressSelected = jest.fn<(address: AddressSearchResult) => void>();
        const onBackToSearchRequested = jest.fn<() => void>();
        const onUkManualEntryRequested = jest.fn<() => void>();
        const content = createMockAddressLookupContent();
        const component = renderAddressResults({
            addresses: [addressOne, addressTwo],
            criteria,
            onAddressSelected,
            onBackToSearchRequested,
            onUkManualEntryRequested,
            content
        });

        const select = component.querySelector<HTMLSelectElement>("#address-select");
        const button = component.querySelector<HTMLButtonElement>("button");
        const backLink = component.querySelector<HTMLAnchorElement>("nav a");

        return { component, select, button, backLink, onAddressSelected, onBackToSearchRequested, content };
    }

    it("should render a select with a disabled placeholder and an option per address", () => {
        const { select } = createResults();
        const options = select!.querySelectorAll("option");

        expect(options).toHaveLength(3);
        expect(options[0]).toHaveValue("");
        expect(options[0]).toBeDisabled();
        expect(options[1]).toHaveValue(addressOne.UPRN);
        expect(options[1]).toHaveTextContent(addressOne.ADDRESS);
        expect(options[2]).toHaveValue(addressTwo.UPRN);
        expect(options[2]).toHaveTextContent(addressTwo.ADDRESS);
    });

    it("should select the placeholder by default", () => {
        const { select } = createResults();

        expect(select).toHaveValue("");
    });

    it("should render a Confirm address button and a Return to postcode search link", () => {
        const { component, button, backLink, content } = createResults();

        expect(button).toHaveTextContent(content.confirmAddressLabel);
        expect(button).toHaveAttribute("type", "button");
        expect(backLink).toHaveTextContent(content.backToPostcodeSearchLabel);
        expect(component.querySelector("nav")).toContainElement(backLink);
    });

    it("should show an error and not confirm when no address is selected", () => {
        const { component, button, onAddressSelected, content } = createResults();

        button!.click();

        expect(component.querySelector("#address-select-error")).toHaveTextContent(
            `Error: ${content.addressSelectErrorMessage}`
        );
        expect(onAddressSelected).not.toHaveBeenCalled();
    });

    it("should clear the error and confirm the selected address", () => {
        const { component, select, button, onAddressSelected } = createResults();
        select!.value = addressTwo.UPRN;

        button!.click();

        expect(component.querySelector("#address-select-error")).toBeNull();
        expect(onAddressSelected).toHaveBeenCalledWith(addressTwo);
    });

    it("should not navigate and should report back-to-search when the link is clicked", () => {
        const { backLink, onBackToSearchRequested } = createResults();
        const clickEvent = new MouseEvent("click", { cancelable: true });

        backLink!.dispatchEvent(clickEvent);

        expect(clickEvent.defaultPrevented).toBe(true);
        expect(onBackToSearchRequested).toHaveBeenCalledTimes(1);
    });

    it("has a select with the 'data-address-lookup-submit-error-anchor' attribute", () => {
        const { component } = createResults();
		const formGroup = component.querySelector(".govuk-form-group");
		expect(formGroup).toHaveAttribute("data-address-lookup-submit-error-anchor");
	});
});
