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
    getValidator: function (form) {
      // We use .call to hand off to jQuery Validate methods. Some require the internal .settings property to be set.
      if (form && !$.validator.prototype.settings) {
        $.validator.prototype.settings = $.data(form, "validator").settings;
      }
      return $.validator;
    },

    /**
     * Ensures the page title is prefixed with 'Error: ' when there is at least one .govuk-error-message displayed
     */
    updateTitle: function () {
      const prefix = "Error: ";
      const hasError = [].slice
        .call(document.querySelectorAll(".govuk-error-message"))
        .filter(function (error) {
          const prefix = error.querySelector(".govuk-visually-hidden");
          const textNode = 3;
          for (let i = 0; i < error.childNodes.length; i++) {
            let x = error.childNodes[i];
            if (
              x !== prefix &&
              (x.nodeType !== textNode || x.textContent.trim())
            ) {
              return true;
            }
          }
          return false;
        }).length;
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
      const isDateField = element.classList.contains("govuk-date-input__input");
      let errorMessage = formGroup.querySelector(
        ".govuk-error-message[data-valmsg-for='" + element.id + "']"
      );
      if (isDateField) {
        const dateFieldId = element.id.substring(
          0,
          element.id.lastIndexOf(".")
        );
        errorMessage = formGroup.querySelector(
          ".govuk-error-message[data-valmsg-for='" +
            dateFieldId +
            ".Day'], .govuk-error-message[data-valmsg-for='" +
            dateFieldId +
            ".Month'], .govuk-error-message[data-valmsg-for='" +
            dateFieldId +
            ".Year']"
        );
      }
      if (!errorMessage) {
        // Create a new error message container
        errorMessage = document.createElement("p");
        errorMessage.classList.add("govuk-error-message");
        errorMessage.setAttribute("data-valmsg-for", element.id);
        errorMessage.setAttribute("data-valmsg-replace", "false");

        

        // Decide where to put it based on the type of component.
        // If it's within a radio, checkbox or date group and NOT within a conditional area within that group, target the group.
        // Otherwise target the original element.
        let list = closest(
          element,
          ".govuk-radios, .govuk-radios__conditional, .govuk-checkboxes, .govuk-checkboxes__conditional, .govuk-date-input, .govuk-input__wrapper"
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

      const errorPrefix = document.createElement("span");
      errorPrefix.classList.add("govuk-visually-hidden");
      errorPrefix.appendChild(document.createTextNode("Error: "));
     
      errorMessage.setAttribute("id", element.id + "-error");

      //const prefix = errorMessage.querySelector(".govuk-visually-hidden");
      [].slice.call(errorMessage.childNodes).map(function (x) {
          errorMessage.removeChild(x); 
      });

      errorMessage.appendChild(errorPrefix);
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
        const validator = govuk.getValidator(element.form);

        let data = {};
        [].forEach.call(element.attributes, function (attr) { // find all data-val attributes for this element
            if (/^data-val/.test(attr.name)) {
                var ruleName = attr.name.substr(9); // Creates a dictionary - keys look like data-val-[KEY]
                data[ruleName] = attr.value;
            }
        });

        for (const [key, value] of Object.entries(data)) {      // Rules look like data-val-[RULE_NAME] so let's find those
            if (!key.includes("-")) {                           // No hyphen means this is a rule                
                let props = {};                                 // Find properties associated with rule
                for (const [k, v] of Object.entries(data)) {
                    if (k.startsWith(key + "-")) {              // look for attributes that look like [RULE_NAME]-property - e.g. range-max
                        let prop = k.replace(key + "-", "");
                        props[prop] = v;
                    }
                }

                if (key) {
                    let method = key;
                    // conditional switch on anything that doesn't match - data-val-length
                    if (method == "length" && props) {
                        // Now query props to see which rule we have to call...
                        let propsKeys = Object.keys(props);
                        let hasMin = propsKeys.find(e => e == "min");
                        let hasMax = propsKeys.find(e => e == "max");

                        if (hasMax && hasMin) {
                            method = "rangelength";
                        }
                    }

                    if (method == "minlength" && props) {
                        let min = parseInt(props["min"]);
                        props = min;
                    }

                    if (method == "maxlength" && props) {
                        let max = parseInt(props["max"]);
                        props = max;
                    }

                    if (method == "equalto") {
                        method = "equalTo";
                        let other = props["other"];
                        props = document.getElementById(other);
                    }

                    if (method == "regex") {
                        let pattern = props["pattern"];
                        props = pattern;
                    }

                    let valid = validator.methods[method].call(
                        validator.prototype,
                        element.value,
                        element,
                        props
                    );
                    if (!valid) {
                        govuk.updateError(element, value);
                        break;  // Break so that the errors are displayed in the order they're specified
                    }
                }
            }
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

    // Ported from .NET Core code so that behaviour matches https://source.dot.net/#System.ComponentModel.Annotations/System/ComponentModel/DataAnnotations/PhoneAttribute.cs
    validatePhone(value, element) {
      if (!value) {
        return true;
      }

      const additionalPhoneNumberCharacters = "-.()";
      const extensionAbbreviationExtDot = "ext.";
      const extensionAbbreviationExt = "ext";
      const extensionAbbreviationX = "x";

      function isDigit(char) {
        return /^\d$/.test(char);
      }

      function isWhiteSpace(char) {
        return /^\s$/.test(char);
      }

      function removePhoneExtension(potentialPhoneNumber) {
        let lastIndexOfExtension = potentialPhoneNumber.lastIndexOf(
          extensionAbbreviationExtDot
        );
        if (lastIndexOfExtension >= 0) {
          var extension = potentialPhoneNumber.substring(
            lastIndexOfExtension + extensionAbbreviationExtDot.length
          );
          if (matchesPhoneExtension(extension)) {
            return potentialPhoneNumber.substring(0, lastIndexOfExtension);
          }
        }

        lastIndexOfExtension = potentialPhoneNumber.lastIndexOf(
          extensionAbbreviationExt
        );
        if (lastIndexOfExtension >= 0) {
          var extension = potentialPhoneNumber.substring(
            lastIndexOfExtension + extensionAbbreviationExt.length
          );
          if (matchesPhoneExtension(extension)) {
            return potentialPhoneNumber.substring(0, lastIndexOfExtension);
          }
        }

        lastIndexOfExtension = potentialPhoneNumber.lastIndexOf(
          extensionAbbreviationX
        );
        if (lastIndexOfExtension >= 0) {
          var extension = potentialPhoneNumber.substring(
            lastIndexOfExtension + extensionAbbreviationX.length
          );
          if (matchesPhoneExtension(extension)) {
            return potentialPhoneNumber.substring(0, lastIndexOfExtension);
          }
        }

        return potentialPhoneNumber;
      }

      function matchesPhoneExtension(potentialExtension) {
        potentialExtension = potentialExtension.trim();
        if (potentialExtension.length === 0) {
          return false;
        }

        for (let i = 0; i < potentialExtension.length; i++) {
          if (!isDigit(potentialExtension[i])) {
            return false;
          }
        }

        return true;
      }

      value = value.replace("+", "").trim();
      value = removePhoneExtension(value);

      let digitFound = false;
      for (let i = 0; i < value.length; i++) {
        if (isDigit(value[i])) {
          digitFound = true;
          break;
        }
      }

      if (!digitFound) {
        return false;
      }

      for (let i = 0; i < value.length; i++) {
        if (
          !(
            isDigit(value[i]) ||
            isWhiteSpace(value[i]) ||
            additionalPhoneNumberCharacters.indexOf(value[i]) !== -1
          )
        ) {
          return false;
        }
      }

      return true;
      },

    // Custom range validator to handle numbers with commas in
    validateRangeWithCommas(value, element, param) {
      var commaFreeVal = Number(value.replace(/,/g, ''));
      if (!value) {
        return true;
      }
      return (commaFreeVal >= param[0] && commaFreeVal <= param[1]);
    },
  };

  return govuk;
}
window.addEventListener("DOMContentLoaded", function () {
  const govuk = createGovUkValidator();

  govuk.createErrorSummary();

  const validator = govuk.getValidator();
  if (validator) {
    validator.setDefaults({
      highlight: govuk.showError,
      unhighlight: govuk.removeOrUpdateError,
    });

    validator.addMethod("phone", govuk.validatePhone);
    validator.addMethod("range", govuk.validateRangeWithCommas);
      
    validator.unobtrusive.adapters.addBool("phone");
    validator.unobtrusive.parse();
  }
});
