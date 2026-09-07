import type { AddressSearchResult, ConfirmedAddress } from './types';

export function mapAddressSearchResult(address: AddressSearchResult) : ConfirmedAddress{
    return {
        organisationName: address.ORGANISATION_NAME ?? '',
        departmentName: address.DEPARTMENT_NAME ?? '',
        subBuildingName: address.SUB_BUILDING_NAME ?? '',
        buildingName: address.BUILDING_NAME ?? '',
        buildingNumber: address.BUILDING_NUMBER ?? '',
        dependentThoroughfareName: address.DEPENDENT_THOROUGHFARE_NAME ?? '',
        thoroughfareName: address.THOROUGHFARE_NAME ?? '',
        doubleDependentLocality: address.DOUBLE_DEPENDENT_LOCALITY ?? '',
        dependentLocality: address.DEPENDENT_LOCALITY ?? '',
        postTown: address.POST_TOWN ?? '',
        postcode: address.POSTCODE ?? '',
        uprn: address.UPRN ?? ''
    };
}