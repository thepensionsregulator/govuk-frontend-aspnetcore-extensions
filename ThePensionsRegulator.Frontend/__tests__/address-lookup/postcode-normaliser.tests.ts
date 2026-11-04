import { normalisePostcode } from "../../Scripts/address-lookup/postcode-normaliser";

describe("Postcode normaliser", () => {;

    /********************************************************************************
    * "A" indicates an alphabetic character and "N" indicates a numeric character. *
    *  Format         Example Postcode                                             *
    *  AN NAA         M1 1AA                                                       *
    *  ANN NAA        M60 1NW                                                      *
    *  AAN NAA        CR2 6XH                                                      *
    *  AANN NAA       DN55 1PT                                                     *
    *  ANA NAA        W1A 1HP                                                      *
    *  AANA NAA       EC1A 1BB                                                     *
    ********************************************************************************/

    it.each([
        ["m11aa", "M1 1AA"],
        ["m601nw", "M60 1NW"],
        ["cr26 xh", "CR2 6XH"],
        ["dn551p t", "DN55 1PT"],
        ["w1a1hp", "W1A 1HP"],
        ["ec1a1bb", "EC1A 1BB"],
        ["SW1", "SW1"]
    ])("puts all letters to upper and ensures there is a single space between the outward and inward code", (userInput, expected) => {
        const result = normalisePostcode(userInput);
        expect(result).toBe(expected);
    });
});