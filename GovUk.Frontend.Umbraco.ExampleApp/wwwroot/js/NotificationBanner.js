window.addEventListener("DOMContentLoaded", function () {
    const dismissButton = document.getElementsByClassName("dismiss")[0];
    dismissButton.addEventListener("click", dismissBanner);

    console.log("hello world agagin");

    function dismissBanner() {
        const banner = document.getElementsByClassName("tpr-notification-banner")[0];
        banner.style.display = "none";
    }
});