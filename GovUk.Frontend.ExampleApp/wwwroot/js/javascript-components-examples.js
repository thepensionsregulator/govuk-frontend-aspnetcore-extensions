import { createButton, createButtonGroup } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/button.js";
import { createTextInputFormGroup, createSelectFormGroup } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/inputs.js";

document.addEventListener("DOMContentLoaded", function () {
    const buttonTarget = document.querySelector(".button-target");
    const buttonGroup = createButtonGroup();
    const primaryButton = createButton({
        labelText: "Primary button",
        variant: "primary",
        type: "button"
    });

    const primaryDisabled = createButton({
        labelText: "Primary disabled",
        variant: "primary",
        type: "button",
        attributes: {
            "disabled": "",
            "aria-disabled": "true"
        }
    });
    const secondaryButton = createButton({
        labelText: "Secondary button",
        variant: "secondary",
        type: "button"
    });

    const secondaryDisabled = createButton({
        labelText: "Secondary disabled",
        variant: "secondary",
        type: "button",
        attributes: {
            "disabled": "",
            "aria-disabled": "true"
        }
    });

    const warningButton = createButton({
        labelText: "Warning button",
        variant: "warning",
        type: "button"
    });

    const warningDisabled = createButton({
        labelText: "Warning disabled",
        variant: "warning",
        type: "button",
        attributes: {
            "disabled": "",
            "aria-disabled": "true"
        }
    });

    const secondaryWarning = createButton({
        labelText: "Secondary warning",
        variant: "secondary-warning",
        type: "button"
    });

    const secondaryWarningDisabled = createButton({
        labelText: "Secondary warning disabled",
        variant: "secondary-warning",
        type: "button",
        attributes: {
            "disabled": "",
            "aria-disabled": "true"
        }
    });

    buttonGroup.appendChild(primaryButton);
    buttonGroup.appendChild(primaryDisabled);
    buttonGroup.appendChild(secondaryButton);
    buttonGroup.appendChild(secondaryDisabled);
    buttonGroup.appendChild(warningButton);
    buttonGroup.appendChild(warningDisabled);
    buttonGroup.appendChild(secondaryWarning);
    buttonGroup.appendChild(secondaryWarningDisabled);

    buttonTarget.appendChild(buttonGroup);

    const inputTarget = document.querySelector(".input-target");

    const postcodeInput = createTextInputFormGroup({
        labelText: "Postcode",
        hintText: "For example, SW1A 2AA",
        input: {
            id: "postcode",
            name: "postcode",
            width: "large",
            attributes: {
                autocomplete: "postal-code",
            },
        },
    });

    inputTarget.appendChild(postcodeInput);

    const selectTarget = document.querySelector(".select-target");
    const select = createSelectFormGroup({
        labelText: "Select an option",
        hintText: "For example, choose one",
        select: {
            id: "example-select",
            name: "example-select",
            width: "large",
            options: [
                { value: "", text: "Please select" },
                { value: "option1", text: "Option 1" },
                { value: "option2", text: "Option 2" },
            ],
            attributes: {},
        },
    });

    selectTarget.appendChild(select);
});