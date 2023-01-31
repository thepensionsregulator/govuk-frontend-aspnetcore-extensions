window.addEventListener("DOMContentLoaded", function () {
    const dismissButton = document.getElementsByClassName("dismiss")[0];
    dismissButton.addEventListener("click", dismissBanner);

    function dismissBanner() {
        const banner = document.getElementsByClassName("govuk-notification-banner")[0];
        banner.style.display = "none";
    }
});