const govuk = require("../wwwroot/govuk/govuk-validation");

let _mockUpdateError;
const _mockValidator = {
  methods: {
    required: jest.fn(),
    email: jest.fn(),
    phone: jest.fn(),
    regex: jest.fn(),
    minlength: jest.fn(),
    maxlength: jest.fn(),
    rangelength: jest.fn(),
    range: jest.fn(),
    equalTo: jest.fn(),
  },
};

function mockCalledFunctions(testSubject) {
  jest.spyOn(testSubject, "getValidator").mockReturnValue(_mockValidator);
  _mockUpdateError = jest
    .spyOn(testSubject, "updateError")
    .mockImplementation(() => null);
}

_allValidatorsApplied = `
    <input value=""
           data-val="true" 
           data-val-required="This field is required"   
           data-val-email="This field must be an email address"
           data-val-phone="This field must be a phone number"
           data-val-regex="This field must match the pattern" data-val-regex-pattern="[0-9]+"
           data-val-minlength="This field must be a minimum length" data-val-minlength-min="4"
           data-val-maxlength="This field must be a maximum length" data-val-maxlength-max="2"
           data-val-length="This field must be between a minimum and maximum length" data-val-length-min="4" data-val-length-max="10"
           data-val-range="This field must be between a minimum and maximum range" data-val-range-min="5" data-val-range-max="10"
           data-val-equalto="This field must match the other" data-val-equalto-other="other" />
    <input id="other" value="" />`;

