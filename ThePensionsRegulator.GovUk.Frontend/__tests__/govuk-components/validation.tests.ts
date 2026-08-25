import { clearControlError, showControlError } from "../../Scripts/govuk-components/validation";
import { createTextInputFormGroup } from "../../Scripts/govuk-components/inputs";
import "@testing-library/jest-dom";

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

describe("showControlError", () => {
	const id = "postcode";
	it("should apply GOV.UK error classes to an input and its form group", () => {
		const { formGroup, input } = createInputFormGroup(id);

		showControlError(input, "Enter a valid postcode");

		expect(formGroup).toHaveClass("govuk-form-group--error");
		expect(input).toHaveClass("govuk-input--error");
	});

	it("should create an accessible error message before the control", () => {
		const { formGroup, input } = createInputFormGroup(id);

		showControlError(input, "Enter a valid postcode");

		const errorMessage = formGroup.querySelector(`#${id}-error`);

		expect(errorMessage).toHaveClass("govuk-error-message");
		expect(errorMessage).toHaveTextContent("Error: Enter a valid postcode");
		expect(errorMessage?.querySelector(".govuk-visually-hidden")).toHaveTextContent("Error:");
		expect(formGroup.children[2]).toBe(errorMessage);
		expect(formGroup.children[3]).toBe(input);
	});

	it("should add the error ID to aria-describedby without removing the hint ID", () => {
		const { input } = createInputFormGroup(id);

		showControlError(input, "Enter a valid postcode");

		expect(input).toHaveAttribute("aria-describedby", `${id}-hint ${id}-error`);
	});

	it("should update an existing error message without adding another error ID", () => {
		const { formGroup, input } = createInputFormGroup(id);

		showControlError(input, "Enter a valid postcode");
		showControlError(input, "Postcode is not recognised");

		expect(formGroup.querySelectorAll(".govuk-error-message")).toHaveLength(1);
		expect(formGroup.querySelector(`#${id}-error`)).toHaveTextContent(
			"Error: Postcode is not recognised"
		);
		expect(input).toHaveAttribute("aria-describedby", `${id}-hint ${id}-error`);
	});
});

describe("clearControlError", () => {
	const id = "first-name";
	it("should remove error classes and the error message while preserving the hint Id and aria-describedby", () => {
		const { formGroup, input } = createInputFormGroup(id);
		showControlError(input, "Enter a valid postcode");

		clearControlError(input);

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

		showControlError(input, "Enter a valid postcode");
		clearControlError(input);

		expect(input).not.toHaveAttribute("aria-describedby");
	});
});
