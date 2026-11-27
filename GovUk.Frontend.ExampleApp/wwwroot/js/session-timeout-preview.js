import { initSessionTimeout } from '/ThePensionsRegulator.Frontend/js/tpr-session-timeout-message.js';

const dialog = document.querySelector('[data-module="tpr-session-timeout"]');

if (dialog) {
    initSessionTimeout(dialog, {
        // This demo simulates extension; it does not change a server-side session.
        extendSession: () => true
    });
}
