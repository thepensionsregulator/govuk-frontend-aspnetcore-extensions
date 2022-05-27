const govuk = require("../Content/govuk/govuk-validation");

describe("updateTitle", () => {
  it("should add Error: when there is an error", () => {
    document.title = "Example";
    document.body.innerHTML = '<p class="govuk-error-message"></p>';

    govuk().updateTitle();

    expect(document.title).toBe("Error: Example");
  });

  it("should not add Error: multiple times", () => {
    document.title = "Error: Example";
    document.body.innerHTML = '<p class="govuk-error-message"></p>';

    govuk().updateTitle();

    expect(document.title).toBe("Error: Example");
  });

  it("should remove Error: when there is no error", () => {
    document.title = "Error: Example";
    document.body.innerHTML = "";

    govuk().updateTitle();

    expect(document.title).toBe("Example");
  });
});
