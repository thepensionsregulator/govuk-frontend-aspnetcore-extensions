import {
    createSelect,
    createSelectFormGroup,
} from "../../../ThePensionsRegulator.GovUk.Frontend/Scripts/govuk-components/inputs";
import "@testing-library/jest-dom";

describe("createSelect", () => {
    it("should render a select element with the correct css class", () => {
        const select = createSelect({ id: "test-select", name: "test-select", options: [{ value: "1", text: "Option 1" }] });
   
        expect(select.tagName).toBe("SELECT");
        expect(select).toHaveClass("govuk-select");
    });

    it("should set the id and name attributes correctly", () => {
        const select = createSelect({ id: "test-select", name: "test-select", options: [] });
        
        expect(select.id).toBe("test-select");
        expect(select.name).toBe("test-select");
    });

    it.each([        
        ["xx-small", "govuk-input--width-2"],
        ["x-small", "govuk-input--width-3"],
        ["small", "govuk-input--width-4"],
        ["medium", "govuk-input--width-5"],
        ["large", "govuk-input--width-10"],
        ["x-large", "govuk-input--width-20"],
        ["xx-large", "govuk-input--width-30"]
    ] as const)
        ("should map width %s to %s class", (width, expectedClass) => {
        const select = createSelect({ id: "test-select", name: "test-select", options: [], width });
        
        expect(select).toHaveClass(expectedClass);
    });

    it("should render options correctly", () => {
        const options = [
            { value: "1", text: "Option 1" },
            { value: "2", text: "Option 2", disabled: true }
        ];
        const select = createSelect({ id: "test-select", name: "test-select", options });
        
        expect(select.options.length).toBe(options.length);
        expect(select.options[0].value).toBe("1");
        expect(select.options[0].text).toBe("Option 1");
        expect(select.options[1].value).toBe("2");
        expect(select.options[1].text).toBe("Option 2");
        expect(select.options[1].disabled).toBe(true);
    });

    it("should select the option matching the supplied value", () => {
        const select = createSelect({
            id: "test-select",
            name: "test-select",
            value: "2",
            options: [
                { value: "1", text: "Option 1" },
                { value: "2", text: "Option 2" },
            ],
        });

        expect(select).toHaveValue("2");
        expect(select.selectedOptions[0]).toHaveTextContent("Option 2");
    });

    it("should select the first option when no value is supplied", () => {
        const select = createSelect({
            id: "test-select",
            name: "test-select",
            options: [
                { value: "1", text: "Option 1" },
                { value: "2", text: "Option 2" },
            ],
        });

        expect(select).toHaveValue("1");
    });

    it("should have no selected value when the supplied value does not match an option", () => {
        const select = createSelect({
            id: "test-select",
            name: "test-select",
            value: "missing",
            options: [{ value: "1", text: "Option 1" }],
        });

        expect(select.value).toBe("");
        expect(select.selectedIndex).toBe(-1);
    });

    it("should apply custom select attributes", () => {
        const select = createSelect({
            id: "test-select",
            name: "test-select",
            options: [],
            attributes: {
                autocomplete: "country",
                "data-test": "country-select",
            },
        });

        expect(select).toHaveAttribute("autocomplete", "country");
        expect(select).toHaveAttribute("data-test", "country-select");
    });

    it("should apply custom option attributes", () => {
        const select = createSelect({
            id: "test-select",
            name: "test-select",
            options: [
                {
                    value: "uk",
                    text: "United Kingdom",
                    attributes: { "data-country-id": "826" },
                },
            ],
        });

        expect(select.options[0]).toHaveAttribute("data-country-id", "826");
    });

    it("should render option text as text rather than HTML", () => {
        const select = createSelect({
            id: "test-select",
            name: "test-select",
            options: [{ value: "1", text: "<strong>Option 1</strong>" }],
        });

        expect(select.options[0].textContent).toBe("<strong>Option 1</strong>");
        expect(select.options[0].querySelector("strong")).toBeNull();
    });
});

describe("createSelectFormGroup", () => {
    it("should associate the label with the select", () => {
        const formGroup = createSelectFormGroup({
            labelText: "Country",
            select: {
                id: "country",
                name: "country",
                options: [],
            },
        });

        const label = formGroup.querySelector("label");
        const select = formGroup.querySelector("select");

        expect(label).toHaveAttribute("for", "country");
        expect(select).toHaveAttribute("id", "country");
    });

    it("should include the hint and aria-describedby when hint text is supplied", () => {
        const formGroup = createSelectFormGroup({
            labelText: "Country",
            hintText: "Select your country",
            select: {
                id: "country",
                name: "country",
                options: [],
            },
        });

        const hint = formGroup.querySelector(".govuk-hint");
        const select = formGroup.querySelector("select");

        expect(hint).toHaveAttribute("id", "country-hint");
        expect(hint).toHaveTextContent("Select your country");
        expect(select).toHaveAttribute("aria-describedby", "country-hint");
    });

    it("should render the label, hint, and select in order", () => {
        const formGroup = createSelectFormGroup({
            labelText: "Country",
            hintText: "Select your country",
            select: {
                id: "country",
                name: "country",
                options: [],
            },
        });

        expect(formGroup.children[0]).toHaveClass("govuk-label");
        expect(formGroup.children[1]).toHaveClass("govuk-hint");
        expect(formGroup.children[2]).toHaveClass("govuk-select");
    });
});