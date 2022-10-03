const govuk = require("../wwwroot/govuk/govuk-validation");

let _mockUpdateError;
const _mockValidator = {
    methods: {
        custom: jest.fn(),
    },
};

function mockCalledFunctions(testSubject) {
    jest.spyOn(testSubject, "getValidator").mockReturnValue(_mockValidator);
    _mockUpdateError = jest
        .spyOn(testSubject, "updateError")
        .mockImplementation(() => null);
}

describe("validateElement", () => {
    it("runs updateError if a custom rule fails validation", () => {
        document.body.innerHTML =
            '<input data-val="true" data-val-custom="Custom rule failed" data-val-custom-prop="Banana" value="" />';

        const testSubject = govuk();
        mockCalledFunctions(testSubject);
        _mockValidator.methods["custom"].mockReturnValue(false);

        const input = document.querySelector("input");
        testSubject.validateElement(input);

        expect(_mockUpdateError.mock.calls.length).toBe(1);
        expect(_mockUpdateError.mock.calls[0][0]).toBe(input);
        expect(_mockUpdateError.mock.calls[0][1]).toBe(
            input.getAttribute("data-val-custom")
        );
    });

    it("passes custom prop to a custom rule", () => {
        document.body.innerHTML =
            '<input data-val="true" data-val-custom="Custom rule failed" data-val-custom-prop="Banana" value="" />';

        const testSubject = govuk();
        mockCalledFunctions(testSubject);

        expect(_mockValidator.methods["custom"]).toBeCalledWith(
            "",                     // element value
            expect.anything(),      // html element
            { "prop": "Banana" }    // custom prop
        );        

    });
});