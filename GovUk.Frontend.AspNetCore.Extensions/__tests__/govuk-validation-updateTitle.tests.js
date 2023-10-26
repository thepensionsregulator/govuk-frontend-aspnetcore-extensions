const govuk = require("../wwwroot/govuk/govuk-validation");
const errorPlaceholderHtml =
  '<p class="govuk-error-message" id="some-field-error" data-valmsg-for="some-field" data-valmsg-replace="false"><span class="govuk-visually-hidden">Error:</span></p>';
const errorHtml =
    '<p class="govuk-error-message" id="some-field-error" data-valmsg-for="some-field" data-valmsg-replace="false"><span class="govuk-visually-hidden">Error:</span> A real error</p>';
const attribute =
    '<p class="govuk-error-message" id="some-field-error" data-valmsg-for="some-field" data-valmsg-replace="false" data-govuk-error-prefix="Error: "> A real error</p>';
const attributePlaceholderHtml =
    '<p class="govuk-error-message" id="some-field-error" data-valmsg-for="some-field" data-valmsg-replace="false" data-govuk-error-prefix="Error: "><span class="govuk-visually-hidden">Error:</span></p>';

describe("updateTitle", () => {
  it("should not add Error: when there is an empty error placeholder", () => {
    document.title = "Example";
    document.body.innerHTML = errorPlaceholderHtml;

    govuk().updateTitle();

    expect(document.title).toBe("Example");
  });

  it("should add Error: when there is an error", () => {
    document.title = "Example";
    document.body.innerHTML = errorHtml;

    govuk().updateTitle();

    expect(document.title).toBe("Error: Example");
  });

  it("should not add Error: multiple times", () => {
    document.title = "Error: Example";
    document.body.innerHTML = errorHtml;

    govuk().updateTitle();

    expect(document.title).toBe("Error: Example");
  });

  it("should remove Error: when there is no error", () => {
    document.title = "Error: Example";
    document.body.innerHTML = "";

    govuk().updateTitle();

    expect(document.title).toBe("Example");
  });

    it("should add Prefix: using data-govuk-error-prefix when there is an error", () => {
        document.title = "Example";
        document.body.innerHTML = attribute;

        govuk().updateTitle();

        expect(document.title).toBe("Error: Example");
    });

    it("should not add Prefix: using data-govuk-error-prefix when there is an empty error placeholder", () => {
        document.title = "Example";
        document.body.innerHTML = attributePlaceholderHtml;

        govuk().updateTitle();

        expect(document.title).toBe("Example");
    });
});
