
(function () {
    "use strict";

    const warnings = new Map();

    class WarningController {
        constructor(dialog) {
            this.dialog = dialog;
            this.previouslyFocused = null;
            this.error = dialog.querySelector(".govuk-error-message[role='alert']");

            dialog.addEventListener("cancel", function (event) {
                event.preventDefault();
            });
        }

        show() {
            if (this.dialog.open) {
                return;
            }

            this.previouslyFocused = document.activeElement;
            this.clearError();
            this.dialog.showModal(); 
            const heading = this.dialog.querySelector("h2[tabindex='-1']");
            if (heading) {
                heading.focus();
            }
        }

        hide() {
            if (!this.dialog.open) {
                return;
            }

            this.dialog.close();
            this.clearError();
            const previous = this.previouslyFocused;
            this.previouslyFocused = null;
            if (previous && previous.isConnected && typeof previous.focus === "function") {
                previous.focus();
            }
        }

        setError(message) {
            if (!this.error) {
                return;
            }
            this.error.textContent = message;
            this.error.hidden = !message;
        }

        clearError() {
            this.setError("");
        }
    }

    function initialise() {
        document.querySelectorAll("dialog[data-tpr-session-timeout='true']").forEach(function (dialog) {
            if (!dialog.id || warnings.has(dialog.id)) {
                return;
            }
            warnings.set(dialog.id, new WarningController(dialog));
        });

        document.dispatchEvent(new CustomEvent("tpr:session-timeout:ready"));
    }

    window.TprSessionTimeoutWarning = Object.freeze({
        get: function (id) {
            return warnings.get(id) || null;
        },
        show: function (id) {
            const controller = warnings.get(id);
            if (controller) controller.show();
        },
        hide: function (id) {
            const controller = warnings.get(id);
            if (controller) controller.hide();
        },
        setError: function (id, message) {
            const controller = warnings.get(id);
            if (controller) controller.setError(message);
        }
    });

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", initialise, { once: true });
    } else {
        initialise();
    }
})();
