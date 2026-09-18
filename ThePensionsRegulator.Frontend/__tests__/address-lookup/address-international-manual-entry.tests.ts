import "@testing-library/jest-dom";
import { jest } from "@jest/globals";
import { renderAddressInternationalManualEntry, validateInternationalUkManualEntryAddress } from "../../Scripts/address-lookup/address-international-manual-entry.js";

const countries = [
	{ value: "FR", text: "France" },
	{ value: "US", text: "United States" }
];

function createValidValues(overrides: Partial<Parameters<typeof validateInternationalUkManualEntryAddress>[0]> = {}) {
	return {
		addressLine1: "123 Rue de Rivoli",
		addressLine2: "",
		addressLine3: "",
		postTown: "Paris",
		countyStateProvince: "",
		countryId: "FR",
		countryName: "France",
		postcode: "75001",
		...overrides
	};
}

describe("renderAddressInternationalManualEntry", () => {
	function createComponent() {
		const onAddressSubmitted = jest.fn();
		const onBackToSearchRequested = jest.fn();
		const component = renderAddressInternationalManualEntry({ onAddressSubmitted, onBackToSearchRequested, countries });

		return { component, onAddressSubmitted, onBackToSearchRequested };
	}

	it("renders the international entry fields, country options, and secondary actions", () => {
		const { component } = createComponent();
		const country = component.querySelector<HTMLSelectElement>("#country");
		const confirmButton = component.querySelector<HTMLButtonElement>("button");
		const backLink = component.querySelector<HTMLAnchorElement>("nav a");

		expect(component.querySelector("#address-line-1")).toHaveClass("govuk-input", "govuk-input--width-20");
		expect(component.querySelector("#address-line-2")).toHaveClass("govuk-input", "govuk-input--width-20");
		expect(component.querySelector("#address-line-3")).toHaveClass("govuk-input", "govuk-input--width-20");
		expect(component.querySelector("#post-town")).toHaveClass("govuk-input", "govuk-input--width-20");
		expect(component.querySelector("#county-state-province")).toHaveClass("govuk-input", "govuk-input--width-20");
		expect(component.querySelector("#postcode")).toHaveClass("govuk-input", "govuk-input--width-20");
		expect(country).toHaveClass("govuk-select", "govuk-input--width-20");
		expect(Array.from(country!.options).map(option => [option.value, option.text])).toEqual([
			["", "Select a country"],
			["FR", "France"],
			["US", "United States"]
		]);
		expect(confirmButton).toHaveTextContent("Confirm address");
		expect(confirmButton).toHaveClass("govuk-button", "govuk-button--secondary");
		expect(backLink).toHaveTextContent("Back to postcode search");
	});

	it("submits the selected country ID and name with a valid international address", () => {
		const { component, onAddressSubmitted } = createComponent();
		const addressLine1 = component.querySelector<HTMLInputElement>("#address-line-1");
		const postTown = component.querySelector<HTMLInputElement>("#post-town");
		const country = component.querySelector<HTMLSelectElement>("#country");
		const confirmButton = component.querySelector<HTMLButtonElement>("button");

		addressLine1!.value = "123 Rue de Rivoli";
		postTown!.value = "Paris";
		country!.value = "FR";

		confirmButton!.click();

		expect(onAddressSubmitted).toHaveBeenCalledWith({
			addressLine1: "123 Rue de Rivoli",
			addressLine2: "",
			addressLine3: "",
			postTown: "Paris",
			countyStateProvince: "",
			countryId: "FR",
			countryName: "France",
			postcode: ""
		});
	});

	it("shows errors for invalid fields and does not submit", () => {
		const { component, onAddressSubmitted } = createComponent();
		const addressLine1 = component.querySelector<HTMLInputElement>("#address-line-1");
		const postTown = component.querySelector<HTMLInputElement>("#post-town");
		const country = component.querySelector<HTMLSelectElement>("#country");

		component.querySelector<HTMLButtonElement>("button")!.click();

		expect(onAddressSubmitted).not.toHaveBeenCalled();
		expect(addressLine1).toHaveAttribute("aria-describedby", expect.stringContaining("address-line-1-error"));
		expect(postTown).toHaveAttribute("aria-describedby", expect.stringContaining("post-town-error"));
		expect(country).toHaveAttribute("aria-describedby", expect.stringContaining("country-error"));
		expect(component.querySelector("#address-line-1-error")).toHaveClass("govuk-error-message");
		expect(component.querySelector("#post-town-error")).toHaveClass("govuk-error-message");
		expect(component.querySelector("#country-error")).toHaveClass("govuk-error-message");
	});

	it("clears validation errors when corrected values are confirmed", () => {
		const { component, onAddressSubmitted } = createComponent();
		const addressLine1 = component.querySelector<HTMLInputElement>("#address-line-1");
		const postTown = component.querySelector<HTMLInputElement>("#post-town");
		const country = component.querySelector<HTMLSelectElement>("#country");
		const confirmButton = component.querySelector<HTMLButtonElement>("button");

		confirmButton!.click();
		addressLine1!.value = "123 Rue de Rivoli";
		postTown!.value = "Paris";
		country!.value = "FR";
		confirmButton!.click();

		expect(onAddressSubmitted).toHaveBeenCalledTimes(1);
		expect(component.querySelector("#address-line-1-error")).toBeNull();
		expect(component.querySelector("#post-town-error")).toBeNull();
		expect(component.querySelector("#country-error")).toBeNull();
	});

	it("calls the back-to-search handler without navigating", () => {
		const { component, onBackToSearchRequested } = createComponent();
		const clickEvent = new MouseEvent("click", { cancelable: true });

		component.querySelector<HTMLAnchorElement>("nav a")!.dispatchEvent(clickEvent);

		expect(clickEvent.defaultPrevented).toBe(true);
		expect(onBackToSearchRequested).toHaveBeenCalledTimes(1);
	});
});

describe("validateInternationalUkManualEntryAddress", () => {
	it("returns an international address when required fields are valid", () => {
		expect(validateInternationalUkManualEntryAddress(createValidValues())).toEqual({
			isValid: true,
			address: createValidValues()
		});
	});

	it.each([
		["addressLine1", { addressLine1: "" }],
		["addressLine1", { addressLine1: "a".repeat(101) }],
		["addressLine2", { addressLine2: "a".repeat(101) }],
		["addressLine3", { addressLine3: "a".repeat(101) }],
		["postTown", { postTown: "" }],
		["postTown", { postTown: "a".repeat(101) }],
		["countyStateProvince", { countyStateProvince: "a".repeat(101) }],
		["countryId", { countryId: "" }],
		["postcode", { postcode: "a".repeat(11) }]
	] as const)("returns one error for an invalid %s value", (fieldName, overrides) => {
		const result = validateInternationalUkManualEntryAddress(createValidValues(overrides));

		expect(result.isValid).toBe(false);
		if (!result.isValid) {
			expect(result.errorMessages).toHaveLength(1);
			expect(result.errorMessages[0].fieldName).toBe(fieldName);
		}
	});

	it("returns one error per invalid field in form order", () => {
		const result = validateInternationalUkManualEntryAddress(createValidValues({
			addressLine1: "",
			postTown: "",
			countryId: ""
		}));

		expect(result).toEqual({
			isValid: false,
			errorMessages: expect.arrayContaining([
				expect.objectContaining({ fieldName: "addressLine1" }),
				expect.objectContaining({ fieldName: "postTown" }),
				expect.objectContaining({ fieldName: "countryId" })
			])
		});
	});
});
