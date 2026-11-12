import { ConfirmedAddress } from "./types";
import { createLink } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/link.js";

export function renderConfirmedAddress(address: ConfirmedAddress, onBackToSearchRequested: () => void): HTMLElement {
    const div = document.createElement("div");

    div.appendChild(createAddressParagraph(address));
    createHiddenAddressInputs(address).forEach(input => div.appendChild(input));

    const backToSearchLink = createLink({labelText: "Change address"});
    backToSearchLink.addEventListener("click", handleBackToSearch);

    div.appendChild(backToSearchLink);

    return div;

    function handleBackToSearch(event: Event): void {
        event.preventDefault();
        onBackToSearchRequested();
    }
}


function createAddressParagraph(address: ConfirmedAddress): HTMLParagraphElement { 
    const paragraph = document.createElement("p");
    paragraph.classList.add("govuk-body");

    const filteredAddress = [
        address.addressLine1,
        address.addressLine2,
        address.addressLine3,
        address.postTown,
        address.postCounty,
        ...(address.countryId !== "GB" && address.countryName ? [address.countryName] : []),
        address.postCode
    ].filter(line => line !== undefined);

    filteredAddress.forEach((addressLine, index) => {
        if (addressLine === address.postCode) {
            paragraph.appendChild(document.createTextNode(addressLine));
        } else {
            const titleCase = toTitleCase(addressLine);
            paragraph.appendChild(document.createTextNode(titleCase));
        }
        if (index < filteredAddress.length - 1) {
            paragraph.appendChild(document.createElement("br"));
        }
    });
        
    return paragraph;
}

function createHiddenAddressInputs(address: ConfirmedAddress) : HTMLInputElement[] {
    const inputs: HTMLInputElement[] = [];
    
    inputs.push(createHiddenInput("AddressLine1", address.addressLine1));
    
    if (address.addressLine2 !== undefined) {
        inputs.push(createHiddenInput("AddressLine2", address.addressLine2));
    }
    if (address.addressLine3 !== undefined) {
        inputs.push(createHiddenInput("AddressLine3", address.addressLine3));
    }
    if (address.postTown !== undefined) {
        inputs.push(createHiddenInput("PostTown", address.postTown));
    }
    if (address.postCounty !== undefined) {
        inputs.push(createHiddenInput("PostCounty", address.postCounty));
    }
    if (address.countryId !== undefined) {
        inputs.push(createHiddenInput("CountryId", address.countryId));
    }
    if (address.postCode !== undefined) {
        inputs.push(createHiddenInput("PostCode", address.postCode));
    }
    if (address.uprnReference !== undefined) {
        inputs.push(createHiddenInput("UPRNReference", address.uprnReference));
    }

    return inputs;
}

function createHiddenInput(name: string, value: string): HTMLInputElement {
    const input = document.createElement("input");
    input.type = "hidden";
    input.id = name;
    input.name = name;
    input.value = value;
    return input;
}

function toTitleCase(text: string): string {
    if (!text) return text;
    return text.toLowerCase().replace(/\b\w/g, (char) => char.toUpperCase());
}