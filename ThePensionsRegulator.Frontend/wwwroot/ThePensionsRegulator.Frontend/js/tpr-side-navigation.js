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

    function collapseAll() {
        // collapse mobile first-level nav if expanded
        const mobileToggleBtn = sideNav.querySelector(".tpr-side-nav__mobile-expand-toggle[aria-expanded='true']");
        if (mobileToggleBtn) {
            const firstLevelNav = sideNav.querySelector("ul");
            if (firstLevelNav) {
                firstLevelNav.classList.remove("tpr-side-nav__list--expanded");
            }
            mobileToggleBtn.setAttribute("aria-expanded", "false");
            const img = mobileToggleBtn.querySelector("img");
            if (img) {
                img.classList.remove("tpr-side-nav__list-item-arrow--up");
                img.setAttribute("alt", expand.replace("{0}", ""));
            }
        }

        // collapse all expanded list-item toggles
        const expandedToggles = sideNav.querySelectorAll(".tpr-side-nav__list-item__expand-toggle[aria-expanded='true']");
        expandedToggles.forEach(function (btn) {
            collapseListItem(btn);
        });

        // ensure top-level list items also lose the expanded class (in case some were expanded without toggles)
        const expandedNodes = sideNav.querySelectorAll(".tpr-side-nav__list-item--expanded");
        expandedNodes.forEach(function (node) {
            node.classList.remove("tpr-side-nav__list-item--expanded");
            const nodeArrow = node.querySelector(".tpr-side-nav__list-item-arrow");
            if (nodeArrow) { nodeArrow.classList.remove("tpr-side-nav__list-item-arrow--up"); }
            const toggle = node.querySelector(".tpr-side-nav__list-item__expand-toggle");
            if (toggle) { toggle.setAttribute("aria-expanded", "false"); }
        });
    }

    // Collapse all expanded sections when the user presses Escape while focused inside the side nav.
    document.addEventListener("keydown", function (e) {
        if (e.key === "Escape" || e.key === "Esc") {
            if (sideNav.contains(document.activeElement) || sideNav.contains(e.target)) {
                collapseAll();
            }
        }
    });

    function clearExpandedBranches(el) {
        if (el.closest("li").classList.contains("tpr-side-nav__list-item--expanded")) {
            return;
        }
        let parent = el.closest("ul");
        if (parent.dataset.level > 1 && parent.closest("li").classList.contains("tpr-side-nav__list-item--expanded")) {
            return;
        }
        for (let node of el.closest("ul[data-level='1']").children) {
            node.classList.remove("tpr-side-nav__list-item--expanded");
            const toggle = node.querySelector(".tpr-side-nav__list-item__expand-toggle");
            if (toggle) { toggle.setAttribute("aria-expanded", "false"); }
            let nodeArrow = node.querySelector(".tpr-side-nav__list-item-arrow");
            if (nodeArrow) { nodeArrow.classList.remove("tpr-side-nav__list-item-arrow--up"); }
        }
    }
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

        function collapseAll() {
            // collapse mobile first-level nav if expanded
            const mobileToggleBtn = sideNav.querySelector(".tpr-side-nav__mobile-expand-toggle[aria-expanded='true']");
            if (mobileToggleBtn) {
                const firstLevelNav = sideNav.querySelector("ul");
                if (firstLevelNav) {
                    firstLevelNav.classList.remove("tpr-side-nav__list--expanded");
                }
                mobileToggleBtn.setAttribute("aria-expanded", "false");
                const img = mobileToggleBtn.querySelector("img");
                if (img) {
                    img.classList.remove("tpr-side-nav__list-item-arrow--up");
                    img.setAttribute("alt", expand.replace("{0}", ""));
                }
            }

            // collapse all expanded list-item toggles
            const expandedToggles = sideNav.querySelectorAll(".tpr-side-nav__list-item__expand-toggle[aria-expanded='true']");
            expandedToggles.forEach(function (btn) {
                collapseListItem(btn);
            });

            // ensure top-level list items also lose the expanded class (in case some were expanded without toggles)
            const expandedNodes = sideNav.querySelectorAll(".tpr-side-nav__list-item--expanded");
            expandedNodes.forEach(function (node) {
                node.classList.remove("tpr-side-nav__list-item--expanded");
                const nodeArrow = node.querySelector(".tpr-side-nav__list-item-arrow");
                if (nodeArrow) { nodeArrow.classList.remove("tpr-side-nav__list-item-arrow--up"); }
                const toggle = node.querySelector(".tpr-side-nav__list-item__expand-toggle");
                if (toggle) { toggle.setAttribute("aria-expanded", "false"); }
            });
        }

        // Collapse all expanded sections when the user presses Escape.
        // Only trigger when:
        //  - there are expanded sections inside the side nav, and
        //  - the focused element isn't an input/select/textarea or contentEditable (so we don't interfere with form UX)
        document.addEventListener("keydown", function (e) {
            if (e.key !== "Escape" && e.key !== "Esc") {
                return;
            }

            const active = document.activeElement;
            if (active && (active.tagName === "INPUT" || active.tagName === "TEXTAREA" || active.tagName === "SELECT" || active.isContentEditable)) {
                return;
            }

            const hasExpanded = sideNav.querySelector(".tpr-side-nav__list--expanded, .tpr-side-nav__list-item--expanded, .tpr-side-nav__list-item__expand-toggle[aria-expanded='true']");
            if (hasExpanded) {
                collapseAll();
            }
        });

        function clearExpandedBranches(el) {
            if (el.closest("li").classList.contains("tpr-side-nav__list-item--expanded")) {
                return;
            }
            let parent = el.closest("ul");
            if (parent.dataset.level > 1 && parent.closest("li").classList.contains("tpr-side-nav__list-item--expanded")) {
                return;
            }
            for (let node of el.closest("ul[data-level='1']").children) {
                node.classList.remove("tpr-side-nav__list-item--expanded");
                const toggle = node.querySelector(".tpr-side-nav__list-item__expand-toggle");
                if (toggle) { toggle.setAttribute("aria-expanded", "false"); }
                let nodeArrow = node.querySelector(".tpr-side-nav__list-item-arrow");
                if (nodeArrow) { nodeArrow.classList.remove("tpr-side-nav__list-item-arrow--up"); }
            }
        }
    });
    document.addEventListener("DOMContentLoaded", function () {
        const sideNav = document.querySelector(".tpr-side-nav");
        if (!sideNav) {
            return;
        }

        const id = sideNav.getAttribute("id") || "tpr-side-nav";
        const expand = sideNav.getAttribute("data-expand-text") || "Expand {0}";
        const collapse = sideNav.getAttribute("data-collapse-text") || "Collapse {0}";

        // Mobile toggle: only replace if the no-JS anchor exists
        const noJsMobileLink = sideNav.querySelector(".tpr-side-nav__mobile-expand-toggle");
        if (noJsMobileLink) {
            const mobileToggle = document.createElement("button");
            mobileToggle.type = "button";
            mobileToggle.classList.add("govuk-link", "govuk-link--no-visited-state", "tpr-side-nav__mobile-expand-toggle");
            mobileToggle.setAttribute("aria-expanded", "false");
            mobileToggle.setAttribute("aria-controls", id + "__list");
            while (noJsMobileLink.firstChild) {
                mobileToggle.appendChild(noJsMobileLink.firstChild);
            }
            mobileToggle.addEventListener("click", expandOrCollapseMobileNav);
            noJsMobileLink.parentNode.replaceChild(mobileToggle, noJsMobileLink);
        }

        // list item expand/collapse handling - create toggles for each arrow image
        const sideNavListItemArrows = sideNav.querySelectorAll(".tpr-side-nav__list-item-arrow");
        sideNavListItemArrows.forEach(function (img) {
            const isExpanded = img.classList.contains("tpr-side-nav__list-item-arrow--up");
            const label = img.closest(".tpr-side-nav__list-item-wrapper")?.querySelector("a")?.textContent?.trim() || "";
            img.setAttribute("alt", (isExpanded ? collapse : expand).replace("{0}", label));
            const itemToggle = document.createElement("button");
            itemToggle.type = "button";
            const li = img.closest("li");
            if (li && li.id) {
                itemToggle.setAttribute("aria-controls", li.id);
            }
            itemToggle.classList.add("tpr-side-nav__list-item__expand-toggle");
            itemToggle.setAttribute("aria-expanded", isExpanded ? "true" : "false");
            itemToggle.addEventListener("click", expandOrCollapseListItem);
            img.parentNode.insertBefore(itemToggle, img);
            itemToggle.appendChild(img);
        });

        function expandOrCollapseMobileNav() {
            const button = this.closest("button");
            const firstLevelNav = button.closest("nav").querySelector("ul");
            if (!firstLevelNav) { return; }
            if (firstLevelNav.classList.contains("tpr-side-nav__list--expanded")) {
                firstLevelNav.classList.remove("tpr-side-nav__list--expanded");
                const img = button.querySelector("img");
                if (img) { img.classList.remove("tpr-side-nav__list-item-arrow--up"); img.setAttribute("alt", expand.replace("{0}", "")); }
                button.setAttribute("aria-expanded", "false");
            } else {
                firstLevelNav.classList.add("tpr-side-nav__list--expanded");
                const img = button.querySelector("img");
                if (img) { img.classList.add("tpr-side-nav__list-item-arrow--up"); img.setAttribute("alt", collapse.replace("{0}", "")); }
                button.setAttribute("aria-expanded", "true");
            }
        }

        function expandOrCollapseListItem(e) {
            const button = e.target.closest("button");
            if (!button) { return; }
            if (button.getAttribute("aria-expanded") === "true") {
                collapseListItem(button);
            } else {
                expandListItem(button);
            }
        }

        function expandListItem(button) {
            clearExpandedBranches(button);
            button.setAttribute("aria-expanded", "true");
            if (button.firstElementChild) { button.firstElementChild.classList.add("tpr-side-nav__list-item-arrow--up"); }
            const li = button.closest("li");
            if (li) { li.classList.add("tpr-side-nav__list-item--expanded"); }
        }

        function collapseListItem(button) {
            button.setAttribute("aria-expanded", "false");
            if (button.firstElementChild) { button.firstElementChild.classList.remove("tpr-side-nav__list-item-arrow--up"); }
            const li = button.closest("li");
            if (li) { li.classList.remove("tpr-side-nav__list-item--expanded"); }
        }

        function collapseAll() {
            // collapse mobile first-level nav if expanded
            const mobileToggleBtn = sideNav.querySelector(".tpr-side-nav__mobile-expand-toggle[aria-expanded='true']");
            if (mobileToggleBtn) {
                const firstLevelNav = sideNav.querySelector("ul");
                if (firstLevelNav) { firstLevelNav.classList.remove("tpr-side-nav__list--expanded"); }
                mobileToggleBtn.setAttribute("aria-expanded", "false");
                const img = mobileToggleBtn.querySelector("img");
                if (img) { img.classList.remove("tpr-side-nav__list-item-arrow--up"); img.setAttribute("alt", expand.replace("{0}", "")); }
            }

            // collapse all expanded list-item toggles
            const expandedToggles = sideNav.querySelectorAll(".tpr-side-nav__list-item__expand-toggle[aria-expanded='true']");
            expandedToggles.forEach(function (btn) { collapseListItem(btn); });

            // ensure any remaining expanded nodes are cleaned up
            const expandedNodes = sideNav.querySelectorAll(".tpr-side-nav__list-item--expanded");
            expandedNodes.forEach(function (node) {
                node.classList.remove("tpr-side-nav__list-item--expanded");
                const nodeArrow = node.querySelector(".tpr-side-nav__list-item-arrow");
                if (nodeArrow) { nodeArrow.classList.remove("tpr-side-nav__list-item-arrow--up"); }
                const toggle = node.querySelector(".tpr-side-nav__list-item__expand-toggle");
                if (toggle) { toggle.setAttribute("aria-expanded", "false"); }
            });
        }

        // Collapse all when the user presses Escape.
        // Only trigger when focus is inside the page (not editing an input), and when there are expanded items.
        document.addEventListener("keydown", function (e) {
            if (e.key !== "Escape") {
                return;
            }

            const active = document.activeElement;
            if (active && (active.tagName === "INPUT" || active.tagName === "TEXTAREA" || active.tagName === "SELECT" || active.isContentEditable)) {
                return;
            }

            // collapse only if the side nav contains the focused element OR there are expanded sections anywhere in the side nav
            const focusedInSideNav = sideNav.contains(active);
            const hasExpanded = !!sideNav.querySelector(".tpr-side-nav__list--expanded, .tpr-side-nav__list-item--expanded, .tpr-side-nav__list-item__expand-toggle[aria-expanded='true']");
            if (focusedInSideNav || hasExpanded) {
                collapseAll();
            }
        });

        function clearExpandedBranches(el) {
            const li = el.closest("li");
            if (li && li.classList.contains("tpr-side-nav__list-item--expanded")) {
                return;
            }
            const parent = el.closest("ul");
            if (parent && parent.dataset && Number(parent.dataset.level) > 1 && parent.closest("li") && parent.closest("li").classList.contains("tpr-side-nav__list-item--expanded")) {
                return;
            }
            const root = el.closest("ul[data-level='1']");
            if (!root) { return; }
            for (let node of root.children) {
                node.classList.remove("tpr-side-nav__list-item--expanded");
                const toggle = node.querySelector(".tpr-side-nav__list-item__expand-toggle");
                if (toggle) { toggle.setAttribute("aria-expanded", "false"); }
                const nodeArrow = node.querySelector(".tpr-side-nav__list-item-arrow");
                if (nodeArrow) { nodeArrow.classList.remove("tpr-side-nav__list-item-arrow--up"); }
            }
        }
    });
    document.addEventListener("DOMContentLoaded", function () {
        try {
            const sideNav = document.querySelector(".tpr-side-nav");
            if (!sideNav) {
                return;
            }

            const id = sideNav.getAttribute("id") || "tpr-side-nav";
            const expand = sideNav.getAttribute("data-expand-text") || "Expand {0}";
            const collapse = sideNav.getAttribute("data-collapse-text") || "Collapse {0}";

            // Mobile toggle: only replace if the no-JS anchor exists
            const noJsMobileLink = sideNav.querySelector(".tpr-side-nav__mobile-expand-toggle");
            if (noJsMobileLink) {
                const mobileToggle = document.createElement("button");
                mobileToggle.type = "button";
                mobileToggle.classList.add("govuk-link", "govuk-link--no-visited-state", "tpr-side-nav__mobile-expand-toggle");
                mobileToggle.setAttribute("aria-expanded", "false");
                mobileToggle.setAttribute("aria-controls", id + "__list");
                while (noJsMobileLink.firstChild) {
                    mobileToggle.appendChild(noJsMobileLink.firstChild);
                }
                mobileToggle.addEventListener("click", expandOrCollapseMobileNav);
                noJsMobileLink.parentNode.replaceChild(mobileToggle, noJsMobileLink);
            }

            // list item expand/collapse handling - create toggles for each arrow image
            const sideNavListItemArrows = sideNav.querySelectorAll(".tpr-side-nav__list-item-arrow");
            sideNavListItemArrows.forEach(function (img) {
                const isExpanded = img.classList.contains("tpr-side-nav__list-item-arrow--up");
                const label = img.closest(".tpr-side-nav__list-item-wrapper")?.querySelector("a")?.textContent?.trim() || "";
                img.setAttribute("alt", (isExpanded ? collapse : expand).replace("{0}", label));
                const itemToggle = document.createElement("button");
                itemToggle.type = "button";
                const li = img.closest("li");
                if (li && li.id) {
                    itemToggle.setAttribute("aria-controls", li.id);
                }
                itemToggle.classList.add("tpr-side-nav__list-item__expand-toggle");
                itemToggle.setAttribute("aria-expanded", isExpanded ? "true" : "false");
                itemToggle.addEventListener("click", expandOrCollapseListItem);
                img.parentNode.insertBefore(itemToggle, img);
                itemToggle.appendChild(img);
            });

            function expandOrCollapseMobileNav() {
                const button = this.closest("button");
                const firstLevelNav = button.closest("nav").querySelector("ul");
                if (!firstLevelNav) { return; }
                if (firstLevelNav.classList.contains("tpr-side-nav__list--expanded")) {
                    firstLevelNav.classList.remove("tpr-side-nav__list--expanded");
                    const img = button.querySelector("img");
                    if (img) { img.classList.remove("tpr-side-nav__list-item-arrow--up"); img.setAttribute("alt", expand.replace("{0}", "")); }
                    button.setAttribute("aria-expanded", "false");
                } else {
                    firstLevelNav.classList.add("tpr-side-nav__list--expanded");
                    const img = button.querySelector("img");
                    if (img) { img.classList.add("tpr-side-nav__list-item-arrow--up"); img.setAttribute("alt", collapse.replace("{0}", "")); }
                    button.setAttribute("aria-expanded", "true");
                }
            }

            function expandOrCollapseListItem(e) {
                const button = e.target.closest("button");
                if (!button) { return; }
                if (button.getAttribute("aria-expanded") === "true") {
                    collapseListItem(button);
                } else {
                    expandListItem(button);
                }
            }

            function expandListItem(button) {
                clearExpandedBranches(button);
                button.setAttribute("aria-expanded", "true");
                if (button.firstElementChild) { button.firstElementChild.classList.add("tpr-side-nav__list-item-arrow--up"); }
                const li = button.closest("li");
                if (li) { li.classList.add("tpr-side-nav__list-item--expanded"); }
            }

            function collapseListItem(button) {
                button.setAttribute("aria-expanded", "false");
                if (button.firstElementChild) { button.firstElementChild.classList.remove("tpr-side-nav__list-item-arrow--up"); }
                const li = button.closest("li");
                if (li) { li.classList.remove("tpr-side-nav__list-item--expanded"); }
            }

            function collapseAll() {
                // collapse mobile first-level nav if expanded
                const mobileToggleBtn = sideNav.querySelector(".tpr-side-nav__mobile-expand-toggle[aria-expanded='true']");
                if (mobileToggleBtn) {
                    const firstLevelNav = sideNav.querySelector("ul");
                    if (firstLevelNav) { firstLevelNav.classList.remove("tpr-side-nav__list--expanded"); }
                    mobileToggleBtn.setAttribute("aria-expanded", "false");
                    const img = mobileToggleBtn.querySelector("img");
                    if (img) { img.classList.remove("tpr-side-nav__list-item-arrow--up"); img.setAttribute("alt", expand.replace("{0}", "")); }
                }

                // collapse all expanded list-item toggles
                const expandedToggles = sideNav.querySelectorAll(".tpr-side-nav__list-item__expand-toggle[aria-expanded='true']");
                expandedToggles.forEach(function (btn) { collapseListItem(btn); });

                // ensure any remaining expanded nodes are cleaned up
                const expandedNodes = sideNav.querySelectorAll(".tpr-side-nav__list-item--expanded");
                expandedNodes.forEach(function (node) {
                    node.classList.remove("tpr-side-nav__list-item--expanded");
                    const nodeArrow = node.querySelector(".tpr-side-nav__list-item-arrow");
                    if (nodeArrow) { nodeArrow.classList.remove("tpr-side-nav__list-item-arrow--up"); }
                    const toggle = node.querySelector(".tpr-side-nav__list-item__expand-toggle");
                    if (toggle) { toggle.setAttribute("aria-expanded", "false"); }
                });
            }

            // Robust Escape handler:
            // - Accepts modern and legacy keys
            // - Runs when Escape pressed anywhere, but avoids inputs/contentEditable
            // - Collapses when focus is inside side nav OR there are expanded items
            function onEscapeKey(e) {
                const key = e.key || (e.keyCode === 27 ? "Escape" : null);
                if (key !== "Escape" && key !== "Esc") {
                    return;
                }

                const active = document.activeElement;
                if (active && (active.tagName === "INPUT" || active.tagName === "TEXTAREA" || active.tagName === "SELECT" || active.isContentEditable)) {
                    return;
                }

                const focusedInSideNav = sideNav.contains(active);
                const hasExpanded = !!sideNav.querySelector(".tpr-side-nav__list--expanded, .tpr-side-nav__list-item--expanded, .tpr-side-nav__list-item__expand-toggle[aria-expanded='true']");
                if (focusedInSideNav || hasExpanded) {
                    // prevent other handlers from interfering
                    e.preventDefault();
                    e.stopPropagation();
                    collapseAll();
                }
            }

            // Attach on document and on the nav itself to maximize chance of catching Escape.
            document.addEventListener("keydown", onEscapeKey, false);
            sideNav.addEventListener("keydown", onEscapeKey, false);

            function clearExpandedBranches(el) {
                const li = el.closest("li");
                if (li && li.classList.contains("tpr-side-nav__list-item--expanded")) {
                    return;
                }
                const parent = el.closest("ul");
                if (parent && parent.dataset && Number(parent.dataset.level) > 1 && parent.closest("li") && parent.closest("li").classList.contains("tpr-side-nav__list-item--expanded")) {
                    return;
                }
                const root = el.closest("ul[data-level='1']");
                if (!root) { return; }
                for (let node of root.children) {
                    node.classList.remove("tpr-side-nav__list-item--expanded");
                    const toggle = node.querySelector(".tpr-side-nav__list-item__expand-toggle");
                    if (toggle) { toggle.setAttribute("aria-expanded", "false"); }
                    const nodeArrow = node.querySelector(".tpr-side-nav__list-item-arrow");
                    if (nodeArrow) { nodeArrow.classList.remove("tpr-side-nav__list-item-arrow--up"); }
                }
            }
        } catch (err) {
            // If initialization fails, log so it doesn't silently break the rest of the page
            console.error("tpr-side-navigation initialization error:", err);
        }
    });
});