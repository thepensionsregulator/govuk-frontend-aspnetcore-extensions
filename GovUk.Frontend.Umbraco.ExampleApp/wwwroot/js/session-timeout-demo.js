document.addEventListener('DOMContentLoaded', function () {
    var dialogs = document.querySelectorAll('.tpr-session-timeout-warning');
    dialogs.forEach(function (dialog) {
        dialog.querySelectorAll('[data-session-timeout-extend]').forEach(function (button) {
            button.addEventListener('click', function () {
                dialog.close();
            });
        });

        if (dialog.closest('[data-tpr-session-timeout-display="inline"]')) {
            dialog.open = true;
        } else {
            dialog.showModal();
        }
    });
});
