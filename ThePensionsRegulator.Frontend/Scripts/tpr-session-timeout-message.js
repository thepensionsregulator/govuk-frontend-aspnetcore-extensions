
"use strict";

const instances = new WeakMap();
const PLACEHOLDER = '{remainingTime}';
const ONE_SECOND_MS = 1000;
const SECONDS_PER_MINUTE = 60;
const DEFAULT_EXTENSION_REQUEST_TIMEOUT_MS = 10000;
const MAX_TIMER_DELAY_MS = 2147483647;

function getExpiryTime(durationMs) {
    return Date.now() + durationMs;
}

function getRemainingTime(expiryTime) {
    return Math.max(0, expiryTime - Date.now());
}

function getNextUpdateDelay(remainingMs, warningMs) {
    const timeUntilWarning = remainingMs - warningMs;

    if (timeUntilWarning > 0) {
        return Math.min(ONE_SECOND_MS, timeUntilWarning);
    }

    return Math.min(ONE_SECOND_MS, remainingMs);
}

function formatUnit(value, unit) {
    return `${value} ${unit}${value === 1 ? '' : 's'}`;
}

function formatRemainingTime(ms) {
    const seconds = Math.ceil(ms / ONE_SECOND_MS);

    return seconds > SECONDS_PER_MINUTE
        ? formatUnit(Math.ceil(seconds / SECONDS_PER_MINUTE), 'minute')
        : formatUnit(seconds, 'second');
}

function createCountdown() {
    const element = document.createElement('span');
    element.setAttribute('data-session-timeout-countdown', '');
    element.setAttribute('role', 'timer');
    element.setAttribute('aria-live', 'off');
    return element;
}

function prepareCountdowns(dialog) {
    const content = dialog.querySelector('[data-session-timeout-content]') || dialog;
    const walker = document.createTreeWalker(content, NodeFilter.SHOW_TEXT);
    const matchingNodes = [];
    const countdowns = [];

    while (walker.nextNode()) {
        if (walker.currentNode.textContent.includes(PLACEHOLDER)) {
            matchingNodes.push(walker.currentNode);
        }
    }

    for (const node of matchingNodes) {
        const fragment = document.createDocumentFragment();

        node.textContent.split(PLACEHOLDER).forEach((text, index) => {
            if (index > 0) {
                const countdown = createCountdown();
                countdowns.push(countdown);
                fragment.append(countdown);
            }

            fragment.append(text);
        });

        node.replaceWith(fragment);
    }

    return {
        countdowns,

        restore() {
            for (const countdown of countdowns) {
                countdown.replaceWith(
                    document.createTextNode(PLACEHOLDER)
                );
            }
        }
    };
}

function validateConfig(config, hasExtensionButton) {
    if (hasExtensionButton && typeof config.extendSession !== 'function') {
        throw new TypeError('An extendSession callback is required when an extension button is present.');
    }

    const durations = hasExtensionButton
        ? ['timeoutMs', 'extensionMs', 'warningMs']
        : ['timeoutMs', 'warningMs'];
    for (const name of durations) {
        const value = config[name];

        if (
            !Number.isFinite(value) ||
            value < 0 ||
            (name !== 'warningMs' && value === 0)
        ) {
            throw new RangeError(
                `${name} must be a finite duration in milliseconds; only warningMs can be zero.`
            );
        }
    }

    if (config.warningMs > config.timeoutMs) {
        throw new RangeError('warningMs must not exceed timeoutMs.');
    }

    if (hasExtensionButton && (
        !Number.isFinite(config.extensionRequestTimeoutMs) ||
        config.extensionRequestTimeoutMs <= 0 ||
        config.extensionRequestTimeoutMs > MAX_TIMER_DELAY_MS)) {
        throw new RangeError('extensionRequestTimeoutMs must be greater than zero and no greater than 2147483647 milliseconds.');
    }

    if (
        config.formatRemainingTime !== undefined &&
        typeof config.formatRemainingTime !== 'function'
    ) {
        throw new TypeError('formatRemainingTime must be a function.');
    }

    if (config.expiryUrl !== undefined && config.expiryUrl !== null) {
        if (typeof config.expiryUrl !== 'string' || !config.expiryUrl.trim()) {
            throw new TypeError('expiryUrl must be a non-empty HTTP(S) URL.');
        }
        const url = new URL(config.expiryUrl, document.baseURI);
        if (url.protocol !== 'http:' && url.protocol !== 'https:') {
            throw new TypeError('expiryUrl must use HTTP or HTTPS.');
        }
    }
}

function readDuration(value) {
    return value === undefined || !value.trim() ? NaN : Number(value);
}

/**
 * @param {HTMLDialogElement} dialog
 * @param {{
 *   timeoutMs?: number,
 *   extensionMs?: number,
 *   warningMs?: number,
 *   expiryUrl?: string | null,
 *   extensionRequestTimeoutMs?: number,
 *   formatRemainingTime?: (remainingMs: number) => string,
 *   extendSession?: (url: string | undefined, signal: AbortSignal) => boolean | { expiryTime: number } | Promise<boolean | { expiryTime: number }>
 * }} [config]
 */
