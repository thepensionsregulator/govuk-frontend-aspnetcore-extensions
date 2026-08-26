export interface AddressSearchCriteria {
    buildingName?: string;
    postcode?: string;
}

export interface AddressSearchOptions {
    onSearch(criteria: AddressSearchCriteria): void;
}