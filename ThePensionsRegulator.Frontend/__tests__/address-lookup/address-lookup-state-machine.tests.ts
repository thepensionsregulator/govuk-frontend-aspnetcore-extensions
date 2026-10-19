import { jest } from "@jest/globals";
import { AddressLookupStateMachine, type AddressLookupEvent, type AddressLookupState } from "../../Scripts/address-lookup/address-lookup-state-machine";
import { mapAddressSearchResult, mapInternationalManualEntryAddress } from "../../Scripts/address-lookup/address-result-mapper";
import type { AddressSearchCriteria, AddressSearchResult, InternationalManualEntryAddress, UkManualEntryAddress } from "../../Scripts/address-lookup/types";
import { createMockAddressLookupContent } from "../../__test-helpers__/mock-address-lookup-content";

describe("AddressLookupStateMachine", () => {
    const content = createMockAddressLookupContent();
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
    const internationalAddress: InternationalManualEntryAddress = {
        addressLine1: "Flat 2",
        addressLine2: "1 High Street",
        postTown: "London",
        countryId: "1",
        postcode: "SW1A 1AA"
    };

    it("should start in the search state", () => {
        const stateMachine = new AddressLookupStateMachine(content);

        expect(stateMachine.getState()).toEqual({ status: "search" });
    });

    it("should move to the results state when more than one address is found", () => {
        const stateMachine = new AddressLookupStateMachine(content);

        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne, addressTwo] });

        expect(stateMachine.getState()).toEqual({ status: "results", criteria, addresses: [addressOne, addressTwo] });
    });

    it("should move directly to the confirmed state when exactly one address is found", () => {
        const stateMachine = new AddressLookupStateMachine(content);

        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne] });

        expect(stateMachine.getState()).toEqual({ status: "confirmed", address: mapAddressSearchResult(addressOne), sameAsPrimary: false });
    });

    it("should reject search-succeeded when not in the search state", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(listener);
        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne, addressTwo] });
        listener.mockClear();

        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne] });

        expect(stateMachine.getState()).toEqual({ status: "results", criteria, addresses: [addressOne, addressTwo] });
        expect(listener).not.toHaveBeenCalled();
    });

    it("should move to the results state with an empty list when no addresses are found", () => {
        const stateMachine = new AddressLookupStateMachine(content);

        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [] });

        // Current behaviour: falls into the "more than one" branch rather than a distinct empty-results outcome.
        expect(stateMachine.getState()).toEqual({ status: "results", criteria, addresses: [] });
    });

    it("should move to the confirmed state when an address is selected", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne, addressTwo] });

        stateMachine.dispatch({ status: "address-selected", address: addressTwo });

        expect(stateMachine.getState()).toEqual({ status: "confirmed", address: mapAddressSearchResult(addressTwo), sameAsPrimary: false });
    });

    it("should move to the international entry state when requested", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        stateMachine.dispatch({ status: "international-manual-entry-requested" });

        expect(stateMachine.getState()).toEqual({ status: "international-manual-entry" });
    });

    it("should move to the confirmed view when an international address is submitted", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        stateMachine.dispatch({ status: "international-manual-entry-requested" });

        stateMachine.dispatch({ status: "international-manual-address-submitted", address: internationalAddress });

        expect(stateMachine.getState()).toEqual({
            status: "confirmed",
            address: mapInternationalManualEntryAddress(internationalAddress),
            sameAsPrimary: false
        });
    });

    it("should reject international-manual-address-submitted when not in the international entry state", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(listener);

        stateMachine.dispatch({ status: "back-to-search-requested" });
        stateMachine.dispatch({ status: "international-manual-address-submitted", address: internationalAddress });

        expect(stateMachine.getState()).toEqual({ status: "search" });
        expect(listener).not.toHaveBeenCalled();
    });

    it("should map a submitted UK manual address and move to the confirmed state", () => {
        const stateMachine = new AddressLookupStateMachine(content);
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
            },
            sameAsPrimary: false
        });
    });

    it("should reject address-selected when not in the results state", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(listener);

        stateMachine.dispatch({ status: "address-selected", address: addressOne });

        expect(stateMachine.getState()).toEqual({ status: "search" });
        expect(listener).not.toHaveBeenCalled();
    });

    it("should move back to the search state with the given criteria", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne, addressTwo] });

        stateMachine.dispatch({ status: "back-to-search-requested" });

        expect(stateMachine.getState()).toEqual({ status: "search", criteria });
    });

    it("should reject back-to-search-requested when already in the search state", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(listener);

        stateMachine.dispatch({ status: "back-to-search-requested" });

        expect(stateMachine.getState()).toEqual({ status: "search" });
        expect(listener).not.toHaveBeenCalled();
    });

    it("should notify a subscribed listener with the new state after a dispatch", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(listener);

        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne, addressTwo] });

        expect(listener).toHaveBeenCalledWith({ status: "results", criteria, addresses: [addressOne, addressTwo] });
    });

    it("should notify every subscribed listener on a single dispatch", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        const firstListener = jest.fn<(state: AddressLookupState) => void>();
        const secondListener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(firstListener);
        stateMachine.subscribe(secondListener);

        stateMachine.dispatch({ status: "search-succeeded", criteria, addresses: [addressOne] });

        expect(firstListener).toHaveBeenCalledTimes(1);
        expect(secondListener).toHaveBeenCalledTimes(1);
    });

    it("should not notify listeners for an unrecognised event", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(listener);
        const unrecognisedEvent = { status: "unrecognised" } as unknown as AddressLookupEvent;

        stateMachine.dispatch(unrecognisedEvent);

        expect(listener).not.toHaveBeenCalled();
    });

    it("copies a primary address when same-as is checked and notifies listeners", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        const listener = jest.fn<(state: AddressLookupState) => void>();
        const address = mapAddressSearchResult(addressOne);
        stateMachine.subscribe(listener);

        stateMachine.dispatch({ status: "same-as-checked", address });

        expect(stateMachine.getState()).toEqual({ status: "confirmed", address, sameAsPrimary: true });
        expect(listener).toHaveBeenCalledTimes(1);
        expect(listener).toHaveBeenCalledWith(stateMachine.getState());
    });

    it("waits for a confirmed primary address and copies successive updates", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        stateMachine.dispatch({ status: "same-as-checked" });
        expect(stateMachine.getState()).toEqual({ status: "search" });

        for (const result of [addressOne, addressTwo]) {
            const address = mapAddressSearchResult(result);
            stateMachine.dispatch({ status: "primary-address-state-change", address });
            expect(stateMachine.getState()).toEqual({ status: "confirmed", address, sameAsPrimary: true });
        }
    });

    it("retains the copied address but stops synchronising when unchecked", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        const address = mapAddressSearchResult(addressOne);
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.dispatch({ status: "same-as-checked", address });
        stateMachine.subscribe(listener);

        stateMachine.dispatch({ status: "same-as-unchecked" });
        expect(stateMachine.getState()).toEqual({ status: "confirmed", address, sameAsPrimary: false });
        expect(listener).toHaveBeenCalledTimes(1);
        listener.mockClear();
        stateMachine.dispatch({ status: "primary-address-state-change", address: mapAddressSearchResult(addressTwo) });

        expect(stateMachine.getState()).toEqual({ status: "confirmed", address, sameAsPrimary: false });
        expect(listener).not.toHaveBeenCalled();
    });

    it("cancels deferred copying when unchecked before the primary is confirmed", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        stateMachine.dispatch({ status: "same-as-checked" });
        stateMachine.dispatch({ status: "same-as-unchecked" });
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(listener);

        stateMachine.dispatch({ status: "primary-address-state-change", address: mapAddressSearchResult(addressOne) });

        expect(stateMachine.getState()).toEqual({ status: "search" });
        expect(listener).not.toHaveBeenCalled();
    });

    it.each([false, true])("ignores a missing primary address while waiting with same-as %s", checked => {
        const stateMachine = new AddressLookupStateMachine(content);
        if (checked) stateMachine.dispatch({ status: "same-as-checked" });
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(listener);

        stateMachine.dispatch({ status: "primary-address-state-change" });

        expect(stateMachine.getState()).toEqual({ status: "search" });
        expect(listener).not.toHaveBeenCalled();
    });

    it("ignores primary confirmation when same-as was never checked", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(listener);

        stateMachine.dispatch({ status: "primary-address-state-change", address: mapAddressSearchResult(addressOne) });

        expect(stateMachine.getState()).toEqual({ status: "search" });
        expect(listener).not.toHaveBeenCalled();
    });

    it("rejects UK manual submission outside the UK manual-entry state", () => {
        const stateMachine = new AddressLookupStateMachine(content);
        const listener = jest.fn<(state: AddressLookupState) => void>();
        stateMachine.subscribe(listener);

        stateMachine.dispatch({ status: "uk-manual-address-submitted", address: manualUkAddress });

        expect(stateMachine.getState()).toEqual({ status: "search" });
        expect(listener).not.toHaveBeenCalled();
    });

    it.each<AddressLookupEvent>([
        { status: "uk-manual-entry-requested" },
        { status: "international-manual-entry-requested" },
        { status: "search-succeeded", criteria, addresses: [addressOne] }
    ])("returns to search without stale criteria after $status", event => {
        const stateMachine = new AddressLookupStateMachine(content);
        stateMachine.dispatch(event);

        stateMachine.dispatch({ status: "back-to-search-requested" });

        expect(stateMachine.getState()).toEqual({ status: "search", criteria: undefined });
    });

    it("initialises a valid address as confirmed without linking to a primary", () => {
        const stateMachine = new AddressLookupStateMachine(content, "GB", {
            addressLine1: "1 High Street", postTown: "London", postcode: "SW1A 2AA", countryId: "GB"
        });

        expect(stateMachine.getState()).toMatchObject({
            status: "confirmed", address: { addressLine1: "1 High Street", postcode: "SW1A 2AA" }, sameAsPrimary: false
        });
    });
});
