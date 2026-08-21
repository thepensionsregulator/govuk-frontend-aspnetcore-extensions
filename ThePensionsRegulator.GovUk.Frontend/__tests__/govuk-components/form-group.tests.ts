import { createFormGroup } from "../../Scripts/govuk-components/form-group";
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