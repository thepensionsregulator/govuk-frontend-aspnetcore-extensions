import { createButton } from "../../Scripts/govuk-components/button";

describe("createButton", () => {
  it("should create a button element", () => {
    const button = createButton({ labelText: "Continue", type: "submit" });

    expect(button.tagName).toEqual("BUTTON");
  });

  it("should apply the GOV.UK button class", () => {
    const button = createButton({ labelText: "Continue", type: "submit" });

    expect(button.getAttribute("class")).toEqual("govuk-button");
  });

  it("should not apply a variant class for the primary variant", () => {
    const button = createButton({
      labelText: "Continue",
      type: "submit",
      variant: "primary",
    });

    expect(button.getAttribute("class")).toEqual("govuk-button");
  });

  it.each(["secondary", "warning"] as const)(
    "should apply the modifier class for the %s variant",
    (variant) => {
      const button = createButton({
        labelText: "Continue",
        type: "submit",
        variant,
      });

      expect(button.getAttribute("class")).toEqual(
        `govuk-button govuk-button--${variant}`
      );
    }
  );

    it("should apply the secondary and warning modifier class for the secondary-warning variant", () => {
        const button = createButton({
            labelText: "Secondary warning",
            type: "button",
            variant: "secondary-warning"
        });

        expect(button.getAttribute("class")).toEqual(
            "govuk-button govuk-button--secondary govuk-button--warning"
        );
  });

  it.each(["submit", "button", "reset"] as const)(
    "should set the type attribute to %s",
    (type) => {
      const button = createButton({ labelText: "Continue", type });

      expect(button.getAttribute("type")).toEqual(type);
    }
  );

  it("should use the label text as the button text", () => {
    const button = createButton({ labelText: "Continue", type: "submit" });

    expect(button.textContent).toEqual("Continue");
    expect(button.childNodes).toHaveLength(1);
    expect(button.childNodes[0].nodeType).toEqual(Node.TEXT_NODE);
  });

  it("should escape HTML in the label text", () => {
    const button = createButton({
      labelText: "<span>Continue</span>",
      type: "submit",
    });

    expect(button.querySelector("span")).toBeNull();
    expect(button.innerHTML).toEqual("&lt;span&gt;Continue&lt;/span&gt;");
  });

  it("should apply additional attributes", () => {
    const button = createButton({
      labelText: "Continue",
      type: "submit",
      attributes: {
        id: "continue-button",
        "data-module": "govuk-button",
        "aria-label": "Continue to the next page",
      },
    });

    expect(button.getAttribute("id")).toEqual("continue-button");
    expect(button.getAttribute("data-module")).toEqual("govuk-button");
    expect(button.getAttribute("aria-label")).toEqual(
      "Continue to the next page"
    );
  });

  it("should produce the expected HTML", () => {
    const button = createButton({
      labelText: "Continue",
      type: "submit",
      variant: "secondary",
    });

    expect(button.outerHTML).toEqual(
      '<button class="govuk-button govuk-button--secondary" type="submit" data-module="govuk-button" data-govuk-button-init="">Continue</button>'
    );
  });
});
