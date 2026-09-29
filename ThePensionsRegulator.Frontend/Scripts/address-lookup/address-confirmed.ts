import { AddressFieldKey, AddressFields, AddressLookupContent, ConfirmedAddress } from "./types";
import { createLink } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/link.js";

export interface ConfirmedAddressOptions {
    address: ConfirmedAddress;
    fields: AddressFields;
    content: AddressLookupContent;
    onBackToSearchRequested: () => void;
}

export function renderConfirmedAddress(options: ConfirmedAddressOptions): HTMLElement {
    const div = document.createElement("div");

    div.appendChild(createAddressParagraph(options.address));
    createHiddenAddressInputs(options.address, options.fields).forEach(input => div.appendChild(input));    

    const backToSearchLink = createLink({labelText: options.content.changeAddressLabel});
    backToSearchLink.addEventListener("click", handleBackToSearch);

    div.appendChild(backToSearchLink);

    return div;

    function handleBackToSearch(event: Event): void {
        event.preventDefault();
        options.onBackToSearchRequested();
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
        address.postcode
    ].filter(line => line !== undefined);

    filteredAddress.forEach((addressLine, index) => {
        if (addressLine === address.postcode) {
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

function createHiddenAddressInputs(address: ConfirmedAddress, fields: AddressFields) : HTMLInputElement[] {
    const inputs: HTMLInputElement[] = [];
    
    (Object.keys(fields) as AddressFieldKey[]).forEach(key => {
        const field = fields[key];

        if (field && address[key]) {
            const input = document.createElement("input");
            input.type = "hidden";
            input.id = key;
            input.name = field.name;
            input.value = address[key] ?? "";
            inputs.push(input);
        }
    });

    return inputs;
}

function toTitleCase(text: string): string {
    if (!text) return text;
    return text.toLowerCase().replace(/\b\w/g, (char) => char.toUpperCase());
}