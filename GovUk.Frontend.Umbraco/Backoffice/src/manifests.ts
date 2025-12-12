import { manifests as blockManifests } from './blocks/manifests';
import { manifests as propertyEditorManifests } from './property-editors/manifests';
import { manifests as tipTapManifests } from './tiptap/manifests';

export const manifests: Array<UmbExtensionManifest> = [
    ...blockManifests,
    ...propertyEditorManifests,
    ...tipTapManifests
];