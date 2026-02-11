class AddressLookupApiService {
    constructor(searchEndpoint, idEndpoint) {
        this.searchEndpoint = searchEndpoint;
        this.idEndpoint = idEndpoint;
    }

    async searchAddresses(postcode, building) {
        const response = await fetch(this.searchEndpoint, {
            headers: { 'Content-Type': 'application/json' }
        });
        return response.json();
    }

    async getAddressById(id) {
        const response = await fetch(this.idEndpoint, {
            headers: { 'Content-Type': 'application/json' }
        });

        return response.json();
    }
}

export { AddressLookupApiService };