import { createButton, createButtonGroup } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/button.js";
import { createTextInputFormGroup, createSelectFormGroup } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/inputs.js";
import { createFieldsetFormGroup } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/fieldset.js";
import { clearFormGroupError, showFormGroupError } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/validation.js";

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

    const validationButtonGroup = createButtonGroup();
    const showErrorButton = createButton({
        labelText: "Show input error",
        type: "button",
    });
    const updateErrorButton = createButton({
        labelText: "Update input error",
        variant: "secondary"
    });

    const clearErrorButton = createButton({
        labelText: "Clear input error",
        type: "button",
        variant: "warning",
    });

    showErrorButton.addEventListener("click", () => {
        showFormGroupError(postcodeInput, "Enter a valid postcode");
    });
    updateErrorButton.addEventListener("click", () => {
        showFormGroupError(postcodeInput, "Enter a valid postcode (updated)");
    });
    clearErrorButton.addEventListener("click", () => {
        clearFormGroupError(postcodeInput);
    });

    validationButtonGroup.appendChild(showErrorButton);
    validationButtonGroup.appendChild(updateErrorButton);
    validationButtonGroup.appendChild(clearErrorButton);
    inputTarget.appendChild(validationButtonGroup);

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

    const fieldsetTarget = document.querySelector(".fieldset-target");
    const fieldset = createFieldsetFormGroup({
        id: "contact-details",
        legendText: "Contact details",
        legendSize: "small",
        children: [
            createTextInputFormGroup({
                labelText: "First name",
                input: {
                    id: "first-name",
                    name: "first-name",
                    width: "x-large",
                },
            }),
            createTextInputFormGroup({
                labelText: "Last name",
                input: {
                    id: "last-name",
                    name: "last-name",
                    width: "x-large",
                },
            }),
        ],
    });

    const showFieldsetErrorButton = createButton({
        labelText: "Show fieldset error"
    });
    const updateFieldsetErrorButton = createButton({
        labelText: "Update fieldset error",
        variant: "secondary"
    });
    const clearFieldsetErrorButton = createButton({
        labelText: "Clear fieldset error",
        variant: "warning"
    });
    const fieldsetButtonGroup = createButtonGroup();

    showFieldsetErrorButton.addEventListener("click", () => {
        showFormGroupError(fieldset, "Enter valid contact details");
    });
    updateFieldsetErrorButton.addEventListener("click", () => {
        showFormGroupError(fieldset, "Enter valid contact details (updated)");
    });
    clearFieldsetErrorButton.addEventListener("click", () => {
        clearFormGroupError(fieldset);
    });

    fieldsetTarget.appendChild(fieldset);
    fieldsetButtonGroup.appendChild(showFieldsetErrorButton);
    fieldsetButtonGroup.appendChild(updateFieldsetErrorButton);
    fieldsetButtonGroup.appendChild(clearFieldsetErrorButton);
    fieldsetTarget.appendChild(fieldsetButtonGroup);
});