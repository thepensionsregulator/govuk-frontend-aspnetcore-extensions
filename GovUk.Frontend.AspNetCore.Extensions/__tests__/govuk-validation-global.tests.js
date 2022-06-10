const govuk = require("../Content/govuk/govuk-validation");

const _mockValidator = {
  methods: {
    required: () => false,
  },
};

describe("govuk-validation with only jQuery Validator mocked", () => {
  it("should add an error to both the error message and summary", () => {
    document.body.innerHTML = `
        <main>
            <div class="govuk-error-summary">
                <ul class="govuk-list govuk-error-summary__list"></ul>
            </div>
            <div class="govuk-form-group">
                <input class="govuk-input" data-val="true" data-val-required="This field is required" />
            </div>
        </main>  
      `;

    const testSubject = govuk();
    jest.spyOn(testSubject, "getValidator").mockReturnValue(_mockValidator);

    testSubject.showError(document.querySelector("input"));

    expect(
      document.querySelector(".govuk-error-summary__list").hasChildNodes()
    ).toBe(true);
    expect(document.querySelector(".govuk-error-message")).not.toBeNull();
  });

  it("should remove both error message and summary when everything is valid", () => {
    document.body.innerHTML = `
        <main>
            <div class="govuk-error-summary">
                <ul class="govuk-list govuk-error-summary__list">
                    <li><a href="#field">Error message</a></li>
                </ul>
            </div>
            <div class="govuk-form-group">
                <p class="govuk-error-message" id="field-error">Error message</p>
                <input id="field" class="govuk-input" data-val="true" data-val-required="This field is required" value="valid" />
            </div>
        </main>  
    `;

    const testSubject = govuk();
    jest.spyOn(testSubject, "getValidator").mockReturnValue(_mockValidator);

    testSubject.removeOrUpdateError(document.querySelector("input"));

    expect(
      document
        .querySelector(".govuk-error-summary")
        .classList.contains("govuk-!-display-none")
    ).toBe(true);
    expect(document.querySelector(".govuk-error-message")).toBeNull();
  });
});
