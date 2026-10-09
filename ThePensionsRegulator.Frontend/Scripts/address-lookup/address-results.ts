import { AddressLookupContent, AddressSearchCriteria, AddressSearchResult } from "./types";
import { createButton } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/button.js";
import { createLink } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/link.js";
import { createSelectFormGroup } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/inputs.js";
import { clearFormGroupError, showFormGroupError } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/validation.js";
import { submitOnEnter } from "./submit-on-enter.js";
import { createParagraph } from "/ThePensionsRegulator.GovUk.Frontend/js/govuk-components/paragraph.js";

export type AddressResultsOptions = {
    addresses: AddressSearchResult[];
    criteria: AddressSearchCriteria;
    content: AddressLookupContent;
    onAddressSelected: (address: AddressSearchResult) => void;
    onBackToSearchRequested: () => void;
    onUkManualEntryRequested: () => void;
}

export function renderAddressResults(options: AddressResultsOptions): HTMLElement {
    const addressResultsWrapper = document.createElement("div");
    const selectFormGroup = createSelectFormGroup({
        labelText: options.content.addressSelectLabel,
        select: {
            id: "address-select",
            name: "address-select",
            value: "",
            options: [ 
                {value: "", text: options.content.addressSelectLabel, disabled: true},
                ...options.addresses.map(address => ({
                    value: address.UPRN,
                    text: address.ADDRESS
                }))
            ]
        }
    });
    selectFormGroup.setAttribute("data-address-lookup-submit-error-anchor", "true");
    const addressSelect = selectFormGroup.querySelector("select")!;
    const button = createButton({labelText: options.content.confirmAddressLabel, variant: "secondary", type: "button"});
    submitOnEnter(selectFormGroup, () => button.click());
    button.addEventListener("click", handleAddressSelect);

    const ukManualEntryLink = createLink({labelText: options.content.ukManualEntryLinkLabel});
    ukManualEntryLink.addEventListener("click", handleUkManualEntry);
    const manualEntryPara = createParagraph();
    manualEntryPara.appendChild(ukManualEntryLink);
    
    const backToSearchLink = createLink({labelText: options.content.backToPostcodeSearchLabel});
    backToSearchLink.addEventListener("click", handleBackToSearch);
    const backToSearchPara = createParagraph();
    backToSearchPara.appendChild(backToSearchLink);
    
    const nav = document.createElement("nav");
    const ul = document.createElement("ul");
    ul.classList = "govuk-list";
    const backToSearchLi = document.createElement("li");
    backToSearchLi.appendChild(backToSearchPara);
    const ukManualEntryLi = document.createElement("li");
    ukManualEntryLi.appendChild(manualEntryPara);
    ul.appendChild(backToSearchLi);
    ul.appendChild(ukManualEntryLi);
    nav.appendChild(ul);
    addressResultsWrapper.appendChild(selectFormGroup);
    addressResultsWrapper.appendChild(button);
    addressResultsWrapper.appendChild(nav);
    
    return addressResultsWrapper;

    function handleAddressSelect(): void{
        const selectedUprn = addressSelect.value;
        const selectedAddress = options.addresses.find(address => address.UPRN === selectedUprn);

        if(!selectedAddress){
            showFormGroupError(selectFormGroup, options.content.addressSelectErrorMessage);
            return;
        }

        clearFormGroupError(selectFormGroup);
        options.onAddressSelected(selectedAddress);
    }

    function handleBackToSearch(event: Event): void {
        event.preventDefault();
        options.onBackToSearchRequested();
    }

    function handleUkManualEntry(event: Event): void {
        event.preventDefault();
        options.onUkManualEntryRequested();
    }
}