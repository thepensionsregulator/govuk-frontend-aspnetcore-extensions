export function sanitisePostcode(postcode: string): string {
    let sanitised = "";
    for (const char of postcode.trim().toUpperCase()) {
        const isLetter = char >= "A" && char <= "Z";
        const isDigit = char >= "0" && char <= "9";
        const isSpace = char === " ";
        
        if (isLetter || isDigit || isSpace) {
            sanitised += char;
        }
    }

    return sanitised;
}