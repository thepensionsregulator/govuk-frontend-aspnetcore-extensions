import { initSessionTimeout } from '/ThePensionsRegulator.Frontend/js/tpr-session-timeout-message.js';

const preview = document.querySelector('[data-session-timeout-preview]');
const dialog = preview?.querySelector('.tpr-session-timeout-warning');

if (dialog) {
    initSessionTimeout(dialog, {
        timeoutMs: Number(preview.dataset.timeoutMs),
        extensionMs: Number(preview.dataset.extensionMs),
        warningMs: Number(preview.dataset.warningMs),
        // Preview only: no server-side session is changed.
        extendSession: () => true
    });
}
