"use strict";

const instances = new WeakMap();

function formatRemainingTime(remaining) {
    const totalSeconds = Math.ceil(remaining / 1000);
    const minutes = Math.floor(totalSeconds / 60);
    const seconds = totalSeconds % 60;
    const secondsText = `${seconds} ${seconds === 1 ? 'second' : 'seconds'}`;
    return minutes > 0
        ? `${minutes} ${minutes === 1 ? 'minute' : 'minutes'} ${secondsText}`
        : secondsText;
}

/**
 * @param {HTMLDialogElement} dialog
 * @param {{ timeoutMs: number, extensionMs: number, warningMs: number,
 *   formatRemainingTime?: (remainingMs: number) => string,
 *   extendSession: (url: string | undefined) => boolean | Promise<boolean> }} config
 */
export function initSessionTimeout(dialog, config) {
    if (instances.has(dialog)) return instances.get(dialog);

    if (!config || typeof config.extendSession !== 'function') {
        throw new TypeError('An extendSession callback is required.');
    }
    for (const name of ['timeoutMs', 'extensionMs', 'warningMs']) {
        if (!Number.isFinite(config[name]) || config[name] < 0 || (name !== 'warningMs' && config[name] === 0)) {
            throw new RangeError(`${name} must be a finite duration in milliseconds; only warningMs can be zero.`);
        }
    }
    if (config.warningMs > config.timeoutMs) {
        throw new RangeError('warningMs must not exceed timeoutMs.');
    }
    if (config.formatRemainingTime !== undefined && typeof config.formatRemainingTime !== 'function') {
        throw new TypeError('formatRemainingTime must be a function.');
    }
    const formatTime = config.formatRemainingTime || formatRemainingTime;

    let expiryTime = Date.now() + config.timeoutMs;
    let timer;
    let extending = false;
    let destroyed = false;
    const content = dialog.querySelector('[data-session-timeout-content]') || dialog;
    const placeholder = '{remainingTime}';
    const countdowns = [];
    const walker = document.createTreeWalker(content, NodeFilter.SHOW_TEXT);
    const textNodes = [];
    while (walker.nextNode()) {
        if (walker.currentNode.textContent.includes(placeholder)) {
            textNodes.push(walker.currentNode);
        }
    }

    function createCountdown(tagName) {
        const countdown = document.createElement(tagName);
        countdown.setAttribute('data-session-timeout-countdown', '');
        countdown.setAttribute('role', 'timer');
        countdown.setAttribute('aria-live', 'off');
        countdowns.push(countdown);
        return countdown;
    }

    for (const textNode of textNodes) {
        const fragment = document.createDocumentFragment();
        const parts = textNode.textContent.split(placeholder);
        parts.forEach((part, index) => {
            if (index > 0) fragment.append(createCountdown('span'));
            fragment.append(document.createTextNode(part));
        });
        textNode.replaceWith(fragment);
    }

    const button = dialog.querySelector('[data-session-timeout-extend]');
    if (countdowns.length === 0) {
        const countdown = createCountdown('p');
        countdown.className = 'govuk-body';
        content.insertBefore(countdown, content === dialog ? button : null);
    }
    async function extendSession() {
        if (destroyed || extending) return;
        extending = true;
        button.disabled = true;
        try {
            const succeeded = await config.extendSession(button.dataset.sessionTimeoutExtendUrl);
            if (succeeded !== true) {
                throw new Error('Session extension was not confirmed.');
            }
        } catch (error) {
            console.error('Failed to extend session:', error);
            dialog.dispatchEvent(new CustomEvent('tpr-session-timeout:error', { detail: error, bubbles: true }));
            return;
        } finally {
            extending = false;
            if (!destroyed) button.disabled = false;
        }
        if (destroyed) return;
        expiryTime = Date.now() + config.extensionMs;
        if (dialog.open) dialog.close();
        update();
    }
    button?.addEventListener('click', extendSession);

    function update() {
        clearTimeout(timer);
        const remaining = Math.max(0, expiryTime - Date.now());
        const formattedTime = formatTime(remaining);
        countdowns.forEach(countdown => {
            if (countdown.textContent !== formattedTime) countdown.textContent = formattedTime;
        });
        if (remaining <= config.warningMs && !dialog.open) {
            dialog.showModal();
        }
        if (remaining > 0) {
            const untilWarning = remaining - config.warningMs;
            timer = setTimeout(update, Math.min(1000, remaining, untilWarning > 0 ? untilWarning : remaining));
        }
    }
    update();
    document.addEventListener('visibilitychange', update);
    window.addEventListener('pageshow', update);

    const controller = {
        get expiryTime() { return expiryTime; },
        destroy() {
            if (destroyed) return;
            destroyed = true;
            if (extending) button.disabled = false;
            clearTimeout(timer);
            button?.removeEventListener('click', extendSession);
            document.removeEventListener('visibilitychange', update);
            window.removeEventListener('pageshow', update);
            countdowns.forEach(countdown => {
                if (textNodes.length > 0) countdown.replaceWith(document.createTextNode(placeholder));
                else countdown.remove();
            });
            instances.delete(dialog);
        }
    };
    instances.set(dialog, controller);
    return controller;
}
