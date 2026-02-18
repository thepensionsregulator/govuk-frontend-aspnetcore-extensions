import '@testing-library/jest-dom';

import { jest } from '@jest/globals';
import { ADDRESS_LOOKUP_CONFIG } from "../wwwroot/tpr/address-lookup/config.js";

describe('UK postcode validation', () => {
    const postcodeRegex = new RegExp(ADDRESS_LOOKUP_CONFIG.PATTERNS.POSTCODE);

    describe('valid postcodes', () => {
        const validPostcodes = [
            // Standard formats
            'SW1A 1AA',     // Westminster
            'EC1A 1BB',     // City of London
            'W1A 0AX',      // BBC
            'M1 1AE',       // Manchester
            'B33 8TH',      // Birmingham
            'CR2 6XH',      // Croydon
            'DN55 1PT',     // Doncaster

            // Without spaces
            'SW1A1AA',
            'EC1A1BB',
            'M11AE',
            'B338TH',

            // Lowercase
            'sw1a 1aa',
            'ec1a1bb',
            'm1 1ae',

            // Mixed case
            'Sw1A 1aA',
            'eC1a 1Bb',

            // With extra spaces
            ' SW1A 1AA ',
            'SW1A  1AA',

            // Various area formats
            'L1 8JQ',       // Single letter area, single digit district
            'B1 1AA',       // Single letter area
            'E1W 1AA',      // Single letter area with letter in district
            'N1C 4AA',      // Single letter area with letter in district
            'W1J 7NT',      // West End
            'SE1P 4AA',     // Double letter area with letter in district
            'EH99 1SP',     // Edinburgh (double digit district)

            // allow dashes seperating the postcode
            'SW1A-1AA'
        ];

        test.each(validPostcodes)('should accept valid postcode: %s', (postcode) => {
            expect(postcode).toMatch(postcodeRegex);
        });
    });

    describe('invalid postcodes', () => {
        const invalidPostcodes = [
            '',             // Empty
            '   ',          // Whitespace only
            'INVALID',      // Not a postcode format
            '12345',        // US zip code format
            'SW1A 1A',      // Missing final letter
            'SW1A 1AAA',    // Extra letter at end
            'SW1A1',        // Missing outward code
            'AAA 1AA',      // Invalid - 3 letters in area
            '1A1 1AA',      // Starts with number
            'SW1A 1AI',     // Invalid final letter (I not allowed)
            'SW1A 1AO',     // Invalid final letter (O not allowed)
            'SW1A 1AK',     // Invalid final letter (K not allowed)
            'SW1A 1AM',     // Invalid final letter (M not allowed)
            'SW1A 1AV',     // Invalid final letter (V not allowed)
            'SW1A!1AV',     // Contains invalid character
        ];

        test.each(invalidPostcodes)('should reject invalid postcode: %s', (postcode) => {
            expect(postcode).not.toMatch(postcodeRegex);
        });
    });
});