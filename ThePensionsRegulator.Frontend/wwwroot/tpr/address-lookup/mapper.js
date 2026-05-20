class AddressMapper {
    constructor(config) {
        this.config = config
    }

    mapFromInput(originalInputs) {
        const get = (attribute) => originalInputs.find(x => x.dataAddressLookup === attribute);
        const getValue = (attribute) => get(attribute)?.value || '';
        return {
            addressLine1: getValue(this.config.DATA_ATTRIBUTES.ADDRESS_LINE_1),
            addressLine2: getValue(this.config.DATA_ATTRIBUTES.ADDRESS_LINE_2),
            town: getValue(this.config.DATA_ATTRIBUTES.TOWN_OR_CITY),
            county: getValue(this.config.DATA_ATTRIBUTES.COUNTY),
            postcode: getValue(this.config.DATA_ATTRIBUTES.POSTCODE) || getValue(this.config.DATA_ATTRIBUTES.POSTCODE_INTERNATIONAL),
            country: get(this.config.DATA_ATTRIBUTES.COUNTRY).selectedOptionLabel,
            countryCode: getValue(this.config.DATA_ATTRIBUTES.COUNTRY),
            UPRN: getValue(this.config.DATA_ATTRIBUTES.UPRN)
        };
    }

    mapFromDpaResult(dpaResult) {
        const addressLines = this.#buildAddressLines(dpaResult);
        const address = {
            addressLine1: addressLines[0] || '',
            addressLine2: addressLines[1] || '',
            addressLine3: addressLines[2] || '',
            town: dpaResult.POST_TOWN,
            postcode: dpaResult.POSTCODE,
            county: '',
            country: this.config.DEFAULTS.COUNTRY,
            UPRN: dpaResult.UPRN
        };

        return address;
    }

    #buildAddressLines(dpaResult) {
        const addressLines = [];

        if (dpaResult.ORGANISATION_NAME) {
            addressLines.push(dpaResult.ORGANISATION_NAME);
        }

        let buildingName = dpaResult.BUILDING_NAME || '';
        let buildingNumber = dpaResult.BUILDING_NUMBER || '';

        if (!buildingNumber && buildingName) {
            if (this.#buildingNameIsNumberWithSuffix(buildingName) || this.#buildingNameIsNumberRange(buildingName) || this.#buildingNameIsNumber(buildingName)) {
                buildingNumber = buildingName;
                buildingName = '';
            } else {
                const parts = this.#splitBuildingNameAndNumber(buildingName);
                if (parts) {
                    buildingName = parts[0];
                    buildingNumber = parts[1];
                }
            }
        }

        if (buildingName) {
            if (dpaResult.SUB_BUILDING_NAME) {
                if (this.#buildingNameIsNumber(dpaResult.SUB_BUILDING_NAME) || this.#buildingNameIsNumberRange(dpaResult.SUB_BUILDING_NAME)) {
                    addressLines.push(`${dpaResult.SUB_BUILDING_NAME} ${buildingName}`);
                } else {
                    addressLines.push(`${dpaResult.SUB_BUILDING_NAME}, ${buildingName}`);
                }
            } else {
                addressLines.push(buildingName);
            }
        }

        if (buildingNumber && dpaResult.THOROUGHFARE_NAME) {
            addressLines.push(`${buildingNumber} ${dpaResult.THOROUGHFARE_NAME}`);
        } else if (dpaResult.THOROUGHFARE_NAME && !addressLines.some(line => line.includes(dpaResult.THOROUGHFARE_NAME))) {
            addressLines.push(dpaResult.THOROUGHFARE_NAME);
        }

        return addressLines;
    }

    #buildingNameIsNumberWithSuffix(buildingName){
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


    mapFromManualEntry(addressLine1, addressLine2, town, countyOrRegion, postcode, country, countryCode) {
        const isUK = country === this.config.DEFAULTS.COUNTRY;
        return {
            addressLine1: addressLine1,
            addressLine2: addressLine2 || '',
            town: town,
            ...(isUK ? { county: countyOrRegion || '' } : { region: countyOrRegion || '' }),
            postcode: postcode,
            country: country,
            countryCode: countryCode || ''
        };
    }

    addressesMatch(a, b) {
        return (a.addressLine1 || '') === (b.addressLine1 || '')
            && (a.addressLine2 || '') === (b.addressLine2 || '')
            && (a.town || '') === (b.town || '')
            && (a.county || '') === (b.county || '')
            && (a.country || '') === (b.country || '')
            && (a.postcode || '') === (b.postcode || '');
    }
}


export { AddressMapper };