import { createButton, createButtonGroup } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/button.js";


document.addEventListener("DOMContentLoaded", function () {
    console.log("JS loaded");
    const target = document.getElementsByClassName("javascript-target")[0];
    const buttonGroup = createButtonGroup();
    console.log("target", target);  
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

    target.appendChild(buttonGroup);
});