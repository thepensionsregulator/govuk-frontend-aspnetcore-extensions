if (typeof(trustedTypes) != undefined && trustedTypes.createPolicy && typeof (DOMPurify) !== 'undefined') {
    const trustedURLs = [];
    trustedTypes.createPolicy('default', {
        createHTML: (html) => DOMPurify.sanitize(html, { RETURN_TRUSTED_TYPE: true }),
        createScriptURL: function (url) {
            
            if (trustedURLs.some(x => url.startsWith(x))) {
                return url;
            };
            throw new Error(`Untrusted script URL: ${url}`);
        },
        createScript: js => DOMPurify.sanitize(js, { RETURN_TRUSTED_TYPE: true })
    });
}