import { govuk } from "../wwwroot/ThePensionsRegulator.GovUk.Frontend/js/govuk-validation";
import { jest } from '@jest/globals';

describe("updateSummary", () => {
  it("should display summary when there is an error", () => {
    document.body.innerHTML = `
      <div class="govuk-error-summary">
        <div class="govuk-error-summary__body">
            <ul class="govuk-list govuk-error-summary__list"></ul>
        </div>
      </div>
      <p class="govuk-error-message">Error message</p>`;

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
        <div class="govuk-error-summary__body">
            <ul class="govuk-list govuk-error-summary__list"></ul>
        </div>
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
            <div class="govuk-error-summary__body">
                <ul class="govuk-list govuk-error-summary__list">
                    <li><a href="#old1">Old one</a></li>
                    <li><a href="#old2">Old two</a></li>
                </ul>
            </div>
      </div>
      <p id="new1-error" class="govuk-error-message">New one</p>
      <p id="new2-error" class="govuk-error-message">New two</p>`;

    govuk().updateErrorSummary();

    expect(
      document.querySelector(".govuk-error-summary__body > .govuk-list")
        .childElementCount
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
        <div class="govuk-error-summary__body">
            <ul class="govuk-list govuk-error-summary__list"></ul>
        </div>
      </div>
      <p id="new1-error" class="govuk-error-message"><span class="govuk-visually-hidden">Error: </span>New one</p>`;

    govuk().updateErrorSummary();

    expect(
      document.querySelector(".govuk-error-summary__list > li > a").innerHTML
    ).toBe("New one");
    expect(
      document.querySelector(".govuk-error-summary .govuk-visually-hidden")
    ).toBeNull();
  });

  it("ignores errors with no content or whitespace", () => {
    document.body.innerHTML = `
      <div class="govuk-error-summary">
        <div class="govuk-error-summary__body">
            <ul class="govuk-list govuk-error-summary__list"></ul>
        </div>
      </div>
      <p id="new1-error" class="govuk-error-message"><span class="govuk-visually-hidden">Error: </span> </p>`; // deliberate whitespace instead of message

    govuk().updateErrorSummary();

    expect(
      document.querySelector(".govuk-error-summary__list").hasChildNodes()
    ).toBe(false);
  });

    it("creates an govuk-error-summary__list if it doesn't exist", () => {
        document.body.innerHTML = `
        <div class="govuk-error-summary">
            <div class="govuk-error-summary__body"></div>
        </div>
        <p class="govuk-error-message">Error message</p>`

        govuk().updateErrorSummary();

        expect(document.querySelector(".govuk-error-summary__list")).not.toBeNull();
    });
});
