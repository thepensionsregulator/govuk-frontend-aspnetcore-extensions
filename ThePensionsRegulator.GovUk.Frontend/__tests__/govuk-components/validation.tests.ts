import { jest } from "@jest/globals";
import { createFieldsetFormGroup } from "../../Scripts/govuk-components/fieldset";
import {
	createSelectFormGroup,
	createTextInputFormGroup,
} from "../../Scripts/govuk-components/inputs";
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
	}),
	{ virtual: true }
);

const {
	clearFormGroupError,
	focusFirstInvalidControl,
	showFormGroupError,
} = await import("../../Scripts/govuk-components/validation");

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

function createFieldsetWithInputs(): {
	fieldsetFormGroup: HTMLElement;
	firstInputFormGroup: HTMLElement;
	firstInput: HTMLInputElement;
	secondInputFormGroup: HTMLElement;
	secondInput: HTMLInputElement;
} {
	const firstInputFormGroup = createTextInputFormGroup({
		labelText: "First name",
		input: { id: "first-name", name: "first-name" },
	});
	const secondInputFormGroup = createTextInputFormGroup({
		labelText: "Last name",
		input: { id: "last-name", name: "last-name" },
	});
	const firstInput = firstInputFormGroup.querySelector("input");
	const secondInput = secondInputFormGroup.querySelector("input");

	if (!firstInput || !secondInput) {
		throw new Error("Expected input controls");
	}

	return {
		fieldsetFormGroup: createFieldsetFormGroup({
			id: "contact-details",
			legendText: "Contact details",
			children: [firstInputFormGroup, secondInputFormGroup],
		}),
		firstInputFormGroup,
		firstInput,
		secondInputFormGroup,
		secondInput,
	};
}

