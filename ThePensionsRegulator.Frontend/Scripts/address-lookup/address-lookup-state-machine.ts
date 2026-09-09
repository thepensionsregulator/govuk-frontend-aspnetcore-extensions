import { mapAddressSearchResult, mapInternationalManualEntryAddress, mapUkManualEntryAddress } from './address-result-mapper.js';
import type { AddressSearchCriteria, AddressSearchResult, ConfirmedAddress, InternationalManualEntryAddress, UkManualEntryAddress } from './types';

export type AddressLookupState = 
    | AddressLookupSearchState
    | AddressLookupResultState
    | AddressLookupConfirmedState
    | AddressLookupUkManualEntryState
    | AddressLookupInternationalManualEntryState

export interface AddressLookupSearchState {
    status: "search";
    criteria?: AddressSearchCriteria;
}

export interface AddressLookupResultState {
    status: "results";
    criteria: AddressSearchCriteria;
    addresses: AddressSearchResult[];
}

export interface AddressLookupConfirmedState {
    status: "confirmed";
    address: ConfirmedAddress;
}

export interface AddressLookupUkManualEntryState{
    status: "uk-manual-entry";
}

export interface AddressLookupInternationalManualEntryState {
    status: "international-manual-entry";
}

export type AddressLookupEvent = 
    | { status: "search-succeeded", criteria: AddressSearchCriteria, addresses: AddressSearchResult[] }
    | { status: "address-selected", address: AddressSearchResult }
    | { status: "back-to-search-requested" }
    | { status: "uk-manual-entry-requested" }
    | { status: "international-manual-entry-requested" }
    | { status: "uk-manual-address-submitted", address: UkManualEntryAddress }
    | { status: "international-manual-address-submitted", address: InternationalManualEntryAddress }

export type AddressLookupStateListener = (state: AddressLookupState) => void;

export class AddressLookupStateMachine {
    private state: AddressLookupState = { status: "search" };
    private readonly listeners: AddressLookupStateListener[] = [];

    public getState(): AddressLookupState {
        return this.state;
    }

    public subscribe(listener: AddressLookupStateListener): void {
        this.listeners.push(listener);
    }

    public dispatch(event: AddressLookupEvent): void {
        const nextState = this.reduce(this.state, event);

        if (nextState) {
            this.transition(nextState);
        }
    }

    private reduce(state: AddressLookupState, event: AddressLookupEvent): AddressLookupState | undefined {
        switch (event.status) {
            case "search-succeeded":
                if (state.status !== "search") {
                    return undefined;
                }
                if (event.addresses.length === 1) {
                    const confirmedAddress = mapAddressSearchResult(event.addresses[0]);
                    return { status: "confirmed", address: confirmedAddress };
                } else {
                    return { status: "results", criteria: event.criteria, addresses: event.addresses };
                }
            
            case "address-selected":
                if (state.status !== "results") {
                    return undefined;
                }
                const confirmedAddress = mapAddressSearchResult(event.address);
                return { status: "confirmed", address: confirmedAddress };
            
            case "back-to-search-requested":
                if (state.status === "search") {
                    return undefined;
                }
                return { status: "search", criteria: state.status === "results" ? state.criteria : undefined };
            case "uk-manual-entry-requested":
                return { status: "uk-manual-entry" }
            case "international-manual-entry-requested":
                return { status: "international-manual-entry" };
            case "uk-manual-address-submitted":
                if (state.status !== "uk-manual-entry") {
                    return undefined;
                }
                const confirmedUkAddress = mapUkManualEntryAddress(event.address);
                return { status: "confirmed", address: confirmedUkAddress };
            case "international-manual-entry-requested":
                return { status: "international-manual-entry" };
            case "international-manual-address-submitted":
                if (state.status !== "international-manual-entry") {
                    return undefined;
                }
                const confirmedInternationalAddress = mapInternationalManualEntryAddress(event.address);
                return { status: "confirmed", address: confirmedInternationalAddress };
            default:
                return undefined;
        }
    }

    private transition(nextState: AddressLookupState): void {
        this.state = nextState;
        this.listeners.forEach(listener => listener(this.state));
    }
}
