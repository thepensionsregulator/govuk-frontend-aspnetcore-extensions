class AddressMapper {
    mapFromApiResult(apiResults) {
        return {
            addressLine1: apiResults.addressLine1,
            addressLine2: apiResults.addressLine2,
            postTown: apiResults.town,
            county: apiResults.county,
            postcode: apiResults.postCode
        };
    }

    mapFromManualUKEntry(addressLine1, addressLine2, town, county, postcode) {
        return {
            addressLine1: addressLine1,
            addressLine2: addressLine2 || '',
            postTown: town,
            county: county || '',
            postcode: postcode
        };
    }

    mapFromManualInternationalEntry(addressLine1, addressLine2, town, region, country, postcode) {
        return {
            addressLine1: addressLine1,
            addressLine2: addressLine2 || '',
            postTown: town,
            county: region || '',
            country: country,
            postcode: postcode
        };
    }
}


export { AddressMapper };