import { createButton, createButtonGroup } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/button.js";
import { createFormGroup } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/form-group.js";

document.addEventListener("DOMContentLoaded", function () {
    const buttonTarget = document.getElementsByClassName("button-target")[0];
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

    var inputTarget = document.getElementsByClassName("input-target")[0];
    var formControl = document.createElement("input");
    var formLabel = document.createElement("label");
    formLabel.innerText = "Text input";
    const formGroup = createFormGroup({ control: formControl, label: formLabel });

    inputTarget.appendChild(formGroup);
});