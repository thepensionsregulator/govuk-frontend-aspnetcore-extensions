import type { AddressSearchCriteria, AddressSearchResult } from './types';

export type AddressLookupState = 
    | AddressLookupSearchState
    | AddressLookupResultState
    | AddressLookupConfirmedState;

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
    address: AddressSearchResult;
}

export type AddressLookupEvent = 
    | { status: "search-succeeded", criteria?: AddressSearchCriteria, addresses: AddressSearchResult[] }
    | { status: "address-selected", criteria: AddressSearchCriteria, address: AddressSearchResult };
   // | { status: "confirmed", address: AddressSearchResult };

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
                if (event.addresses.length === 1) {
                    return { status: "confirmed", address: event.addresses[0] };
                } else {
                    return { status: "results", criteria: event.criteria!, addresses: event.addresses };
                }
            case "address-selected":
                return { status: "confirmed", address: event.address };
            default:
                return state;
        }
    }

    private transition(nextState: AddressLookupState): void {
        this.state = nextState;
        this.listeners.forEach(listener => listener(this.state));
    }
}
