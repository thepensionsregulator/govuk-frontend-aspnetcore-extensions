
document.addEventListener("DOMContentLoaded", function () {
    var buttonToggle = document.getElementsByClassName("tpr-side-nav__mobile-expand-toggle");
    for (var i = 0; i < buttonToggle.length; i++) {
        buttonToggle[i].parentNode.parentNode.classList.add("tpr-side-nav__list--collapse");
        buttonToggle[i].addEventListener("click", ExpandOrRetractSideNav);
    }
    var sideNavLists = document.getElementsByClassName("tpr-side-nav__list");
    for (var i = 0; i < sideNavLists.length; i++) {
        sideNavLists[i].classList.remove("tpr-side-nav__list--no-js");
    }
});

function ExpandOrRetractSideNav() {
    var ul = this.parentNode.nextElementSibling;
    if (ul.parentNode.classList.contains("tpr-side-nav__list--collapse")) {
        ExpandSideNav(ul);
    }
    else {
        CollapseSideNav(ul);
    }
}

function ExpandSideNav(ul) {
    ul.style.display = "block";
    ul.style.maxHeight = ul.scrollHeight + "px";
    ul.parentNode.classList.remove("tpr-side-nav__list--collapse");
    ul.parentNode.firstElementChild.firstElementChild.setAttribute("aria-expanded", "true");
}

function CollapseSideNav(ul) {
    ul.style.maxHeight = "0px";
    ul.style.display = "none";
    ul.parentNode.classList.add("tpr-side-nav__list--collapse");
    ul.parentNode.firstElementChild.firstElementChild.setAttribute("aria-expanded", "false");
}
