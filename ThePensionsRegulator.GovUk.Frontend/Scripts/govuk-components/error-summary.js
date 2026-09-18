// Intentionally a .js file, so it can be imported into a .ts file without needing to be compiled first. This is because the govuk-validation.js file is imported into the validator.ts file, which is then compiled into govuk-validation.js. If this file was a .ts file, it would create a circular dependency and cause an error when compiling.

export function ensureErrorSummary() {
    const existingErrorSummary = document.querySelector(".govuk-error-summary");

    if (existingErrorSummary) {
        return existingErrorSummary;
    }

    const newErrorSummary = createErrorSummary();
    const main = document.querySelector("main");
    main?.insertBefore(newErrorSummary, main.firstChild);

    return newErrorSummary;
}

function createErrorSummary() {
    const errorSummary = document.createElement("div");
    errorSummary.className = "govuk-error-summary";
    errorSummary.setAttribute("role", "alert");
    errorSummary.setAttribute("aria-labelledby", "error-summary-title");
    errorSummary.setAttribute("data-module", "govuk-error-summary");
    errorSummary.classList.add("govuk-!-display-none");

    const h2 = document.createElement("h2");
    h2.className = "govuk-error-summary__title";
    h2.id = "error-summary-title";
    h2.appendChild(document.createTextNode("There is a problem"));
    errorSummary.appendChild(h2);

    const body = document.createElement("div");
    body.className = "govuk-error-summary__body";
    errorSummary.appendChild(body);

    const ul = document.createElement("ul");
    ul.className = "govuk-list govuk-error-summary__list";
    body.appendChild(ul);

    return errorSummary;
}

export function updateErrorSummary() {
    const summary = document.querySelector(".govuk-error-summary");
    if (!summary) {
        return;
    }

    const errorSummaryBody = summary.querySelector(".govuk-error-summary__body");
    if (!errorSummaryBody) {
        return;
    }

    let list = errorSummaryBody.querySelector("ul");
    if (!list) {
        const ul = document.createElement("ul");
        ul.classList.add("govuk-list");
        ul.classList.add("govuk-error-summary__list");
        errorSummaryBody.appendChild(ul);
        list = ul;
    }

    const textNode = 3;

    // Get the current links in the error summary, and the links that need to be there
    const currentErrors = [].slice.call(list.querySelectorAll("a"));

    const updatedErrors = [].slice
        .call(
          document.querySelectorAll(
            ".govuk-error-message:not(.govuk-character-count__status)"
          )
        )
        .map(function (error) {
          const link = document.createElement("a");
          const prefix = error.querySelector(".govuk-visually-hidden");
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
          return link;
        })
        .filter(function (link) {
          return link !== undefined;
        });

    function findErrorsNotMatched(errorsToLookFor, errorsToSearch) {
        const result = [];
        for (let i = 0; i < errorsToLookFor.length; i++) {
            let matchingErrors = errorsToSearch.filter(function (link) {
                return matchErrorLink(link, errorsToLookFor[i]);
            });
            if (!matchingErrors.length) {
                result.push(errorsToLookFor[i]);
            }
        }
        return result;
    }

    function matchErrorLink(link1, link2) {
        return (
            link1.href === link2.href && link1.textContent == link2.textContent
        );
    }

    // Remove any errors from the error summary that are no longer in the page.
    const errorsToRemove = findErrorsNotMatched(currentErrors, updatedErrors);

    for (let i = 0; i < errorsToRemove.length; i++) {
        list.removeChild(errorsToRemove[i].parentElement);
    }

    // Find any new errors and insert them at the correct position in the error summary.
    // It's important to leave existing links untouched. If your focus is in a field and you click
    // on an error summary link, validation will run on focusout of the field before the link is followed.
    // If validation removes the link (even if it recreates an identical one) it cannot be followed.
    let errorsToAdd = findErrorsNotMatched(updatedErrors, currentErrors);

    for (let i = 0; i < errorsToAdd.length; i++) {
        let summaryError = document.createElement("li");
        summaryError.appendChild(errorsToAdd[i]);

        if (list.childNodes.length == 0) {
            list.appendChild(summaryError);
        } else {
            let indexOfThisError = updatedErrors.indexOf(errorsToAdd[i]);
            if (indexOfThisError === 0) {
                list.insertBefore(summaryError, list.firstChild);
            } else {
                let errorToInsertAfter = updatedErrors[indexOfThisError - 1];
                for (let j = 0; j < list.childNodes.length; j++) {
                    let link = list.childNodes[j].querySelector("a");
                    if (matchErrorLink(link, errorToInsertAfter)) {
                        if (list.childNodes[j].nextSibling) {
                            list.insertBefore(
                                summaryError,
                                list.childNodes[j].nextSibling
                            );
                        } else {
                            list.appendChild(summaryError);
                        }
                        break;
                    }
                }
            }
        }
    }

    const hasError = list.querySelector("li");
    summary.classList.remove(
        hasError ? "govuk-!-display-none" : "govuk-!-display-block"
    );
    summary.classList.add(
        hasError ? "govuk-!-display-block" : "govuk-!-display-none"
    );
}

export function updateTitle() {
    const titleTag = document.getElementsByTagName("title")[0];
    let prefix = "";

    if (!titleTag || titleTag.getAttribute("data-govuk-error-prefix") == null) {
        prefix = "Error: ";
    } else {
        prefix = titleTag.getAttribute("data-govuk-error-prefix");
    }

    const hasError = [].slice
        .call(document.querySelectorAll(".govuk-error-message"))
        .filter(function (error) {
            const errorPrefix = error.querySelector(".govuk-visually-hidden");
            const textNode = 3;

            for (let i = 0; i < error.childNodes.length; i++) {
                let childNode = error.childNodes[i];
                if (
                    childNode !== errorPrefix &&
                    (childNode.nodeType !== textNode || childNode.textContent.trim())
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
}