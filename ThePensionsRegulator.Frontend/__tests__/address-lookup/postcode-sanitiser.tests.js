import '@testing-library/jest-dom';

import { jest } from '@jest/globals';
import { PostcodeSanitiser } from "../../wwwroot/tpr/address-lookup/postcode-sanitiser.js";

describe("Postcode sanitiser", () => {
    const sanitiser = new PostcodeSanitiser();

    it.each([
        ["bn1 4dw", "BN1 4DW"],
        ["bn14dw", "BN14DW"],
    ])("puts all letters to upper", (userInput, expected) => {
        const result = sanitiser.sanitise(userInput);
        expect(result).toBe(expected);

    });

    it.each([
        ["    BN1 4DW", "BN1 4DW"],
        ["BN1 4DW     ", "BN1 4DW"],
        ["   BN14DW   ", "BN14DW"]
    ])("removes leading and trailing whitespace", (userInput, expected) => {
        const result = sanitiser.sanitise(userInput);
        expect(result).toBe(expected);
    });

    it.each([
        ["BN1-4DW", "BN14DW"]
    ])("removes hyphen", (userInput, expected) => {
        const result = sanitiser.sanitise(userInput);
        expect(result).toBe(expected);
    });
});