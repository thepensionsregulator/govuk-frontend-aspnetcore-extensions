class PostcodeSanitiser {
    sanitise(postcode) {
        if (!postcode) {
            return "";
        }

        let sanitised = postcode.trim();
        sanitised = sanitised.toUpperCase();
        sanitised = sanitised.replace('-', '');
        return sanitised;
    }
}

export { PostcodeSanitiser };