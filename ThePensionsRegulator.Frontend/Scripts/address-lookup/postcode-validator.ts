type PostcodeValidationResult = 
    | { isValid: true; } 
    | { isValid: false; error: "required" | "invalid-format" };

export function validatePostcode(postcode: string): PostcodeValidationResult {
    if (postcode.length === 0) {
        return { isValid: false, error: "required" };
    }

    if (isSpecialPostcode(postcode) || isValidStandardPostcode(postcode)) {
        return { isValid: true };
    }

    return { isValid: false, error: "invalid-format" };
}

function isSpecialPostcode(postcode: string): boolean {
    return postcode === "GIR 0AA";
}

function isValidStandardPostcode(postcode: string): boolean {
    const [outwardCode, inwardCode] = postcode.split(" ");

    if (!outwardCode || !inwardCode || postcode.split(" ").length !== 2) {
        return false;
    }

    return isValidOutwardCode(outwardCode) && isValidInwardCode(inwardCode);
}

function isValidOutwardCode(outwardCode: string): boolean {
    const firstCharacter = outwardCode[0];
    const firstCharacterIsValid = firstCharacter >= "A" && firstCharacter <= "Z"
        && firstCharacter !== "Q"
        && firstCharacter !== "V"
        && firstCharacter !== "X";

    if (!firstCharacterIsValid) {
        return false;
    }

    const remainder = outwardCode.slice(1);
    const isDigit = (character: string) => character >= "0" && character <= "9";
    const isLetter = (character: string) => character >= "A" && character <= "Z";

    return (
        (remainder.length === 1 && isDigit(remainder[0])) ||
        (remainder.length === 2 && isDigit(remainder[0]) && isDigit(remainder[1])) ||
        (remainder.length === 2 && isLetter(remainder[0]) && isDigit(remainder[1])) ||
        (remainder.length === 3 && isLetter(remainder[0]) && isDigit(remainder[1]) && isDigit(remainder[2])) ||
        (remainder.length === 3 && isLetter(remainder[0]) && isDigit(remainder[1]) && isLetter(remainder[2])) ||
        (remainder.length === 2 && isDigit(remainder[0]) && isLetter(remainder[1]))
    );
}

function isValidInwardCode(inwardCode: string): boolean {
    if (inwardCode.length !== 3) {
        return false;
    }

    const isDigit = (character: string) => character >= "0" && character <= "9";
    const disallowedLetters = "CIKMOV";
    const isLetter = (character: string) => character >= "A" && character <= "Z" && !disallowedLetters.includes(character);

    return isDigit(inwardCode[0]) && isLetter(inwardCode[1]) && isLetter(inwardCode[2]);
}
