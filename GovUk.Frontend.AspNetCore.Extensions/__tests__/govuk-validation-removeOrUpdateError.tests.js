const govuk = require("../wwwroot/govuk/govuk-validation");

let _mockUpdateTitle, _mockUpdateErrorSummary, _mockShowError;

function mockCalledFunctions(
  testSubject,
  errorMessageForElement,
  formGroupForElement
) {
  _mockUpdateTitle = jest
    .spyOn(testSubject, "updateTitle")
    .mockImplementation(() => null);
  _mockUpdateErrorSummary = jest
    .spyOn(testSubject, "updateErrorSummary")
    .mockImplementation(() => null);
  jest
    .spyOn(testSubject, "errorMessageForElement")
    .mockImplementation(errorMessageForElement);
  jest
    .spyOn(testSubject, "formGroupForElement")
    .mockImplementation(formGroupForElement);
  _mockShowError = jest
    .spyOn(testSubject, "showError")
    .mockImplementation(() => null);
}

describe("removeOrUpdateError", () => {
  it("should remove error classes from the element", () => {
    document.body.innerHTML =
      '<input class="govuk-input govuk-input--error govuk-textarea--error govuk-select--error" />';

    const testSubject = govuk();
    mockCalledFunctions(testSubject);

    testSubject.removeOrUpdateError(document.firstElementChild);

    expect(
      document.firstElementChild.classList.contains("govuk-input--error")
    ).toBe(false);
    expect(
      document.firstElementChild.classList.contains("govuk-textarea--error")
    ).toBe(false);
    expect(
      document.firstElementChild.classList.contains("govuk-select--error")
    ).toBe(false);
  });

  it("should remove aria-describedby from the element", () => {
    document.body.innerHTML = `<p class="govuk-error-message field-validation-error" data-valmsg-for="Field1" data-valmsg-replace="false" id="Field1-error"><span class="govuk-visually-hidden">Error: </span>Some error message</p>
       <input id="Field1" aria-describedby="Field1-error some-other-id" />`;

    const testSubject = govuk();
    mockCalledFunctions(testSubject, () =>
      document.querySelector("#Field1-error")
    );

    const input = document.querySelector("input");
    testSubject.removeOrUpdateError(input);

    expect(input.getAttribute("aria-describedby")).toBe("some-other-id");
  });

  it("removes an existing error message", () => {
    document.body.innerHTML = `<p class="govuk-error-message"></p>
         <input class="govuk-input govuk-input--error govuk-textarea--error govuk-select--error" />`;

    const testSubject = govuk();
    mockCalledFunctions(testSubject, () =>
      document.querySelector(".govuk-error-message")
    );

    testSubject.removeOrUpdateError(document.querySelector("input"));

    expect(document.querySelector(".govuk-error-message")).toBeNull();
  });

  it("updates the page title and error summary", () => {
    document.body.innerHTML = '<input class="govuk-input" />';

    const testSubject = govuk();
    mockCalledFunctions(testSubject);

    testSubject.removeOrUpdateError(document.firstElementChild);

    expect(_mockUpdateTitle.mock.calls.length).toBe(1);
    expect(_mockUpdateErrorSummary.mock.calls.length).toBe(1);
  });

  it("removes .govuk-form-group--error when there are no more errors", () => {
    document.body.innerHTML = `
        <div class="govuk-form-group govuk-form-group--error">
            <input class="govuk-input" />
        </div>`;

    const testSubject = govuk();
    mockCalledFunctions(testSubject, null, () =>
      document.querySelector(".govuk-form-group")
    );

    testSubject.removeOrUpdateError(document.querySelector("input"));

    expect(
      document
        .querySelector(".govuk-form-group")
        .classList.contains("govuk-form-group--error")
    ).toBe(false);
  });

  it("calls showError if there is another invalid input in the form group", () => {
    document.body.innerHTML = `
        <div class="govuk-form-group govuk-form-group--error">
            <input class="govuk-input" id="now-valid" />
            <input class="govuk-input govuk-input--error" id="still-invalid" />
        </div>`;

    const testSubject = govuk();
    mockCalledFunctions(testSubject, null, () =>
      document.querySelector(".govuk-form-group")
    );

    testSubject.removeOrUpdateError(document.querySelector("input"));

    expect(_mockShowError.mock.calls[0][0]).toBe(
      document.getElementById("still-invalid")
    );
  });

  it("calls showError if there is another invalid textarea in the form group", () => {
    document.body.innerHTML = `
        <div class="govuk-form-group govuk-form-group--error">
            <input class="govuk-input" id="now-valid" />
            <textarea class="govuk-textarea govuk-textarea--error" id="still-invalid"></textarea>
        </div>`;

    const testSubject = govuk();
    mockCalledFunctions(testSubject, null, () =>
      document.querySelector(".govuk-form-group")
    );

    testSubject.removeOrUpdateError(document.querySelector("input"));

    expect(_mockShowError.mock.calls[0][0]).toBe(
      document.getElementById("still-invalid")
    );
  });

  it("calls showError if there is another invalid select in the form group", () => {
    document.body.innerHTML = `
        <div class="govuk-form-group govuk-form-group--error">
            <input class="govuk-input" id="now-valid" />
            <select class="govuk-textarea govuk-select--error" id="still-invalid"></select>
        </div>`;

    const testSubject = govuk();
    mockCalledFunctions(testSubject, null, () =>
      document.querySelector(".govuk-form-group")
    );

    testSubject.removeOrUpdateError(document.querySelector("input"));

    expect(_mockShowError.mock.calls[0][0]).toBe(
      document.getElementById("still-invalid")
    );
  });
});
