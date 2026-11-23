import type { AddressLookupInstance, AddressLookupOptions } from './types';
import { FetchAddressSearchService } from './address-search-service.js';
import { renderAddressSearch } from './address-search.js';
import { AddressLookupState, AddressLookupStateMachine } from './address-lookup-state-machine.js';
import { renderAddressResults } from './address-results.js';
import { renderAddressUkManualEntry } from './address-uk-manual-entry.js';
import { renderConfirmedAddress } from './address-confirmed.js';
import { renderAddressInternationalManualEntry } from './address-international-manual-entry.js';
import { clearFormGroupError, showFormGroupError } from '/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/validation.js';
import { renderSameAsCheckbox } from './address-same-as-checkbox.js';
import { confirmedAddressesAreEqual } from './address-result-mapper.js';

export function createAddressLookup(options: AddressLookupOptions) : AddressLookupInstance {
    const componentContainer = document.createElement("div");
    const checkboxContainer = document.createElement("div");
    const stateContainer = document.createElement("div");
    const content = options.content;
    const fields = options.fields;
    const addressSearchService = new FetchAddressSearchService({
        searchEndpoint: options.settings.searchEndpoint,
    });
    const ukCountryId = options.countries?.find(x => x.name === "United Kingdom" || x.name === "UK")?.value;
    const stateMachine = new AddressLookupStateMachine(options.content, ukCountryId, options.initialAddress);
    const primaryStateMachine = options.settings.role === "Secondary" ? options.stateMachine : undefined;
    let sameAsInput: HTMLInputElement | null = null;

    if (primaryStateMachine){
        let sameAsChecked = false;
        const primaryStateMachineState = primaryStateMachine.getState();
        const secondaryStateMachineState = stateMachine.getState();
        if (primaryStateMachineState.status === "confirmed" && secondaryStateMachineState.status === "confirmed") {
            if (confirmedAddressesAreEqual(primaryStateMachineState.address, secondaryStateMachineState.address)) {
                stateMachine.dispatch({ status: "same-as-checked", address: primaryStateMachineState.address });
                sameAsChecked = true;
            }
        }
        const sameAsCheckbox = renderSameAsCheckbox(options.content, sameAsChecked, () => {
            if (!isSameAsChecked()){
                stateMachine.dispatch({ status: "same-as-unchecked" });
                return;
            }

            const primaryState = primaryStateMachine.getState();
            stateMachine.dispatch({ status: "same-as-checked", address: primaryState.status === "confirmed" ? primaryState.address : undefined });
        });

        sameAsInput = sameAsCheckbox.querySelector<HTMLInputElement>("input[type='checkbox']");
        componentContainer.appendChild(checkboxContainer);
        checkboxContainer.appendChild(sameAsCheckbox);
        primaryStateMachine?.subscribe(primaryState => stateMachine.dispatch({ status: "primary-address-state-change", address: primaryState.status === "confirmed" ? primaryState.address : undefined }));

    }
    componentContainer.appendChild(stateContainer);

    stateMachine.subscribe(render);
    render(stateMachine.getState());

    return {
        element: componentContainer,
        stateMachine: stateMachine,
        isConfirmed: () => stateMachine.getState().status === "confirmed",
        showNotConfirmedError: () => {
            const state = stateMachine.getState();
            if (state.status === "confirmed") { return; }

            const errorAnchor = stateContainer.querySelector<HTMLElement>("[data-address-lookup-submit-error-anchor]");

            if (!errorAnchor) { return; }

            showFormGroupError(errorAnchor, getNotConfirmedErrorMessage(state))
        }
    };

    function isSameAsChecked(): boolean {
        return sameAsInput?.checked ?? false;
    }

    function clearSubmitError(): void {
        const previousErrorAnchor = stateContainer.querySelector<HTMLElement>("[data-address-lookup-submit-error-anchor]");
        if (previousErrorAnchor) {
            clearFormGroupError(previousErrorAnchor);
        }
    }

    function render(state: AddressLookupState): void {
        clearSubmitError();
        stateContainer.replaceChildren();
        switch(state.status){
            case "search":
                const searchView = renderAddressSearch({
                    searchService: addressSearchService,
                    criteria: state.criteria,
                    content,
                    onSearchSuccess: (results, criteria) => stateMachine.dispatch({ status: "search-succeeded", addresses: results, criteria: criteria }),
                    onInternationalEntryRequested: () => stateMachine.dispatch({ status: "international-manual-entry-requested" })
                });
                stateContainer.appendChild(searchView);
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
                stateContainer.appendChild(resultsView);
                resultsView.querySelector<HTMLSelectElement>("#address-select")?.focus();
                break;
            case "confirmed":
                const confirmedView = renderConfirmedAddress({
                    address: state.address,
                    fields,
                    content,
                    onBackToSearchRequested: state.sameAsPrimary ? undefined : () => stateMachine.dispatch({ status: "back-to-search-requested" })
                });
                stateContainer.appendChild(confirmedView);
                const confirmedParagraph = confirmedView.querySelector<HTMLParagraphElement>("p");
                confirmedParagraph?.focus();
                break;

            case "uk-manual-entry":
                const ukManualEntryView = renderAddressUkManualEntry({
                    content,
                    onAddressSubmitted: (address) => stateMachine.dispatch({ status: "uk-manual-address-submitted", address}),
                    onBackToSearchRequested: () => stateMachine.dispatch({ status: "back-to-search-requested" })
                });
                stateContainer.appendChild(ukManualEntryView);
                ukManualEntryView.querySelector<HTMLInputElement>("input")?.focus();
                break;
            case "international-manual-entry":
                const manualEntryView = renderAddressInternationalManualEntry({
                    content,
                    onAddressSubmitted: (address) => stateMachine.dispatch({ status: "international-manual-address-submitted", address}),
                    onBackToSearchRequested: () => stateMachine.dispatch({ status: "back-to-search-requested" }),
                    countries: options.countries || [],
                });
                stateContainer.appendChild(manualEntryView);
                manualEntryView.querySelector<HTMLInputElement>("input")?.focus()
                break;
        }
    }

    function getNotConfirmedErrorMessage(state: AddressLookupState): string {
        switch (state.status){
            case "search":
                return content.i18n.searchNotConfirmedErrorMessage;
            case "results":
                return content.i18n.resultsNotConfirmedErrorMessage;
            case "uk-manual-entry":
                return content.i18n.ukManualEntryNotConfirmedErrorMessage;
            case "international-manual-entry":
                return content.i18n.internationalManualEntryNotConfirmedErrorMessage;
            case "confirmed":
                throw new Error("A confirmed address does not need a submission error");
        }
    }
}
