document.addEventListener('DOMContentLoaded', function () {
    var dialogs = document.querySelectorAll('.tpr-session-timeout-warning');
    dialogs.forEach(function (dialog) {
        if (dialog.closest('[data-tpr-session-timeout-display="inline"]')) {
            dialog.open = true;
        } else {
            dialog.showModal();
        }
    });
});
