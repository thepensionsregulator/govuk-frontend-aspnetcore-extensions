import { ButtonOptions } from "./types";

export function createButton(options: ButtonOptions): HTMLButtonElement {

  const button = document.createElement("button");
  let variantClass = options.variant && options.variant !== "primary" && options.variant !== "secondary-warning" ? ` govuk-button--${options.variant}` : "";
  
  if (variantClass === "" && options.variant === "secondary-warning") {
    variantClass = " govuk-button--secondary govuk-button--warning";
  }


  button.className = `govuk-button${variantClass}`;
  button.type = options.type;
  button.textContent = options.labelText;
  button.setAttribute("data-module", "govuk-button");
  button.toggleAttribute("data-govuk-button-init", true);

  if (options.attributes) {
    for (const [key, value] of Object.entries(options.attributes)) {
      button.setAttribute(key, value);
    }
  } 

  return button;
}

export function createButtonGroup(){
    const div = document.createElement("div");
    div.className = "govuk-button-group";
    return div;
}