const govuk = require("../Content/govuk/govuk-validation");

describe("validatePhone", () => {
  it("should allow valid patterns", () => {
    const shouldBeValid = [
      "",
      "1",
      "+44",
      "+44 1",
      "(0800) 123",
      "123-456-789",
      "123.456.789",
      "123 x 123",
      "123 ext 123",
      "123 ext. 123",
    ];

    shouldBeValid.map((x) => {
      const result = govuk().validatePhone(x);
      expect(result).toBe(true);
    });
  });

  it("should disallow invalid patterns", () => {
    const shouldBeInvalid = ["a", "#44", "123 y 123"];

    shouldBeInvalid.map((x) => {
      const result = govuk().validatePhone(x);
      expect(result).toBe(false);
    });
  });
});
