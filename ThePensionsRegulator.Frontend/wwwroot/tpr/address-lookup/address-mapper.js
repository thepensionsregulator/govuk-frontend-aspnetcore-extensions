class AddressMapper {
    mapFromDPA(DPA) {
        // This is going to be more complicated...
    }

    mapFromManualUKEntry(addressLine1, addressLine2, town, county, postcode) {
        return new Address(addressLine1, addressLine2, town, county, postcode);
    }

    mapFromManualInternationalEntry(addressLine1, addressLine2, town, region, country, postcode) {
        return new Address(addressLine1, addressLine2, town, region, country, postcode);
    }
}

class Address {
    constructor(addressLine1, addressLine2, townOrCity, county, postcode) {
        this.type = "UK";
        this.addressLine1 = addressLine1;
        this.addressLine2 = addressLine2;
        this.townOrCity = townOrCity;
        this.county = county;
    }

    constructor(addressLine1, addressLine2, townOrCity, region, country, postcode) {
        this.type = "International";
        this.addressLine1 = addressLine1;
        this.addressLine2 = addressLine2;
        this.townOrCity = townOrCity;
        this.county = region;
        this.country = country;
        this.postcode = postcode;
    }

}

export { AddressMapper };