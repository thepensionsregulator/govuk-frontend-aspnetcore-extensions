class AddressLookupStateMachine {
    static STATES = {
        SEARCH: 'search',
        SELECT: 'select',
        CONFIRMED: 'confirmed'
    };

    constructor() {
        this.currentState = AddressLookupStateMachine.STATES.SEARCH;
        this.listeners = [];
    }

    transition(newState, data) {
        const validTransitions = {
            [AddressLookupStateMachine.STATES.SEARCH]: [AddressLookupStateMachine.STATES.SELECT],
            [AddressLookupStateMachine.STATES.SELECT]: [AddressLookupStateMachine.STATES.CONFIRMED, AddressLookupStateMachine.STATES.SEARCH],
            [AddressLookupStateMachine.STATES.CONFIRMED]: [AddressLookupStateMachine.STATES.SEARCH]
        };

        if (!validTransitions[this.currentState].includes(newState)) {
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