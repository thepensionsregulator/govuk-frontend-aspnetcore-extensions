window.addEventListener("DOMContentLoaded", function () {
  let summary = document.querySelector(".govuk-error-summary");
  if (!summary) {
    summary = document.createElement("div");
    summary.classList.add("govuk-error-summary");
    summary.setAttribute("aria-labelledby", "error-summary-title");
    summary.setAttribute("role", "alert");
    summary.setAttribute("data-module", "govuk-error-summary");
    summary.style.display = "none";

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
  const list = summary.querySelector("ul");

  function updateTitle() {
    const prefix = "Error: ";
    const hasError = document.querySelector(".govuk-error-message");
    const index = document.title.indexOf(prefix);
    if (hasError && index !== 0) {
      document.title = prefix + document.title;
    } else if (!hasError && index === 0) {
      document.title = document.title.substring(prefix.length);
    }
  }

  function updateSummary() {
    while (list.firstChild) {
      list.removeChild(list.firstChild);
    }
    [].slice
      .call(document.querySelectorAll(".govuk-error-message"))
      .map(function (error) {
        let summaryError = document.createElement("li");
        link = document.createElement("a");
        link.href = "#" + error.id.substring(0, error.id.length - 6);
        const prefix = error.querySelector(".govuk-visually-hidden");
        [].slice.call(error.childNodes).map(function (x) {
          if (x !== prefix) {
            link.appendChild(x.cloneNode(true));
          }
        });
        summaryError.appendChild(link);
        list.appendChild(summaryError);
      });

    summary.style.display = list.querySelector("li") ? "block" : "none";
  }

  function updateError(element, message) {
    updateErrorMessage(element, message);
    updateSummary();
    updateTitle();
  }

  function updateErrorMessage(element, message) {
    const errorMessage = errorMessageForElement(element);
    errorMessage.setAttribute("id", element.id + "-error");
    const prefix = errorMessage.querySelector(".govuk-visually-hidden");
    [].slice.call(errorMessage.childNodes).map(function (x) {
      if (x !== prefix) {
        errorMessage.removeChild(x);
      }
    });
    errorMessage.appendChild(document.createTextNode(message));
  }

  function closest(element, selector) {
    if (element.closest) {
      return element.closest(selector);
    } else {
      // jQuery for IE11
      const jQueryResult = $(element).closest(selector);
      return jQueryResult.length ? jQueryResult[0] : null;
    }
  }

  function formGroupForElement(element) {
    const isDateField = element.classList.contains("govuk-date-input__input");
    let formGroup = closest(element, ".govuk-form-group");
    if (isDateField) {
      formGroup = closest(formGroup.parentElement, ".govuk-form-group");
    }
    return formGroup;
  }

  function errorMessageForElement(element) {
    const formGroup = formGroupForElement(element);
    let errorMessage = formGroup.querySelector(".govuk-error-message");
    if (!errorMessage) {
      // Create a new error message container
      errorMessage = document.createElement("p");
      errorMessage.classList.add("govuk-error-message");
      errorMessage.setAttribute("data-valmsg-for", element.id);
      errorMessage.setAttribute("data-valmsg-replace", "true");

      const errorPrefix = document.createElement("span");
      errorPrefix.classList.add("govuk-visually-hidden");
      errorPrefix.innerText = "Error:";
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
  }

  function displayError(element, errorClass, validClass) {
    const formGroup = formGroupForElement(element);
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

    validateElement(firstErrorInGroup);
  }

  function validateElement(element) {
    // We cannot copy the error that jQuery validate generates, because it hasn't done it yet when this event fires.
    // Instead execute the same tests as 'else ifs' so that only one message appears, and in the same order as jQuery,
    // to get the same error message to display.
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
      !$.validator.methods.required.call(
        $.validator.prototype,
        element.value,
        element
      )
    ) {
      updateError(element, required);
    } else if (
      email &&
      !$.validator.methods.email.call(
        $.validator.prototype,
        element.value,
        element
      )
    ) {
      updateError(element, email);
    } else if (pattern && !element.value.match("^" + pattern + "$")) {
      updateError(element, element.getAttribute("data-val-regex"));
    } else if (
      minLengthOnly &&
      !$.validator.methods.minlength.call(
        $.validator.prototype,
        element.value,
        element,
        parseInt(minLengthOnly)
      )
    ) {
      updateError(element, element.getAttribute("data-val-minlength"));
    } else if (
      maxLengthOnly &&
      !$.validator.methods.maxlength.call(
        $.validator.prototype,
        element.value,
        element,
        parseInt(maxLengthOnly)
      )
    ) {
      updateError(element, element.getAttribute("data-val-maxlength"));
    } else if (
      minLength &&
      maxLength &&
      !$.validator.methods.rangelength.call(
        $.validator.prototype,
        element.value,
        element,
        [minLength, maxLength]
      )
    ) {
      updateError(element, element.getAttribute("data-val-length"));
    } else if (
      minRange &&
      maxRange &&
      !$.validator.methods.range.call(
        $.validator.prototype,
        element.value,
        element,
        [minRange, maxRange]
      )
    ) {
      updateError(element, element.getAttribute("data-val-range"));
    } else if (
      compareTo &&
      element.value !== document.getElementById(compareTo).value
    ) {
      updateError(element, element.getAttribute("data-val-equalto"));
    }
  }

  function removeOrUpdateError(element, errorClass, validClass) {
    element.classList.remove("govuk-input--error");
    element.classList.remove("govuk-textarea--error");
    element.classList.remove("govuk-select--error");

    const formGroup = formGroupForElement(element);
    if (formGroup) {
      const errorMessage = formGroup.querySelector(".govuk-error-message");
      if (errorMessage) {
        errorMessage.parentElement.removeChild(errorMessage);
      }
      updateSummary();
      updateTitle();

      const nextError = formGroup.querySelector(
        ".govuk-input--error, .govuk-textarea--error, .govuk-select--error"
      );
      if (!nextError) {
        formGroup.classList.remove("govuk-form-group--error");
      } else {
        displayError(nextError);
      }
    }
  }

  if ($.validator) {
    $.validator.setDefaults({
      highlight: displayError,
      unhighlight: removeOrUpdateError,
    });
  }
});
