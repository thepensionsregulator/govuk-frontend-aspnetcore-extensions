import { jest } from "@jest/globals";
import { AddressLookupStateMachine, type AddressLookupEvent, type AddressLookupState } from "../../Scripts/address-lookup/address-lookup-state-machine";
import type { AddressSearchCriteria, AddressSearchResult } from "../../Scripts/address-lookup/types";

describe("AddressLookupStateMachine", () => {
    const criteria: AddressSearchCriteria = { buildingName: "1", postcode: "SW1A 2AA" };
    const addressOne: AddressSearchResult = {
        UPRN: "1", UDPRN: "1", ADDRESS: "1 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA"
    };
    const addressTwo: AddressSearchResult = {
        UPRN: "2", UDPRN: "2", ADDRESS: "2 High Street", POST_TOWN: "London", POSTCODE: "SW1A 2AA"
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

        expect(stateMachine.getState()).toEqual({ status: "confirmed", address: addressOne });
    });

    it("should move to the results state with an empty list when no addresses are found", () => {
        const stateMachine = new AddressLookupStateMachine();

        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [] });

        // Current behaviour: falls into the "more than one" branch rather than a distinct empty-results outcome.
        expect(stateMachine.getState()).toEqual({ status: "results", criteria, addresses: [] });
    });

    it("should not populate criteria when search-succeeded is dispatched without it", () => {
        const stateMachine = new AddressLookupStateMachine();

        stateMachine.dispatch({ status: "search-succeeded", addresses: [addressOne, addressTwo] });

        // Current behaviour: the non-null assertion in reduce() lets criteria through as undefined at runtime,
        // even though AddressLookupResultState declares criteria as required.
        expect(stateMachine.getState()).toEqual({ status: "results", criteria: undefined, addresses: [addressOne, addressTwo] });
    });

    it("should move to the confirmed state when an address is selected", () => {
        const stateMachine = new AddressLookupStateMachine();
        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne, addressTwo] });

        stateMachine.dispatch({ status: "address-selected", criteria, address: addressTwo });

        expect(stateMachine.getState()).toEqual({ status: "confirmed", address: addressTwo });
    });

    it("should currently allow address-selected to be dispatched from the search state", () => {
        const stateMachine = new AddressLookupStateMachine();

        // Current behaviour: there is no guard preventing this transition from an unexpected state.
        stateMachine.dispatch({ status: "address-selected", criteria, address: addressOne });

        expect(stateMachine.getState()).toEqual({ status: "confirmed", address: addressOne });
    });

    it("should move back to the search state with the given criteria", () => {
        const stateMachine = new AddressLookupStateMachine();
        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne, addressTwo] });

        stateMachine.dispatch({ status: "back-to-search-requested", criteria });

        expect(stateMachine.getState()).toEqual({ status: "search", criteria });
    });

    it("should currently allow back-to-search-requested to be dispatched from the search state", () => {
        const stateMachine = new AddressLookupStateMachine();

        // Current behaviour: there is no guard preventing this transition from an unexpected state.
        stateMachine.dispatch({ status: "back-to-search-requested", criteria });

        expect(stateMachine.getState()).toEqual({ status: "search", criteria });
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

    it("should still notify listeners for an unrecognised event, leaving state unchanged", () => {
        const stateMachine = new AddressLookupStateMachine();
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(listener);
        const unrecognisedEvent = { status: "unrecognised" } as unknown as AddressLookupEvent;

        // Current behaviour: reduce()'s default branch returns the same state rather than undefined,
        // so dispatch still transitions and notifies listeners for a no-op event.
        stateMachine.dispatch(unrecognisedEvent);

        expect(listener).toHaveBeenCalledWith({ status: "search" });
    });
});
