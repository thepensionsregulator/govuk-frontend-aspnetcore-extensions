function toSentenceCase(text) {
    return text.toLowerCase().replace(/\b\w/g, (char) => char.toUpperCase());
}

export { toSentenceCase };