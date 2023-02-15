window.addEventListener("DOMContentLoaded", function () {
    const dismissButton = document.getElementsByClassName("dismiss")[0];

    if (dismissButton) {
        dismissButton.addEventListener("click", dismissBanner);
        dismissButton.classList.remove("govuk-!-display-none");
    }

    function dismissBanner() {
        const banner = document.getElementsByClassName("govuk-notification-banner")[0];
        banner.style.display = "none";
    }
});