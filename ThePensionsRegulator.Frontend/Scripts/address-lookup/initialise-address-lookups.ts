import { InitialAddress, CountryOption, AddressLookupContent, AddressFieldKey, AddressFields, AddressLookupSettings, AddressLookupInstance } from "./types";
import { createAddressLookup } from "./address-lookup.js";

const addressFieldKeys = [
    "addressLine1",
    "addressLine2",
    "addressLine3",
    "postTown",
    "postCounty",
    "countryId",
    "postcode",
    "uprnReference"
] as const satisfies readonly AddressFieldKey[];


export function initialiseAddressLookups() {
    const lookupsByForm = new Map<HTMLFormElement, AddressLookupInstance[]>();
    const targets = document.querySelectorAll(".tpr-address-lookup");

    targets.forEach(target => {
        const fields = getAddressFields(target as HTMLElement);
        const countries = getCountries(target as HTMLElement);
        const content = getContent(target as HTMLElement);
        const settings = getSettings(target as HTMLElement);

        const addressLookup = createAddressLookup({
            searchEndpoint: settings.searchEndpoint,
            initialAddress: toInitialAddress(fields, countries),
            fields, 
            countries,
            content,
        });

        target.replaceChildren(addressLookup.element);

        const form = target.closest("form");
        if (!form) return;
        const lookups = lookupsByForm.get(form) ?? [];
        lookups.push(addressLookup);
        lookupsByForm.set(form, lookups);
    });

    lookupsByForm.forEach((lookups, form) => {
        form.addEventListener("submit", (event) => {
            const unconfirmedLookups = lookups.filter(lookup => !lookup.isConfirmed());

            if (unconfirmedLookups.length === 0) {
                return;
            }

            event.preventDefault();
            
            unconfirmedLookups.forEach(lookup => lookup.showNotConfirmedError());
        });
    })
}


function getAddressFields(container: HTMLElement): AddressFields {
    const fields: AddressFields = {};

    container.querySelectorAll<HTMLInputElement | HTMLSelectElement>("[data-address-field]").forEach(input => {
        const key = input.dataset.addressField;

        if (key && isAddressFieldKey(key)) {
            fields[key] = { name: input.name, value: input.value.trim() };
        }
    });

    return fields;
}

function isAddressFieldKey(value: string): value is AddressFieldKey {
    return (addressFieldKeys as readonly string[]).includes(value);
}

function toInitialAddress(fields: AddressFields, countries: CountryOption[]): InitialAddress {
    const initialAddress: InitialAddress = {};

    addressFieldKeys.forEach(key => {
        initialAddress[key] = fields[key]?.value || undefined;
    });

    initialAddress.countryName = countries.find(country => country.value === initialAddress.countryId)?.name;

    return initialAddress;
}


function getContent(container: HTMLElement): AddressLookupContent {
    const get = (name: string): string => container.dataset[name] ?? "";

    const content: AddressLookupContent = {
        addressLookupLegend: get("addressLookupLegend"),
        addressLookupHint: get("addressLookupHint"),
        addressLine1Label: get("addressLookupAddressLine-1Label"),
        addressLine1RequiredErrorMessage: get("addressLookupAddressLine-1RequiredErrorMessage"),
        addressLine1MaxLengthErrorMessage: get("addressLookupAddressLine-1MaxLengthErrorMessage"),
        addressLine2Label: get("addressLookupAddressLine-2Label"),
        addressLine2MaxLengthErrorMessage: get("addressLookupAddressLine-2MaxLengthErrorMessage"),
        addressLine3Label: get("addressLookupAddressLine-3Label"),
        addressLine3MaxLengthErrorMessage: get("addressLookupAddressLine-3MaxLengthErrorMessage"),
        postTownLabel: get("addressLookupPostTownLabel"),
        postTownRequiredErrorMessage: get("addressLookupPostTownRequiredErrorMessage"),
        postTownMaxLengthErrorMessage: get("addressLookupPostTownMaxLengthErrorMessage"),
        countyLabel: get("addressLookupCountyLabel"),
        countyMaxLengthErrorMessage: get("addressLookupCountyMaxLengthErrorMessage"),
        countyStateProvinceLabel: get("addressLookupCountyStateProvinceLabel"),
        countyStateProvinceMaxLengthErrorMessage: get("addressLookupCountyStateProvinceMaxLengthErrorMessage"),
        postcodeLabel: get("addressLookupPostcodeLabel"),
        postcodeRequiredErrorMessage: get("addressLookupPostcodeRequiredErrorMessage"),
        postcodeMaxLengthErrorMessage: get("addressLookupPostcodeMaxLengthErrorMessage"),
        postcodePatternErrorMessage: get("addressLookupPostcodePatternErrorMessage"),
        countryLabel: get("addressLookupCountryLabel"),
        countrySelectPlaceholder: get("addressLookupCountrySelectPlaceholder"),
        countryRequiredErrorMessage: get("addressLookupCountryRequiredErrorMessage"),
        buildingNameOrNumberLabel: get("addressLookupBuildingNameOrNumberLabel"),
        findAddressButton: get("addressLookupFindAddressButton"),
        addressNotFoundMessage: get("addressLookupAddressNotFoundMessage"),
        serviceUnavailableMessage: get("addressLookupServiceUnavailableMessage"),
        addressSelectLabel: get("addressLookupAddressSelectLabel"),
        confirmAddressLabel: get("addressLookupConfirmAddressLabel"),
        backToPostcodeSearchLabel: get("addressLookupBackToPostcodeSearchLabel"),
        changeAddressLabel: get("addressLookupChangeAddressLabel"),
        ukManualEntryLinkLabel: get("addressLookupUkManualEntryLinkLabel"),
        ukManualEntryLegend: get("addressLookupUkManualEntryLegend"),
        internationalManualEntryLabel: get("addressLookupInternationalManualEntryLabel"),
        internationalManualEntryLegend: get("addressLookupInternationalManualEntryLegend"),
        addressSelectErrorMessage: get("addressLookupAddressSelectErrorMessage"),
        internationalManualEntryNotConfirmedErrorMessage: get("addressLookupInternationalManualEntryNotConfirmedErrorMessage"),
        ukManualEntryNotConfirmedErrorMessage: get("addressLookupUkManualEntryNotConfirmedErrorMessage"),
        resultsNotConfirmedErrorMessage: get("addressLookupResultsNotConfirmedErrorMessage"),
        searchNotConfirmedErrorMessage: get("addressLookupSearchNotConfirmedErrorMessage")
    };

    return content;
}

function getSettings(container: HTMLElement): AddressLookupSettings {
    const searchEndpoint = container.getAttribute("data-address-lookup-search-url") || "";
    return { searchEndpoint };
}

function getCountries(container: HTMLElement): CountryOption[] {
    const select = container.querySelector("select");
    const countries: CountryOption[] = [];

    if (!select) {
        return countries;
    }

    const options = select.querySelectorAll<HTMLOptionElement>("option");
    options.forEach(option => {
        const value = Number(option.value);
        if (option.value && !Number.isNaN(value)) {
            countries.push({ value: option.value, name: option.textContent || "" });
        }
    });

    return countries;
}


