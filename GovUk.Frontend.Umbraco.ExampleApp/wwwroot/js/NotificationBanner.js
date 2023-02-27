window.addEventListener("DOMContentLoaded", function () {
    const dismissButton = document.getElementsByClassName("dismiss")[0];
    const dismissButton1 = document.getElementsByClassName("dismiss-1")[0];

    if (dismissButton) {
        dismissButton.addEventListener("click", () => dismissBanner("govuk-notification-banner"));
        dismissButton.classList.remove("govuk-!-display-none");
    }

    if (dismissButton1) {
        dismissButton1.addEventListener("click", () => dismissBanner("notification-banner-second"));
        dismissButton1.classList.remove("govuk-!-display-none");
    }

    function dismissBanner(className) {
        const banner = document.getElementsByClassName(className)[0];
        banner.style.display = "none";
    }
});