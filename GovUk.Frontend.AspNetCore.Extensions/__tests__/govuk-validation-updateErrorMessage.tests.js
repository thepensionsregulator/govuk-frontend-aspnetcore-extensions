const govuk = require("../wwwroot/govuk/govuk-validation");

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

  it("inserts a .govuk-error-message with prefix before the form element", () => {
    document.body.innerHTML = `
      <div class="govuk-form-group">
          <p class="govuk-hint"></p>
          <input class="govuk-input" id="example" />
      </div>`;

    const testSubject = govuk();
      jest
        .spyOn(testSubject, "formGroupForElement")
        .mockReturnValue(document.body.firstElementChild);

      testSubject.updateErrorMessage(
          document.querySelector("input"),
          "Error message"
    );

    const error = document.querySelector(".govuk-error-message");
    expect(error.nextElementSibling.id).toEqual("example");
    expect(error.getAttribute("data-valmsg-for")).toEqual("example");
    expect(error.getAttribute("data-valmsg-replace")).toEqual("false");
    expect(
      error.firstElementChild.classList.contains("govuk-visually-hidden")
    ).toBe(true);
    expect(error.firstElementChild.innerHTML).toEqual("Error: ");
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
