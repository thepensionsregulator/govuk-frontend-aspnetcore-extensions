import { createFormGroup, createLabel, createHint, createTextInput, createTextInputFormGroup, createCheckbox, createCheckboxesFormGroup } from "../../Scripts/govuk-components/inputs";
import "@testing-library/jest-dom";

describe("createFormGroup", () => {
    const testLabel = document.createElement("label");
    const testControl = document.createElement("input");
    it("should render a div with the govuk form group class", () => {
        const formGroup = createFormGroup({ label: testLabel, control: testControl });

        expect(formGroup.tagName).toBe("DIV");
        expect(formGroup).toHaveClass("govuk-form-group");
    });

    it("should contain the label and control, in that order", () => {
        const formGroup = createFormGroup({ label: testLabel, control: testControl });

        expect(formGroup.children[0]).toBe(testLabel);
        expect(formGroup.children[1]).toBe(testControl);
    });
});

describe("createLabel", () => {
    it("should render a label with the govuk label class", () => {
        const label = createLabel({
            labelText: "Test Label",
            htmlFor: "test-input",
        });

        expect(label.tagName).toBe("LABEL");
        expect(label).toHaveClass("govuk-label");
    });

    it("should contain set text content", () => {
        const label = createLabel({
            labelText: "Test Label",
            htmlFor: "test-input",
        });

        expect(label.textContent).toBe("Test Label");
    });

    it("should set the for attribute correctly", () => {
        const label = createLabel({
            labelText: "Test Label",
            htmlFor: "test-input",
        });

        expect(label).toHaveAttribute("for", "test-input");
    });
});

describe("createHint", () => {
    it("should render a div with the govuk hint class", () => {
        const hint = createHint({
            hintText: "Test Hint",
            id: "test-hint",
        });

        expect(hint.tagName).toBe("DIV");
        expect(hint).toHaveClass("govuk-hint");
    });

    it("should set the id attribute correctly", () => {
        const hint = createHint({
            hintText: "Test Hint",
            id: "test-hint",
        });

        expect(hint).toHaveAttribute("id", "test-hint");
    });
});

describe("createTextInput", () => {
    it("should render an input with the govuk input class", () => {
        const input = createTextInput({id: "test-input", name: "test-input"});

        expect(input.tagName).toBe("INPUT");
        expect(input).toHaveClass("govuk-input");
    });

    it("should set the id and name attributes correctly", () => {
        const input = createTextInput({id: "test-input", name: "test-input"});

        expect(input).toHaveAttribute("id", "test-input");
        expect(input).toHaveAttribute("name", "test-input");
    });

    it("should default the type to text if not provided", () => {
        const input = createTextInput({id: "test-input", name: "test-input"});

        expect(input).toHaveAttribute("type", "text");
    });

    it("should set the type attribute correctly if provided", () => {
        const input = createTextInput({id: "test-input", name: "test-input", type: "email"});
        expect(input).toHaveAttribute("type", "email");
    });

    it("should set the value attribute correctly if provided", () => {
        const input = createTextInput({id: "test-input", name: "test-input", value: "test value"});
        expect(input).toHaveValue("test value");
    });

    it("should set the autocomplete attribute correctly if provided", () => {
        const input = createTextInput({id: "test-input", name: "test-input", attributes: { autocomplete: "address-line-1" }});
        expect(input).toHaveAttribute("autocomplete", "address-line-1");
    });

    it.each([    
        ["xx-small", "govuk-input--width-2"],
        ["x-small", "govuk-input--width-3"],
        ["small", "govuk-input--width-4"],
        ["medium", "govuk-input--width-5"],
        ["large", "govuk-input--width-10"],
        ["x-large", "govuk-input--width-20"],
        ["xx-large", "govuk-input--width-30"]
    ] as const)(
        "should map width %s to %s class",
        (width, expectedClass) => {
        const input = createTextInput({id: "test-input", name: "test-input", width});
        expect(input).toHaveClass(expectedClass);
    });
});

