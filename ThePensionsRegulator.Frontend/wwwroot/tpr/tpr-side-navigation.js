
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
    var sideNavListItemToggle = document.getElementsByClassName("tpr-side-nav__list-item__expand-toggle");
    for (var i = 0; i < sideNavListItemToggle.length; i++) {
        sideNavListItemToggle[i].addEventListener("click", ExpandOrCollapseListItem);
        if (sideNavListItemToggle[i].parentNode.parentNode.classList.contains("tpr-side-nav__list-item--expanded")) {
            ExpandListItem(sideNavListItemToggle[i].firstElementChild);
        }
    }
});

function ExpandOrCollapseListItem() {
    var liArrow = this.firstElementChild;
    if (liArrow.classList.contains("tpr-side-nav__arrow--up")) {
        CollapseListItem(liArrow);
    }
    else {
        ExpandListItem(liArrow);
    }
}

function ExpandListItem(liArrow) {
    if (liArrow.parentNode.parentNode.parentNode.parentNode.style.overflow === "visible") {
        liArrow.parentNode.parentNode.nextElementSibling.style.overflow = "visible hidden";
    }
    else {
        liArrow.parentNode.parentNode.nextElementSibling.style.overflow = "visible";
    }
    liArrow.parentNode.parentNode.nextElementSibling.style.maxHeight = liArrow.parentNode.parentNode.nextElementSibling.scrollHeight + "px";

    var rootUl = document.getElementsByClassName("tpr-side-nav__list")[0];
    rootUl.style.maxHeight = rootUl.scrollHeight + "px";

    liArrow.parentNode.parentNode.parentNode.parentNode.style.maxHeight = liArrow.parentNode.parentNode.parentNode.parentNode.scrollHeight + "px";

    var rootLiAncestor = liArrow.closest(".tpr-side-nav__list-item--expanded--active");

    for (var i = 0; i < rootUl.children.length; i++) {
        if (rootUl.children[i].classList.contains("tpr-side-nav__list-item--expanded--active")) {
            rootUl.children[i].classList.remove("tpr-side-nav__list-item--expanded--active");
        }
    }
    
    liArrow.classList.add("tpr-side-nav__arrow--up");
    liArrow.parentNode.parentNode.parentNode.classList.add("tpr-side-nav__list-item--expanded");
    liArrow.parentNode.setAttribute("aria-expanded", "true");

    if (rootLiAncestor !== null) {
        rootLiAncestor.classList.add("tpr-side-nav__list-item--expanded--active");
    }
    else {
        Furthest(liArrow, "tpr-side-nav__list-item--expanded").classList.add("tpr-side-nav__list-item--expanded--active");
    }
}

function Furthest(element, selector) {
    var match = null;
    var current = element.parentElement;
    while (current) {
        if (current.classList.contains(selector)) {
            match = current;
        }
        current = current.parentElement;
    }
    return match;
}


function CollapseListItem(liArrow) {
    liArrow.parentNode.parentNode.nextElementSibling.style.overflow = "hidden";
    liArrow.parentNode.parentNode.nextElementSibling.style.maxHeight = "0px";

    liArrow.parentNode.parentNode.parentNode.parentNode.style.maxHeight = liArrow.parentNode.parentNode.parentNode.parentNode.scrollHeight + "px";

    var rootUl = document.getElementsByClassName("tpr-side-nav__list")[0];
    rootUl.style.maxHeight = rootUl.scrollHeight + "px";

    if (liArrow.parentNode.parentNode.parentNode.classList.contains("tpr-side-nav__list-item--expanded--active")) {
        liArrow.parentNode.parentNode.parentNode.classList.remove("tpr-side-nav__list-item--expanded--active");
    }

    liArrow.classList.remove("tpr-side-nav__arrow--up");
    liArrow.parentNode.parentNode.parentNode.classList.remove("tpr-side-nav__list-item--expanded");
    liArrow.parentNode.setAttribute("aria-expanded", "false");
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
    ul.parentNode.firstElementChild.firstElementChild.firstElementChild.classList.add("tpr-side-nav__arrow--up");
}

function CollapseSideNav(ul) {
    ul.style.maxHeight = "0px";
    ul.style.display = "none";
    ul.parentNode.classList.add("tpr-side-nav__list--collapse");
    ul.parentNode.firstElementChild.firstElementChild.setAttribute("aria-expanded", "false");
    ul.parentNode.firstElementChild.firstElementChild.firstElementChild.classList.remove("tpr-side-nav__arrow--up");
}