export function initSessionTimeout(dialog, config) {
    if (instances.has(dialog)) {
        return instances.get(dialog);
    }

    config = {
        timeoutMs: readDuration(dialog.dataset.timeoutMs),
        extensionMs: readDuration(dialog.dataset.extensionMs),
        warningMs: readDuration(dialog.dataset.warningMs),
        expiryUrl: dialog.dataset.sessionTimeoutExpiryUrl || undefined,
        ...config,
        extensionRequestTimeoutMs: config?.extensionRequestTimeoutMs ?? DEFAULT_EXTENSION_REQUEST_TIMEOUT_MS
    };
    const button = dialog.querySelector('[data-session-timeout-extend]');
    validateConfig(config, button !== null);

    const { countdowns, restore } = prepareCountdowns(dialog);
    const formatTime = config.formatRemainingTime || formatRemainingTime;

    let expiryTime = getExpiryTime(config.timeoutMs);
    let timer;
    let extending = false;
    let destroyed = false;
    let expired = false;
    let extensionRequest;
    let expiryRevision = 0;

    /** @param {number} timestamp Absolute expiry in Unix milliseconds. */
    function setExpiryTime(timestamp) {
        if (destroyed) {
            throw new Error('Session timeout component was destroyed.');
        }
        if (!Number.isFinite(timestamp)) {
            throw new RangeError('expiryTime must be a finite Unix timestamp in milliseconds.');
        }

        if (timestamp !== expiryTime) {
            expired = false;
        }
        expiryTime = timestamp;
        expiryRevision++;

        if (getRemainingTime(expiryTime) > config.warningMs && dialog.open) {
            dialog.close();
        }
        updateCountdown();
    }

    function updateCountdown() {
        if (destroyed) return;

        clearTimeout(timer);

        if (extensionRequest && Date.now() >= extensionRequest.deadline) {
            extensionRequest.cancel(extensionRequest.timeoutError);
        }

        const remaining = getRemainingTime(expiryTime);
        const text = formatTime(remaining);

        for (const countdown of countdowns) {
            if (countdown.textContent !== text) {
                countdown.textContent = text;
            }
        }

        if (remaining <= config.warningMs && !dialog.open) {
            dialog.showModal();
        }

        if (remaining > 0) {
            timer = setTimeout(
                updateCountdown,
                getNextUpdateDelay(remaining, config.warningMs)
            );
        } else if (!expired && !extending) {
            expired = true;
            const navigate = dialog.dispatchEvent(new CustomEvent('tpr-session-timeout:expired', {
                bubbles: true,
                cancelable: true
            }));
            if (navigate && config.expiryUrl) {
                window.location.assign(config.expiryUrl);
            }
        }
    }

    async function handleExtension() {
        if (destroyed || extending) return;

        extending = true;
        button.disabled = true;
        const abortController = new AbortController();
        const request = {
            deadline: getExpiryTime(config.extensionRequestTimeoutMs),
            timeoutError: new Error('Session extension request timed out.'),
            expiryRevision
        };
        extensionRequest = request;
        const timeout = new Promise((resolve, reject) => {
            request.cancel = reason => {
                reject(reason);
                abortController.abort(reason);
            };
            request.timer = setTimeout(() => request.cancel(request.timeoutError), config.extensionRequestTimeoutMs);
        });

        try {
            const succeeded = await Promise.race([
                config.extendSession(button.dataset.sessionTimeoutExtendUrl, abortController.signal),
                timeout
            ]);

            if (Date.now() >= request.deadline) {
                abortController.abort(request.timeoutError);
                throw request.timeoutError;
            }

            if (succeeded !== true && (succeeded === null || typeof succeeded !== 'object')) {
                throw new Error('Session extension was not confirmed.');
            }

            if (destroyed || request.expiryRevision !== expiryRevision) return;

            setExpiryTime(succeeded === true ? getExpiryTime(config.extensionMs) : succeeded.expiryTime);
        } catch (error) {
            if (!destroyed) {
                console.error('Failed to extend session:', error);

                dialog.dispatchEvent(
                    new CustomEvent('tpr-session-timeout:error', {
                        detail: error,
                        bubbles: true
                    })
                );
            }
        } finally {
            clearTimeout(request.timer);
            extensionRequest = undefined;
            extending = false;

            if (!destroyed) {
                button.disabled = false;
                updateCountdown();
            }
        }
    }

    function destroy() {
        if (destroyed) return;

        destroyed = true;

        clearTimeout(timer);
        if (extensionRequest) {
            clearTimeout(extensionRequest.timer);
            extensionRequest.cancel(new Error('Session timeout component was destroyed.'));
        }

        if (extending) {
            button.disabled = false;
        }

        button?.removeEventListener('click', handleExtension);
        document.removeEventListener('visibilitychange', updateCountdown);
        window.removeEventListener('pageshow', updateCountdown);

        restore();
        instances.delete(dialog);

        if (dialog.open) {
            dialog.close();
        }
    }

    updateCountdown();

    button?.addEventListener('click', handleExtension);
    document.addEventListener('visibilitychange', updateCountdown);
    window.addEventListener('pageshow', updateCountdown);

    const controller = {
        get expiryTime() {
            return expiryTime;
        },
        setExpiryTime,
        destroy
    };

    instances.set(dialog, controller);

    return controller;
}
