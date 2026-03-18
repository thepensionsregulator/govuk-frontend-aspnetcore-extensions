document.addEventListener("DOMContentLoaded", function () {
    try {
        var sideNav = document.querySelector(".tpr-side-nav");
        if (!sideNav) {
            return;
        }

        var id = sideNav.getAttribute("id") || "tpr-side-nav";
        var expandText = sideNav.getAttribute("data-expand-text") || "Expand {0}";
        var collapseText = sideNav.getAttribute("data-collapse-text") || "Collapse {0}";

        // Replace no-JS anchor for mobile with a button (if present)
        var noJsMobileLink = sideNav.querySelector(".tpr-side-nav__mobile-expand-toggle");
        if (noJsMobileLink) {
            var mobileToggle = document.createElement("button");
            mobileToggle.type = "button";
            mobileToggle.classList.add("govuk-link", "govuk-link--no-visited-state", "tpr-side-nav__mobile-expand-toggle");
            mobileToggle.setAttribute("aria-expanded", "false");
            mobileToggle.setAttribute("aria-controls", id + "__list");
            while (noJsMobileLink.firstChild) {
                mobileToggle.appendChild(noJsMobileLink.firstChild);
            }
            mobileToggle.addEventListener("click", expandOrCollapseMobileNav);
            if (noJsMobileLink.parentNode) {
                noJsMobileLink.parentNode.replaceChild(mobileToggle, noJsMobileLink);
            }
        }

        // Create toggles for every arrow image
        var sideNavListItemArrows = sideNav.querySelectorAll(".tpr-side-nav__list-item-arrow");
        for (var i = 0; i < sideNavListItemArrows.length; i++) {
            var img = sideNavListItemArrows[i];
            var isExpanded = img.classList.contains("tpr-side-nav__list-item-arrow--up");
            var wrapper = img.closest(".tpr-side-nav__list-item-wrapper");
            var label = "";
            if (wrapper) {
                var link = wrapper.querySelector("a");
                if (link && link.textContent) {
                    label = link.textContent.trim();
                }
            }
            img.setAttribute("alt", (isExpanded ? collapseText : expandText).replace("{0}", label));

            var itemToggle = document.createElement("button");
            itemToggle.type = "button";
            var li = img.closest("li");
            if (li && li.id) {
                itemToggle.setAttribute("aria-controls", li.id);
            }
            itemToggle.classList.add("tpr-side-nav__list-item__expand-toggle");
            itemToggle.setAttribute("aria-expanded", isExpanded ? "true" : "false");
            itemToggle.addEventListener("click", expandOrCollapseListItem);
            if (img.parentNode) {
                img.parentNode.insertBefore(itemToggle, img);
                itemToggle.appendChild(img);
            }
        }

        // Mobile toggle handler
        function expandOrCollapseMobileNav(e) {
            // "this" is the button when non-arrow function used in addEventListener
            var button = this;
            if (!button) { return; }
            var nav = button.closest("nav");
            if (!nav) { return; }
            var firstLevelNav = nav.querySelector("ul");
            if (!firstLevelNav) { return; }

            var img = button.querySelector("img");

            if (firstLevelNav.classList.contains("tpr-side-nav__list--expanded")) {
                firstLevelNav.classList.remove("tpr-side-nav__list--expanded");
                if (img) {
                    img.classList.remove("tpr-side-nav__list-item-arrow--up");
                    img.setAttribute("alt", expandText.replace("{0}", ""));
                }
                button.setAttribute("aria-expanded", "false");
            } else {
                firstLevelNav.classList.add("tpr-side-nav__list--expanded");
                if (img) {
                    img.classList.add("tpr-side-nav__list-item-arrow--up");
                    img.setAttribute("alt", collapseText.replace("{0}", ""));
                }
                button.setAttribute("aria-expanded", "true");
            }
        }

        // List item toggle handler
        function expandOrCollapseListItem(e) {
            var target = e.target || e.srcElement;
            if (!target) { return; }
            var button = target.closest("button");
            if (!button) { return; }
            var expanded = button.getAttribute("aria-expanded");
            if (expanded === "true") {
                collapseListItem(button);
            } else {
                expandListItem(button);
            }
        }

        function expandListItem(button) {
            if (!button) { return; }
            clearExpandedBranches(button);
            button.setAttribute("aria-expanded", "true");
            if (button.firstElementChild) {
                button.firstElementChild.classList.add("tpr-side-nav__list-item-arrow--up");
            }
            var li = button.closest("li");
            if (li) {
                li.classList.add("tpr-side-nav__list-item--expanded");
            }
        }

        function collapseListItem(button) {
            if (!button) { return; }
            button.setAttribute("aria-expanded", "false");
            if (button.firstElementChild) {
                button.firstElementChild.classList.remove("tpr-side-nav__list-item-arrow--up");
            }
            var li = button.closest("li");
            if (li) {
                li.classList.remove("tpr-side-nav__list-item--expanded");
            }
        }

        function collapseAll() {
            // collapse mobile first-level nav if expanded
            var mobileToggleBtn = sideNav.querySelector(".tpr-side-nav__mobile-expand-toggle[aria-expanded='true']");
            if (mobileToggleBtn) {
                var firstLevelNav = sideNav.querySelector("ul");
                if (firstLevelNav) {
                    firstLevelNav.classList.remove("tpr-side-nav__list--expanded");
                }
                mobileToggleBtn.setAttribute("aria-expanded", "false");
                var img = mobileToggleBtn.querySelector("img");
                if (img) {
                    img.classList.remove("tpr-side-nav__list-item-arrow--up");
                    img.setAttribute("alt", expandText.replace("{0}", ""));
                }
            }

            // collapse all expanded list-item toggles
            var expandedToggles = sideNav.querySelectorAll(".tpr-side-nav__list-item__expand-toggle[aria-expanded='true']");
            for (var j = 0; j < expandedToggles.length; j++) {
                collapseListItem(expandedToggles[j]);
            }

            // ensure top-level list items also lose the expanded class
            var expandedNodes = sideNav.querySelectorAll(".tpr-side-nav__list-item--expanded");
            for (var k = 0; k < expandedNodes.length; k++) {
                var node = expandedNodes[k];
                node.classList.remove("tpr-side-nav__list-item--expanded");
                var nodeArrow = node.querySelector(".tpr-side-nav__list-item-arrow");
                if (nodeArrow) {
                    nodeArrow.classList.remove("tpr-side-nav__list-item-arrow--up");
                }
                var toggle = node.querySelector(".tpr-side-nav__list-item__expand-toggle");
                if (toggle) {
                    toggle.setAttribute("aria-expanded", "false");
                }
            }
        }

        // Robust Escape handler
        function onEscapeKey(e) {
            var key = e.key || (e.keyCode === 27 ? "Escape" : null);
            if (key !== "Escape" && key !== "Esc") {
                return;
            }

            var active = document.activeElement;
            if (active && (active.tagName === "INPUT" || active.tagName === "TEXTAREA" || active.tagName === "SELECT" || active.isContentEditable)) {
                return;
            }

            var focusedInSideNav = sideNav.contains(active);
            var hasExpanded = !!sideNav.querySelector(".tpr-side-nav__list--expanded, .tpr-side-nav__list-item--expanded, .tpr-side-nav__list-item__expand-toggle[aria-expanded='true']");
            if (focusedInSideNav || hasExpanded) {
                try {
                    e.preventDefault();
                    e.stopPropagation();
                } catch (err) {
                    // ignore if preventDefault isn't supported in some contexts
                }
                collapseAll();
            }
        }

        // Attach Escape handlers
        document.addEventListener("keydown", onEscapeKey, false);
        sideNav.addEventListener("keydown", onEscapeKey, false);

        // Close other expanded branches at the same top-level
        function clearExpandedBranches(el) {
            if (!el) { return; }
            var li = el.closest("li");
            if (li && li.classList.contains("tpr-side-nav__list-item--expanded")) {
                return;
            }

            var parentUl = el.closest("ul");
            if (parentUl && parentUl.dataset && Number(parentUl.dataset.level) > 1) {
                var parentLi = parentUl.closest("li");
                if (parentLi && parentLi.classList.contains("tpr-side-nav__list-item--expanded")) {
                    return;
                }
            }

            var root = el.closest("ul[data-level='1']");
            if (!root) { return; }

            var children = root.children;
            for (var m = 0; m < children.length; m++) {
                var node = children[m];
                node.classList.remove("tpr-side-nav__list-item--expanded");
                var toggle = node.querySelector(".tpr-side-nav__list-item__expand-toggle");
                if (toggle) {
                    toggle.setAttribute("aria-expanded", "false");
                }
                var nodeArrow = node.querySelector(".tpr-side-nav__list-item-arrow");
                if (nodeArrow) {
                    nodeArrow.classList.remove("tpr-side-nav__list-item-arrow--up");
                }
            }
        }
    } catch (err) {
        // If initialization fails, log to console so it doesn't silently break the rest of the page
        console.error("tpr-side-navigation initialization error:", err);
    }
});