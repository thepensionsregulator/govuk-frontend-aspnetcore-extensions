const govuk = require("../Content/govuk/govuk-validation");

describe("errorMessageForElement", () => {
  it("returns an existing .govuk-error-message if it exists", () => {
    document.body.innerHTML = `
      <div class="govuk-form-group">
        <p class="govuk-error-message" id="existing" data-valmsg-for="Field1"></p>
        <input class="govuk-input" name="Field1" id="Field1" />
      </div>`;
    const testSubject = govuk();
    jest
      .spyOn(testSubject, "formGroupForElement")
      .mockReturnValue(document.body.firstElementChild);

    const result = testSubject.errorMessageForElement(
      document.querySelector("input")
    );

    expect(result.id).toEqual("existing");
  });

  it("does not return an existing error for a field that is conditional upon the target field", () => {
    document.body.innerHTML = `
      <div class="govuk-form-group">
        <fieldset class="govuk-fieldset">
          <legend class="govuk-fieldset__legend"></legend>
          <div class="govuk-checkboxes">
            <div class="govuk-checkboxes__item">
              <input class="govuk-checkboxes__input" id="Field1" name="Field1" type="checkbox" />
            </div>  
            <div class="govuk-checkboxes__conditional">
                <div class="govuk-form-group" id="inner">
                    <label class="govuk-label" id="text-input-label"></label>
                    <p class="govuk-error-message" id="Field2-error" data-valmsg-for="Field2"></p>
                    <input class="govuk-input" id="text-input" />
                </div>
            </div>
          </div>
        </fieldset>
      </div>`;

    const testSubject = govuk();
    jest
      .spyOn(testSubject, "formGroupForElement")
      .mockReturnValue(document.body.firstElementChild);

    const result = testSubject.errorMessageForElement(
      document.getElementById("Field1")
    );

    expect(result.getAttribute("data-valmsg-for")).toBe("Field1");
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

    const error = testSubject.errorMessageForElement(
      document.querySelector("input")
    );

    expect(error.nextElementSibling.id).toEqual("example");
    expect(error.getAttribute("data-valmsg-for")).toEqual("example");
    expect(error.getAttribute("data-valmsg-replace")).toEqual("false");
    expect(
      error.firstElementChild.classList.contains("govuk-visually-hidden")
    ).toBe(true);
    expect(error.firstElementChild.innerHTML).toEqual("Error: ");
  });

  it("puts a error with radios at the top of the radio group", () => {
    document.body.innerHTML = `
        <div class="govuk-form-group">
            <fieldset class="govuk-fieldset">
                <legend class="govuk-fieldset__legend"></legend>
                <div class="govuk-radios">
                    <div class="govuk-radios__item">
                        <input class="govuk-radios__input" type="radio" />
                        <label class="govuk-label govuk-radios__label"></label>
                    </div>
                </div>
            </fieldset>
        </div>`;

    const testSubject = govuk();
    jest
      .spyOn(testSubject, "formGroupForElement")
      .mockReturnValue(document.body.firstElementChild);

    const error = testSubject.errorMessageForElement(
      document.querySelector("input")
    );

    expect(error.previousElementSibling.tagName).toEqual("LEGEND");
    expect(error.nextElementSibling.classList.contains("govuk-radios")).toBe(
      true
    );
  });

  it("puts a error with a conditional field below a radio button above the field", () => {
    document.body.innerHTML = `
        <div class="govuk-form-group">
            <fieldset class="govuk-fieldset">
                <legend class="govuk-fieldset__legend"></legend>
                <div class="govuk-radios">
                    <div class="govuk-radios__item">
                        <input class="govuk-radios__input" type="radio" />
                        <label class="govuk-label govuk-radios__label" />
                    </div>
                    <div class="govuk-radios__conditional">
                        <div class="govuk-form-group" id="inner">
                            <label class="govuk-label" id="text-input-label"></label>
                            <input class="govuk-input" id="text-input" />
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>`;

    const testSubject = govuk();
    jest
      .spyOn(testSubject, "formGroupForElement")
      .mockReturnValue(document.getElementById("inner"));

    const error = testSubject.errorMessageForElement(
      document.querySelector("#text-input")
    );

    expect(error.previousElementSibling.id).toEqual("text-input-label");
    expect(error.nextElementSibling.id).toEqual("text-input");
  });

  it("puts a error with checkboxes at the top of the checkbox group", () => {
    document.body.innerHTML = `
        <div class="govuk-form-group">
            <fieldset class="govuk-fieldset">
                <legend class="govuk-fieldset__legend"></legend>
                <div class="govuk-checkboxes">
                    <div class="govuk-checkboxes__item">
                        <input class="govuk-checkboxes__input" type="checkbox" />
                        <label class="govuk-label govuk-checkboxes__label"></label>
                    </div>
                </div>
            </fieldset>
        </div>`;

    const testSubject = govuk();
    jest
      .spyOn(testSubject, "formGroupForElement")
      .mockReturnValue(document.body.firstElementChild);

    const error = testSubject.errorMessageForElement(
      document.querySelector("input")
    );

    expect(error.previousElementSibling.tagName).toEqual("LEGEND");
    expect(
      error.nextElementSibling.classList.contains("govuk-checkboxes")
    ).toBe(true);
  });

  it("puts a error with a conditional field below a checkbox above the field", () => {
    document.body.innerHTML = `
        <div class="govuk-form-group">
            <fieldset class="govuk-fieldset">
                <legend class="govuk-fieldset__legend"></legend>
                <div class="govuk-checkboxes">
                    <div class="govuk-checkboxes__item">
                        <input class="govuk-checkboxes__input" type="checkbox" />
                        <label class="govuk-label govuk-checkboxes__label"></label>
                    </div>
                    <div class="govuk-checkboxes__conditional">
                        <div class="govuk-form-group" id="inner">
                            <label class="govuk-label" id="text-input-label"></label>
                            <input class="govuk-input" id="text-input" />
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>`;

    const testSubject = govuk();
    jest
      .spyOn(testSubject, "formGroupForElement")
      .mockReturnValue(document.getElementById("inner"));

    const error = testSubject.errorMessageForElement(
      document.querySelector("#text-input")
    );

    expect(error.previousElementSibling.id).toEqual("text-input-label");
    expect(error.nextElementSibling.id).toEqual("text-input");
  });

  it("puts a error with a date field at the top of the set of date fields", () => {
    document.body.innerHTML = `
        <div class="govuk-form-group">
            <fieldset class="govuk-fieldset">
                <legend class="govuk-fieldset__legend"></legend>
                <div class="govuk-hint"></div>
                <div class="govuk-date-input">
                    <div class="govuk-form-group" id="inner">
                        <label class="govuk-label govuk-date-input__label"></label>
                        <input class="govuk-input govuk-date-input__input" />
                    </div>
                </div>
            </fieldset>
        </div>`;

    const testSubject = govuk();
    jest
      .spyOn(testSubject, "formGroupForElement")
      .mockReturnValue(document.getElementById("inner"));

    const error = testSubject.errorMessageForElement(
      document.querySelector("input")
    );

    expect(error.previousElementSibling.classList.contains("govuk-hint")).toBe(
      true
    );
    expect(
      error.nextElementSibling.classList.contains("govuk-date-input")
    ).toBe(true);
  });
});
