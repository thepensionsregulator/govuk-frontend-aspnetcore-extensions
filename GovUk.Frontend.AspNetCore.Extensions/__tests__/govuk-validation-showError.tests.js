const govuk = require("../Content/govuk/govuk-validation");

let _mockValidateElement;

function mockCalledFunctions(testSubject) {
  jest
    .spyOn(testSubject, "formGroupForElement")
    .mockReturnValue(document.firstElementChild);
  _mockValidateElement = jest
    .spyOn(testSubject, "validateElement")
    .mockImplementation(() => null);
}

describe("showError", () => {
  it("should add .govuk-form-group--error to the form group", () => {
    document.body.innerHTML = `
        <div class-"govuk-form-group">
            <input class="govuk-input" />
        <div>`;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);

    testSubject.showError(document.querySelector("input"));

    expect(
      document.firstElementChild.classList.contains("govuk-form-group--error")
    ).toBe(true);
  });

  it("should add .govuk-input--error if the element is an input", () => {
    document.body.innerHTML = `
        <div class-"govuk-form-group">
            <input class="govuk-input" />
        <div>`;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);

    const input = document.querySelector("input");
    testSubject.showError(input);

    expect(input.classList.contains("govuk-input--error")).toBe(true);
  });

  it("should add .govuk-textarea--error if the element is a textarea", () => {
    document.body.innerHTML = `
        <div class-"govuk-form-group">
            <textarea class="govuk-textarea"></textarea>
        <div>`;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);

    const textarea = document.querySelector("textarea");
    testSubject.showError(textarea);

    expect(textarea.classList.contains("govuk-textarea--error")).toBe(true);
  });

  it("should add .govuk-select--error if the element is a select", () => {
    document.body.innerHTML = `
        <div class-"govuk-form-group">
            <select class="govuk-select"></select>
        <div>`;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);

    const select = document.querySelector("select");
    testSubject.showError(select);

    expect(select.classList.contains("govuk-select--error")).toBe(true);
  });

  it("should call validateError on the first error in the form group, if that is an input", () => {
    document.body.innerHTML = `
        <div class-"govuk-form-group">
            <input class="govuk-input govuk-input--error" id="already-invalid" />
            <input class="govuk-input" id="becoming-invalid" />
        <div>`;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);

    const element = document.getElementById("becoming-invalid");
    testSubject.showError(element);

    expect(_mockValidateElement.mock.calls[0][0]).toBe(
      document.getElementById("already-invalid")
    );
  });

  it("should call validateError on the first error in the form group, if that is an textarea", () => {
    document.body.innerHTML = `
        <div class-"govuk-form-group">
            <textarea class="govuk-textarea govuk-textarea--error" id="already-invalid"></textarea>
            <input class="govuk-input" id="becoming-invalid" />
        <div>`;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);

    const element = document.getElementById("becoming-invalid");
    testSubject.showError(element);

    expect(_mockValidateElement.mock.calls[0][0]).toBe(
      document.getElementById("already-invalid")
    );
  });

  it("should call validateError on the first error in the form group, if that is an select", () => {
    document.body.innerHTML = `
        <div class-"govuk-form-group">
            <select class="govuk-select govuk-select--error" id="already-invalid"></select>
            <input class="govuk-input" id="becoming-invalid" />
        <div>`;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);

    const element = document.getElementById("becoming-invalid");
    testSubject.showError(element);

    expect(_mockValidateElement.mock.calls[0][0]).toBe(
      document.getElementById("already-invalid")
    );
  });
});
