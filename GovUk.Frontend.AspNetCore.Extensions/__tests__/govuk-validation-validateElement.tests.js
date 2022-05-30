const govuk = require("../Content/govuk/govuk-validation");

let _mockUpdateError;
const _mockValidator = {
  methods: {
    required: jest.fn(),
    email: jest.fn(),
    minlength: jest.fn(),
    maxlength: jest.fn(),
    rangelength: jest.fn(),
    range: jest.fn(),
  },
};

function mockCalledFunctions(testSubject) {
  jest.spyOn(testSubject, "getValidator").mockReturnValue(_mockValidator);
  _mockUpdateError = jest
    .spyOn(testSubject, "updateError")
    .mockImplementation(() => null);
}

// value deliberately fails email and compare validators, which are not from jQuery Validate and not mocked
_allValidatorsApplied = `
    <input value="abc"
           data-val="true" 
           data-val-required="This field is required"   
           data-val-email="This field must be an email address"
           data-val-regex="This field must match the pattern" data-val-regex-pattern="[0-9]+"
           data-val-minlength="This field must be a minimum length" data-val-minlength-min="4"
           data-val-maxlength="This field must be a maximum length" data-val-maxlength-max="2"
           data-val-length="This field must be between a minimum and maximum length" data-val-length-min="4" data-val-length-max="10"
           data-val-range="This field must be between a minimum and maximum range" data-val-range-min="5" data-val-range-max="10"
           data-val-equalto="This field must match the other" data-val-equalto-other="other" />
    <input id="other" value="def" />`;

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
    // regex is invalid in _allValidatorsApplied
    _mockValidator.methods.minlength.mockReturnValue(false);
    _mockValidator.methods.maxlength.mockReturnValue(false);
    _mockValidator.methods.rangelength.mockReturnValue(false);
    _mockValidator.methods.range.mockReturnValue(false);
    // compare is invalid in _allValidatorsApplied

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
    // regex is invalid in _allValidatorsApplied
    _mockValidator.methods.minlength.mockReturnValue(false);
    _mockValidator.methods.maxlength.mockReturnValue(false);
    _mockValidator.methods.rangelength.mockReturnValue(false);
    _mockValidator.methods.range.mockReturnValue(false);
    // compare is invalid in _allValidatorsApplied

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-email")
    );
  });

  it("matches regex third when there are multiple errors", () => {
    document.body.innerHTML = _allValidatorsApplied;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.required.mockReturnValue(true);
    _mockValidator.methods.email.mockReturnValue(true);
    // regex is invalid in _allValidatorsApplied
    _mockValidator.methods.minlength.mockReturnValue(false);
    _mockValidator.methods.maxlength.mockReturnValue(false);
    _mockValidator.methods.rangelength.mockReturnValue(false);
    _mockValidator.methods.range.mockReturnValue(false);
    // compare is invalid in _allValidatorsApplied

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-regex")
    );
  });

  it("matches min length fourth when there are multiple errors", () => {
    document.body.innerHTML = _allValidatorsApplied;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.required.mockReturnValue(true);
    _mockValidator.methods.email.mockReturnValue(true);
    document.querySelector("input").setAttribute("value", "123"); // valid for regex, still invalid for compare
    _mockValidator.methods.minlength.mockReturnValue(false);
    _mockValidator.methods.maxlength.mockReturnValue(false);
    _mockValidator.methods.rangelength.mockReturnValue(false);
    _mockValidator.methods.range.mockReturnValue(false);
    // compare is invalid

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-minlength")
    );
  });

  it("matches max length fifth when there are multiple errors", () => {
    document.body.innerHTML = _allValidatorsApplied;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.required.mockReturnValue(true);
    _mockValidator.methods.email.mockReturnValue(true);
    document.querySelector("input").setAttribute("value", "123"); // valid for regex, still invalid for compare
    _mockValidator.methods.minlength.mockReturnValue(true);
    _mockValidator.methods.maxlength.mockReturnValue(false);
    _mockValidator.methods.rangelength.mockReturnValue(false);
    _mockValidator.methods.range.mockReturnValue(false);
    // compare is invalid

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-maxlength")
    );
  });

  it("matches min/max length fifth when there are multiple errors", () => {
    document.body.innerHTML = _allValidatorsApplied;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.required.mockReturnValue(true);
    _mockValidator.methods.email.mockReturnValue(true);
    document.querySelector("input").setAttribute("value", "123"); // valid for regex, still invalid for compare
    _mockValidator.methods.minlength.mockReturnValue(true);
    _mockValidator.methods.maxlength.mockReturnValue(true);
    _mockValidator.methods.rangelength.mockReturnValue(false);
    _mockValidator.methods.range.mockReturnValue(false);
    // compare is invalid

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-length")
    );
  });

  it("matches range sixth when there are multiple errors", () => {
    document.body.innerHTML = _allValidatorsApplied;

    const testSubject = govuk();
    mockCalledFunctions(testSubject);
    _mockValidator.methods.required.mockReturnValue(true);
    _mockValidator.methods.email.mockReturnValue(true);
    document.querySelector("input").setAttribute("value", "123"); // valid for regex, still invalid for compare
    _mockValidator.methods.minlength.mockReturnValue(true);
    _mockValidator.methods.maxlength.mockReturnValue(true);
    _mockValidator.methods.rangelength.mockReturnValue(true);
    _mockValidator.methods.range.mockReturnValue(false);
    // compare is invalid

    const input = document.querySelector("input");
    testSubject.validateElement(input);

    expect(_mockUpdateError.mock.calls.length).toBe(1);
    expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
    expect(_mockUpdateError.mock.calls[0][1]).toBe(
      input.getAttribute("data-val-range")
    );
  });
});
