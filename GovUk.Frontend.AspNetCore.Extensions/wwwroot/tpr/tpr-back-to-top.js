// Back to top should only appear when the page is longer than 4vh
document.addEventListener('scroll', function () {
    const backToTop = document.querySelector('.tpr-back-to-top');
    if (!backToTop) { return; }
    const backToTopScrollPosition = 4 * window.innerHeight;
    const y = document.getElementsByTagName("html")[0].scrollTop;
    if (y > backToTopScrollPosition) {

        backToTop.style.display = 'block';

    } else {
        backToTop.style.display = 'none';
    }
});
