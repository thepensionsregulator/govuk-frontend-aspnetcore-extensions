const govuk = require("../wwwroot/govuk/govuk-validation");

describe("createErrorSummary", () => {
  it("should do nothing if an error summary already exists", () => {
    document.body.innerHTML =
      '<main><div class="govuk-error-summary"></div></main>';

    govuk().createErrorSummary();

    const summary = document.querySelector(".govuk-error-summary");
    expect(summary.hasChildNodes()).toBe(false);
  });

  it("should insert a hidden summary as the first child of the main element", () => {
    document.body.innerHTML =
      '<main><div id="some-other-content"></div></main>';

    govuk().createErrorSummary();

    const summary = document.querySelector("main").firstChild;
    expect(summary.classList.contains("govuk-error-summary"));
    expect(summary.firstChild.classList.contains("govuk-!-display-none"));
  });

  it('should apply role="alert" to the summary', () => {
    document.body.innerHTML = "<main></main>";

    govuk().createErrorSummary();

    const summary = document.querySelector(".govuk-error-summary");
    expect(summary.getAttribute("role")).toEqual("alert");
  });

  it("should wire up GOV.UK JavaScript", () => {
    document.body.innerHTML = "<main></main>";

    govuk().createErrorSummary();

    const summary = document.querySelector(".govuk-error-summary");
    expect(summary.getAttribute("data-module")).toEqual("govuk-error-summary");
  });

  it("should add a heading to the summary", () => {
    document.body.innerHTML = "<main></main>";

    govuk().createErrorSummary();

    const summary = document.querySelector(".govuk-error-summary");
    const h2 = summary.querySelector("h2");

    expect(summary.getAttribute("aria-labelledby")).not.toBeNull();
    expect(summary.getAttribute("aria-labelledby")).toEqual(
      h2.getAttribute("id")
    );
    expect(h2.classList.contains("govuk-error-summary__title")).toBe(true);
    expect(h2.innerHTML).toEqual("There is a problem");
  });

  it("should add a error list to the summary", () => {
    document.body.innerHTML = "<main></main>";

    govuk().createErrorSummary();

    const summary = document.querySelector(".govuk-error-summary");
    const ul = summary.querySelector("ul");
    expect(ul.classList.contains("govuk-list")).toBe(true);
    expect(ul.classList.contains("govuk-error-summary__list")).toBe(true);
    expect(
      ul.parentElement.classList.contains("govuk-error-summary__body")
    ).toBe(true);
  });
});
