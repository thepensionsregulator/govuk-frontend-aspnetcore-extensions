import { AddressSearchCriteria, AddressSearchResult } from "./types";
import { createButton } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/button.js";
import { createLink } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/link.js";
import { createSelectFormGroup } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/inputs.js";
import { clearFormGroupError, showFormGroupError } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/validation.js";

export type AddressResultsOptions = {
    addresses: AddressSearchResult[];
    criteria: AddressSearchCriteria;
    onAddressSelected: (address: AddressSearchResult) => void;
    onBackToSearchRequested: () => void;
}

export function renderAddressResults(options: AddressResultsOptions): HTMLElement {
    const addressResultsWrapper = document.createElement("div");
    const selectFormGroup = createSelectFormGroup({
        labelText: "Select an address",
        select: {
            id: "address-select",
            name: "address-select",
            value: "",
            options: [ 
                {value: "", text: "Select an address", disabled: true},
                ...options.addresses.map(address => ({
                    value: address.UPRN,
                    text: address.ADDRESS
                }))
            ]
        }
    });
    const addressSelect = selectFormGroup.querySelector("select")!;
    const button = createButton({labelText: "Confirm address", variant: "secondary", type: "button"});
    button.addEventListener("click", handleAddressSelect);

    const backToSearchLink = createLink({labelText: "Return to postcode search"});
    backToSearchLink.addEventListener("click", handleBackToSearch);

    const nav = document.createElement("nav");
    nav.appendChild(backToSearchLink);
    addressResultsWrapper.appendChild(selectFormGroup);
    addressResultsWrapper.appendChild(button);
    addressResultsWrapper.appendChild(nav);
    
    return addressResultsWrapper;

    function handleAddressSelect(): void{
        const selectedUprn = addressSelect.value;
        const selectedAddress = options.addresses.find(address => address.UPRN === selectedUprn);

        if(!selectedAddress){
            showFormGroupError(selectFormGroup, "Select an address");
            return;
        }

        clearFormGroupError(selectFormGroup);
        options.onAddressSelected(selectedAddress);
    }

    function handleBackToSearch(event: Event): void {
        event.preventDefault();
        options.onBackToSearchRequested();
    }
}