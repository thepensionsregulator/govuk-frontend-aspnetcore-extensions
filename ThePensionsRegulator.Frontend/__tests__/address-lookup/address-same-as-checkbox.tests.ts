import "@testing-library/jest-dom";
import { jest } from "@jest/globals";
import { renderSameAsCheckbox } from "../../Scripts/address-lookup/address-same-as-checkbox";
import { createMockAddressLookupContent } from "../../__test-helpers__/mock-address-lookup-content";

describe("renderSameAsCheckbox", () => {
    it.each([true, false])("renders the initial checked state as %s", checked => {
        const component = renderSameAsCheckbox(createMockAddressLookupContent(), checked);
        const input = component.querySelector<HTMLInputElement>("input[type='checkbox']")!;

        expect(input.checked).toBe(checked);
        expect(input).toHaveAttribute("name", "same-as");
    });

    it.each(["", "Same as registered office"])("renders an associated label for %s", sameAsLabel => {
        const component = renderSameAsCheckbox({ ...createMockAddressLookupContent(), sameAsLabel }, false);
        const input = component.querySelector<HTMLInputElement>("input[type='checkbox']")!;
        const label = component.querySelector("label")!;

        expect(label).toHaveTextContent(sameAsLabel || "Same as above");
        expect(label.htmlFor).toBe(input.id);
        expect(input.id).not.toBe("");
    });

    it("passes the click event to the callback after toggling the input", () => {
        const onClick = jest.fn<(event: Event) => void>();
        const component = renderSameAsCheckbox(createMockAddressLookupContent(), false, onClick);
        const input = component.querySelector<HTMLInputElement>("input[type='checkbox']")!;

        input.click();

        expect(input).toBeChecked();
        expect(onClick).toHaveBeenCalledTimes(1);
        expect(onClick.mock.calls[0][0].target).toBe(input);
    });

    it("can be toggled without a callback", () => {
        const component = renderSameAsCheckbox(createMockAddressLookupContent(), true);
        const input = component.querySelector<HTMLInputElement>("input[type='checkbox']")!;

        input.click();

        expect(input).not.toBeChecked();
    });
});