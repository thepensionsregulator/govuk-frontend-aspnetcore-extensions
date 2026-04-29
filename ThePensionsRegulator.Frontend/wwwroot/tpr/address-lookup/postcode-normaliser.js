class PostcodeNormaliser {
    normalise(postcode) {
        postcode = postcode.toUpperCase().replace(/\s/g, '');

        if (postcode.length >= 5 && postcode.length < 10) {
            if (postcode.substring(postcode.length - 4, postcode.length - 3) !== " ") {
                const lastTwo = postcode.substring(postcode.length - 2);

                if (/[A-Z][A-Z]/.test(lastTwo)) {
                    const thirdFromLast = postcode.substring(postcode.length - 3, postcode.length - 2);

                    if (/[0-9]/.test(thirdFromLast)) {
                        return postcode.slice(0, postcode.length - 3) + " " + postcode.slice(postcode.length - 3);
                    }
                }
            }
        }

        return postcode;
    }
}

export { PostcodeNormaliser };