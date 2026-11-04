import { govuk } from "../Scripts/govuk-validation";

describe("createErrorSummary", () => {
  it("returns the summary through the govuk API", () => {
    document.body.innerHTML = "<main></main>";

    const summary = govuk().createErrorSummary();

    expect(summary).toBe(document.querySelector(".govuk-error-summary"));
  });
});
describe("ensureErrorSummary", () => {
  it("returns the summary through the govuk API", () => {
    document.body.innerHTML = "<main></main>";

    const summary = govuk().ensureErrorSummary();

    expect(summary).toBe(document.querySelector(".govuk-error-summary"));
  });
})
