
document.addEventListener("DOMContentLoaded", function () {


    highlightCurrentSection();
    const mediaQuery = window.matchMedia("(max-width: 993px)");

    function SelectNavOption(e) {

        const toggles = document.querySelectorAll(".tpr-mobile-menu__toggle");
        const overlay = document.querySelectorAll(".tpr-header-menu__nav-overlay");
        const menuItems = document.querySelectorAll(".tpr-header-menu__nav-menu-item");
        const arrows = document.querySelectorAll(".tpr-mobile-menu__arrow");
        const nav = document.querySelector(".tpr-header-menu__nav");

        const isMobile = e.matches;


        if (isMobile) {

            toggles.forEach(t => t.removeAttribute("href"));
            toggles.forEach(t => t.addEventListener("click", toggleMobileMenu));
            toggles.forEach(t => t.addEventListener("keydown", keyboardToggleMobileMenu));

            if (overlay) { overlay.forEach(o => o.addEventListener("click", toggleMobileMenu)); }

            arrows.forEach(a => a.addEventListener("click", expandMobileMenuSubMenu))

            nav.removeEventListener("focusin", restoreDefaultMenuState);

            menuItems.forEach(m => {
                const subMenu = m.querySelector(".tpr-header-menu__nav-sub-menu");
                if (subMenu) {
                    const a = m.querySelector("a");
                    a.setAttribute("aria-expanded", false);
                    subMenu.style.display = "none";
                }
                m.removeEventListener("keydown", desktopKeyboardNavigation)
                m.addEventListener("keydown", mobileKeyboardNavigation)
            });
        } else {

            if (overlay) { overlay.forEach(o => o.classList.remove("tpr-header-menu__nav-overlay--visible")); }

            toggles.forEach(t => t.removeEventListener("click", toggleMobileMenu))
            overlay.forEach(o => o.removeEventListener("click", toggleMobileMenu));
            nav.addEventListener("focusin", restoreDefaultMenuState);

            arrows.forEach(a => a.removeEventListener("click", expandMobileMenuSubMenu))

            menuItems.forEach(m => {
                const a = m.querySelector("a");
                a.setAttribute("aria-expanded", false);

                const subMenu = m.querySelector(".tpr-header-menu__nav-sub-menu");
                if (subMenu) {

                    subMenu.style.display = "none";
                }
                m.removeEventListener("keydown", mobileKeyboardNavigation)
                m.addEventListener("keydown", desktopKeyboardNavigation)
            });

            removeActiveClasses()
            displayDesktopOverlayOnHover();
        }

    }

    SelectNavOption(mediaQuery);
    mediaQuery.addEventListener("change", SelectNavOption);
});

