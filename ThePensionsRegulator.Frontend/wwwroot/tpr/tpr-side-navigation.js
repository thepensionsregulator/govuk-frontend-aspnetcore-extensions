
document.addEventListener("DOMContentLoaded", function () {
    // mobile menu expand/collapse handling
    var buttonToggle = document.getElementsByClassName("tpr-side-nav__mobile-expand-toggle");
    for (var i = 0; i < buttonToggle.length; i++) {
        buttonToggle[i].parentNode.parentNode.classList.add("tpr-side-nav__list--collapse");
        buttonToggle[i].addEventListener("click", ExpandOrCollapseSideNav);
    }
    // js/no-js handling
    var sideNavLists = document.getElementsByClassName("tpr-side-nav__list");
    for (var i = 0; i < sideNavLists.length; i++) {
        sideNavLists[i].classList.remove("tpr-side-nav__list--no-js");
    }
    // list item expand/collapse handling
    var sideNavListItemArrows = document.getElementsByClassName("tpr-side-nav__list-item__arrow");
    for (var i = 0; i < sideNavListItemArrows.length; i++) {
        sideNavListItemArrows[i].addEventListener("click", ExpandOrCollapseListItem);
        if (sideNavListItemArrows[i].parentNode.classList.contains("tpr-side-nav__list-item--expanded")) {
            ExpandListItem(sideNavListItemArrows[i]);
        }
    }
});

function ExpandOrCollapseListItem() {
    var liArrow = this;
    if (this.classList.contains("tpr-side-nav__list-item__arrow--up")) {
        CollapseListItem(liArrow);
    }
    else {
        ExpandListItem(liArrow);
    }
}

function ExpandListItem(liArrow) {
    liArrow.nextElementSibling.style.maxHeight = liArrow.nextElementSibling.scrollHeight + "px";
    liArrow.nextElementSibling.style.overflow = "visible";
    liArrow.classList.add("tpr-side-nav__list-item__arrow--up");
    liArrow.parentNode.classList.add("tpr-side-nav__list-item--expanded");
    liArrow.setAttribute("aria-expanded", "true");
}

function CollapseListItem(liArrow) {
    liArrow.nextElementSibling.style.maxHeight = "0px";
    liArrow.nextElementSibling.style.overflow = "hidden";
    liArrow.classList.remove("tpr-side-nav__list-item__arrow--up");
    liArrow.parentNode.classList.remove("tpr-side-nav__list-item--expanded");
    liArrow.setAttribute("aria-expanded", "false");
}

function ExpandOrCollapseSideNav() {
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
