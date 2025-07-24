
document.addEventListener("DOMContentLoaded", function () {
    var toggle = document.getElementsByClassName("side-nav-mobile-expand-toggle");
    for (var i = 0; i < toggle.length; i++) {
        toggle[i].addEventListener("click", ExpandOrRetractSideNav);
    }
});

function ExpandOrRetractSideNav() {
    var content = this.parentNode.nextElementSibling;
    if (content.style.maxHeight !== "0px") {
        content.style.maxHeight = "0px";
    }
    else {
        content.style.maxHeight = content.scrollHeight + "px";
    }
}
