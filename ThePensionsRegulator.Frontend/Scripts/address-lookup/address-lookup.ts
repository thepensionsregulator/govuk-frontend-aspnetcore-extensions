import type { AddressLookupOptions } from './types';
import { FetchAddressSearchService } from './address-search-service.js';
import { renderAddressSearch } from './address-search.js';
import { AddressLookupState, AddressLookupStateMachine } from './address-lookup-state-machine.js';
import { renderAddressResults } from './address-results.js';
import { renderAddressUkManualEntry } from './address-uk-manual-entry.js';
import { renderConfirmedAddress } from './address-confirmed.js';
import { renderAddressInternationalManualEntry } from './address-international-manual-entry.js';

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
                const searchView = renderAddressSearch({
                    searchService: addressSearchService, 
                    onSearchSuccess: (results, criteria) => stateMachine.dispatch({ status: "search-succeeded", addresses: results, criteria: criteria }),
                    criteria: state.criteria,
                    onInternationalEntryRequested: () => stateMachine.dispatch({ status: "international-manual-entry-requested" })
                });
                container.appendChild(searchView);
                if (state.from !== "initial"){
                    searchView.querySelector<HTMLInputElement>("#building")?.focus();
                }
                break;
            case "results":
                const resultsView = renderAddressResults({
                    addresses: state.addresses,
                    criteria: state.criteria,
                    onAddressSelected: (address) => stateMachine.dispatch({ status: "address-selected", address }),
                    onBackToSearchRequested: () => stateMachine.dispatch({ status: "back-to-search-requested" }),
                    onUkManualEntryRequested: () => stateMachine.dispatch({ status: "uk-manual-entry-requested" })
                });
                container.appendChild(resultsView);
                resultsView.querySelector<HTMLSelectElement>("#address-select")?.focus();
                break;
            case "confirmed":
                const confirmedView = renderConfirmedAddress(state.address, () => stateMachine.dispatch({ status: "back-to-search-requested" }));
                container.appendChild(confirmedView);
                const confirmedParagraph = confirmedView.querySelector<HTMLParagraphElement>("p");
                confirmedParagraph?.focus();
                break;
            case "uk-manual-entry":
                const ukManualEntryView = renderAddressUkManualEntry({
                    onAddressSubmitted: (address) => stateMachine.dispatch({ status: "uk-manual-address-submitted", address}),
                    onBackToSearchRequested: () => stateMachine.dispatch({ status: "back-to-search-requested" })
                });
                container.appendChild(ukManualEntryView);
                ukManualEntryView.querySelector<HTMLInputElement>("input")?.focus();
                break;
            case "international-manual-entry":
                const manualEntryView = renderAddressInternationalManualEntry({
                    onAddressSubmitted: (address) => stateMachine.dispatch({ status: "international-manual-address-submitted", address}),
                    onBackToSearchRequested: () => stateMachine.dispatch({ status: "back-to-search-requested" }),
                    countries: [{ value: 'FR', text: 'France' }]
                });
                container.appendChild(manualEntryView);
                manualEntryView.querySelector<HTMLInputElement>("input")?.focus()
                break;
        }
    }
}

