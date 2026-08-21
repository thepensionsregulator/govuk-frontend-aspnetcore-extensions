import { createButton, createButtonGroup } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/button.js";
import { createTextInputFormGroup } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/inputs.js";

document.addEventListener("DOMContentLoaded", function () {
    const buttonTarget = document.querySelector(".button-target");
    const buttonGroup = createButtonGroup();
    const primaryButton = createButton({
         labelText: "Primary button",
         variant: "primary",
         type: "button"
    });
    const secondaryButton = createButton({
        labelText: "Secondary button",
        variant: "secondary",
        type: "button"
    });

    const warningButton = createButton({
        labelText: "Warning button",
        variant: "warning",
        type: "button"
    });

    const secondaryWarning = createButton({
        labelText: "Secondary warning",
        variant: "secondary-warning",
        type: "button"
    });

    buttonGroup.appendChild(primaryButton);
    buttonGroup.appendChild(secondaryButton);
    buttonGroup.appendChild(warningButton);
    buttonGroup.appendChild(secondaryWarning);

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
});