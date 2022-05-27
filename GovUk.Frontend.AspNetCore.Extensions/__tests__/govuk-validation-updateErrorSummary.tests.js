const govuk = require("../Content/govuk/govuk-validation");

describe("updateSummary", () => {
  it("should display summary when there is an error", () => {
    document.body.innerHTML = `
      <div class="govuk-error-summary">
        <ul class="govuk-list govuk-error-summary__list"></ul>
      </div>
      <p class="govuk-error-message"></p>`;

    govuk().updateErrorSummary();

    expect(
      document
        .querySelector(".govuk-error-summary")
        .classList.contains("govuk-!-display-block")
    ).toBe(true);
  });

  it("should hide summary when there is no error", () => {
    document.body.innerHTML = `
      <div class="govuk-error-summary">
        <ul class="govuk-list govuk-error-summary__list"></ul>
      </div>`;

    govuk().updateErrorSummary();

    expect(
      document
        .querySelector(".govuk-error-summary")
        .classList.contains("govuk-!-display-none")
    ).toBe(true);
  });

  it("should replace all existing errors", () => {
    document.body.innerHTML = `
      <div class="govuk-error-summary">
        <ul class="govuk-list govuk-error-summary__list">
          <li><a href="#old1">Old one</a></li>
          <li><a href="#old2">Old two</a></li>
        </ul>
      </div>
      <p id="new1-error" class="govuk-error-message">New one</p>
      <p id="new2-error" class="govuk-error-message">New two</p>`;

    govuk().updateErrorSummary();

    expect(
      document.querySelector(".govuk-error-summary > .govuk-list").childNodes
        .length
    ).toBe(2);
    expect(
      document.querySelector(".govuk-error-summary a[href='#new1']")
    ).not.toBeNull();
    expect(
      document.querySelector(".govuk-error-summary a[href='#new2']")
    ).not.toBeNull();
  });

  it("doesn't copy the prefix from the error", () => {
    document.body.innerHTML = `
      <div class="govuk-error-summary">
        <ul class="govuk-list govuk-error-summary__list"></ul>
      </div>
      <p id="new1-error" class="govuk-error-message"><span class="govuk-visually-hidden">Error: </span>New one</p>`;

    govuk().updateErrorSummary();

    expect(document.querySelector(".govuk-error-summary").innerText).not.toBe(
      "New one"
    );
    expect(
      document.querySelector(".govuk-error-summary .govuk-visually-hidden")
    ).toBeNull();
  });
});
