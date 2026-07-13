import '@testing-library/jest-dom';
import { jest } from '@jest/globals';

const mockGovUkValidator = {
    showError: jest.fn(),
    removeOrUpdateError: jest.fn(),
    updateError: jest.fn(),
    updateErrorSummary: jest.fn(),
    formGroupForElement: jest.fn()
};

jest.unstable_mockModule("/_content/ThePensionsRegulator.GovUk.Frontend/govuk/govuk-validation.js", () => ({
    govuk: jest.fn(() => mockGovUkValidator)
}));

global.$ = jest.fn((selector) => {
    const mockJQueryObject = {
        data: jest.fn(() => null),
        removeData: jest.fn(() => mockJQueryObject),
        validate: jest.fn(() => ({
            settings: {
                highlight: jest.fn(),
                unhighlight: jest.fn()
            },
            element: jest.fn(() => true),
            errorMap: {}
        }))
    };
    return mockJQueryObject;
});

global.$.validator = {
    unobtrusive: {
        parse: jest.fn()
    }
};

const { AddressLookupValidator } = await import("../../wwwroot/tpr/address-lookup/validator.js");

describe("AddressLookupValidator", () => {
    let validator;
    let container;
    const testIndex = 0;

    beforeEach(() => {
        document.body.innerHTML = `
            <form>
                <div id="container">
                    <fieldset class="govuk-fieldset">
                        <legend class="govuk-fieldset__legend">Test Legend</legend>
                        <input id="test-input-0" name="test-input" />
                    </fieldset>
                </div>
            </form>`;

        container = document.getElementById("container");
        validator = new AddressLookupValidator(container, testIndex);
    });

    describe("addOrUpdateCustomFieldsetError", () => {
        it("should wrap fieldset in a form group if not already wrapped", () => {
            const fieldset = document.querySelector("fieldset");
            validator.addOrUpdateCustomFieldsetError(fieldset, "Test error message");

            const formGroup = fieldset.parentElement;
            expect(formGroup).not.toBeNull();
            expect(formGroup.classList.contains("govuk-form-group")).toBe(true);
        });

        it("should add error class to the form group", () => {
            const fieldset = document.querySelector("fieldset");
            validator.addOrUpdateCustomFieldsetError(fieldset, "Test error message");

            const formGroup = fieldset.parentElement;
            expect(formGroup.classList.contains("govuk-form-group--error")).toBe(true);
        });

        it("should not create a new form group if fieldset is already wrapped", () => {
            document.body.innerHTML = `
                <form>
                    <div id="container">
                        <div class="govuk-form-group">
                            <fieldset class="govuk-fieldset">
                                <legend class="govuk-fieldset__legend">Test Legend</legend>
                                <input id="test-input-0" name="test-input" />
                            </fieldset>
                        </div>
                    </div>
                </form>`;
            container = document.getElementById("container");
            validator = new AddressLookupValidator(container, testIndex);
            const fieldset = document.querySelector("fieldset");
            const existingFormGroup = document.querySelector(".govuk-form-group");

            validator.addOrUpdateCustomFieldsetError(fieldset, "Test error message");

            const formGroups = container.querySelectorAll(".govuk-form-group");
            expect(formGroups.length).toBe(1);
            expect(fieldset.parentElement).toBe(existingFormGroup);
        });

        it("should create an error message element with correct CSS class", () => {
            const fieldset = document.querySelector("fieldset");
            validator.addOrUpdateCustomFieldsetError(fieldset, "Test error message");

            const errorElement = fieldset.querySelector(".govuk-error-message");
            expect(errorElement).not.toBeNull();
            expect(errorElement.tagName).toBe("P");
            expect(errorElement.classList.contains("fieldset-error-message")).toBe(true);
        });

        it("should create error message with visually hidden 'Error:' prefix", () => {
            const fieldset = document.querySelector("fieldset");
            validator.addOrUpdateCustomFieldsetError(fieldset, "Test error message");

            const errorElement = fieldset.querySelector(".govuk-error-message");
            const hiddenSpan = errorElement.querySelector(".govuk-visually-hidden");

            expect(hiddenSpan).not.toBeNull();
            expect(hiddenSpan.innerText).toBe("Error: ");
        });

        it("should set error message id based on first input id", () => {
            const fieldset = document.querySelector("fieldset");
            validator.addOrUpdateCustomFieldsetError(fieldset, "Test error message");

            const errorElement = fieldset.querySelector(".govuk-error-message");
            expect(errorElement.id).toBe("test-input-0-error");
        });

        it("should insert error message after the legend", () => {
            const fieldset = document.querySelector("fieldset");
            validator.addOrUpdateCustomFieldsetError(fieldset, "Test error message");

            const legend = fieldset.querySelector("legend");
            const errorElement = fieldset.querySelector(".govuk-error-message");

            expect(legend.nextElementSibling).toBe(errorElement);
        });

        it("should set aria-describedby attribute on fieldset", () => {
            const fieldset = document.querySelector("fieldset");
            validator.addOrUpdateCustomFieldsetError(fieldset, "Test error message");

            expect(fieldset.getAttribute("aria-describedby")).toBe("test-input-0-error");
        });

        it("should display the error message text", () => {
            const fieldset = document.querySelector("fieldset");
            const errorMessage = "This is a test error message";
            validator.addOrUpdateCustomFieldsetError(fieldset, errorMessage);

            const errorElement = fieldset.querySelector(".govuk-error-message");
            expect(errorElement.textContent).toContain(errorMessage);
        });

        it("should update existing error message when called multiple times", () => {
            const fieldset = document.querySelector("fieldset");
            validator.addOrUpdateCustomFieldsetError(fieldset, "First error");
            validator.addOrUpdateCustomFieldsetError(fieldset, "Second error");

            const errorElements = fieldset.querySelectorAll(".govuk-error-message");
            expect(errorElements.length).toBe(1);
            expect(errorElements[0].textContent).toContain("Second error");
            expect(errorElements[0].textContent).not.toContain("First error");
        });

        it("should remove previous error text node when updating", () => {
            const fieldset = document.querySelector("fieldset");
            validator.addOrUpdateCustomFieldsetError(fieldset, "First error message");
            validator.addOrUpdateCustomFieldsetError(fieldset, "Updated error");

            const errorElement = fieldset.querySelector(".govuk-error-message");
            const textContent = errorElement.textContent.replace("Error: ", "");

            expect(textContent).toBe("Updated error");
        });

        it("should call updateErrorSummary", () => {
            mockGovUkValidator.updateErrorSummary.mockClear();
            const fieldset = document.querySelector("fieldset");

            validator.addOrUpdateCustomFieldsetError(fieldset, "Test error message");

            expect(mockGovUkValidator.updateErrorSummary).toHaveBeenCalledTimes(1);
        });

        it("should not insert error message when fieldset has no legend", () => {
            document.body.innerHTML = `
                <form>
                    <div id="container">
                        <fieldset class="govuk-fieldset">
                            <input id="test-input-0" name="test-input" />
                        </fieldset>
                    </div>
                </form>`;
            container = document.getElementById("container");
            validator = new AddressLookupValidator(container, testIndex);
            const fieldset = document.querySelector("fieldset");

            validator.addOrUpdateCustomFieldsetError(fieldset, "Test error message");

            const errorElement = fieldset.querySelector(".govuk-error-message");
            expect(errorElement).toBeNull();
        });

        it("should maintain form group structure when fieldset already in form group with error class", () => {
            document.body.innerHTML = `
                <form>
                    <div id="container">
                        <div class="govuk-form-group govuk-form-group--error">
                            <fieldset class="govuk-fieldset">
                                <legend class="govuk-fieldset__legend">Test Legend</legend>
                                <input id="test-input-0" name="test-input" />
                            </fieldset>
                        </div>
                    </div>
                </form>`;
            container = document.getElementById("container");
            validator = new AddressLookupValidator(container, testIndex);
            const fieldset = document.querySelector("fieldset");
            const existingFormGroup = document.querySelector(".govuk-form-group");

            validator.addOrUpdateCustomFieldsetError(fieldset, "Test error message");

            expect(fieldset.parentElement).toBe(existingFormGroup);
            expect(existingFormGroup.classList.contains("govuk-form-group--error")).toBe(true);
        });

        it("should preserve existing aria-describedby when adding error", () => {
            const fieldset = document.querySelector("fieldset");
            validator.addOrUpdateCustomFieldsetError(fieldset, "Test error message");

            const ariaDescribedBy = fieldset.getAttribute("aria-describedby");
            expect(ariaDescribedBy).toBe("test-input-0-error");

            validator.addOrUpdateCustomFieldsetError(fieldset, "Updated error message");

            expect(fieldset.getAttribute("aria-describedby")).toBe(ariaDescribedBy);
        });

        it("should handle multiple inputs in fieldset by using first input for error id", () => {
            document.body.innerHTML = `
                <form>
                    <div id="container">
                        <fieldset class="govuk-fieldset">
                            <legend class="govuk-fieldset__legend">Test Legend</legend>
                            <input id="test-input-0" name="test-input" />
                            <input id="test-input-2-0" name="test-input-2" />
                        </fieldset>
                    </div>
                </form>`;
            container = document.getElementById("container");
            validator = new AddressLookupValidator(container, testIndex);
            const fieldset = document.querySelector("fieldset");

            validator.addOrUpdateCustomFieldsetError(fieldset, "Test error message");

            const errorElement = fieldset.querySelector(".govuk-error-message");
            expect(errorElement.id).toBe("test-input-0-error");
        });
    });

    describe("removeCustomFieldsetError", () => {
        it("should remove fieldset error message when fieldset is in a form group", () => {
            const fieldset = document.querySelector("fieldset");
            validator.addOrUpdateCustomFieldsetError(fieldset, "Test error message");

            expect(fieldset.querySelector(".fieldset-error-message")).not.toBeNull();

            validator.removeCustomFieldsetError(fieldset);

            expect(fieldset.querySelector(".fieldset-error-message")).toBeNull();
        });

        it("should not fail when there is no fieldset error message", () => {
            document.body.innerHTML = `
                <form>
                    <div id="container">
                        <div class="govuk-form-group">
                            <fieldset class="govuk-fieldset">
                                <legend class="govuk-fieldset__legend">Test Legend</legend>
                                <input id="test-input-0" name="test-input" />
                            </fieldset>
                        </div>
                    </div>
                </form>`;
            container = document.getElementById("container");
            validator = new AddressLookupValidator(container, testIndex);
            const fieldset = document.querySelector("fieldset");

            expect(() => validator.removeCustomFieldsetError(fieldset)).not.toThrow();
            expect(fieldset.querySelector(".fieldset-error-message")).toBeNull();
        });

        it("should not remove error when fieldset is not in a form group", () => {
            const fieldset = document.querySelector("fieldset");
            validator.addOrUpdateCustomFieldsetError(fieldset, "Test error message");

            const formGroup = fieldset.parentElement;
            formGroup.replaceWith(fieldset);

            expect(fieldset.querySelector(".fieldset-error-message")).not.toBeNull();

            validator.removeCustomFieldsetError(fieldset);

            expect(fieldset.querySelector(".fieldset-error-message")).not.toBeNull();
        });

        it("should keep govuk-form-group--error class on parent form group", () => {
            document.body.innerHTML = `
                <form>
                    <div id="container">
                        <div class="govuk-form-group govuk-form-group--error">
                            <fieldset class="govuk-fieldset">
                                <legend class="govuk-fieldset__legend">Test Legend</legend>
                                <p class="govuk-error-message fieldset-error-message">
                                    <span class="govuk-visually-hidden">Error: </span>
                                    Test error message
                                </p>
                                <input id="test-input-0" name="test-input" />
                            </fieldset>
                        </div>
                    </div>
                </form>`;
            container = document.getElementById("container");
            validator = new AddressLookupValidator(container, testIndex);
            const fieldset = document.querySelector("fieldset");
            const formGroup = document.querySelector(".govuk-form-group");

            validator.removeCustomFieldsetError(fieldset);

            expect(formGroup.classList.contains("govuk-form-group--error")).toBe(true);
            expect(fieldset.querySelector(".fieldset-error-message")).toBeNull();
        });
    });

    describe("constructor focusout handling", () => {
        it.each([
            "button[data-address-lookup]",
            "button[type='submit']",
            "input[type='submit']",
            "a[data-address-lookup='return-to-postcode']",
            "a[data-address-lookup='address-not-on-list']"
        ])("should skip blur validation when focus moves to %s", (selector) => {
            const input = document.querySelector("#test-input-0");
            input.setAttribute("data-val", "true");
            const validateOnBlurSpy = jest.spyOn(validator, "validateOnBlur");

            const nextFocusedElement = document.createElement(selector.startsWith("a[") ? "a" : selector.startsWith("input[") ? "input" : "button");
            if (selector === "button[data-address-lookup]") {
                nextFocusedElement.setAttribute("data-address-lookup", "find-address");
            }
            if (selector === "button[type='submit']" || selector === "input[type='submit']") {
                nextFocusedElement.setAttribute("type", "submit");
            }
            if (selector === "a[data-address-lookup='return-to-postcode']") {
                nextFocusedElement.setAttribute("data-address-lookup", "return-to-postcode");
            }
            if (selector === "a[data-address-lookup='address-not-on-list']") {
                nextFocusedElement.setAttribute("data-address-lookup", "address-not-on-list");
            }

            document.body.appendChild(nextFocusedElement);

            input.dispatchEvent(new FocusEvent("focusout", { bubbles: true, relatedTarget: nextFocusedElement }));

            expect(validateOnBlurSpy).not.toHaveBeenCalled();
        });

        it("should validate on blur when related target does not match skip selectors", () => {
            const input = document.querySelector("#test-input-0");
            input.setAttribute("data-val", "true");
            const validateOnBlurSpy = jest.spyOn(validator, "validateOnBlur");

            const nextFocusedElement = document.createElement("button");
            nextFocusedElement.setAttribute("type", "button");
            document.body.appendChild(nextFocusedElement);

            input.dispatchEvent(new FocusEvent("focusout", { bubbles: true, relatedTarget: nextFocusedElement }));

            expect(validateOnBlurSpy).toHaveBeenCalledTimes(1);
            expect(validateOnBlurSpy).toHaveBeenCalledWith(input);
        });

        it("should not validate on blur for elements without data-val='true'", () => {
            const input = document.querySelector("#test-input-0");
            const validateOnBlurSpy = jest.spyOn(validator, "validateOnBlur");

            input.dispatchEvent(new FocusEvent("focusout", { bubbles: true }));

            expect(validateOnBlurSpy).not.toHaveBeenCalled();
        });
    });

    describe("focusInvalid", () => {
        it("should delegate to the current form validator's focusInvalid", () => {
            const focusInvalidMock = jest.fn();
            const mockValidator = { focusInvalid: focusInvalidMock };
            global.$ = jest.fn(() => ({
                data: jest.fn(() => mockValidator),
                removeData: jest.fn(function () { return this; }),
                validate: jest.fn(() => mockValidator)
            }));
            global.$.validator = { unobtrusive: { parse: jest.fn() } };

            validator.focusInvalid();

            expect(focusInvalidMock).toHaveBeenCalledTimes(1);
        });
    });
});

