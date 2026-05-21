import '@testing-library/jest-dom';
import { jest } from '@jest/globals';
import { ADDRESS_LOOKUP_CONFIG } from "../../wwwroot/tpr/address-lookup/config.js";
import { createMockAddressLookup, createMockEvent } from "./test-helpers.js";
import { AddressLookupStateMachine } from "../../wwwroot/tpr/address-lookup/state-machine.js";
import { submitOnClick } from "../../wwwroot/tpr/address-lookup/index.js";

describe("submitOnClick", () => {
    afterEach(() => {
        document.body.innerHTML = "";
    });

    it("does not call preventDefault when all address lookups are confirmed and form is valid", () => {
        const event = createMockEvent();
        const lookup = createMockAddressLookup(AddressLookupStateMachine.STATES.CONFIRMED);

        submitOnClick(event, [lookup]);

        expect(event.preventDefault).not.toHaveBeenCalled();
    });

    it("calls preventDefault when an address lookup is in SEARCH state", () => {
        const event = createMockEvent();
        const lookup = createMockAddressLookup(AddressLookupStateMachine.STATES.SEARCH);

        submitOnClick(event, [lookup]);

        expect(event.preventDefault).toHaveBeenCalled();
        expect(lookup.validator.addOrUpdateCustomFieldsetError).toHaveBeenCalledWith(
            expect.any(HTMLFieldSetElement),
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.SELECT_FIND_ADDRESS
        );
    });

    it("calls addSelectError when an address lookup is in SELECT state", () => {
        const event = createMockEvent();
        const lookup = createMockAddressLookup(AddressLookupStateMachine.STATES.SELECT);

        submitOnClick(event, [lookup]);

        expect(event.preventDefault).toHaveBeenCalled();
        expect(lookup.fieldDefaults.errorMessage).toHaveBeenCalledWith("select-confirm");
        expect(lookup.validator.addSelectError).toHaveBeenCalledWith(
            expect.any(HTMLSelectElement),
            "mock-error-select-confirm"
        );
    });

    it("calls preventDefault when an address lookup is in MANUAL_UK_ENTRY state", () => {
        const event = createMockEvent();
        const lookup = createMockAddressLookup(AddressLookupStateMachine.STATES.MANUAL_UK_ENTRY);

        submitOnClick(event, [lookup]);

        expect(event.preventDefault).toHaveBeenCalled();
        expect(lookup.validator.addOrUpdateCustomFieldsetError).toHaveBeenCalledWith(
            expect.any(HTMLFieldSetElement),
            ADDRESS_LOOKUP_CONFIG.ERROR_MESSAGES.SELECT_CONFIRM
        );
    });

    it("address lookup errors are preserved even when form validation passes", () => {
        const event = createMockEvent();
        const lookup = createMockAddressLookup(AddressLookupStateMachine.STATES.SEARCH);

        submitOnClick(event, [lookup]);

        // The key regression test: address errors should be added BEFORE form validation runs,
        // so form validation doesn't clear them.
        expect(lookup.validator.addOrUpdateCustomFieldsetError).toHaveBeenCalled();
        expect(event.preventDefault).toHaveBeenCalled();
    });

    it("calls preventDefault when form validation fails even if all address lookups are confirmed", () => {
        const form = document.createElement("form");
        const button = document.createElement("button");
        form.appendChild(button);
        document.body.appendChild(form);

        global.$ = jest.fn(() => ({
            valid: jest.fn(() => false),
        }));

        const event = { target: button, preventDefault: jest.fn() };
        const lookup = createMockAddressLookup(AddressLookupStateMachine.STATES.CONFIRMED);

        submitOnClick(event, [lookup]);

        expect(event.preventDefault).toHaveBeenCalled();
    });

    it("validates all address lookups when there are multiple", () => {
        const event = createMockEvent();
        const confirmedLookup = createMockAddressLookup(AddressLookupStateMachine.STATES.CONFIRMED);
        const searchLookup = createMockAddressLookup(AddressLookupStateMachine.STATES.SEARCH);

        submitOnClick(event, [confirmedLookup, searchLookup]);

        expect(confirmedLookup.validator.addOrUpdateCustomFieldsetError).not.toHaveBeenCalled();
        expect(searchLookup.validator.addOrUpdateCustomFieldsetError).toHaveBeenCalled();
        expect(event.preventDefault).toHaveBeenCalled();
    });
});