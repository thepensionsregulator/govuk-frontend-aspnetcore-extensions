import {
    FormGroupOptions,
    HintOptions,
    FormControlWidth,
    LabelOptions,
    SelectFormGroupOptions,
    Option,
    SelectOptions,
    TextInputFormGroupOptions,
    TextInputOptions,
} from "./types";

interface LabeledControlFormGroupOptions {
    labelText: string;
    hintText?: string;
    control: HTMLInputElement | HTMLSelectElement;
}

export function createFormGroup(formGroupOptions: FormGroupOptions): HTMLElement {
    const div = document.createElement("div");
    div.className = "govuk-form-group";

    div.appendChild(formGroupOptions.label);
    if (formGroupOptions.hint) {
        div.appendChild(formGroupOptions.hint);
    }
    div.appendChild(formGroupOptions.control);

    return div;
}

function createLabeledControlFormGroup(options: LabeledControlFormGroupOptions): HTMLElement {
    const hintId = `${options.control.id}-hint`;

    if (options.hintText) {
        const describedBy = options.control.getAttribute("aria-describedby");

        options.control.setAttribute(
            "aria-describedby",
            [describedBy, hintId].filter(Boolean).join(" ")
        );
    }

    return createFormGroup({
        label: createLabel({
            labelText: options.labelText,
            htmlFor: options.control.id,
        }),
        hint: options.hintText
            ? createHint({ hintText: options.hintText, id: hintId })
            : undefined,
        control: options.control,
    });
}

export function createLabel(labelOptions: LabelOptions): HTMLLabelElement {
    const label = document.createElement("label");
    label.className = "govuk-label";
    label.setAttribute("for", labelOptions.htmlFor);
    label.textContent = labelOptions.labelText;
    setAttributes(label, labelOptions.attributes);

    return label;
}

export function createHint(hintOptions: HintOptions): HTMLDivElement {
    const hint = document.createElement("div");
    hint.className = "govuk-hint";
    hint.setAttribute("id", hintOptions.id);
    hint.textContent = hintOptions.hintText;
    setAttributes(hint, hintOptions.attributes);

    return hint;
}

export function createTextInput(options: TextInputOptions): HTMLInputElement {
    const input = document.createElement("input");
    input.className = "govuk-input";
    input.name = options.name;
    input.id = options.id;
    input.type = options.type || "text";
    input.value = options.value || "";
    setAttributes(input, options.attributes);

    if (options.width) {
        input.classList.add(inputWidthClasses[options.width]);
    }

    return input;
}


export function createTextInputFormGroup(
    options: TextInputFormGroupOptions
): HTMLElement {
    return createLabeledControlFormGroup({
        labelText: options.labelText,
        hintText: options.hintText,
        control: createTextInput(options.input),
    });
}

export function createSelect(options: SelectOptions): HTMLSelectElement {
    const select = document.createElement("select");
    select.className = "govuk-select";
    select.id = options.id;
    select.name = options.name;
    setAttributes(select, options.attributes);
    
    if (options.width) {
        select.classList.add(inputWidthClasses[options.width]);
    }
    
    for (const option of options.options) {
        select.appendChild(createSelectOption(option));
    }

    if (options.value !== undefined) {
        select.value = options.value;
    }

    return select;
}

function createSelectOption(option: Option): HTMLOptionElement {
    const optionElement = document.createElement("option");
    optionElement.value = option.value;
    optionElement.textContent = option.text;
    
    if (option.disabled) {
        optionElement.disabled = true;
    }
    if (option.attributes) {
        setAttributes(optionElement, option.attributes);
    }

    return optionElement;
}

export function createSelectFormGroup(options: SelectFormGroupOptions): HTMLElement {
    return createLabeledControlFormGroup({
        labelText: options.labelText,
        hintText: options.hintText,
        control: createSelect(options.select),
    });
}


const inputWidthClasses: Record<FormControlWidth, string> = {
    "xx-small": "govuk-input--width-2",
    "x-small": "govuk-input--width-3",
    "small": "govuk-input--width-4",
    "medium": "govuk-input--width-5",
    "large": "govuk-input--width-10",
    "x-large": "govuk-input--width-20",
    "xx-large": "govuk-input--width-30",
};

function setAttributes(element: HTMLElement, attributes?: Record<string, string>): void {
    if (!attributes) {
        return;
    }

    for (const [name, value] of Object.entries(attributes)) {
        element.setAttribute(name, value);
    }
}