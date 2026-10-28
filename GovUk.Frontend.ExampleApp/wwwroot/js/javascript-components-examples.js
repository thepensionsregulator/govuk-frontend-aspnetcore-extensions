import { createButton, createButtonGroup } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/button.js";


document.addEventListener("DOMContentLoaded", function () {
    const target = document.getElementsByClassName("javascript-target")[0];
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

    target.appendChild(buttonGroup);
});