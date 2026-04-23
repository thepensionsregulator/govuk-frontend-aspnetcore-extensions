export function disableLinks(html: string | undefined): string | undefined {
    if (!html) return html;

    // Replace <a ...href="...">...</a> with <span class="govuk-link" ...>...</span> (removes href and changes tag)
    // so that links in backoffice previews are not clickable but still styled as links.
    return html
        .replace(
            /<a\b([^>]*)\bhref\s*=\s*(['"])[^'"]*\2([^>]*)>/gi,
            (_match, preAttrs, _quote, postAttrs) => {
                // Ensure class="govuk-link" is present
                let attrs = `${preAttrs}${postAttrs}`;
                if (/class\s*=/.test(attrs)) {
                    // Add govuk-link to existing class attribute
                    attrs = attrs.replace(/class\s*=\s*(['"])([^'"]*)\1/, (_match, quote, classes) =>
                        `class=${quote}${classes} govuk-link${quote}`
                    );
                } else {
                    attrs += ' class="govuk-link"';
                }
                return `<span${attrs}>`;
            }
        )
        .replace(/<\/a>/gi, '</span>');
}