// Back to top should only appear when the page is longer than 4vh
document.addEventListener('scroll', function () {
    const backToTopScrollPosition = 4 * window.innerHeight;
    const y = document.getElementsByTagName("html")[0].scrollTop;
    if (y > backToTopScrollPosition) {

        document.querySelector('.tpr-back-to-top').style.display= 'block';

    } else {
        document.querySelector('.tpr-back-to-top').style.display = 'none';
    }
});
