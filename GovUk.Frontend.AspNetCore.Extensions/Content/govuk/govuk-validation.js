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

  function updateSummary(errorId, errorMessage) {
    summary.style.display = "block";

    // update existing error for this element if present
    let link = list.querySelector("a[href='#" + errorId + "']");
    if (!link) {
      let error = document.createElement("li");
      link = document.createElement("a");
      link.href = "#" + errorId;
      error.appendChild(link);
      list.appendChild(error);
    }

    // add new error
    while (link.firstChild) {
      link.removeChild(link.firstChild);
    }
    link.appendChild(document.createTextNode(errorMessage));
  }

  function removeFromSummary(errorId) {
    let link = list.querySelector("a[href='#" + errorId + "']");
    if (link) {
      list.removeChild(link.parentElement);
    }
    if (!list.querySelector("li")) {
      summary.style.display = "none";
    }
  }

  if ($.validator) {
    $.validator.setDefaults({
      highlight: function (element, errorClass, validClass) {
        element.classList.add("govuk-input--error");
        $(element)
          .closest(".govuk-form-group")
          .addClass("govuk-form-group--error");

        const id = element.getAttribute("id");
        const errorId = id + "-error";
        if (!document.getElementById(errorId)) {
          const error = document.createElement("p");
          error.classList.add("govuk-error-message");
          error.setAttribute("id", errorId);
          error.setAttribute("data-valmsg-for", id);
          error.setAttribute("data-valmsg-replace", "true");

          let list = $(element).closest(".govuk-radios, .govuk-checkboxes");
          let targetElement = list.length ? list[0] : element;
          if (targetElement.parentElement) {
            targetElement.parentElement.insertBefore(error, targetElement);
          }

          const errorPrefix = document.createElement("span");
          errorPrefix.classList.add("govuk-visually-hidden");
          errorPrefix.innerText = "Error:";
          error.appendChild(errorPrefix);
        }

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
          updateSummary(id, required);
        } else if (
          email &&
          !$.validator.methods.email.call(
            $.validator.prototype,
            element.value,
            element
          )
        ) {
          updateSummary(id, email);
        } else if (pattern && !element.value.match("^" + pattern + "$")) {
          updateSummary(id, element.getAttribute("data-val-regex"));
        } else if (
          minLengthOnly &&
          !$.validator.methods.minlength.call(
            $.validator.prototype,
            element.value,
            element,
            parseInt(minLengthOnly)
          )
        ) {
          updateSummary(id, element.getAttribute("data-val-minlength"));
        } else if (
          maxLengthOnly &&
          !$.validator.methods.maxlength.call(
            $.validator.prototype,
            element.value,
            element,
            parseInt(maxLengthOnly)
          )
        ) {
          updateSummary(id, element.getAttribute("data-val-maxlength"));
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
          updateSummary(id, element.getAttribute("data-val-length"));
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
          updateSummary(id, element.getAttribute("data-val-range"));
        } else if (
          compareTo &&
          element.value !== document.getElementById(compareTo).value
        ) {
          updateSummary(id, element.getAttribute("data-val-equalto"));
        }
      },
      unhighlight: function (element, errorClass, validClass) {
        element.classList.remove("govuk-input--error");
        $(element)
          .closest(".govuk-form-group")
          .removeClass("govuk-form-group--error");

        const errorId = element.getAttribute("id") + "-error";
        let serverSideError = document.getElementById(errorId);
        if (serverSideError) {
          serverSideError.parentElement.removeChild(serverSideError);
        }

        removeFromSummary(element.getAttribute("id"));
      },
    });
  }
});
