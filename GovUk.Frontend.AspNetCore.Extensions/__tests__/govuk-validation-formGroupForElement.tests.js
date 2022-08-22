const govuk = require("../wwwroot/govuk/govuk-validation");

describe("formGroupForElement", () => {
  it("should choose the closest group for .govuk-input", () => {
    document.body.innerHTML = `
        <div class="govuk-form-group" id="outer">
            <div class="govuk-form-group" id="inner">
                <input class="govuk-input" />
            </div>
        </div>`;

    const result = govuk().formGroupForElement(document.querySelector("input"));

    expect(result.id).toBe("inner");
  });

  it("should choose the closest group for .govuk-select", () => {
    document.body.innerHTML = `
        <div class="govuk-form-group" id="outer">
            <div class="govuk-form-group" id="inner">
                <select class="govuk-select"></select>
            </div>
        </div>`;

    const result = govuk().formGroupForElement(
      document.querySelector("select")
    );

    expect(result.id).toBe("inner");
  });

  it("should choose the closest group for .govuk-textarea", () => {
    document.body.innerHTML = `
        <div class="govuk-form-group" id="outer">
            <div class="govuk-form-group" id="inner">
                <textarea class="govuk-textarea"></textarea>
            </div>
        </div>`;

    const result = govuk().formGroupForElement(
      document.querySelector("textarea")
    );

    expect(result.id).toBe("inner");
  });

  it("should choose the outer group for a date field", () => {
    document.body.innerHTML = `
        <div class="govuk-form-group" id="outer">
            <div class="govuk-form-group" id="inner">
                <input class="govuk-date-input__input" />
            </div>
        </div>`;

    const result = govuk().formGroupForElement(document.querySelector("input"));

    expect(result.id).toBe("outer");
  });
});
