"use strict";

// For Jest tests
if (typeof module !== "undefined" && typeof module.exports !== "undefined") {
  module.exports = createGovUkValidator;
}

function createGovUkValidator() {
  function closest(element, selector) {
    if (element.closest) {
      return element.closest(selector);
    } else {
      // jQuery for IE11
      const jQueryResult = $(element).closest(selector);
      return jQueryResult.length ? jQueryResult[0] : null;
    }
  }

  const govuk = {
    /**
     * Gets the global jQuery Validator instance
     * @returns jQuery Validator instance
     */
    getValidator: function () {
      return $.validator;
    },

    /**
     * Ensures the page title is prefixed with 'Error: ' when there is at least one .govuk-error-message displayed
     */
    updateTitle: function () {
      const prefix = "Error: ";
      const hasError = document.querySelector(".govuk-error-message");
      const index = document.title.indexOf(prefix);
      if (hasError && index !== 0) {
        document.title = prefix + document.title;
      } else if (!hasError && index === 0) {
        document.title = document.title.substring(prefix.length);
      }
    },

    /**
     * Creates a hidden error summary component if one does not already exist
     * @returns void
     */
    createErrorSummary: function () {
      if (!document.querySelector(".govuk-error-summary")) {
        const summary = document.createElement("div");
        summary.classList.add("govuk-error-summary");
        summary.setAttribute("aria-labelledby", "error-summary-title");
        summary.setAttribute("role", "alert");
        summary.setAttribute("data-module", "govuk-error-summary");
        summary.classList.add("govuk-!-display-none");

        const h2 = document.createElement("h2");
        h2.classList.add("govuk-error-summary__title");
        h2.setAttribute("id", "error-summary-title");
        h2.appendChild(document.createTextNode("There is a problem"));
        summary.appendChild(h2);

        const body = document.createElement("div");
        body.classList.add("govuk-error-summary__body");
        summary.appendChild(body);

        const ul = document.createElement("ul");
        ul.classList.add("govuk-list");
        ul.classList.add("govuk-error-summary__list");
        body.appendChild(ul);

        const main = document.querySelector("main");
        main.insertBefore(summary, main.firstChild);
      }
    },

    /**
     * Updates the error summary to match all .govuk-error-message elements displayed on the page
     * @returns void
     */
    updateErrorSummary: function () {
      const summary = document.querySelector(".govuk-error-summary");
      if (!summary) {
        return;
      }
      const list = summary.querySelector("ul");
      if (!list) {
        return;
      }
      while (list.firstChild) {
        list.removeChild(list.firstChild);
      }
      [].slice
        .call(document.querySelectorAll(".govuk-error-message"))
        .map(function (error) {
          const link = document.createElement("a");
          const prefix = error.querySelector(".govuk-visually-hidden");
          const textNode = 3;
          [].slice.call(error.childNodes).map(function (x) {
            if (
              x !== prefix &&
              (x.nodeType !== textNode || x.textContent.trim())
            ) {
              link.appendChild(x.cloneNode(true));
            }
          });
          if (!link.hasChildNodes()) {
            return;
          }
          link.href = "#" + error.id.substring(0, error.id.length - 6);
          let summaryError = document.createElement("li");
          summaryError.appendChild(link);
          list.appendChild(summaryError);
        });

      const hasError = list.querySelector("li");
      summary.classList.remove(
        hasError ? "govuk-!-display-none" : "govuk-!-display-block"
      );
      summary.classList.add(
        hasError ? "govuk-!-display-block" : "govuk-!-display-none"
      );
    },

    /**
     * Finds the appropriate .govuk-form-group that encompasses a form element or set of elements
     * @param {HTMLElement} element A form element such as <input />
     * @returns HTMLElement
     */
    formGroupForElement: function (element) {
      const isDateField = element.classList.contains("govuk-date-input__input");
      let formGroup = closest(element, ".govuk-form-group");
      if (isDateField) {
        formGroup = closest(formGroup.parentElement, ".govuk-form-group");
      }
      return formGroup;
    },

    /**
     * Finds or creates a .govuk-error-message to display errors for a form element
     * @param {HTMLElement} element A form element such as <input />
     * @returns A .govuk-error-message element
     */
    errorMessageForElement: function (element) {
      const formGroup = govuk.formGroupForElement(element);
      let errorMessage = formGroup.querySelector(".govuk-error-message");
      if (!errorMessage) {
        // Create a new error message container
        errorMessage = document.createElement("p");
        errorMessage.classList.add("govuk-error-message");
        errorMessage.setAttribute("data-valmsg-for", element.id);
        errorMessage.setAttribute("data-valmsg-replace", "true");

        const errorPrefix = document.createElement("span");
        errorPrefix.classList.add("govuk-visually-hidden");
        errorPrefix.appendChild(document.createTextNode("Error: "));
        errorMessage.appendChild(errorPrefix);

        // Decide where to put it based on the type of component.
        // If it's within a radio, checkbox or date group and NOT within a conditional area within that group, target the group.
        // Otherwise target the original element.
        let list = closest(
          element,
          ".govuk-radios, .govuk-radios__conditional, .govuk-checkboxes, .govuk-checkboxes__conditional, .govuk-date-input"
        );
        let targetElement =
          list &&
          !list.classList.contains("govuk-radios__conditional") &&
          !list.classList.contains("govuk-checkboxes__conditional")
            ? list
            : element;
        if (targetElement.parentElement) {
          targetElement.parentElement.insertBefore(errorMessage, targetElement);
        }
      }
      return errorMessage;
    },

    /**
     * Updates the message displayed by an existing .govuk-error-message component
     * @param {string} element A form element such as <input /> which is invalid
     * @param {string} message The error message to display
     * @returns void
     */
    updateErrorMessage: function (element, message) {
      if (!element) {
        return;
      }
      const errorMessage = govuk.errorMessageForElement(element);
      if (!errorMessage) {
        return;
      }
      errorMessage.setAttribute("id", element.id + "-error");
      const prefix = errorMessage.querySelector(".govuk-visually-hidden");
      [].slice.call(errorMessage.childNodes).map(function (x) {
        if (x !== prefix) {
          errorMessage.removeChild(x);
        }
      });
      errorMessage.appendChild(document.createTextNode(message));
    },

    /**
     * Handler for the jQuery Validate 'highlight' event
     * @param {HTMLElement} element A form element which is invalid
     * @param {string} errorClass Not used
     * @param {string} validClass Not used
     * @returns void
     */
    showError: function (element, errorClass, validClass) {
      const formGroup = govuk.formGroupForElement(element);
      formGroup.classList.add("govuk-form-group--error");

      if (element.tagName === "SELECT") {
        element.classList.add("govuk-select--error");
      } else if (element.tagName === "TEXTAREA") {
        element.classList.add("govuk-textarea--error");
      } else {
        element.classList.add("govuk-input--error");
      }
      const firstErrorInGroup = formGroup.querySelector(
        ".govuk-input--error, .govuk-textarea--error, .govuk-select--error"
      );

      govuk.validateElement(firstErrorInGroup);
    },

    validateElement: function (element) {
      const validator = govuk.getValidator();

      // We cannot copy the error that jQuery validate generates, because it hasn't done it yet when this event fires.
      // Instead execute the same tests as 'else ifs' so that only one message appears.
      // Do it in the same order as jQuery to get the same error message to display.
      const required = element.getAttribute("data-val-required");
      const email = element.getAttribute("data-val-email");
      const pattern = element.getAttribute("data-val-regex-pattern");
      const minLength = element.getAttribute("data-val-length-min");
      const minLengthOnly = element.getAttribute("data-val-minlength-min");
      const maxLength = element.getAttribute("data-val-length-max");
      const maxLengthOnly = element.getAttribute("data-val-maxlength-max");
      const minRange = element.getAttribute("data-val-range-min");
      const maxRange = element.getAttribute("data-val-range-max");
      const compareTo = element.getAttribute("data-val-equalto-other");
      if (
        required &&
        !validator.methods.required.call(
          validator.prototype,
          element.value,
          element
        )
      ) {
        govuk.updateError(element, required);
      } else if (
        email &&
        !validator.methods.email.call(
          validator.prototype,
          element.value,
          element
        )
      ) {
        govuk.updateError(element, email);
      } else if (pattern && !element.value.match("^" + pattern + "$")) {
        govuk.updateError(element, element.getAttribute("data-val-regex"));
      } else if (
        minLengthOnly &&
        !validator.methods.minlength.call(
          validator.prototype,
          element.value,
          element,
          parseInt(minLengthOnly)
        )
      ) {
        govuk.updateError(element, element.getAttribute("data-val-minlength"));
      } else if (
        maxLengthOnly &&
        !validator.methods.maxlength.call(
          validator.prototype,
          element.value,
          element,
          parseInt(maxLengthOnly)
        )
      ) {
        govuk.updateError(element, element.getAttribute("data-val-maxlength"));
      } else if (
        minLength &&
        maxLength &&
        !validator.methods.rangelength.call(
          validator.prototype,
          element.value,
          element,
          [minLength, maxLength]
        )
      ) {
        govuk.updateError(element, element.getAttribute("data-val-length"));
      } else if (
        minRange &&
        maxRange &&
        !validator.methods.range.call(
          validator.prototype,
          element.value,
          element,
          [minRange, maxRange]
        )
      ) {
        govuk.updateError(element, element.getAttribute("data-val-range"));
      } else if (
        compareTo &&
        element.value !== document.getElementById(compareTo).value
      ) {
        govuk.updateError(element, element.getAttribute("data-val-equalto"));
      }
    },

    /**
     * Updates the page title, error summary and error message when a form element becomes invalid
     * @param {HTMLElement} element A form element which is now invalid
     * @param {string} message Error message
     */
    updateError: function (element, message) {
      govuk.updateErrorMessage(element, message);
      govuk.updateErrorSummary();
      govuk.updateTitle();
    },

    /**
     * Handler for the jQuery Validate 'unhighlight' event
     * @param {HTMLElement} element A form element which is now valid
     * @param {string} errorClass Not used
     * @param {string} validClass Not used
     */
    removeOrUpdateError: function (element, errorClass, validClass) {
      if (!element) {
        return;
      }
      element.classList.remove("govuk-input--error");
      element.classList.remove("govuk-textarea--error");
      element.classList.remove("govuk-select--error");

      const errorMessage = govuk.errorMessageForElement(element);
      if (errorMessage) {
        errorMessage.parentElement.removeChild(errorMessage);
      }
      govuk.updateErrorSummary();
      govuk.updateTitle();

      const formGroup = govuk.formGroupForElement(element);
      if (formGroup) {
        const nextError = formGroup.querySelector(
          ".govuk-input--error, .govuk-textarea--error, .govuk-select--error"
        );
        if (!nextError) {
          formGroup.classList.remove("govuk-form-group--error");
        } else {
          govuk.showError(nextError);
        }
      }
    },
  };

  return govuk;
}
window.addEventListener("DOMContentLoaded", function () {
  const govuk = createGovUkValidator();

  govuk.createErrorSummary();

  if (govuk.getValidator()) {
    govuk.getValidator().setDefaults({
      highlight: govuk.showError,
      unhighlight: govuk.removeOrUpdateError,
    });
  }
});
