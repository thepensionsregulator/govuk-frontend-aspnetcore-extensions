import {validatePostcode} from "../../Scripts/address-lookup/postcode-validator";

describe("validatePostcode", () => {
    it.each([
        "GIR 0AA",
        "M1 1AE",
        "M11 1AE",
        "CR2 6XH",
        "DN55 1PT",
        "W1A 1HQ",
        "EC1A 1BB",
        "SW1A 1AA",
        "BN1 4DW",
        "BN1 6AF"])
        ("%s is a valid postcode", (postcode) => {
            const result = validatePostcode(postcode);
            expect(result.isValid).toBeTruthy(); 
    });

    it("should return a required error when the postcode is empty", () => {
        const result = validatePostcode("");
        expect(result).toEqual({ isValid: false, error: "required" });
    });

    it.each([
        "SW1A2AA",  // invalid if validator expects normalised input
        "SW1A 22A", // inward code must begin with one digit
        "SW1A 2A",  // inward code too short
        "SW1A 2AAA",// inward code too long
        "123 4AB",  // outward code must begin with letters
        "ABCDE"     // not a postcode
    ])("%s is an invalid postcode", (postcode) => {
        const result = validatePostcode(postcode);
        expect(result).toEqual({ isValid: false, error: "invalid-format" });
    });
});