import type { AddressLookupOptions } from './types';
import { FetchAddressSearchService } from './address-search-service.js';
import { renderAddressSearch } from './address-search.js';
import { AddressLookupState, AddressLookupStateMachine } from './address-lookup-state-machine.js';

export function createAddressLookup(options: AddressLookupOptions){
    const container = document.createElement("div");
    const addressSearchService = new FetchAddressSearchService({
        searchEndpoint: options.searchEndpoint,
        addressByIdEndpoint: options.addressByIdEndpoint
    });

    const stateMachine = new AddressLookupStateMachine();
    stateMachine.subscribe(render);
    render(stateMachine.getState());

    return container;
    
    function render(state: AddressLookupState): void {
        switch(state.status){
            case "search":
                container.appendChild(renderAddressSearch({
                    searchService: addressSearchService, 
                    onSearchSuccess: (results, criteria) => stateMachine.dispatch({ status: "search-succeeded", addresses: results, criteria: criteria })
                }));
                break;
            case "results":
                console.log("transiting to results");
                break;
        }
    }
}