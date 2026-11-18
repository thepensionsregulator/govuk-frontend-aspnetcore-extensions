
"use strict";

const instances = new WeakMap();
const PLACEHOLDER = '{remainingTime}';
const ONE_SECOND_MS = 1000;
const SECONDS_PER_MINUTE = 60;

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

function getTimeParts(ms) {
    const totalSeconds = Math.ceil(ms / ONE_SECOND_MS);

    return {
        minutes: Math.floor(totalSeconds / SECONDS_PER_MINUTE),
        seconds: totalSeconds % SECONDS_PER_MINUTE
    };
}

function formatUnit(value, unit) {
    return `${value} ${unit}${value === 1 ? '' : 's'}`;
}

function formatRemainingTime(ms) {
    const { minutes, seconds } = getTimeParts(ms);
    const secondsText = formatUnit(seconds, 'second');

    return minutes > 0
        ? `${formatUnit(minutes, 'minute')} ${secondsText}`
        : secondsText;
}

function createCountdown(tagName) {
    const element = document.createElement(tagName);
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
                const countdown = createCountdown('span');
                countdowns.push(countdown);
                fragment.append(countdown);
            }

            fragment.append(text);
        });

        node.replaceWith(fragment);
    }

    if (countdowns.length === 0) {
        const countdown = createCountdown('p');
        countdown.className = 'govuk-body';

        const button = dialog.querySelector('[data-session-timeout-extend]');
        const before = content === dialog && button?.parentNode === content
            ? button
            : null;

        content.insertBefore(countdown, before);
        countdowns.push(countdown);
    }

    return {
        countdowns,

        restore() {
            for (const countdown of countdowns) {
                if (matchingNodes.length) {
                    countdown.replaceWith(
                        document.createTextNode(PLACEHOLDER)
                    );
                } else {
                    countdown.remove();
                }
            }
        }
    };
}

function validateConfig(config) {
    if (!config || typeof config.extendSession !== 'function') {
        throw new TypeError('An extendSession callback is required.');
    }

    for (const name of ['timeoutMs', 'extensionMs', 'warningMs']) {
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

    if (
        config.formatRemainingTime !== undefined &&
        typeof config.formatRemainingTime !== 'function'
    ) {
        throw new TypeError('formatRemainingTime must be a function.');
    }
}

/**
 * @param {HTMLDialogElement} dialog
 * @param {{
 *   timeoutMs: number,
 *   extensionMs: number,
 *   warningMs: number,
 *   formatRemainingTime?: (remainingMs: number) => string,
 *   extendSession: (url: string | undefined) => boolean | Promise<boolean>
 * }} config
 */
export function initSessionTimeout(dialog, config) {
    if (instances.has(dialog)) {
        return instances.get(dialog);
    }

    validateConfig(config);

    const button = dialog.querySelector('[data-session-timeout-extend]');
    const { countdowns, restore } = prepareCountdowns(dialog);
    const formatTime = config.formatRemainingTime || formatRemainingTime;

    let expiryTime = getExpiryTime(config.timeoutMs);
    let timer;
    let extending = false;
    let destroyed = false;

    function updateCountdown() {
        if (destroyed) return;

        clearTimeout(timer);

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
        }
    }

    async function handleExtension() {
        if (destroyed || extending) return;

        extending = true;
        button.disabled = true;

        try {
            const succeeded = await config.extendSession(
                button.dataset.sessionTimeoutExtendUrl
            );

            if (succeeded !== true) {
                throw new Error('Session extension was not confirmed.');
            }

            if (destroyed) return;

            expiryTime = getExpiryTime(config.extensionMs);

            if (dialog.open) {
                dialog.close();
            }

            updateCountdown();

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
            extending = false;

            if (!destroyed) {
                button.disabled = false;
            }
        }
    }

    function destroy() {
        if (destroyed) return;

        destroyed = true;

        clearTimeout(timer);

        if (extending) {
            button.disabled = false;
        }

        button?.removeEventListener('click', handleExtension);
        document.removeEventListener('visibilitychange', updateCountdown);
        window.removeEventListener('pageshow', updateCountdown);

        restore();
        instances.delete(dialog);
    }

    updateCountdown();

    button?.addEventListener('click', handleExtension);
    document.addEventListener('visibilitychange', updateCountdown);
    window.addEventListener('pageshow', updateCountdown);

    const controller = {
        get expiryTime() {
            return expiryTime;
        },
        destroy
    };

    instances.set(dialog, controller);

    return controller;
}
