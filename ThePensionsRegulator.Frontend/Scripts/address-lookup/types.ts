export interface AddressLookupOptions {
    searchEndpoint: string;
    initialAddress?: InitialAddress;
    fields: AddressFields;
    countries?: CountryOption[];
    content: AddressLookupContent;
}

export interface AddressSearchCriteria {
    buildingName?: string;
    postcode: string;
}

export interface AddressSearchService {
    searchAddress(postcode: string): Promise<AddressSearchResult[]>;
}

export interface AddressSearchOptions {
    searchService: AddressSearchService;
    criteria?: AddressSearchCriteria;
    content: AddressLookupContent
    onSearchSuccess: (results: AddressSearchResult[], criteria: AddressSearchCriteria) => void;
    onInternationalEntryRequested: () => void;
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

export type AddressFieldKey = Exclude<keyof InitialAddress, 'countryName'>;

export interface AddressField{
    name: string;
    value: string
}

export type AddressFields = Partial<Record<AddressFieldKey, AddressField>>;

export interface FetchAddressSearchServiceOptions{
    searchEndpoint: string;
    fetchFunction?: typeof globalThis.fetch;
}

export interface UkManualEntryAddress {
    addressLine1: string;
    addressLine2?: string;
    addressLine3?: string;
    postTown: string;
    county?: string
    postcode: string;
}

export interface InternationalManualEntryAddress {
    addressLine1: string;
    addressLine2?: string;
    addressLine3?: string;
    postTown: string;
    countyStateProvince?: string;
    countryId?: string;
    countryName?: string;
    postcode?: string;
}

export interface ConfirmedAddress {
    addressLine1: string;
    addressLine2?: string;
    addressLine3?: string;
    postTown?: string;
    postCounty?: string;
    countryId?: string;
    countryName?: string;
    postcode?: string;
    uprnReference?: string;
}

export interface InitialAddress {
    addressLine1?: string;
    addressLine2?: string;
    addressLine3?: string;
    postTown?: string;
    postCounty?: string;
    countryId?: string;
    countryName?: string;
    postcode?: string;
    uprnReference?: string;
}

export interface AddressLookupContent {
    // component level legend and hint
    addressLookupLegend: string;
    addressLookupHint?: string;
    noJsLegend: string;

    // manual entry labels and error messages
    addressLine1Label: string;
    addressLine1RequiredErrorMessage: string;
    addressLine1MaxLengthErrorMessage: string;
    addressLine2Label: string;
    addressLine2MaxLengthErrorMessage: string;
    addressLine3Label: string;
    addressLine3MaxLengthErrorMessage: string;
    postTownLabel: string;
    postTownRequiredErrorMessage: string;
    postTownMaxLengthErrorMessage: string;
    countyLabel: string;
    countyMaxLengthErrorMessage: string;
    countyStateProvinceLabel: string;
    countyStateProvinceMaxLengthErrorMessage: string;
    postcodeLabel: string;
    postcodeRequiredErrorMessage: string;
    postcodeMaxLengthErrorMessage: string;
    postcodePatternErrorMessage: string;
    countryLabel: string;
    countrySelectPlaceholder: string;
    countryRequiredErrorMessage: string;
    buildingNameOrNumberLabel: string;

    // UK manual entry labels
    ukManualEntryLinkLabel: string;
    ukManualEntryLegend: string;
    
    // international manual entry labels 
    internationalManualEntryLabel: string;
    internationalManualEntryLegend: string;

    // search view
    searchLegend: string;
    findAddressButton: string;
    addressNotFoundMessage: string;
    serviceUnavailableMessage: string;
    searchNotConfirmedErrorMessage: string;

    // results view
    addressSelectLabel: string;
    confirmAddressLabel: string;
    backToPostcodeSearchLabel: string;
    addressSelectErrorMessage: string;
    resultsNotConfirmedErrorMessage: string;

    // UK manual entry view
    ukManualEntryNotConfirmedErrorMessage: string;

    // international manual entry view
    internationalManualEntryNotConfirmedErrorMessage: string;

    // confirmed view
    changeAddressLabel: string;
    
}

export interface AddressLookupSettings {
    searchEndpoint: string;
}

export interface AddressLookupInstance {
    element: HTMLElement;
    isConfirmed(): boolean;
    showNotConfirmedError(): void;
}

export type CountryOption = { value: string, name: string};