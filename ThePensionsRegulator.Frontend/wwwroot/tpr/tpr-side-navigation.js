
document.addEventListener("DOMContentLoaded", function () {
    var toggle = document.getElementsByClassName("side-nav-mobile-expand-toggle");
    for (var i = 0; i < toggle.length; i++) {
        toggle[i].addEventListener("click", ExpandOrRetractSideNav);
    }
});

function ExpandOrRetractSideNav() {
    var ul = this.parentNode.nextElementSibling;
    if (ul.parentNode.classList.contains("side-nav-list--collapse")) {
        ExpandSideNav(ul);
    }
    else {
        CollapseSideNav(ul);
    }
}

function ExpandSideNav(ul) {
    ul.style.display = "block";
    ul.style.maxHeight = ul.scrollHeight + "px";
    ul.parentNode.classList.remove("side-nav-list--collapse");
}

function CollapseSideNav(ul) {
    ul.style.maxHeight = "0px";
    ul.style.display = "none";
    ul.parentNode.classList.add("side-nav-list--collapse");
}
