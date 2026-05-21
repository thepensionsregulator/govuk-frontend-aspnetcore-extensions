function toTitleCase(text) {
    if (!text) return text;
    return text.toLowerCase().replace(/\b\w/g, (char) => char.toUpperCase());
}

export { toTitleCase };