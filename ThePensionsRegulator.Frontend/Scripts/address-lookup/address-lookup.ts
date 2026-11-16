import type { AddressLookupInstance, AddressLookupOptions } from './types';
import { FetchAddressSearchService } from './address-search-service.js';
import { renderAddressSearch } from './address-search.js';
import { AddressLookupState, AddressLookupStateMachine } from './address-lookup-state-machine.js';
import { renderAddressResults } from './address-results.js';
import { renderAddressUkManualEntry } from './address-uk-manual-entry.js';
import { renderConfirmedAddress } from './address-confirmed.js';
import { renderAddressInternationalManualEntry } from './address-international-manual-entry.js';
import { clearFormGroupError, showFormGroupError } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/validation.js';

export function createAddressLookup(options: AddressLookupOptions) : AddressLookupInstance {
    const container = document.createElement("div");
    const content = options.content;
    const fields = options.fields;
    const addressSearchService = new FetchAddressSearchService({
        searchEndpoint: options.searchEndpoint,
    });
    const ukCountryId = options.countries?.find(x => x.name === "United Kingdom" || x.name === "UK")?.value;
    const stateMachine = new AddressLookupStateMachine(content, ukCountryId, options.initialAddress);
    stateMachine.subscribe(render);
    render(stateMachine.getState());

    return {
        element: container,
        isConfirmed: () => stateMachine.getState().status === "confirmed",
        showNotConfirmedError: () => {
            const state = stateMachine.getState();
            if (state.status === "confirmed") { return; }

            const errorAnchor = container.querySelector<HTMLElement>("[data-address-lookup-submit-error-anchor]");

            if (!errorAnchor) { return; }

            showFormGroupError(errorAnchor, getNotConfirmedErrorMessage(state))
        }
    };

    
    function render(state: AddressLookupState): void {
        const previousErrorAnchor = container.querySelector<HTMLElement>("[data-address-lookup-submit-error-anchor]");
        if (previousErrorAnchor) {
            clearFormGroupError(previousErrorAnchor);
        }

        container.replaceChildren();
        switch(state.status){
            case "search":
                const searchView = renderAddressSearch({
                    searchService: addressSearchService, 
                    criteria: state.criteria,
                    content,
                    onSearchSuccess: (results, criteria) => stateMachine.dispatch({ status: "search-succeeded", addresses: results, criteria: criteria }),
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
                    content,
                    onAddressSelected: (address) => stateMachine.dispatch({ status: "address-selected", address }),
                    onBackToSearchRequested: () => stateMachine.dispatch({ status: "back-to-search-requested" }),
                    onUkManualEntryRequested: () => stateMachine.dispatch({ status: "uk-manual-entry-requested" })
                });
                container.appendChild(resultsView);
                resultsView.querySelector<HTMLSelectElement>("#address-select")?.focus();
                break;
            case "confirmed":
                const confirmedView = renderConfirmedAddress({
                    address: state.address, 
                    fields,
                    content,
                    onBackToSearchRequested: () => stateMachine.dispatch({ status: "back-to-search-requested" })
                });
                container.appendChild(confirmedView);
                const confirmedParagraph = confirmedView.querySelector<HTMLParagraphElement>("p");
                confirmedParagraph?.focus();
                break;
            
            case "uk-manual-entry":
                const ukManualEntryView = renderAddressUkManualEntry({
                    content,
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
                    countries: options.countries || [],
                    content,
                });
                container.appendChild(manualEntryView);
                manualEntryView.querySelector<HTMLInputElement>("input")?.focus()
                break;
        }
    }

    function getNotConfirmedErrorMessage(state: AddressLookupState): string {
        switch (state.status){
            case "search":
                return content.searchNotConfirmedErrorMessage;
            case "results":
                return content.resultsNotConfirmedErrorMessage;
            case "uk-manual-entry":
                return content.ukManualEntryNotConfirmedErrorMessage;
            case "international-manual-entry":
                return content.internationalManualEntryNotConfirmedErrorMessage;
            case "confirmed":
                throw new Error("A confirmed address does not need a submission error");
        }
    }
}

