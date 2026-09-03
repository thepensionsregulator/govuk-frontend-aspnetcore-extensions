export interface AddressLookupOptions {
    searchEndpoint: string;
    addressByIdEndpoint: string;
}

export interface AddressSearchCriteria {
    buildingName?: string;
    postcode: string;
}

export interface AddressSearchService {
    searchAddress(postcode: string): Promise<AddressSearchResult[]>;
    // getAddressById(uprn: string) : AddressSearchResult | null;
}

export interface AddressSearchOptions {
    searchService: AddressSearchService;
    onSearchSuccess: (results: AddressSearchResult[], criteria: AddressSearchCriteria) => void;
}

/** A Delivery Point Address as returned by the Ordnance Survey Places API. */
export interface AddressSearchResult {
    UPRN: string;
    UDPRN: string;
    ADDRESS: string;
    ORGANISATION_NAME?: string;
    DEPARTMENT_NAME?: string;
    SUB_BUILDING_NAME?: string;
    BUILDING_NAME?: string;
    BUILDING_NUMBER?: string;
    DEPENDENT_THOROUGHFARE_NAME?: string;
    THOROUGHFARE_NAME?: string;
    DOUBLE_DEPENDENT_LOCALITY?: string;
    DEPENDENT_LOCALITY?: string;
    POST_TOWN: string;
    POSTCODE: string;
    POSTAL_ADDRESS_CODE?: string;
    POSTAL_ADDRESS_CODE_DESCRIPTION?: string;
    CLASSIFICATION_CODE?: string;
    CLASSIFICATION_CODE_DESCRIPTION?: string;
    LOCAL_CUSTODIAN_CODE?: number;
    LOCAL_CUSTODIAN_CODE_DESCRIPTION?: string;
    LOGICAL_STATUS_CODE?: string;
    ENTRY_DATE?: string;
    LAST_UPDATE_DATE?: string;
    LANGUAGE?: string;
    MATCH?: number;
    MATCH_DESCRIPTION?: string;
    DELIVERY_POINT_SUFFIX?: string;
    X_COORDINATE?: number;
    Y_COORDINATE?: number;
    LNG?: number;
    LAT?: number;
}

export interface FetchAddressSearchServiceOptions{
    searchEndpoint: string;
    addressByIdEndpoint: string;
    fetchFunction?: typeof globalThis.fetch;
}