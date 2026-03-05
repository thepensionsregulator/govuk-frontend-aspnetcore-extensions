class AddressLookupApiService {
    constructor(searchEndpoint, idEndpoint) {
        this.searchEndpoint = searchEndpoint;
        this.idEndpoint = idEndpoint;
        this.exampleMode = searchEndpoint.endsWith('.json');
    }

    async searchAddresses(postcode, building) {
        const url = new URL(this.searchEndpoint, window.location.origin);
        if (!this.exampleMode) {
            //TODO: the postcode must be 'AA11 1AA' but GDS says we must accept 'AA11-1AA' so we will need to sanitise the postcode
            url.searchParams.set('postcode', postcode);
        }

        const response = await fetch(url, {
            headers: { 'Content-Type': 'application/json' }
        });

        const results = await response.json();
        let filteredResults = results.results.map(result => result.DPA);

        if (building && building !== '') {
            building = building.trim();
            if (Number.isInteger(Number(building))) {
                filteredResults = filteredResults.filter(result => result.BUILDING_NUMBER === building);
            } else {
                filteredResults = filteredResults.filter(result => result.BUILDING_NAME?.toLowerCase().includes(building.toLowerCase()));
            }
        }

        return filteredResults;
    }

    async getAddressById(id) {
        if (this.exampleMode) {
            const response = await fetch(this.idEndpoint, {
                headers: { 'Content-Type': 'application/json' }
            });
            const addresses = await response.json();
            return addresses.results.map(result => result.DPA).find(address => address.UPRN === id);
        }

        const url = new URL(this.idEndpoint, window.location.origin);
        url.searchParams.set('uprn', id);

        const response = await fetch(url, {
            headers: { 'Content-Type': 'application/json' }
        });

        const addresses = await response.json();

        return addresses.results.map(result => result.DPA);
    }
}

export { AddressLookupApiService };