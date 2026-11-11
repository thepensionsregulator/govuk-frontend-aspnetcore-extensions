document.addEventListener("DOMContentLoaded", function () {
    var warning = document.querySelector(".tpr-session-timeout-warning[data-session-timeout-enabled='true']");
    var overlay;
    var extendButton;
    var timeoutMilliseconds;
    var timeoutId;
    var resetOnActivity;
    var warningVisible = false;
    var lastMouseMoveAt = 0;
    var mouseMoveThrottleMilliseconds = 1000;

    function hideSessionTimeoutWarning() {
        warning.classList.add("govuk-visually-hidden");

        if (overlay) {
            overlay.classList.add("govuk-visually-hidden");
        }

        warningVisible = false;
    }

    function showSessionTimeoutWarning() {
        warning.classList.remove("govuk-visually-hidden");

        if (overlay) {
            overlay.classList.remove("govuk-visually-hidden");
        }

        warningVisible = true;
        warning.focus();
    }

    function scheduleSessionTimeoutWarning() {
        if (timeoutId) {
            window.clearTimeout(timeoutId);
        }

        timeoutId = window.setTimeout(showSessionTimeoutWarning, timeoutMilliseconds);
    }

    function resetSessionTimeoutTimer() {
        hideSessionTimeoutWarning();
        scheduleSessionTimeoutWarning();
    }

    function onUserActivity(event) {
        if (!resetOnActivity || warningVisible) {
            return;
        }

        if (event && event.type === "mousemove") {
            const now = Date.now();
            if (now - lastMouseMoveAt < mouseMoveThrottleMilliseconds) {
                return;
            }

            lastMouseMoveAt = now;
        }

        scheduleSessionTimeoutWarning();
    }

    async function extendSession() {
        const url = extendButton ? extendButton.getAttribute("data-session-extend-url") : null;

        if (!url) {
            resetSessionTimeoutTimer();
            return;
        }

        try {
            const response = await fetch(url, {
                method: "POST",
                credentials: "same-origin"
            });

            if (response.ok) {
                resetSessionTimeoutTimer();
            }
        }
        catch (error) {
            console.error("Failed to extend session timeout", error);
        }
    }

    if (!warning) {
        return;
    }

    overlay = document.querySelector(".tpr-session-timeout-warning-overlay");
    extendButton = warning.querySelector("[data-session-extend]");
    resetOnActivity = warning.getAttribute("data-session-timeout-reset-on-activity") === "true";

    const defaultTimeoutMilliseconds = 900000;
    const timeoutAttribute = warning.getAttribute("data-session-timeout-ms");
    const configuredTimeout = Number.parseInt(timeoutAttribute || "", 10);
    timeoutMilliseconds = Number.isFinite(configuredTimeout) && configuredTimeout > 0
        ? configuredTimeout
        : defaultTimeoutMilliseconds;

    if (resetOnActivity) {
        ["click", "keydown", "pointerdown", "scroll", "touchstart", "mousemove"].forEach(function (eventName) {
            document.addEventListener(eventName, onUserActivity);
        });
    }

    if (extendButton) {
        extendButton.addEventListener("click", function (event) {
            event.preventDefault();
            void extendSession();
        });
    }

    resetSessionTimeoutTimer();
});
