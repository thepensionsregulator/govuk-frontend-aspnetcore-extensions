const govuk = require("../wwwroot/govuk/govuk-validation");

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

  it("returns an existing .govuk-error-message for another field in the same date if it exists", () => {
    document.body.innerHTML = `
        <div class="govuk-form-group">
            <fieldset class="govuk-fieldset">
                <legend class="govuk-fieldset__legend"></legend>
                <div class="govuk-hint"></div>
                <p class="govuk-error-message" id="existing" data-valmsg-for="Field1.Year"></p>
                <div class="govuk-date-input">
                    <div class="govuk-form-group" id="inner">
                        <label class="govuk-label govuk-date-input__label"></label>
                        <input class="govuk-input govuk-date-input__input" id="Field1.Month" />
                    </div>
                </div>
            </fieldset>
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

    it("puts a error with text input with prefix before govuk-input__wrapper and after govuk-hint", () => {
        document.body.innerHTML = `
          <div class="govuk-form-group">
            <label class="govuk-label"></label>
            <div class="govuk-hint"></div>
            <div class="govuk-input__wrapper" >
              <div class="govuk-input__prefix">�</div>
              <input class="govuk-input type="text" id="inner" />
            </div>
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
            error.nextElementSibling.classList.contains("govuk-input__wrapper")
        ).toBe(true);
    });
});
