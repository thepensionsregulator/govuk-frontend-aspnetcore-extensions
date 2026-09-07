import type { AddressLookupOptions } from './types';
import { FetchAddressSearchService } from './address-search-service.js';
import { renderAddressSearch } from './address-search.js';
import { AddressLookupState, AddressLookupStateMachine } from './address-lookup-state-machine.js';
import { renderAddressResults } from './address-results.js';

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
        container.replaceChildren();
        switch(state.status){
            case "search":
                container.appendChild(renderAddressSearch({
                    searchService: addressSearchService, 
                    onSearchSuccess: (results, criteria) => stateMachine.dispatch({ status: "search-succeeded", addresses: results, criteria: criteria }),
                    criteria: state.criteria
                }));
                break;
            case "results":
                container.appendChild(renderAddressResults({
                    addresses: state.addresses,
                    criteria: state.criteria,
                    onAddressSelected: (address) => stateMachine.dispatch({ status: "address-selected", address }),
                    onBackToSearchRequested: () => stateMachine.dispatch({ status: "back-to-search-requested" })
                }));
                break;
            case "confirmed":
                console.log("Address confirmed:", state.address);
                break;
        }
    }
}