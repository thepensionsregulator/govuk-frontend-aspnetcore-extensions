import { ADDRESS_LOOKUP_CONFIG } from "./config.js";

/**
 * Resolves field metadata in priority order:
 *   1. Value captured from the original rendered input/label (server-side customisation)
 *   2. data-* override on the host <tpr-address-lookup> element
 *   3. Default in ADDRESS_LOOKUP_CONFIG.FIELDS[key]
 */
export class FieldDefaults {
    constructor(container, originalInputs) {
        this.container = container;
        this.originalInputs = originalInputs;
    }

    #captured(key) {
        return this.originalInputs.find(i => i.dataAddressLookup === key);
    }

    #fromAttr(key, suffix) {
        return this.container.getAttribute(`data-address-lookup-${key}-${suffix}`) || undefined;
    }

    #defaults(key) {
        return ADDRESS_LOOKUP_CONFIG.FIELD_DEFAULTS[key] || {};
    }

    label(key) {
        return this.#captured(key)?.label
            ?? this.#fromAttr(key, "label")
            ?? this.#defaults(key).label;
    }

    labelSize(key) {
        return this.#captured(key)?.labelSize
            ?? this.#fromAttr(key, "label-size")
            ?? this.#defaults(key).labelSize
    }

    requiredMessage(key) {
        return this.#captured(key)?.requiredMessage
            ?? this.#fromAttr(key, "required")
            ?? this.#defaults(key).validation?.required?.message;
    }

    maxLengthMessage(key) {
        return this.#captured(key)?.maxLengthMessage
            ?? this.#fromAttr(key, "maxlength")
            ?? this.#defaults(key).validation?.maxLength?.message;
    }

    maxLength(key) {
        return this.#captured(key)?.maxLength
            ?? this.#defaults(key).validation?.maxLength?.value;
    }

    patternMessage(key) {
        return this.#captured(key)?.patternMessage
            ?? this.#fromAttr(key, "pattern")
            ?? this.#defaults(key).validation?.pattern?.message;
    }

    pattern(key) {
        return this.#defaults(key).validation?.pattern?.value;
    }

    width(key) {
        return this.#defaults(key).width;
    }
}