describe("validateElement", () => {
  it("runs updateError if a required field fails validation", () => {
    document.body.innerHTML =
      '<input data-val="true" data-val-required="This field is required" value="" />';

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.required.mockReturnValue(false);

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-required")
    );
  });

  it("runs updateError if an email field fails validation", () => {
    document.body.innerHTML =
      '<input data-val="true" data-val-email="This field must be an email address" value="abc" />';

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.email.mockReturnValue(false);

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-email")
    );
  });

  it("runs updateError if an phone field fails validation", () => {
    document.body.innerHTML =
      '<input data-val="true" data-val-phone="This field must be an phone number" value="abc" />';

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.phone.mockReturnValue(false);

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-phone")
    );
  });

  it("runs updateError if a regex field fails validation", () => {
    document.body.innerHTML =
      '<input data-val="true" data-val-regex="This field must match the pattern" data-val-regex-pattern="[0-9]+" value="abc" />';

    const testSubject = govuk();
    mockCalledFunctions(testSubject);

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-regex")
    );
  });

  it("runs updateError if an min length field fails validation", () => {
    document.body.innerHTML =
      '<input data-val="true" data-val-minlength="This field must be a minimum length" data-val-minlength-min="4" value="abc" />';

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.minlength.mockReturnValue(false);

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-minlength")
    );
  });

  it("runs updateError if an max length field fails validation", () => {
    document.body.innerHTML =
      '<input data-val="true" data-val-maxlength="This field must be a maximum length" data-val-maxlength-max="2" value="abc" />';

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.maxlength.mockReturnValue(false);

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-maxlength")
    );
  });

  it("runs updateError if an min & max length field fails validation", () => {
    document.body.innerHTML =
      '<input data-val="true" data-val-length="This field must be between a minimum and maximum length" data-val-length-min="4" data-val-length-max="10" value="abc" />';

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.rangelength.mockReturnValue(false);

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-length")
    );
  });

  it("runs updateError if a range field fails validation", () => {
    document.body.innerHTML =
      '<input data-val="true" data-val-range="This field must be between a minimum and maximum range" data-val-range-min="5" data-val-range-max="10" value="11" />';

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.range.mockReturnValue(false);

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-range")
    );
  });

  it("runs updateError if two compared fields don't match", () => {
    document.body.innerHTML = `
        <input data-val="true" data-val-equalto="This field must match the other" data-val-equalto-other="other" value="abc" />
        <input id="other" value="def" />`;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);

    const input = document.querySelector("input[data-val]");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-equalto")
    );
  });

  it("matches required first when there are multiple errors", () => {
    document.body.innerHTML = _allValidatorsApplied;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.required.mockReturnValue(false);
    _mockValidator.methods.email.mockReturnValue(false);
    _mockValidator.methods.phone.mockReturnValue(false);
    _mockValidator.methods.regex.mockReturnValue(false);
    _mockValidator.methods.minlength.mockReturnValue(false);
    _mockValidator.methods.maxlength.mockReturnValue(false);
    _mockValidator.methods.rangelength.mockReturnValue(false);
    _mockValidator.methods.range.mockReturnValue(false);
    _mockValidator.methods.equalTo.mockReturnValue(false);

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-required")
    );
  });

  it("matches email second when there are multiple errors", () => {
    document.body.innerHTML = _allValidatorsApplied;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.required.mockReturnValue(true);
    _mockValidator.methods.email.mockReturnValue(false);
    _mockValidator.methods.phone.mockReturnValue(false);
    _mockValidator.methods.regex.mockReturnValue(false);
    _mockValidator.methods.minlength.mockReturnValue(false);
    _mockValidator.methods.maxlength.mockReturnValue(false);
    _mockValidator.methods.rangelength.mockReturnValue(false);
    _mockValidator.methods.range.mockReturnValue(false);
    _mockValidator.methods.equalTo.mockReturnValue(false);

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-email")
    );
  });

  it("matches phone third when there are multiple errors", () => {
    document.body.innerHTML = _allValidatorsApplied;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.required.mockReturnValue(true);
    _mockValidator.methods.email.mockReturnValue(true);
    _mockValidator.methods.phone.mockReturnValue(false);
    _mockValidator.methods.regex.mockReturnValue(false);
    _mockValidator.methods.minlength.mockReturnValue(false);
    _mockValidator.methods.maxlength.mockReturnValue(false);
    _mockValidator.methods.rangelength.mockReturnValue(false);
    _mockValidator.methods.range.mockReturnValue(false);
    _mockValidator.methods.equalTo.mockReturnValue(false);

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-phone")
    );
  });

  it("matches regex fourth when there are multiple errors", () => {
    document.body.innerHTML = _allValidatorsApplied;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.required.mockReturnValue(true);
    _mockValidator.methods.email.mockReturnValue(true);
    _mockValidator.methods.phone.mockReturnValue(true);
    _mockValidator.methods.regex.mockReturnValue(false);
    _mockValidator.methods.minlength.mockReturnValue(false);
    _mockValidator.methods.maxlength.mockReturnValue(false);
    _mockValidator.methods.rangelength.mockReturnValue(false);
    _mockValidator.methods.range.mockReturnValue(false);
    _mockValidator.methods.equalTo.mockReturnValue(false);

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-regex")
    );
  });

  it("matches min length fifth when there are multiple errors", () => {
    document.body.innerHTML = _allValidatorsApplied;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.required.mockReturnValue(true);
    _mockValidator.methods.email.mockReturnValue(true);
    _mockValidator.methods.phone.mockReturnValue(true);
    _mockValidator.methods.regex.mockReturnValue(true);
    _mockValidator.methods.minlength.mockReturnValue(false);
    _mockValidator.methods.maxlength.mockReturnValue(false);
    _mockValidator.methods.rangelength.mockReturnValue(false);
    _mockValidator.methods.range.mockReturnValue(false);
    _mockValidator.methods.equalTo.mockReturnValue(false);

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-minlength")
    );
  });

  it("matches max length sixth when there are multiple errors", () => {
    document.body.innerHTML = _allValidatorsApplied;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.required.mockReturnValue(true);
    _mockValidator.methods.email.mockReturnValue(true);
    _mockValidator.methods.phone.mockReturnValue(true);
    _mockValidator.methods.regex.mockReturnValue(true);
    _mockValidator.methods.minlength.mockReturnValue(true);
    _mockValidator.methods.maxlength.mockReturnValue(false);
    _mockValidator.methods.rangelength.mockReturnValue(false);
    _mockValidator.methods.range.mockReturnValue(false);
    _mockValidator.methods.equalTo.mockReturnValue(false);

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-maxlength")
    );
  });

  it("matches min/max length seventh when there are multiple errors", () => {
    document.body.innerHTML = _allValidatorsApplied;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.required.mockReturnValue(true);
    _mockValidator.methods.email.mockReturnValue(true);
    _mockValidator.methods.phone.mockReturnValue(true);
    _mockValidator.methods.regex.mockReturnValue(true);
    _mockValidator.methods.minlength.mockReturnValue(true);
    _mockValidator.methods.maxlength.mockReturnValue(true);
    _mockValidator.methods.rangelength.mockReturnValue(false);
    _mockValidator.methods.range.mockReturnValue(false);
    _mockValidator.methods.equalTo.mockReturnValue(false);

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-length")
    );
  });

  it("matches range eighth when there are multiple errors", () => {
    document.body.innerHTML = _allValidatorsApplied;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.required.mockReturnValue(true);
    _mockValidator.methods.email.mockReturnValue(true);
    _mockValidator.methods.phone.mockReturnValue(true);
    _mockValidator.methods.regex.mockReturnValue(true);
    _mockValidator.methods.minlength.mockReturnValue(true);
    _mockValidator.methods.maxlength.mockReturnValue(true);
    _mockValidator.methods.rangelength.mockReturnValue(true);
    _mockValidator.methods.range.mockReturnValue(false);
    _mockValidator.methods.equalTo.mockReturnValue(false);

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-range")
    );
  });
});
