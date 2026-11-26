import { sanitisePostcode } from "../../Scripts/address-lookup/postcode-sanitiser";

describe("sanitisePostcode", () => {
    it.each([
        ["", ""],
        ["  @#!  ", ""],
        ["sw1a\t2aa\n", "SW1A2AA"]
    ])("sanitises boundary input %j to %j", (input, expected) => {
        expect(sanitisePostcode(input)).toBe(expected);
    });

    it("should trim whitespace from the start and end of the postcode", () => {
        const input = "  AB1 2CD  ";
        const expectedOutput = "AB1 2CD";
        
        expect(sanitisePostcode(input)).toBe(expectedOutput);
    });

    it("should convert the postcode to uppercase", () => {
        const input = "ab1 2cd";
        const expectedOutput = "AB1 2CD";
   
        expect(sanitisePostcode(input)).toBe(expectedOutput);
    });

    it("should remove any hyphens from the postcode", () => {
        const input = "AB1-2CD";
        const expectedOutput = "AB12CD";
       
        expect(sanitisePostcode(input)).toBe(expectedOutput);
    });

    it("should remove any special characters from the postcode", () => {
        const input = "AB1@2#CD!";
        const expectedOutput = "AB12CD";

        expect(sanitisePostcode(input)).toBe(expectedOutput);
    });
});