function highlightCurrentSection() {

    let url = window.location.pathname;

    url = url.replace(/^\/[a-z]{2}\//i, '/');
    url = url.replace(/\/[a-z]{2}(?=\/|$)/gi, '');

    let section = url.split('/').slice(1, 2).join('').replace("-", " ");

    if (!section) {
        return;
    }

    const translations = {
        "cyflogwyr": "employers",
        "amdanom ni": "about us",
        "llyfrgell ddogfen": "document library"
    }

    section = translations[section.toLowerCase()] || section.toLowerCase();

    document.querySelectorAll(".tpr-header-menu__nav-menu-item").forEach((item) => {

        const anchor = item.querySelector('a');
        let x = anchor.innerHTML.toLowerCase();

        if (x.includes(section.toLowerCase())) {

            anchor.classList.toggle("tpr-header-menu__nav-menu-item--active")
        }
    });
}

function displayDesktopOverlayOnHover() {

    const overlay = document.querySelector(".tpr-header-menu__nav-overlay");

    document.querySelectorAll(".tpr-header-menu__nav-menu-item").forEach((item) => {
        const a = item.querySelector('a');
        var subMenu = item.querySelector(".tpr-header-menu__nav-sub-menu");
        if (subMenu != null) {

            ["mouseenter"].forEach((evt) =>
                item.addEventListener(evt, () => {
                    if (overlay) { overlay.classList.add("tpr-header-menu__nav-overlay--visible"); }
                    if (window.innerWidth > 993) {
                        subMenu.style.display = 'grid';

                        a.setAttribute("aria-expanded", "true")
                    }
                })

            );
            ["mouseleave"].forEach((evt) =>
                item.addEventListener(evt, () => {
                    if (overlay) { overlay.classList.remove("tpr-header-menu__nav-overlay--visible"); }
                    if (window.innerWidth > 993) {
                        subMenu.style.display = 'none';

                        a.setAttribute("aria-expanded", "false")
                    }
                })
            );
        }
    });
}


function keyboardToggleMobileMenu(e) {

    switch (e.key) {

        case "ArrowDown":
            e.preventDefault()
            toggleMobileMenu()
            break;
        case "ArrowUp":
            e.preventDefault()
            toggleMobileMenu()
            break;
        default:
            break;
    }
}

function desktopKeyboardNavigation(e) {

    const menuItem = e.currentTarget
    const menuItemElements = document.querySelectorAll(".tpr-header-menu__nav-menu-item")

    let menuItems = Array.from(menuItemElements);

    let a = menuItem.querySelector('a');

    let subMenu = menuItem.querySelector(".tpr-header-menu__nav-sub-menu");
    let hasSubMenu = subMenu != null;

    const currentIndex = menuItems.indexOf(menuItem);

    const overlay = document.querySelector(".tpr-header-menu__nav-overlay")

    switch (e.key) {
        case "ArrowRight":
            e.preventDefault();
            const rightLink = menuItems[(currentIndex + 1) % menuItems.length].querySelector('a');
            if (rightLink) { rightLink.focus(); }
            if (hasSubMenu) {
                subMenu.style.display = 'none';
                a.setAttribute("aria-expanded", "false");
                if (overlay) { overlay.classList.remove("tpr-header-menu__nav-overlay--visible"); }
            }
            break;
        case "ArrowLeft":
            e.preventDefault();
            const leftLink = menuItems[(currentIndex - 1 + menuItems.length) % menuItems.length].querySelector('a');
            if (leftLink) { leftLink.focus(); }
            if (hasSubMenu) {
                subMenu.style.display = 'none';
                a.setAttribute("aria-expanded", "false");
                if (overlay) { overlay.classList.remove("tpr-header-menu__nav-overlay--visible"); }
            }
            break;
        case "ArrowUp":
            e.preventDefault()
            if (hasSubMenu) {
                subMenu.style.display = 'none';
                a.setAttribute("aria-expanded", "false");
                a.focus();
                if (overlay) { overlay.classList.remove("tpr-header-menu__nav-overlay--visible"); }
            }
            break;
        case "ArrowDown":
            e.preventDefault();
            if (hasSubMenu) {
                subMenu.style.display = 'grid';
                a.setAttribute("aria-expanded", "true");
                subMenu.querySelector(".tpr-header-menu__nav-sub-menu-item").removeAttribute("style");
                if (overlay) { overlay.classList.add("tpr-header-menu__nav-overlay--visible"); }
            }
            break;
        case "Escape":
            if (hasSubMenu) {
                subMenu.style.display = 'none';
                a.setAttribute("aria-expanded", "false");
                if (overlay) { overlay.classList.remove("tpr-header-menu__nav-overlay--visible"); }
                a.focus();
            }
            break;
        default:
            setTimeout(() => {

                const nav = document.querySelector(".tpr-header-menu__nav");
                const activeElement = document.activeElement;

                if (!nav.contains(activeElement)) {
                    if (overlay) { overlay.classList.remove("tpr-header-menu__nav-overlay--visible"); }

                    document.querySelectorAll(".tpr-header-menu__nav-sub-menu").forEach(s => s.style.display = "none");

                    document.querySelectorAll(".tpr-header-menu__nav-menu-item").forEach(i => {

                        let anchor = i.querySelector("a");
                        anchor.setAttribute("aria-expanded", "false");

                    });
                }
            }, 0);
            break;
    }
}

function restoreDefaultMenuState(e) {

    const menuItemElements = document.querySelectorAll(".tpr-header-menu__nav-menu-item")
    const overlay = document.querySelector(".tpr-header-menu__nav-overlay")

    const newlyFocusedItem = e.target.closest(".tpr-header-menu__nav-menu-item");
    if (!newlyFocusedItem) return;

    menuItemElements.forEach(item => {

        const sub = item.querySelector(".tpr-header-menu__nav-sub-menu");
        const a = item.querySelector("a");

        if (item !== newlyFocusedItem) {
            if (sub) {
                sub.style.display = "none";
                if (a) { a.setAttribute("aria-expanded", "false"); }
            }
        }

        const focusedTopLevelItem = newlyFocusedItem.querySelector("a");
        if (e.target === focusedTopLevelItem) {
            if (overlay) { overlay.classList.remove("tpr-header-menu__nav-overlay--visible"); }
            if (sub) {
                sub.style.display = "none";
                if (a) { a.setAttribute("aria-expanded", "false"); }
            }
        }
    });
}

function toggleMobileMenu() {

    const toggle = document.querySelector(".tpr-mobile-menu__toggle");
    toggle.classList.toggle("tpr-mobile-menu__toggle--open");

    const svg = document.querySelector(".tpr-mobile-menu__svg");
    if (svg) { svg.classList.toggle("tpr-mobile-menu__svg--hide"); }

    const button = document.querySelector(".tpr-header-menu__button");
    if (button) {
        const closeButtonText = button.getAttribute("data-close-label");
        const openButtonText = button.getAttribute("data-open-label")
        const isMenu = button.textContent === closeButtonText;
        button.textContent = isMenu ? openButtonText : closeButtonText;
        button.classList.toggle("tpr-header-menu__button--opened")

        const expanded = button.getAttribute("aria-expanded") === "true";
        button.setAttribute("aria-expanded", !expanded);
    }

    document.querySelectorAll(".tpr-header-menu__nav-container").forEach((nav) => {
        nav.classList.toggle("tpr-header-menu__nav-container--active");
    });
}

function expandMobileMenuSubMenu(e) {

    const arrow = e.currentTarget;

    const menuItem = arrow.closest(".tpr-header-menu__nav-menu-item");

    const subMenu = menuItem.querySelector(".tpr-header-menu__nav-sub-menu");
    subMenu.removeAttribute("style");

    subMenu.classList.toggle("tpr-header-menu__nav-sub-menu--active");
    arrow.classList.toggle("tpr-mobile-menu__arrow-down");

    const a = menuItem.querySelector("a");
    const expanded = a.getAttribute("aria-expanded") === "true";
    a.setAttribute("aria-expanded", !expanded);
}

function mobileKeyboardNavigation(e) {

    if (e.key === " ") {
        e.preventDefault();

        const menuItem = e.currentTarget;
        const subMenu = menuItem.querySelector(".tpr-header-menu__nav-sub-menu");
        if (subMenu) {
            subMenu.removeAttribute("style");      
            subMenu.classList.toggle("tpr-header-menu__nav-sub-menu--active");
        }
        
        const arrow = menuItem.querySelector(".tpr-mobile-menu__arrow");
        arrow.classList.toggle("tpr-mobile-menu__arrow-down");

        const a = menuItem.querySelector("a");
        const expanded = a.getAttribute("aria-expanded") === "true";
        a.setAttribute("aria-expanded", !expanded);
    }
}

function removeActiveClasses() {

    document.querySelectorAll(".tpr-header-menu__nav-container").forEach((nav) => {
        nav.classList.remove("tpr-header-menu__nav-container--active");
    });

    document.querySelectorAll(".tpr-mobile-menu__toggle").forEach((toggle) => {
        toggle.classList.remove("tpr-mobile-menu__toggle--open");

        var toggleText = toggle.querySelector(".tpr-header-menu__button")
        toggleText.classList.remove("tpr-header-menu__button--opened")

        var svg = toggle.querySelector(".tpr-mobile-menu__svg")
        svg.classList.remove("tpr-mobile-menu__svg--hide")

        const button = toggle.querySelector(".tpr-header-menu__button");
        button.textContent = button.getAttribute("data-close-label");
    });

    document.querySelectorAll(".tpr-mobile-menu__arrow").forEach((arrow) => {
        arrow.classList.remove("tpr-mobile-menu__arrow-down");
    });

    document.querySelectorAll(".tpr-header-menu__nav-sub-menu").forEach((subMenu) => {
        subMenu.classList.remove("tpr-header-menu__nav-sub-menu--active")
    });
}
