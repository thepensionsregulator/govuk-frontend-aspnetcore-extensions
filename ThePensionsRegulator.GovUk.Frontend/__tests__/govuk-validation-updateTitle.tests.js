import { govuk } from "../Scripts/govuk-validation";
import { updateTitle } from "../Scripts/govuk-components/error-summary.ts";

describe("updateTitle", () => {
  it("exposes the component implementation through the govuk API", () => {
    expect(govuk().updateTitle).toBe(updateTitle);
  });
});
