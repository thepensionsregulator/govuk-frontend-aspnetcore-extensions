const govuk = require("../wwwroot/govuk/govuk-validation");

describe("validateRangeWithCommas", () => {
  const minValue = -100000000;
  const maxValue = 100000000;
  it("should allow values in range", () => {
    const shouldBeValid = [
      "100,000",
      "1,000,000",
      "-100,000",
      "-1,000,000",
      "10000000",
      "-10000000"
    ];

      shouldBeValid.map((x) => {
        const result = govuk().validateRangeWithCommas(x, null, [minValue, maxValue]);
        expect(result).toBe(true);
    });
  });

  it("should disallow values outside of range", () => {
      const shouldBeInvalid = [
          "-100,000,001",
          "100,000,001",
          "-100000001",
          "100000001",
      ];

    shouldBeInvalid.map((x) => {
      const result = govuk().validateRangeWithCommas(x, null, [minValue, maxValue]); 
      expect(result).toBe(false);
    });
  });
});
