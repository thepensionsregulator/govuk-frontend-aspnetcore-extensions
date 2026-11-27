document.addEventListener('DOMContentLoaded', function () {
    var dialogs = document.querySelectorAll('.tpr-session-timeout-warning');
    dialogs.forEach(function (dialog) {
        var inlineExample = dialog.closest('[data-tpr-session-timeout-display="inline"]');
        var content = dialog.querySelector('[data-session-timeout-content]');
        if (inlineExample && content) {
            var staticText = inlineExample.dataset.sessionTimeoutStaticText;
            if (staticText === undefined) {
                throw new Error('Static session timeout example text is required.');
            }
            var walker = document.createTreeWalker(content, NodeFilter.SHOW_TEXT);
            while (walker.nextNode()) {
                var text = walker.currentNode.textContent;
                if (text.includes('{remainingTime}')) {
                    walker.currentNode.textContent = text.split('{remainingTime}').join(staticText);
                }
            }
        }

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
