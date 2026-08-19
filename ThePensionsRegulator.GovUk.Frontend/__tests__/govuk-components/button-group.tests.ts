import { createButtonGroup } from "../../Scripts/govuk-components/button";

it("should create a div with the button group class", () => {
    const buttonGroup = createButtonGroup();

    expect(buttonGroup.outerHTML).toBe(`<div class="govuk-button-group"></div>`);
});