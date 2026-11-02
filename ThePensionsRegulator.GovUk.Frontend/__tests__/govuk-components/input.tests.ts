import { createFormGroup, createLabel, createHint, createTextInput, createTextInputFormGroup } from "../../Scripts/govuk-components/inputs";
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