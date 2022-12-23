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
      <div class="govuk-form-group" id="outside-fieldset">
        <fieldset>
          <div class="govuk-form-group" id="outer">
              <div class="govuk-form-group" id="inner">
                  <input class="govuk-date-input__input" />
              </div>
          </div>
        </fieldset>
      </div>`;

    const result = govuk().formGroupForElement(document.querySelector("input"));

    expect(result.id).toBe("outer");
  });

  it("should choose the group outside the fieldset for a date field in a .govuk-date-input__fieldset", () => {
    document.body.innerHTML = `
      <div class="govuk-form-group" id="outside-fieldset">
        <fieldset class="govuk-date-input__fieldset">
          <div class="govuk-form-group" id="outer">
              <div class="govuk-form-group" id="inner">
                  <input class="govuk-date-input__input" />
              </div>
          </div>
        </fieldset>
      </div>`;

    const result = govuk().formGroupForElement(document.querySelector("input"));

    expect(result.id).toBe("outside-fieldset");
  });

  it("should choose the closest group for .govuk-radios", () => {
    document.body.innerHTML = `
      <div class="govuk-form-group" id="outside-fieldset">
        <fieldset>
            <div class="govuk-form-group" id="inner">
              <div class="govuk-radios">
                <input class="govuk-radios__input" />
              </div>
            </div>
          </fieldset>
        </div>`;

    const result = govuk().formGroupForElement(document.querySelector("input"));

    expect(result.id).toBe("inner");
  });

  it("should choose the group outside the fieldset for radio in a .govuk-radios__fieldset", () => {
    document.body.innerHTML = `
      <div class="govuk-form-group" id="outside-fieldset">
      <fieldset class="govuk-radios__fieldset">
          <div class="govuk-form-group" id="inner">
            <div class="govuk-radios">
              <input class="govuk-radios__input" />
            </div>
          </div>
        </fieldset>
      </div>`;

    const result = govuk().formGroupForElement(document.querySelector("input"));

    expect(result.id).toBe("outside-fieldset");
  });

  it("should choose the closest group for .govuk-checkboxes", () => {
    document.body.innerHTML = `
      <div class="govuk-form-group" id="outside-fieldset">
        <fieldset>
            <div class="govuk-form-group" id="inner">
              <div class="govuk-checkboxes">
                <input class="govuk-checkboxes__input" />
              </div>
            </div>
          </fieldset>
        </div>`;

    const result = govuk().formGroupForElement(document.querySelector("input"));

    expect(result.id).toBe("inner");
  });

  it("should choose the group outside the fieldset for checkbox in a .govuk-checkboxes__fieldset", () => {
    document.body.innerHTML = `
      <div class="govuk-form-group" id="outside-fieldset">
      <fieldset class="govuk-checkboxes__fieldset">
          <div class="govuk-form-group" id="inner">
            <div class="govuk-checkboxes">
              <input class="govuk-checkboxes__input" />
            </div>
          </div>
        </fieldset>
      </div>`;

    const result = govuk().formGroupForElement(document.querySelector("input"));

    expect(result.id).toBe("outside-fieldset");
  });
});
