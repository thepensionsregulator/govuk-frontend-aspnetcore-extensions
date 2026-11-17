import { initSessionTimeout } from '/ThePensionsRegulator.Frontend/js/tpr-session-timeout-message.js';

const preview = document.querySelector('[data-session-timeout-preview]');
const dialog = preview?.querySelector('.tpr-session-timeout-warning');

if (dialog) {
    initSessionTimeout(dialog, {
        timeoutMs: Number(preview.dataset.timeoutMs),
        extensionMs: Number(preview.dataset.extensionMs),
        warningMs: Number(preview.dataset.warningMs),
        formatRemainingTime: (remainingMs) => {
            const seconds = Math.ceil(remainingMs / 1000);
            const minutes = Math.ceil(seconds / 60);
            return seconds > 60
                ? `${minutes} ${minutes === 1 ? 'minute' : 'minutes'}`
                : `${seconds} ${seconds === 1 ? 'second' : 'seconds'}`;
        },
        // Preview only: no server-side session is changed.
        extendSession: () => true
    });
}
