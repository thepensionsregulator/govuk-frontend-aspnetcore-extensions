class AddressLookupApiService {
    constructor(apiEndpoint) {
        this.apiEndpoint = apiEndpoint;
    }

    async searchAddresses(postcode, building) {
        const response = await fetch(this.apiEndpoint, {
            headers: { 'Content-Type': 'application/json' }
        });
        return response.json();
    }
}

export { AddressLookupApiService };