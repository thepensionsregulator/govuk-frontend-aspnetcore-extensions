document.addEventListener("DOMContentLoaded", function () {
    const warning = document.querySelector(".tpr-session-timeout-warning[data-session-timeout-enabled='true']");
    if (!warning) {
        return;
    }

    const overlay = document.querySelector(".tpr-session-timeout-warning-overlay");
    const extendButton = warning.querySelector("[data-session-extend]");
    const resetOnActivity = warning.getAttribute("data-session-timeout-reset-on-activity") === "true";
    const timeoutAttribute = warning.getAttribute("data-session-timeout-ms");
    const configuredTimeout = Number.parseInt(timeoutAttribute || "", 10);
    const timeoutMilliseconds = Number.isFinite(configuredTimeout) && configuredTimeout > 0 ? configuredTimeout : 900000;
    const activityEvents = ["click", "keydown", "pointerdown", "scroll", "touchstart", "mousemove"];
    const hiddenClass = "govuk-visually-hidden";
    const mouseMoveThrottleMilliseconds = 1000;

    let timeoutId;
    let warningVisible = false;
    let lastMouseMoveAt = 0;

    function setHidden(element, isHidden) {
        if (!element) {
            return;
        }

        element.classList.toggle(hiddenClass, isHidden);
    }

    function showSessionTimeoutWarning() {
        setHidden(warning, false);
        setHidden(overlay, false);
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
        setHidden(warning, true);
        setHidden(overlay, true);
        warningVisible = false;
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

    if (resetOnActivity) {
        activityEvents.forEach(function (eventName) {
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
