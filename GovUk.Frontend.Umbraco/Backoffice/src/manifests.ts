import { manifests as blockManifests } from './blocks/manifests';
import { manifests as propertyEditorManifests } from './property-editors/manifests';

export const manifests: Array<UmbExtensionManifest> = [
    ...blockManifests,
    ...propertyEditorManifests
];