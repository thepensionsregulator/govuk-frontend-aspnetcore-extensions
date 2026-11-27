import { initSessionTimeout } from '/ThePensionsRegulator.Frontend/js/tpr-session-timeout-message.js';

const dialog = document.querySelector('[data-module="tpr-session-timeout"]');

if (dialog) {
    initSessionTimeout(dialog, {
        // Preview only: no server-side session is changed.
        extendSession: () => true
    });
}
