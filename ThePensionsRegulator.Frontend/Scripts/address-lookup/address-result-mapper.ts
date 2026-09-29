import type { AddressSearchResult, ConfirmedAddress } from './types';

// const unitedKingdomCountryId = 'GB';
// const unitedKingdomCountryName = 'United Kingdom';

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
        postCode: address.POSTCODE,
        uprnReference: address.UPRN ?? undefined
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