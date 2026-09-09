import { jest } from "@jest/globals";
import { AddressLookupStateMachine, type AddressLookupEvent, type AddressLookupState } from "../../Scripts/address-lookup/address-lookup-state-machine";
import { mapAddressSearchResult } from "../../Scripts/address-lookup/address-result-mapper";
import type { AddressSearchCriteria, AddressSearchResult, UkManualEntryAddress } from "../../Scripts/address-lookup/types";

describe("AddressLookupStateMachine", () => {
    const criteria: AddressSearchCriteria = { buildingName: "1", postcode: "SW1A 2AA" };
    const addressOne: AddressSearchResult = {
        UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA"
    };
    const addressTwo: AddressSearchResult = {
        UPRN: "2", UDPRN: "2", ADDRESS: "2 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA"
    };
    const manualUkAddress: UkManualEntryAddress = {
        addressLine1: "Flat 2",
        addressLine2: "1 High Street",
        postTown: "London",
        county: "Greater London",
        postcode: "SW1A 1AA"
    };

    it("should start in the search state", () => {
        const stateMachine = new AddressLookupStateMachine();

        expect(stateMachine.getState()).toEqual({ status: "search" });
    });

    it("should move to the results state when more than one address is found", () => {
        const stateMachine = new AddressLookupStateMachine();

        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne, addressTwo] });

        expect(stateMachine.getState()).toEqual({ status: "results", criteria, addresses: [addressOne, addressTwo] });
    });

    it("should move directly to the confirmed state when exactly one address is found", () => {
        const stateMachine = new AddressLookupStateMachine();

        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne] });

        expect(stateMachine.getState()).toEqual({ status: "confirmed", address: mapAddressSearchResult(addressOne) });
    });

    it("should reject search-succeeded when not in the search state", () => {
        const stateMachine = new AddressLookupStateMachine();
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(listener);
        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne, addressTwo] });
        listener.mockClear();

        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne] });

        expect(stateMachine.getState()).toEqual({ status: "results", criteria, addresses: [addressOne, addressTwo] });
        expect(listener).not.toHaveBeenCalled();
    });

    it("should move to the results state with an empty list when no addresses are found", () => {
        const stateMachine = new AddressLookupStateMachine();

        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [] });

        // Current behaviour: falls into the "more than one" branch rather than a distinct empty-results outcome.
        expect(stateMachine.getState()).toEqual({ status: "results", criteria, addresses: [] });
    });

    it("should move to the confirmed state when an address is selected", () => {
        const stateMachine = new AddressLookupStateMachine();
        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne, addressTwo] });

        stateMachine.dispatch({ status: "address-selected", address: addressTwo });

        expect(stateMachine.getState()).toEqual({ status: "confirmed", address: mapAddressSearchResult(addressTwo) });
    });

    it("should map a submitted UK manual address and move to the confirmed state", () => {
        const stateMachine = new AddressLookupStateMachine();
        stateMachine.dispatch({ status: "uk-manual-entry-requested" });

        stateMachine.dispatch({ status: "uk-manual-address-submitted", address: manualUkAddress });

        expect(stateMachine.getState()).toEqual({
            status: "confirmed",
            address: {
                addressLine1: "Flat 2",
                addressLine2: "1 High Street",
                postTown: "London",
                postCounty: "Greater London",
                postcode: "SW1A 1AA"
            }
        });
    });

    it("should reject address-selected when not in the results state", () => {
        const stateMachine = new AddressLookupStateMachine();
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(listener);

        stateMachine.dispatch({ status: "address-selected", address: addressOne });

        expect(stateMachine.getState()).toEqual({ status: "search" });
        expect(listener).not.toHaveBeenCalled();
    });

    it("should move back to the search state with the given criteria", () => {
        const stateMachine = new AddressLookupStateMachine();
        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne, addressTwo] });

        stateMachine.dispatch({ status: "back-to-search-requested" });

        expect(stateMachine.getState()).toEqual({ status: "search", criteria });
    });

    it("should reject back-to-search-requested when not in the results state", () => {
        const stateMachine = new AddressLookupStateMachine();
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(listener);

        stateMachine.dispatch({ status: "back-to-search-requested" });

        expect(stateMachine.getState()).toEqual({ status: "search" });
        expect(listener).not.toHaveBeenCalled();
    });

    it("should notify a subscribed listener with the new state after a dispatch", () => {
        const stateMachine = new AddressLookupStateMachine();
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(listener);

        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne, addressTwo] });

        expect(listener).toHaveBeenCalledWith({ status: "results", criteria, addresses: [addressOne, addressTwo] });
    });

    it("should notify every subscribed listener on a single dispatch", () => {
        const stateMachine = new AddressLookupStateMachine();
        const firstListener = jest.fn<(state: AddressLookupState) => void>();
        const secondListener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(firstListener);
        stateMachine.subscribe(secondListener);

        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne] });

        expect(firstListener).toHaveBeenCalledTimes(1);
        expect(secondListener).toHaveBeenCalledTimes(1);
    });

    it("should not notify listeners for an unrecognised event", () => {
        const stateMachine = new AddressLookupStateMachine();
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(listener);
        const unrecognisedEvent = { status: "unrecognised" } as unknown as AddressLookupEvent;

        stateMachine.dispatch(unrecognisedEvent);

        expect(listener).not.toHaveBeenCalled();
    });
});
