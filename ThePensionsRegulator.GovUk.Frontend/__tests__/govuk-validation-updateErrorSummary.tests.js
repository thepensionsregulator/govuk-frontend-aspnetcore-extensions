import { govuk } from "../Scripts/govuk-validation";
import { updateErrorSummary } from "../Scripts/govuk-components/error-summary.ts";

describe("updateSummary", () => {
  it("exposes the component implementation through the govuk API", () => {
    expect(govuk().updateErrorSummary).toBe(updateErrorSummary);
  });
});