function createContactDetailsFieldsetFormGroup(): HTMLElement {
	return createFieldsetFormGroup({
		id: "contact-details",
		legendText: "Contact details",
		children: [],
	});
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

describe("select control errors", () => {
	it("should add and clear a select error", () => {
		const formGroup = createSelectFormGroup({
			labelText: "Country",
			hintText: "Select your country",
			select: {
				id: "country",
				name: "country",
				options: [{ value: "uk", text: "United Kingdom" }],
			},
		});
		const select = formGroup.querySelector("select");

		if (!select) {
			throw new Error("Expected a select control");
		}

		showFormGroupError(formGroup, "Select a country");

		expect(formGroup).toHaveClass("govuk-form-group--error");
		expect(select).toHaveClass("govuk-select--error");
		expect(formGroup.querySelector("#country-error")).toHaveTextContent(
			"Error: Select a country"
		);
		expect(select).toHaveAttribute(
			"aria-describedby",
			"country-hint country-error"
		);

		clearFormGroupError(formGroup);

		expect(formGroup).not.toHaveClass("govuk-form-group--error");
		expect(select).not.toHaveClass("govuk-select--error");
		expect(formGroup.querySelector("#country-error")).toBeNull();
		expect(select).toHaveAttribute("aria-describedby", "country-hint");
	});
});

describe("nested control errors", () => {
	it("should style only the input and its form group when there is no fieldset error", () => {
		const {
			fieldsetFormGroup,
			firstInputFormGroup,
			firstInput,
		} = createFieldsetWithInputs();

		showFormGroupError(firstInputFormGroup, "Enter a first name");

		expect(fieldsetFormGroup).not.toHaveClass("govuk-form-group--error");
		expect(firstInputFormGroup).toHaveClass("govuk-form-group--error");
		expect(firstInput).toHaveClass("govuk-input--error");
		expect(firstInputFormGroup.querySelector("#first-name-error")).toHaveTextContent(
			"Error: Enter a first name"
		);
		expect(firstInput).toHaveAttribute("aria-describedby", "first-name-error");
	});

	it("should leave the fieldset wrapper unstyled while another nested input has an error", () => {
		const {
			fieldsetFormGroup,
			firstInputFormGroup,
			secondInputFormGroup,
			secondInput,
		} = createFieldsetWithInputs();

		showFormGroupError(firstInputFormGroup, "Enter a first name");
		showFormGroupError(secondInputFormGroup, "Enter a last name");
		clearFormGroupError(firstInputFormGroup);

		expect(fieldsetFormGroup).not.toHaveClass("govuk-form-group--error");
		expect(firstInputFormGroup).not.toHaveClass("govuk-form-group--error");
		expect(secondInputFormGroup).toHaveClass("govuk-form-group--error");
		expect(secondInput).toHaveClass("govuk-input--error");
	});

	it("should leave the fieldset wrapper unstyled after its final nested input error is cleared", () => {
		const {
			fieldsetFormGroup,
			firstInputFormGroup,
		} = createFieldsetWithInputs();

		showFormGroupError(firstInputFormGroup, "Enter a first name");
		clearFormGroupError(firstInputFormGroup);

		expect(fieldsetFormGroup).not.toHaveClass("govuk-form-group--error");
	});
});

describe("fieldset form group errors", () => {
	it("should add a fieldset error without replacing a nested input error", () => {
		const {
			fieldsetFormGroup,
			firstInputFormGroup,
			firstInput,
		} = createFieldsetWithInputs();

		showFormGroupError(firstInputFormGroup, "Enter a first name");
		showFormGroupError(fieldsetFormGroup, "Enter valid contact details");

		expect(firstInputFormGroup.querySelector("#first-name-error")).toHaveTextContent(
			"Error: Enter a first name"
		);
		expect(firstInput).toHaveAttribute("aria-describedby", "first-name-error");
		expect(fieldsetFormGroup.querySelector("#contact-details-error")).toHaveTextContent(
			"Error: Enter valid contact details"
		);
		expect(fieldsetFormGroup.querySelectorAll(".govuk-error-message")).toHaveLength(2);
	});

	it("should preserve fieldset error styling when clearing a nested input error", () => {
		const { fieldsetFormGroup, firstInputFormGroup } =
			createFieldsetWithInputs();

		showFormGroupError(firstInputFormGroup, "Enter a first name");
		showFormGroupError(fieldsetFormGroup, "Enter valid contact details");
		clearFormGroupError(firstInputFormGroup);

		expect(fieldsetFormGroup).toHaveClass("govuk-form-group--error");
		expect(
			fieldsetFormGroup.querySelector("#contact-details-error")
		).toHaveTextContent("Error: Enter valid contact details");
	});

	it("should clear the fieldset wrapper styling while preserving a nested input error", () => {
		const { fieldsetFormGroup, firstInputFormGroup, firstInput } =
			createFieldsetWithInputs();
		const fieldset = fieldsetFormGroup.querySelector("fieldset");

		showFormGroupError(firstInputFormGroup, "Enter a first name");
		showFormGroupError(fieldsetFormGroup, "Enter valid contact details");
		expect(fieldsetFormGroup).toHaveClass("govuk-form-group--error");

		clearFormGroupError(fieldsetFormGroup);

		expect(fieldsetFormGroup).not.toHaveClass("govuk-form-group--error");
		expect(firstInputFormGroup).toHaveClass("govuk-form-group--error");
		expect(firstInput).toHaveClass("govuk-input--error");
		expect(firstInputFormGroup.querySelector("#first-name-error")).toHaveTextContent(
			"Error: Enter a first name"
		);
		expect(firstInput).toHaveAttribute("aria-describedby", "first-name-error");
		expect(fieldsetFormGroup.querySelector("#contact-details-error")).toBeNull();
		expect(fieldset).not.toHaveAttribute("aria-describedby");
	});
	
	it("should add a fieldset error after the legend and describe the fieldset", () => {
		const formGroup = createContactDetailsFieldsetFormGroup();
		const fieldset = formGroup.querySelector("fieldset");

		showFormGroupError(formGroup, "Enter valid contact details");

		const errorMessage = formGroup.querySelector("#contact-details-error");

		expect(formGroup).toHaveClass("govuk-form-group--error");
		expect(errorMessage).toHaveTextContent("Error: Enter valid contact details");
		expect(fieldset?.children[1]).toBe(errorMessage);
		expect(fieldset).toHaveAttribute("aria-describedby", "contact-details-error");
		expect(ensureErrorSummary).toHaveBeenCalledTimes(1);
		expect(updateErrorSummary).toHaveBeenCalledTimes(1);
		expect(updateTitle).toHaveBeenCalledTimes(1);
	});

	it("should clear a fieldset error and remove its description", () => {
		const formGroup = createContactDetailsFieldsetFormGroup();
		const fieldset = formGroup.querySelector("fieldset");
		showFormGroupError(formGroup, "Enter valid contact details");
		jest.clearAllMocks();

		clearFormGroupError(formGroup);

		expect(formGroup).not.toHaveClass("govuk-form-group--error");
		expect(formGroup.querySelector("#contact-details-error")).toBeNull();
		expect(fieldset).not.toHaveAttribute("aria-describedby");
		expect(ensureErrorSummary).not.toHaveBeenCalled();
		expect(updateErrorSummary).toHaveBeenCalledTimes(1);
		expect(updateTitle).toHaveBeenCalledTimes(1);
	});
});

describe("focusFirstInvalidControl", () => {
	it ("should focus on the first invalid element in a form", () => {
		const { fieldsetFormGroup, secondInput, secondInputFormGroup } = createFieldsetWithInputs();
		const form = document.createElement("form");
		form.appendChild(fieldsetFormGroup);
		document.body.appendChild(form);
		showFormGroupError(secondInputFormGroup, "Enter a second name");

		focusFirstInvalidControl(form);

		expect(document.activeElement).toBe(secondInput);
		document.body.removeChild(form);
	});

	it ("should focus on the first input in the fieldset if the fieldset has an error", () => {
		const firstInputFormGroup = createTextInputFormGroup({ labelText: "First element", input : { name: "first-element", id: "first-element" }});
		const { fieldsetFormGroup, firstInput } = createFieldsetWithInputs();
		const form = document.createElement("form");
		form.appendChild(firstInputFormGroup);
		form.appendChild(fieldsetFormGroup);
		document.body.appendChild(form);
		showFormGroupError(fieldsetFormGroup, "Enter valid contact details");

		focusFirstInvalidControl(form);

		expect(document.activeElement).toBe(firstInput);
		document.body.removeChild(form);
	});
});
