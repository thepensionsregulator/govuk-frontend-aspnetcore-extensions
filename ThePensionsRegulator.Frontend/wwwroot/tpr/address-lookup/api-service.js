class AddressLookupApiService {
    constructor(searchEndpoint, idEndpoint) {
        this.searchEndpoint = searchEndpoint;
        this.idEndpoint = idEndpoint;
        this.exampleMode = searchEndpoint.endsWith('.json');
    }

    async searchAddresses(postcode, building) {
        const url = new URL(this.searchEndpoint, window.location.origin);
        if (!this.isStaticMode) {
            url.searchParams.set('term', postcode);
            if (building) {
                //TODO: I don't think it accepts a building, need to find out what 'term' actually means
                url.searchParams.set('building', building);
            }
        }

        const response = await fetch(this.searchEndpoint, {
            headers: { 'Content-Type': 'application/json' }
        });
        return response.json();
    }

    async getAddressById(id) {

        if (this.exampleMode) {
            const response = await fetch(this.idEndpoint, {
                headers: { 'Content-Type': 'application/json' }
            });
            const addresses = await response.json();
            return addresses[id];
        }

        const url = new URL(this.idEndpoint, window.location.origin);
        url.searchParams.set('moniker', id);

        const response = await fetch(this.idEndpoint, {
            headers: { 'Content-Type': 'application/json' }
        });

        return response.json();
    }
}

export { AddressLookupApiService };