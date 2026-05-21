import { jest } from '@jest/globals';

export function createMockAddressLookup(currentState) {
    const fieldset = document.createElement("fieldset");
    const container = document.createElement("div");
    container.appendChild(fieldset);

    return {
        stateMachine: { currentState },
        container,
        validator: {
            addSelectError: jest.fn(),
            addOrUpdateCustomFieldsetError: jest.fn(),
        },
        fieldDefaults: {
            errorMessage: jest.fn((key) => `mock-error-${key}`),
        },
        getComponentByDataAddressAttribute: jest.fn(() => document.createElement("select")),
    };
}

export function createMockEvent(formValid = true) {
    const form = document.createElement("form");
    const button = document.createElement("button");
    form.appendChild(button);
    document.body.appendChild(form);

    global.$ = jest.fn(() => ({
        valid: jest.fn(() => formValid),
    }));

    return {
        target: button,
        preventDefault: jest.fn(),
    };
}