describe("createTextInputFormGroup",() => {
    it("should create a GOV.UK form group", () => {
        const formGroup = createTextInputFormGroup({
            labelText: "Email address",
            input: {
                id: "email",
                name: "email",
            },
        });

        expect(formGroup).toHaveClass("govuk-form-group");
    });

    it("should create the label and associate it with the input", () => {
        const formGroup = createTextInputFormGroup({
            labelText: "Email address",
            input: {
                id: "email",
                name: "email",
            },
        });

        const label = formGroup.querySelector("label");
        const input = formGroup.querySelector("input");

        expect(label).toHaveTextContent("Email address");
        expect(label).toHaveAttribute("for", "email");
        expect(input).toHaveAttribute("id", "email");
    });

    it("should create the text input with the supplied options", () => {
        const formGroup = createTextInputFormGroup({
            labelText: "Email address",
            input: {
                id: "email",
                name: "email",
                type: "email",
                value: "person@example.com",
                width: "medium",
            },
        });

        const input = formGroup.querySelector("input");

        expect(input).toHaveAttribute("name", "email");
        expect(input).toHaveAttribute("type", "email");
        expect(input).toHaveValue("person@example.com");
        expect(input).toHaveClass("govuk-input", "govuk-input--width-5");
    });

    it("should include the hint when hint text is supplied", () => {
        const formGroup = createTextInputFormGroup({
            labelText: "Postcode",
            hintText: "For example, SW1A 2AA",
            input: {
                id: "postcode",
                name: "postcode",
            },
        });

        const hint = formGroup.querySelector(".govuk-hint");
        const input = formGroup.querySelector("input");

        expect(hint).toHaveTextContent("For example, SW1A 2AA");
        expect(hint).toHaveAttribute("id", "postcode-hint");
        expect(input).toHaveAttribute("aria-describedby", "postcode-hint");
    });

    it("should not include a hint when hint text is omitted", () => {
        const formGroup = createTextInputFormGroup({
            labelText: "Postcode",
            input: {
                id: "postcode",
                name: "postcode",
            },
        });

        expect(formGroup.querySelector(".govuk-hint")).toBeNull();
        expect(formGroup.querySelector("input")).not.toHaveAttribute(
            "aria-describedby"
        );
    });

    it("should render the label, hint, and input in order", () => {
        const formGroup = createTextInputFormGroup({
            labelText: "Postcode",
            hintText: "For example, SW1A 2AA",
            input: {
                id: "postcode",
                name: "postcode",
            },
        });

        expect(formGroup.children[0]).toHaveClass("govuk-label");
        expect(formGroup.children[1]).toHaveClass("govuk-hint");
        expect(formGroup.children[2]).toHaveClass("govuk-input");
    });
});

describe("createCheckbox", () => {
    const checkbox = createCheckbox({ name: "test", labelText: "Test Label" });
    it("should create a label", () => {
        const label = checkbox.querySelector("label");
        expect(label).not.toBeNull();
    });
    it("should create a label with the correct text", () => {
        const label = checkbox.querySelector("label");
        expect(label).toHaveTextContent("Test Label");
    });
    it("should create a label with the correct classes applied", () => {
        const label = checkbox.querySelector("label");
        expect(label).toHaveClass("govuk-label");
        expect(label).toHaveClass("govuk-checkboxes__label");
    });

    it("should create an input",() => {
        const input = checkbox.querySelector("input");
        expect(input).not.toBeNull();
    });
    it("should have the correct type attribute", () => {
        const input = checkbox.querySelector("input");
        expect(input).toHaveAttribute("type", "checkbox");
    });
    it("should have the correct name attribute", () => {
        const input = checkbox.querySelector("input");
        expect(input).toHaveAttribute("name", "test");
    });
    it("should have the correct id attribute", () => {
        const input = checkbox.querySelector("input");
        expect(input).toHaveAttribute("id", "test");
    });
    it("should have the correct classes applied", () => {
        const input = checkbox.querySelector("input");
        expect(input).toHaveClass("govuk-checkboxes__input");
    });

    it("should be wrapped in a div with the correct class", () => {
        expect(checkbox).toHaveClass("govuk-checkboxes__item");

        const input = checkbox.querySelector("input");
        const label = checkbox.querySelector("label");
        expect(checkbox).toContainElement(input);
        expect(checkbox).toContainElement(label);
    });
    it("should trigger the onClick event when the checkbox is clicked", () => {
        const mockOnClick = jest.fn();
        const checkboxWithOnClick = createCheckbox({ name: "test-onclick", labelText: "Test OnClick", onClick: mockOnClick });
        const input = checkboxWithOnClick.querySelector("input");
        if (input) {
            input.click();
        }
        expect(mockOnClick).toHaveBeenCalled();
    });
});

describe("createCheckboxFormGroup", () => {
    const checkbox1 = createCheckbox({ name: "test1", labelText: "Test Label 1" });
    const checkbox2 = createCheckbox({ name: "test2", labelText: "Test Label 2" });
    const formGroup = createCheckboxesFormGroup({
        checkboxes: [checkbox1, checkbox2],
    });

    it("should create a div with the correct class", () => {
        expect(formGroup.tagName).toBe("DIV");
        expect(formGroup).toHaveClass("govuk-form-group");
    });

    it("should contain a div with the correct class", () => {
        const div = formGroup.querySelector("div");
        expect(div).not.toBeNull();
        expect(div).toHaveClass("govuk-checkboxes");
        expect(formGroup).toContainElement(div);
    });

    it("should contain the individual checkboxes within the inner div", () => {
        const div = formGroup.querySelector("div");
        expect(div).toContainElement(checkbox1);
        expect(div).toContainElement(checkbox2);
    });

    it("should create a form group containing the checkboxes", () => {
        expect(formGroup).toContainElement(checkbox1);
        expect(formGroup).toContainElement(checkbox2);
    });
});