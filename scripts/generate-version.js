#!/usr/bin/env node

import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const rootDir = path.join(__dirname, '..');

/**
 * Extracts the Version property from Directory.Build.props
 */
function getVersionFromProps() {
    const propsPath = path.join(rootDir, 'Directory.Build.props');
    const content = fs.readFileSync(propsPath, 'utf-8');

    // Extract version from <Version>...</Version>
    const match = content.match(/<Version>(.*?)<\/Version>/);
    if (!match) {
        throw new Error('Could not find <Version> property in Directory.Build.props');
    }

    return match[1];
}

/**
 * Generates the package-version.generated.ts file in a backoffice directory
 * to use as a cache-busting parameter for backoffice customisation
 */
function generateVersionFile(version, backofficeDir) {
    const filePath = path.join(rootDir, backofficeDir, 'src', 'package-version.generated.ts');
    const dirPath = path.dirname(filePath);

    // Ensure src directory exists
    if (!fs.existsSync(dirPath)) {
        fs.mkdirSync(dirPath, { recursive: true });
    }

    const content = `export const PACKAGE_VERSION = '${version}';\n`;
    fs.writeFileSync(filePath, content, 'utf-8');
    console.log(`Generated: ${filePath}`);
}

function main() {
    try {
        const version = getVersionFromProps();
        console.log(`Version from Directory.Build.props: ${version}`);

        // Generate version files in both backoffice directories
        const backofficeDirectories = [
            'ThePensionsRegulator.GovUk.Frontend.Umbraco/Backoffice',
            'ThePensionsRegulator.Frontend.Umbraco/Backoffice'
        ];

        for (const dir of backofficeDirectories) {
            generateVersionFile(version, dir);
        }

        console.log('Version files for TypeScript build generated successfully');
    } catch (error) {
        console.error('Error generating version files:', error.message);
        process.exit(1);
    }
}

main();
