import { LinkOptions } from "./types";

export function createLink(options: LinkOptions): HTMLAnchorElement {
  const link = document.createElement("a");
  link.className = options.variant === "back" ? "govuk-back-link" : "govuk-link";
  link.href = options.href ?? "#";
  link.textContent = options.labelText;

  if (options.attributes) {
    for (const [key, value] of Object.entries(options.attributes)) {
      link.setAttribute(key, value);
    }
  }

  return link;
}
