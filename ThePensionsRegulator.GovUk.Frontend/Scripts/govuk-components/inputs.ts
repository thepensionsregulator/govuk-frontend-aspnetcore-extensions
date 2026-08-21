import {
    FormGroupOptions,
    HintOptions,
    InputWidth,
    LabelOptions,
    TextInputFormGroupOptions,
    TextInputOptions,
} from "./types";

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

export function createTextInputFormGroup(options: TextInputFormGroupOptions): HTMLElement {
    const hintId = `${options.input.id}-hint`;
    const inputAttributes = { ...options.input.attributes };

    if (options.hintText) {
        inputAttributes["aria-describedby"] = [inputAttributes["aria-describedby"], hintId]
            .filter(Boolean)
            .join(" ");
    }

    return createFormGroup({
        label: createLabel({ labelText: options.labelText, htmlFor: options.input.id }),
        hint: options.hintText ? createHint({ hintText: options.hintText, id: hintId }) : undefined,
        control: createTextInput({ ...options.input, attributes: inputAttributes }),
    });
}


const inputWidthClasses: Record<InputWidth, string> = {
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