document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.tpr-session-timeout-warning').forEach(dialog => {
        const content = dialog.querySelector('[data-session-timeout-content]');
        if (content) {
            const walker = document.createTreeWalker(content, NodeFilter.SHOW_TEXT);
            while (walker.nextNode()) {
                walker.currentNode.textContent = walker.currentNode.textContent.replaceAll('{remainingTime}', '2 minutes');
            }
        }

        dialog.querySelectorAll('[data-session-timeout-extend]').forEach(button => {
            button.addEventListener('click', () => dialog.close());
        });
        dialog.open = true;
    });
});
