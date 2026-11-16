import { AddressSearchResult } from "./types";

export function filterAddressesByBuilding(addresses : AddressSearchResult[], buildingName: string): AddressSearchResult[] {
    const filteredAddresses = addresses.filter(address => searchByBuilding(address, buildingName));
 
    return filteredAddresses;
}

function searchByBuilding(address: AddressSearchResult, buildingName: string): boolean {
    const building = buildingName.toLowerCase();

    let matchesBuildingNumber = false;
    if (address.BUILDING_NUMBER){
        matchesBuildingNumber = address.BUILDING_NUMBER.toLowerCase() === building;
    }
    let isInBuildingName = false;
    if (address.BUILDING_NAME){
        isInBuildingName = address.BUILDING_NAME.toLowerCase().includes(building);
    }
    let isSubBuildingName = false;
    if (address.SUB_BUILDING_NAME){
        isSubBuildingName = address.SUB_BUILDING_NAME.toLowerCase().includes(building);
    }

    return matchesBuildingNumber || isInBuildingName || isSubBuildingName;
}