import type { AddressLookupContent, AddressSearchResult, ConfirmedAddress, InitialAddress, InternationalManualEntryAddress, UkManualEntryAddress } from './types';
import { validateUkManualEntryAddress } from './address-uk-manual-entry.js';
import { validateInternationalUkManualEntryAddress } from './address-international-manual-entry.js';
import { normalisePostcode } from './postcode-normaliser.js';
import { sanitisePostcode } from './postcode-sanitiser.js';


export function mapAddressSearchResult(address: AddressSearchResult): ConfirmedAddress {
    const lines: string[] = [];
    const organisationName = address.ORGANISATION_NAME?.trim() ?? '';
    let buildingName = address.BUILDING_NAME?.trim() ?? '';
    const subBuildingName = address.SUB_BUILDING_NAME?.trim() ?? '';
    let buildingNumber = address.BUILDING_NUMBER?.trim() ?? '';

    if (buildingName && !buildingNumber) {
        const splitResult = splitBuildingNameAndNumber(buildingName);
        if (splitResult) {
            [buildingName, buildingNumber] = splitResult;
        }
    }

    if (organisationName) {
        lines.push(organisationName);
    }

    const isNumericBuildingName = isNumericStyleBuildingName(buildingName);

    if (subBuildingName) {
        if (isNumericBuildingName) {
            lines.push(subBuildingName);
            pushStreetLine(lines, buildingName, address);
        } else if (buildingName) {
            const subBuildingIsNumeric = isNumericStyleBuildingName(subBuildingName);
            const premiseLine = joinNonEmpty(
                [subBuildingName, buildingName],
                subBuildingIsNumeric ? ' ' : ', '
            );

            if (premiseLine) {
                lines.push(premiseLine);
            }

            if (!subBuildingIsNumeric || buildingNumber) {
                pushStreetLine(lines, buildingNumber, address);
            }
        } else {
            const streetLine = buildStreetLine(buildingNumber, address);
            const premiseLine = joinNonEmpty([subBuildingName, streetLine], ', ');

            if (premiseLine) {
                lines.push(premiseLine);
            }
        }
    } else if (isNumericBuildingName) {
        pushStreetLine(lines, buildingName, address);
    } else if (buildingName) {
        lines.push(buildingName);
        pushStreetLine(lines, buildingNumber, address);
    } else {
        pushStreetLine(lines, buildingNumber, address);
    }

    return {
        addressLine1: lines[0],
        addressLine2: lines[1] ?? undefined,
        addressLine3: lines[2] ?? undefined,
        postTown: address.POST_TOWN?.trim() || undefined,
        postcode: address.POSTCODE,
        uprnReference: address.UPRN ?? undefined
    };
}

export function mapUkManualEntryAddress(address: UkManualEntryAddress): ConfirmedAddress {
    const confirmedAddress : ConfirmedAddress = {
        addressLine1: address.addressLine1,
        addressLine2: address.addressLine2 ?? undefined,
        addressLine3: address.addressLine3 ?? undefined,
        postTown: address.postTown,
        postCounty: address.county ?? undefined,
        postcode: address.postcode,
    };

    return confirmedAddress;
}

export function mapInternationalManualEntryAddress(address: InternationalManualEntryAddress): ConfirmedAddress {
    return {
        addressLine1: address.addressLine1,
        addressLine2: address.addressLine2 ?? undefined,
        addressLine3: address.addressLine3 ?? undefined,
        postTown: address.postTown,
        postCounty: address.countyStateProvince ?? undefined,
        countryId: address.countryId ?? undefined,
        countryName: address.countryName ?? undefined,
        postcode: address.postcode ?? undefined
    };
}

export function mapInitialAddress(content: AddressLookupContent, address?: InitialAddress, ukCountryId?: string): ConfirmedAddress | undefined {
    if (address == undefined || !address.addressLine1){
        return undefined;
    }

    const isInternational = !!address.countryId && address.countryId !== ukCountryId;

    const ukPostcode = address.postcode ? normalisePostcode(sanitisePostcode(address.postcode)) : '';

    if (!isInternational && address.uprnReference && address.postTown && ukPostcode) {
        return {
            addressLine1: address.addressLine1,
            addressLine2: address.addressLine2 || undefined,
            addressLine3: address.addressLine3 || undefined,
            postTown: address.postTown,
            postCounty: address.postCounty || undefined,
            countryId: address.countryId,
            countryName: address.countryName,
            postcode: ukPostcode,
            uprnReference: address.uprnReference
        };
    }

    // The manual entry validators expect every optional field to be a string.
    const validationResult = isInternational
        ? validateInternationalUkManualEntryAddress(
            {
                addressLine1: address.addressLine1,
                addressLine2: address.addressLine2 ?? '',
                addressLine3: address.addressLine3 ?? '',
                postTown: address.postTown ?? '',
                countyStateProvince: address.postCounty ?? '',
                countryId: address.countryId,
                countryName: address.countryName,
                postcode: address.postcode ?? ''
            },
            content,
        )
        : validateUkManualEntryAddress(
            {
                addressLine1: address.addressLine1,
                addressLine2: address.addressLine2 ?? '',
                addressLine3: address.addressLine3 ?? '',
                townOrCity: address.postTown ?? '',
                county: address.postCounty ?? '',
                postcode: ukPostcode,
            },
            content
        );

    if (!validationResult.isValid) {
        return undefined;
    }

    return {
        addressLine1: address.addressLine1,
        addressLine2: address.addressLine2 || undefined,
        addressLine3: address.addressLine3 || undefined,
        postTown: address.postTown,
        postCounty: address.postCounty || undefined,
        countryId: address.countryId,
        countryName: address.countryName,
        postcode: (isInternational ? address.postcode : ukPostcode) || undefined,
        uprnReference: address.uprnReference || undefined
    };
}

function pushStreetLine(lines: string[], buildingNumber: string, address: AddressSearchResult): void {
    const streetLine = buildStreetLine(buildingNumber, address);
    if (streetLine) {
        lines.push(streetLine);
    }
}

function buildStreetLine(buildingNumber: string, address: AddressSearchResult): string {
    return joinNonEmpty([
        buildingNumber,
        address.DEPENDENT_THOROUGHFARE_NAME?.trim() ?? '',
        address.THOROUGHFARE_NAME?.trim() ?? ''
    ], ' ');
}

function joinNonEmpty(parts: string[], separator: string): string {
    return parts.filter(part => part).join(separator);
}

function isNumericStyleBuildingName(value: string): boolean {
    return /^\d+(?:\s*[-/]\s*\d+)?[A-Za-z]?$/.test(value);
}

function splitBuildingNameAndNumber(value: string): [string, string] | undefined {
    const match = value.match(/^(.+?)\s+(\d+(?:\s*[-/]\s*\d+)?[A-Za-z]?)$/);
    return match ? [match[1].trim(), match[2].trim()] : undefined;
}