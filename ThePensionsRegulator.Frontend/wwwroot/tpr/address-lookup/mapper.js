class AddressMapper {
    constructor(config) {
        this.config = config
    }

    mapFromInput(originalInputs) {
        const get = (attribute) => originalInputs.find(x => x.dataAddressLookup === attribute)?.value || '';
        return {
            addressLine1: get(this.config.DATA_ATTRIBUTES.ADDRESS_LINE_1),
            addressLine2: get(this.config.DATA_ATTRIBUTES.ADDRESS_LINE_2),
            town: get(this.config.DATA_ATTRIBUTES.TOWN_OR_CITY),
            county: get(this.config.DATA_ATTRIBUTES.COUNTY),
            country: get(this.config.DATA_ATTRIBUTES.COUNTRY),
            postcode: get(this.config.DATA_ATTRIBUTES.POSTCODE),
        };
    }

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