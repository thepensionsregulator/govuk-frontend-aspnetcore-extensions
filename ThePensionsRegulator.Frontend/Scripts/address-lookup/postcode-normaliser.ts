export function normalisePostcode(postcode: string): string {
    const compactPostcode = postcode.split(" ").join("").toUpperCase();
    
    if (compactPostcode.length <= 3) {
        return compactPostcode;
    }

    return `${compactPostcode.slice(0, -3)} ${compactPostcode.slice(-3)}`;
}