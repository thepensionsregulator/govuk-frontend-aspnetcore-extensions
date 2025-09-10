
document.addEventListener("DOMContentLoaded", function () {
    // mobile menu expand/collapse handling
    var buttonToggle = document.getElementsByClassName("tpr-side-nav__mobile-expand-toggle")[0]
        .addEventListener("click", expandOrCollapseMobileNav);
   
    // list item expand/collapse handling
    var sideNavListItemToggle = document.getElementsByClassName("tpr-side-nav__list-item__expand-toggle");
    for (var i = 0; i < sideNavListItemToggle.length; i++) {
        sideNavListItemToggle[i].addEventListener("click", expandOrCollapseListItem);
    }
});
function expandOrCollapseMobileNav() {
    let firstLevelNav = this.closest("nav").querySelector("ul");
    if (firstLevelNav.style.display == 'block') {
        firstLevelNav.style.display = 'none';
        this.querySelector("img").classList.remove("tpr-side-nav__arrow--up");
    }
    else {
        firstLevelNav.style.display = 'block';
        this.querySelector("img").classList.add("tpr-side-nav__arrow--up");
    }
}
function expandOrCollapseListItem(e) {
    var el = this.firstElementChild;
    if (el.classList.contains("tpr-side-nav__arrow--up")) {
        collapseListItem(el);
    }
    else {
        expandListItem(el);
    }
}
function expandListItem(liArrow) {
    //check for open branches outside this path and close them first
    clearExpandedBranches(liArrow);
    liArrow.classList.add("tpr-side-nav__arrow--up");
    liArrow.closest("li").classList.add("tpr-side-nav__list-item--expanded");
    
}
function collapseListItem(liArrow) {
    liArrow.classList.remove("tpr-side-nav__arrow--up");
    liArrow.closest("li").classList.remove("tpr-side-nav__list-item--expanded");
}
function clearExpandedBranches(el) {
    if (el.closest("li").classList.contains("tpr-side-nav__list-item--expanded")) {
        return;
    }
    let parent = el.closest("ul");
    if (parent.dataset.level > 1 && parent.closest("li").classList.contains("tpr-side-nav__list-item--expanded")) {
        return;
    }
    for(let node of el.closest("ul[data-level='1']").children){
        node.classList.remove("tpr-side-nav__list-item--expanded");
        let nodeArrow = node.querySelector("img.tpr-side-nav__list-item__arrow");
        if (nodeArrow != null) {
            nodeArrow.classList.remove("tpr-side-nav__arrow--up");
        }
    }
}
