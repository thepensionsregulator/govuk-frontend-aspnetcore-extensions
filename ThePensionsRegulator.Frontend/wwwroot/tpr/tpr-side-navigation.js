
document.addEventListener("DOMContentLoaded", function () {
    var sideNav = document.querySelector(".tpr-side-nav");
    if (!sideNav) {
        return;
    }

    const id = sideNav.getAttribute("id") || "tpr-side-nav";
    const expand = sideNav.getAttribute("data-expand-text") || "Expand {0}";
    const collapse = sideNav.getAttribute("data-collapse-text") || "Collapse {0}";

    var noJsMobileLink = document.getElementsByClassName("tpr-side-nav__mobile-expand-toggle");

    const mobileToggle = document.createElement("button");
    mobileToggle.type = "button";
    mobileToggle.classList.add("govuk-link");
    mobileToggle.classList.add("govuk-link--no-visited-state");
    mobileToggle.classList.add("tpr-side-nav__mobile-expand-toggle");
    mobileToggle.setAttribute("aria-expanded", "false");
    mobileToggle.setAttribute("aria-controls", id + "__list");
    while (noJsMobileLink[0].firstChild) {
        mobileToggle.appendChild(noJsMobileLink[0].firstChild);
    }
    mobileToggle.addEventListener("click", expandOrCollapseMobileNav);
    noJsMobileLink[0].parentNode.replaceChild(mobileToggle, noJsMobileLink[0]);

   
    // list item expand/collapse handling
    var sideNavListItemToggles = document.getElementsByClassName("tpr-side-nav__list-item-arrow");
    for (var i = 0; i < sideNavListItemToggles.length; i++) {
        const img = sideNavListItemToggles[i];
        const isExpanded = img.classList.contains("tpr-side-nav__list-item-arrow--up");
        img.setAttribute("alt", (isExpanded ? collapse : expand).replace("{0}", img.closest(".tpr-side-nav__list-item-wrapper").querySelector("a").textContent.trim()));
        const itemToggle = document.createElement("button");
        itemToggle.type = "button";
        itemToggle.setAttribute("aria-controls", img.closest("li").id);
        itemToggle.classList.add("tpr-side-nav__list-item__expand-toggle");
        itemToggle.setAttribute("aria-expanded", isExpanded);
        itemToggle.addEventListener("click", expandOrCollapseListItem);
        img.parentNode.insertBefore(itemToggle, img);
        itemToggle.appendChild(img);
    }

    function expandOrCollapseMobileNav() {
        let button = this.closest("button");
        let firstLevelNav = button.closest("nav").querySelector("ul");
        if (firstLevelNav.classList.contains("tpr-side-nav__list--expanded")) {
            firstLevelNav.classList.remove("tpr-side-nav__list--expanded");
            const img = button.querySelector("img");
            img.classList.remove("tpr-side-nav__list-item-arrow--up");
            img.setAttribute("alt", expand.replace("{0}", ""));
            button.setAttribute("aria-expanded", "false");
        }
        else {
            firstLevelNav.classList.add("tpr-side-nav__list--expanded");
            const img = button.querySelector("img");
            img.classList.add("tpr-side-nav__list-item-arrow--up");
            img.setAttribute("alt", collapse.replace("{0}", ""));
            button.setAttribute("aria-expanded", "true");
        }
    }
    function expandOrCollapseListItem(e) {
        const button = e.target.closest("button");
        if (button.getAttribute("aria-expanded") === "true") {
            collapseListItem(button);
        }
        else {
            expandListItem(button);
        }
    }
    function expandListItem(button) {
        //check for open branches outside this path and close them first
        clearExpandedBranches(button);
        button.setAttribute("aria-expanded", "true");
        button.firstElementChild.classList.add("tpr-side-nav__list-item-arrow--up");
        button.closest("li").classList.add("tpr-side-nav__list-item--expanded");
    }

    function collapseListItem(button) {
        button.setAttribute("aria-expanded", "false");
        button.firstElementChild.classList.remove("tpr-side-nav__list-item-arrow--up");
        button.closest("li").classList.remove("tpr-side-nav__list-item--expanded");
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
            const toggle = node.querySelector(".tpr-side-nav__list-item__expand-toggle");
            if (toggle) { toggle.setAttribute("aria-expanded", "false"); }
            let nodeArrow = node.querySelector(".tpr-side-nav__list-item-arrow");
            if (nodeArrow) { nodeArrow.classList.remove("tpr-side-nav__list-item-arrow--up"); }
        }
    }
});