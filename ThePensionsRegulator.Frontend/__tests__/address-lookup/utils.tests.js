import { jest } from '@jest/globals';
import { toTitleCase } from "../../wwwroot/tpr/address-lookup/utils.js";


describe("toTitleCase", () => {
    it.each([
        ["LONDON", "London"],
        ["NEW YORK", "New York"],
        ["123 HIGH STREET", "123 High Street"],
        ["BOURNEMOUTH", "Bournemouth"],
        ["ST. ALBANS", "St. Albans"],
        ["hello world", "Hello World"],
        ["Already Title Case", "Already Title Case"],
        ["a", "A"],
    ])("converts '%s' to '%s'", (input, expected) => {
        expect(toTitleCase(input)).toBe(expected);
    });

    it.each([
        [null, null],
        [undefined, undefined],
        ["", ""],
    ])("returns %s unchanged", (input, expected) => {
        expect(toTitleCase(input)).toBe(expected);
    });
});