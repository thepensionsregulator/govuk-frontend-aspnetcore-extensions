import { jest } from "@jest/globals";
import { createTextInputFormGroup } from "../../Scripts/govuk-components/inputs";
import "@testing-library/jest-dom";


const ensureErrorSummary = jest.fn();
const updateErrorSummary = jest.fn();
const updateTitle = jest.fn();

jest.unstable_mockModule(
	"../../Scripts/govuk-components/error-summary.js",
    () => ({
        ensureErrorSummary,
        updateErrorSummary,
        updateTitle,
    })
);

const { clearFormGroupError, showFormGroupError } = await import("../../Scripts/govuk-components/validation");

function createInputFormGroup(id: string): { formGroup: HTMLElement; input: HTMLInputElement } {
	const formGroup = createTextInputFormGroup({
		labelText: "Postcode",
		hintText: "For example, SW1A 2AA",
		input: {
			id,
			name: id,
		},
	});
	const input = formGroup.querySelector("input");

	if (!input) {
		throw new Error("Expected an input control");
	}

	return { formGroup, input };
}

beforeEach(() => {
	jest.clearAllMocks();
});

describe("showFormGroupError", () => {
	const id = "postcode";
	it("should apply GOV.UK error classes to an input and its form group", () => {
		const { formGroup, input } = createInputFormGroup(id);

		showFormGroupError(formGroup, "Enter a valid postcode");

		expect(formGroup).toHaveClass("govuk-form-group--error");
		expect(input).toHaveClass("govuk-input--error");
	});

	it("should create an accessible error message before the control", () => {
		const { formGroup, input } = createInputFormGroup(id);

		showFormGroupError(formGroup, "Enter a valid postcode");

		const errorMessage = formGroup.querySelector(`#${id}-error`);

		expect(errorMessage).toHaveClass("govuk-error-message");
		expect(errorMessage).toHaveTextContent("Error: Enter a valid postcode");
		expect(errorMessage?.querySelector(".govuk-visually-hidden")).toHaveTextContent("Error:");
		expect(formGroup.children[2]).toBe(errorMessage);
		expect(formGroup.children[3]).toBe(input);
	});

	it("should add the error ID to aria-describedby without removing the hint ID", () => {
		const { formGroup, input } = createInputFormGroup(id);

		showFormGroupError(formGroup, "Enter a valid postcode");

		expect(input).toHaveAttribute("aria-describedby", `${id}-hint ${id}-error`);
	});

	it("should update an existing error message without adding another error ID", () => {
		const { formGroup, input } = createInputFormGroup(id);

		showFormGroupError(formGroup, "Enter a valid postcode");
		showFormGroupError(formGroup, "Postcode is not recognised");

		expect(formGroup.querySelectorAll(".govuk-error-message")).toHaveLength(1);
		expect(formGroup.querySelector(`#${id}-error`)).toHaveTextContent(
			"Error: Postcode is not recognised"
		);
		expect(input).toHaveAttribute("aria-describedby", `${id}-hint ${id}-error`);
	});

	it("should update the page error summary and title", () => {
    	const { formGroup } = createInputFormGroup("postcode");

    	showFormGroupError(formGroup, "Enter a valid postcode");

    	expect(ensureErrorSummary).toHaveBeenCalledTimes(1);
    	expect(updateErrorSummary).toHaveBeenCalledTimes(1);
    	expect(updateTitle).toHaveBeenCalledTimes(1);
	});
});

describe("clearFormGroupError", () => {
	const id = "first-name";
	it("should remove error classes and the error message while preserving the hint Id and aria-describedby", () => {
		const { formGroup, input } = createInputFormGroup(id);
		showFormGroupError(formGroup, "Enter a valid postcode");
		clearFormGroupError(formGroup);

		expect(formGroup).not.toHaveClass("govuk-form-group--error");
		expect(input).not.toHaveClass("govuk-input--error");
		expect(formGroup.querySelector(`#${id}-error`)).toBeNull();
		expect(input).toHaveAttribute("aria-describedby", `${id}-hint`);
	});

	it("should remove aria-describedby when it only contained the error ID", () => {
		const input = document.createElement("input");
		input.id = id;
		const formGroup = document.createElement("div");
		formGroup.className = "govuk-form-group";
		formGroup.appendChild(input);

		showFormGroupError(formGroup, "Enter a valid postcode");
		clearFormGroupError(formGroup);

		expect(input).not.toHaveAttribute("aria-describedby");
	});

	it("should update the page error summary and title after clearing", () => {
    	const { formGroup } = createInputFormGroup("postcode");

    	showFormGroupError(formGroup, "Enter a valid postcode");
    	jest.clearAllMocks();

    	clearFormGroupError(formGroup);

    	expect(ensureErrorSummary).not.toHaveBeenCalled();
    	expect(updateErrorSummary).toHaveBeenCalledTimes(1);
    	expect(updateTitle).toHaveBeenCalledTimes(1);
	});
});
