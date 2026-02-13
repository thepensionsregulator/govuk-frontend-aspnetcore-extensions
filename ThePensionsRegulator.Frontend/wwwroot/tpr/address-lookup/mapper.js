class AddressMapper {
    mapFromApiResult(apiResults) {
        return {
            organisationName: apiResults.organisationName,
            addressLine1: apiResults.addressLine1,
            addressLine2: apiResults.addressLine2,
            town: apiResults.town,
            county: apiResults.county,
            postcode: apiResults.postCode
        };
    }

    mapFromManualUKEntry(addressLine1, addressLine2, town, county, postcode) {
        return {
            addressLine1: addressLine1,
            addressLine2: addressLine2 || '',
            town: town,
            county: county || '',
            postcode: postcode
        };
    }

    mapFromManualInternationalEntry(addressLine1, addressLine2, town, region, country, postcode) {
        return {
            addressLine1: addressLine1,
            addressLine2: addressLine2 || '',
            town: town,
            county: region || '',
            country: country,
            postcode: postcode
        };
    }
}


export { AddressMapper };