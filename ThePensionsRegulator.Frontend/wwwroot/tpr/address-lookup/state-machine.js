class AddressLookupStateMachine {
    static STATES = {
        SEARCH: 'search',
        SELECT: 'select',
        CONFIRMED: 'confirmed',
        MANNUAL_UK_ENTRY: 'manual_uk_entry',
        MANNUAL_INTERNATIONAL_ENTRY: 'manual_international_entry'
    };

    constructor() {
        this.listeners = [];
    }

    transition(newState, data) {
        const validTransitions = {
            [AddressLookupStateMachine.STATES.SEARCH]: [AddressLookupStateMachine.STATES.SELECT, AddressLookupStateMachine.STATES.MANNUAL_INTERNATIONAL_ENTRY, AddressLookupStateMachine.STATES.CONFIRMED],
            [AddressLookupStateMachine.STATES.SELECT]: [AddressLookupStateMachine.STATES.CONFIRMED, AddressLookupStateMachine.STATES.SEARCH, AddressLookupStateMachine.STATES.MANNUAL_UK_ENTRY],
            [AddressLookupStateMachine.STATES.CONFIRMED]: [AddressLookupStateMachine.STATES.SEARCH],
            [AddressLookupStateMachine.STATES.MANNUAL_INTERNATIONAL_ENTRY]: [AddressLookupStateMachine.STATES.SEARCH, AddressLookupStateMachine.STATES.CONFIRMED],
            [AddressLookupStateMachine.STATES.MANNUAL_UK_ENTRY]: [AddressLookupStateMachine.STATES.SEARCH, AddressLookupStateMachine.STATES.CONFIRMED]
        };

        if (this.currentState !== undefined && !validTransitions[this.currentState].includes(newState)) {
            console.error(`Invalid state transition from ${this.currentState} to ${newState}`);
            return false;
        }

        console.log(`Transitioning from ${this.currentState} to ${newState}`);
        this.notifyListeners(newState, data);
        this.currentState = newState;
        return true;
    }

    notifyListeners(newState, data) {
        this.listeners.forEach(listener => listener(newState, data));
    }

    onChange(callback) {
        this.listeners.push(callback);
    }

}

export { AddressLookupStateMachine };