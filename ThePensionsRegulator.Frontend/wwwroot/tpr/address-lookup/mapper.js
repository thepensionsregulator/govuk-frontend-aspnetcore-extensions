class AddressMapper {
    constructor(config, countryOptions) {
        this.config = config;
        this.defaultCountryCode = countryOptions.find(x => x.label === config.DEFAULTS.COUNTRY)?.value || '';
    }

    mapFromInput(originalInputs) {
        const get = (attribute) => originalInputs.find(x => x.dataAddressLookup === attribute);
        const getValue = (attribute) => get(attribute)?.value || '';
        return {
            addressLine1: getValue(this.config.DATA_ATTRIBUTES.ADDRESS_LINE_1),
            addressLine2: getValue(this.config.DATA_ATTRIBUTES.ADDRESS_LINE_2),
            addressLine3: getValue(this.config.DATA_ATTRIBUTES.ADDRESS_LINE_3),
            town: getValue(this.config.DATA_ATTRIBUTES.TOWN_OR_CITY),
            county: getValue(this.config.DATA_ATTRIBUTES.COUNTY) || getValue(this.config.DATA_ATTRIBUTES.REGION_INTERNATIONAL),
            postcode: getValue(this.config.DATA_ATTRIBUTES.POSTCODE) || getValue(this.config.DATA_ATTRIBUTES.POSTCODE_INTERNATIONAL),
            country: get(this.config.DATA_ATTRIBUTES.COUNTRY_CODE)?.selectedOptionLabel || '',
            countryCode: getValue(this.config.DATA_ATTRIBUTES.COUNTRY_CODE),
            UPRN: getValue(this.config.DATA_ATTRIBUTES.UPRN)
        };
    }

    mapFromDpaResult(dpaResult) {
        const lines = [];

        if (dpaResult.ORGANISATION_NAME) {
            lines.push(dpaResult.ORGANISATION_NAME);
        }

        let buildingName = (dpaResult.BUILDING_NAME || '').trim();
        const subBuildingName = (dpaResult.SUB_BUILDING_NAME || '').trim();
        let buildingNumber = (dpaResult.BUILDING_NUMBER || '').trim();

        // Try to split building name if it contains a trailing number or range
        if (buildingName && !buildingNumber) {
            const splitResult = this.#splitBuildingNameAndNumber(buildingName);
            if (splitResult) {
                buildingName = splitResult[0];
                buildingNumber = splitResult[1];
            }
        }

        const isNumericBuildingName = this.#isNumericStyleBuildingName(buildingName);

        if (subBuildingName) {
            if (isNumericBuildingName) {
                lines.push(subBuildingName);

                const streetLine = this.#buildStreetLine(
                    buildingName,
                    dpaResult.DEPENDENT_THOROUGHFARE_NAME,
                    dpaResult.THOROUGHFARE_NAME
                );

                if (streetLine) {
                    lines.push(streetLine);
                }
            } else if (buildingName) {
                const subBuildingIsNumeric = this.#isNumericStyleBuildingName(subBuildingName);

                const premiseLine = this.#joinNonEmpty(
                    [subBuildingName, buildingName],
                    subBuildingIsNumeric ? ' ' : ', '
                );

                if (premiseLine) {
                    lines.push(premiseLine);
                }

                const streetLine = this.#buildStreetLine(
                    buildingNumber,
                    dpaResult.DEPENDENT_THOROUGHFARE_NAME,
                    dpaResult.THOROUGHFARE_NAME
                );

                if (streetLine) {
                    lines.push(streetLine);
                }
            } else {
                // Sub-building only, with building number and street
                const streetParts = this.#buildStreetLine(
                    buildingNumber,
                    dpaResult.DEPENDENT_THOROUGHFARE_NAME,
                    dpaResult.THOROUGHFARE_NAME
                );

                const premiseLine = this.#joinNonEmpty([subBuildingName, streetParts], ', ');

                if (premiseLine) {
                    lines.push(premiseLine);
                }
            }
        } else if (isNumericBuildingName) {
            const streetLine = this.#buildStreetLine(
                buildingName,
                dpaResult.DEPENDENT_THOROUGHFARE_NAME,
                dpaResult.THOROUGHFARE_NAME
            );

            if (streetLine) {
                lines.push(streetLine);
            }
        } else if (buildingName) {
            lines.push(buildingName);

            const streetLine = this.#buildStreetLine(
                buildingNumber,
                dpaResult.DEPENDENT_THOROUGHFARE_NAME,
                dpaResult.THOROUGHFARE_NAME
            );

            if (streetLine) {
                lines.push(streetLine);
            }
        } else {
            const streetLine = this.#buildStreetLine(
                buildingNumber,
                dpaResult.DEPENDENT_THOROUGHFARE_NAME,
                dpaResult.THOROUGHFARE_NAME
            );

            if (streetLine) {
                lines.push(streetLine);
            }
        }

        return {
            addressLine1: lines[0] || '',
            addressLine2: lines[1] || '',
            addressLine3: lines[2] || '',
            town: dpaResult.POST_TOWN || '',
            postcode: dpaResult.POSTCODE || '',
            county: '',
            country: this.config.DEFAULTS.COUNTRY,
            countryCode: this.defaultCountryCode,
            UPRN: dpaResult.UPRN
        };
    }

    #joinNonEmpty(parts, separator) {
        return parts.filter(Boolean).join(separator);
    }

    #buildStreetLine(firstPart, dependentThoroughfareName, thoroughfareName) {
        return this.#joinNonEmpty(
            [firstPart, dependentThoroughfareName, thoroughfareName],
            ' '
        );
    }

    #isNumericStyleBuildingName(buildingName) {
        return this.#buildingNameIsNumber(buildingName)
            || this.#buildingNameIsNumberWithSuffix(buildingName)
            || this.#buildingNameIsNumberRange(buildingName);
    }

    #buildingNameIsNumberWithSuffix(buildingName) {
        if (buildingName.length < 2) return false;

        const lastChar = buildingName.charAt(buildingName.length - 1).toLowerCase();
        const prefix = buildingName.slice(0, -1);

        return lastChar >= 'a' && lastChar <= 'z' && prefix.split('').every(ch => ch >= '0' && ch <= '9');
    }

    #buildingNameIsNumberRange(buildingName) {
        const parts = buildingName.split('-');
        if (parts.length !== 2) return false;

        const left = parts[0].trim();
        const right = parts[1].trim();

        if (left === '' || right === '') return false;

        const startsWithDigit = (str) => str.charAt(0) >= '0' && str.charAt(0) <= '9';

        return startsWithDigit(left) && startsWithDigit(right);
    }

    #buildingNameIsNumber(buildingName) {
        return buildingName.length > 0 && buildingName.split('').every(ch => ch >= '0' && ch <= '9');
    }

    #splitBuildingNameAndNumber(buildingName) {
        const lastSpaceIndex = buildingName.lastIndexOf(' ');
        if (lastSpaceIndex === -1) return null;

        const namePart = buildingName.substring(0, lastSpaceIndex).trim();
        const numberPart = buildingName.substring(lastSpaceIndex + 1).trim();

        if (namePart === '' || numberPart === '') return null;

        if (this.#buildingNameIsNumber(numberPart) ||
            this.#buildingNameIsNumberWithSuffix(numberPart) ||
            this.#buildingNameIsNumberRange(numberPart)) {
            return [namePart, numberPart];
        }

        return null;
    }

    mapFromManualEntry(addressLine1, addressLine2, addressLine3, town, countyOrRegion, postcode, country, countryCode) {
        const isUK = country === this.config.DEFAULTS.COUNTRY;
        return {
            addressLine1,
            addressLine2: addressLine2 || '',
            addressLine3: addressLine3 || '',
            town,
            ...(isUK ? { county: countyOrRegion || '' } : { region: countyOrRegion || '' }),
            postcode,
            country,
            countryCode: countryCode || ''
        };
    }

    addressesMatch(a, b) {
        const fields = ['addressLine1', 'addressLine2', 'addressLine3', 'town', 'county', 'country', 'postcode'];
        return fields.every(field => (a[field] || '') === (b[field] || ''));
    }
}

export { AddressMapper };