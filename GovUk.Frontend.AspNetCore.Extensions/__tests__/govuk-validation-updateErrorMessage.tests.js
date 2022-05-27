const govuk = require("../Content/govuk/govuk-validation");

describe("updateErrorMessage", () => {
  it("should set the error message id based on the id of the invalid element", () => {
    document.body.innerHTML = `
      <p class="govuk-error-message" id="old-field-error">
        <span class="govuk-visually-hidden">Error: </span>Old message
      </p>
      <input id="new-field" />`;

    const error = document.querySelector(".govuk-error-message");
    const testSubject = govuk();
    jest.spyOn(testSubject, "errorMessageForElement").mockReturnValue(error);

    testSubject.updateErrorMessage(
      document.getElementById("new-field"),
      "New message"
    );

    expect(error.id).toEqual("new-field-error");
  });

  it("should replace the existing message", () => {
    document.body.innerHTML = `
      <p class="govuk-error-message">
        <span class="govuk-visually-hidden">Error: </span>Old message
      </p>
      <input id="new-field" />`;

    const error = document.querySelector(".govuk-error-message");
    const testSubject = govuk();
    jest.spyOn(testSubject, "errorMessageForElement").mockReturnValue(error);

    testSubject.updateErrorMessage(
      document.getElementById("new-field"),
      "New message"
    );

    expect(error.innerHTML.trim()).toEqual(
      '<span class="govuk-visually-hidden">Error: </span>New message'
    );
  });
});
