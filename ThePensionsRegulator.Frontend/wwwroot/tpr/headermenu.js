document.addEventListener("DOMContentLoaded", function () {

    const mediaQuery = window.matchMedia("(max-width: 993px)");
    function SelectNavOption(e) {

        const mobileMenuInnerContainers = document.querySelectorAll(".tpr-mobile-menu__container-inner");
        const toggles = document.querySelectorAll(".tpr-header-menu__button");
        const noJsLinks = document.querySelectorAll(".tpr-mobile-menu__no-js-link");
        const overlay = document.querySelectorAll(".tpr-header-menu__nav-overlay");
        const menuItems = document.querySelectorAll(".tpr-header-menu__nav-menu-item");
        const arrowContainers = document.querySelectorAll(".tpr-header-menu__arrow-container");
        const arrows = document.querySelectorAll(".tpr-header-menu__arrow");
        const nav = document.querySelector(".tpr-header-menu__nav");

        const isMobile = e.matches;

        highlightCurrentSection(isMobile);

        if (isMobile) {

            mobileMenuInnerContainers.forEach(m => m.classList.add("tpr-mobile-menu__container-inner--hidden"));
            toggles.forEach(t => t.classList.remove("tpr-header-menu__button--hidden"));
            noJsLinks.forEach(l => l.classList.add("tpr-mobile-menu__no-js-link--hidden"))
            toggles.forEach(t => t.addEventListener("click", toggleMobileMenu));
            toggles.forEach(t => t.addEventListener("keydown", keyboardToggleMobileMenu));

            overlay.forEach(o => o.addEventListener("click", toggleMobileMenu));

            arrowContainers.forEach(a => a.removeEventListener("click", onClickDisplaySubMenuDesktop));

            arrows.forEach(a => a.addEventListener("click", expandMobileMenuSubMenu));
            arrows.forEach(a => a.classList.toggle("tpr-header-menu__arrow-right"));
            arrows.forEach(a => a.classList.remove("tpr-header-menu__arrow-down"));

            nav.removeEventListener("focusin", restoreDefaultMenuState);

            menuItems.forEach(m => {
                const subMenu = m.querySelector(".tpr-header-menu__nav-sub-menu");
                if (subMenu) {
                    const button = m.querySelector("button");
                    button.setAttribute("aria-expanded", false);
                    subMenu.style.display = "none";
                }
                m.removeEventListener("keydown", desktopKeyboardNavigation)
                m.addEventListener("keydown", closeMobileMenuOnFocusLeave)
            });
            arrows.forEach(a => a.addEventListener("keydown", mobileKeyboardNavigation))
        } else {

            overlay?.forEach(o => o.classList.remove("tpr-header-menu__nav-overlay--visible"));
            toggles.forEach(t => t.removeEventListener("click", toggleMobileMenu))
            overlay.forEach(o => o.removeEventListener("click", toggleMobileMenu));
            nav.addEventListener("focusin", restoreDefaultMenuState);
            arrowContainers.forEach(a => a.addEventListener("click", onClickDisplaySubMenuDesktop));
            arrowContainers.forEach(a => a.classList.add("js"));
            arrows.forEach(a => a.removeEventListener("click", expandMobileMenuSubMenu));

            menuItems.forEach(m => {
                const a = m.querySelector("button");
                a.setAttribute("aria-expanded", false);

                const subMenu = m.querySelector(".tpr-header-menu__nav-sub-menu");
                if (subMenu) {

                    subMenu.style.display = "none";
                }
                m.removeEventListener("keydown", closeMobileMenuOnFocusLeave)
                m.addEventListener("keydown", desktopKeyboardNavigation)
            });

            arrows.forEach(a => a.removeEventListener("keydown", mobileKeyboardNavigation));
            removeActiveClasses()
            handleOutsideClick()
        }

    }

    SelectNavOption(mediaQuery);
    mediaQuery.addEventListener("change", SelectNavOption);
});

function highlightCurrentSection(isMobile) {

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
        let anchorText = anchor.innerHTML.toLowerCase();

        const arrow = item.querySelector(".tpr-header-menu__arrow")

        if (anchorText.includes(section.toLowerCase())) {

            if (isMobile) {
                anchor.classList.remove("tpr-header-menu__nav-menu-item--active-desktop")
                anchor.classList.add("tpr-header-menu__nav-menu-item--active-mobile")
                arrow.classList.add("tpr-header-menu_arrow--active")
            } else {
                anchor.classList.remove("tpr-header-menu__nav-menu-item--active-mobile")
                arrow.classList.remove("tpr-header-menu_arrow--active")
                anchor.classList.add("tpr-header-menu__nav-menu-item--active-desktop")
            }
        }
    });
}

function onClickDisplaySubMenuDesktop(e) {

    const container = e.currentTarget;
    const item = container.querySelector(".tpr-header-menu__arrow")
    const isExpanded = item.getAttribute("aria-expanded") === "true";
    const menuItem = item.closest(".tpr-header-menu__nav-menu-item");
    const subMenu = menuItem.querySelector(".tpr-header-menu__nav-sub-menu");
    const overlay = document.querySelector(".tpr-header-menu__nav-overlay");

    item.focus();

    if (isExpanded) {
        closeSubMenuDesktop(item, subMenu, overlay);
    } else {
        openSubMenuDesktop(item, subMenu, overlay);

        document.addEventListener("click", handleOutsideClick)
    }
}

function handleOutsideClick(e) {
    const openItem = document.querySelector('[aria-expanded="true"]');
    if (!openItem) return;

    const menuItem = openItem.closest(".tpr-header-menu__nav-menu-item")
    if (menuItem === null) return

    const subMenu = menuItem.querySelector(".tpr-header-menu__nav-sub-menu");
    const overlay = document.querySelector(".tpr-header-menu__nav-overlay");

    if (menuItem.contains(e.target)) return;

    closeSubMenuDesktop(openItem, subMenu, overlay);
    openItem.focus();
}

let subMenuObservers = { desktop: null, mobile: null };

function observeSubMenuVisibility(type, element, onExit) {

    if (!("IntersectionObserver" in window)) return;

    if (subMenuObservers[type]) {
        subMenuObservers[type].disconnect();
    }

    subMenuObservers[type] = new IntersectionObserver(([entry]) => {
        if (!entry.isIntersecting) {

            if (type === 'mobile') {
               
                const mobileIsOpen = document.querySelector(".tpr-header-menu__nav-container--active") !== null;
                if (!mobileIsOpen) return;
            }

            try {
                subMenuObservers[type]?.disconnect();
                subMenuObservers[type] = null;
                onExit()
            } catch (err) {
                
                console.error(err);
            }
        }
    }, {
        root: null,
        threshold: 0
    });

    subMenuObservers[type].observe(element);
}

function closeSubMenuDesktop(item, subMenu, overlay) {
    subMenu.style.display = 'none';
    item.setAttribute("aria-expanded", "false");
    item.classList.remove("tpr-header-menu__arrow-up");
    overlay?.classList.remove("tpr-header-menu__nav-overlay--visible");
}

function openSubMenuDesktop(item, subMenu, overlay) {
    subMenu.style.display = 'grid';
    item.setAttribute("aria-expanded", "true");
    item.classList.add("tpr-header-menu__arrow-up");
    subMenu.querySelector(".tpr-header-menu__nav-sub-menu-item").removeAttribute("style");
    overlay?.classList.add("tpr-header-menu__nav-overlay--visible");

    observeSubMenuVisibility('desktop', subMenu, () => closeSubMenuDesktop(item, subMenu, overlay));
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

    let focusableElements = [];
    menuItemElements.forEach(item => {
        const anchor = item.querySelector('a');
        const button = item.querySelector('button');

        if (anchor) focusableElements.push(anchor);
        if (button) focusableElements.push(button);

    });

    const activeElement = document.activeElement;
    const currentIndex = Array.from(focusableElements).indexOf(activeElement);

    let a = menuItem.querySelector('a');
    let button = menuItem.querySelector('button');

    let subMenu = menuItem.querySelector(".tpr-header-menu__nav-sub-menu");
    let hasSubMenu = subMenu != null;

    const overlay = document.querySelector(".tpr-header-menu__nav-overlay")

    const isToggleFocused = activeElement === menuItem.querySelector("button");

    switch (e.key) {
        case "ArrowRight":
            e.preventDefault();
            const next = focusableElements[(currentIndex + 1) % focusableElements.length]
            if (next) { next.focus() }
            if (hasSubMenu) {
                closeSubMenuDesktop(button, subMenu, overlay);
            }
            break;
        case "ArrowLeft":
            e.preventDefault();
            const previous = focusableElements[(currentIndex - 1 + focusableElements.length) % focusableElements.length]
            if (previous) { previous.focus() }
            if (hasSubMenu) {
                closeSubMenuDesktop(button, subMenu, overlay);
            }
            break;
        case "ArrowUp":
            e.preventDefault()
            if (hasSubMenu) {
                closeSubMenuDesktop(button, subMenu, overlay);
                button.focus();
            }
            break;
        case "ArrowDown":
            e.preventDefault();
            if (hasSubMenu && isToggleFocused) {
                openSubMenuDesktop(button, subMenu, overlay);
            }
            break;
        case " ":
            e.preventDefault();
            if (hasSubMenu && isToggleFocused) {
                const isExpanded = button.getAttribute("aria-expanded") === "true";
                if (isExpanded) {
                    closeSubMenuDesktop(button, subMenu, overlay);
                    button.focus();
                } else {
                    openSubMenuDesktop(button, subMenu, overlay);
                }
            }
            break;
        case "Enter":
            if (hasSubMenu && isToggleFocused) {
                e.preventDefault();
                const isExpanded = button.getAttribute("aria-expanded") === "true";
                if (isExpanded) {
                    closeSubMenuDesktop(button, subMenu, overlay);
                    button.focus();
                } else {
                    openSubMenuDesktop(button, subMenu, overlay);
                }
            }
            break;
        case "Escape":
            if (hasSubMenu) {
                closeSubMenuDesktop(button, subMenu, overlay);
                button.focus();
            }
            break;
        default:
            setTimeout(() => {

                const nav = document.querySelector(".tpr-header-menu__nav");
                const activeElement = document.activeElement;

                if (!nav.contains(activeElement)) {
                    overlay?.classList.remove("tpr-header-menu__nav-overlay--visible");

                    document.querySelectorAll(".tpr-header-menu__nav-sub-menu").forEach(s => s.style.display = "none");

                    document.querySelectorAll(".tpr-header-menu__nav-menu-item").forEach(i => {

                        let button = i.querySelector("button");
                        button.setAttribute("aria-expanded", "false");
                        button.classList.remove("tpr-header-menu__arrow-up");
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
        const button = item.querySelector("button");

        if (item !== newlyFocusedItem) {
            if (sub) {
                sub.style.display = "none";
                button?.setAttribute("aria-expanded", "false");
                button.classList.remove("tpr-header-menu__arrow-up");
            }
        }

        const focusedTopLevelItem = newlyFocusedItem.querySelector("a");
        if (e.target === focusedTopLevelItem) {
            overlay?.classList.remove("tpr-header-menu__nav-overlay--visible");
            if (sub) {
                sub.style.display = "none";
                button?.setAttribute("aria-expanded", "false");
                button.classList.remove("tpr-header-menu__arrow-up");
            }
        }
    });
}

function closeMobileMenu() {
    const toggle = document.querySelector(".tpr-header-menu__button");
    toggle.classList.remove("tpr-header-menu__button--open");

    const svgs = document.querySelectorAll(".tpr-mobile-menu__svg");
    svgs.forEach(s => s.classList.remove("tpr-mobile-menu__svg--hide"));

    const button = document.querySelector(".tpr-header-menu__button-inner");
    if (button) {
        const closeButtonText = button.getAttribute("data-close-label");

        button.textContent = closeButtonText;
        button.classList.remove("tpr-header-menu__button-inner--opened")

        toggle.setAttribute("aria-expanded", false);
    }

    document.querySelectorAll(".tpr-header-menu__nav-container").forEach((nav) => {
        nav.classList.remove("tpr-header-menu__nav-container--active");

        nav.querySelectorAll(".tpr-header-menu__arrow").forEach(arrow => {
            arrow.classList.remove("tpr-header-menu__arrow-down");
            arrow.classList.add("tpr-header-menu__arrow-right");
            arrow.setAttribute("aria-expanded", "false");
        });

        nav.querySelectorAll(".tpr-header-menu__nav-sub-menu").forEach(subMenu => {
            subMenu.classList.remove("tpr-header-menu__nav-sub-menu--active");
        });

    });

}

function openMobileMenu() {
    const toggle = document.querySelector(".tpr-header-menu__button");
    if (!toggle) return;
    if (toggle.classList.contains("tpr-header-menu__button--open")) return;

    toggle.classList.add("tpr-header-menu__button--open");

    const svgs = document.querySelectorAll(".tpr-mobile-menu__svg");
    svgs.forEach(s => s.classList.add("tpr-mobile-menu__svg--hide"));

    const button = document.querySelector(".tpr-header-menu__button-inner");
    if (button) {
        const openButtonText = button.getAttribute("data-open-label");
        button.textContent = openButtonText;
        button.classList.add("tpr-header-menu__button-inner--opened")

        toggle.setAttribute("aria-expanded", true);
    }

    document.querySelectorAll(".tpr-header-menu__nav-container").forEach((nav) => {
        nav.classList.add("tpr-header-menu__nav-container--active");
        observeSubMenuVisibility('mobile', nav, closeMobileMenu)
    });
}

function toggleMobileMenu() {
    const toggle = document.querySelector(".tpr-header-menu__button");
    if (!toggle) return;
    const expanded = toggle.getAttribute("aria-expanded") === "true";
    if (expanded) {
        closeMobileMenu();
    } else {
        openMobileMenu();
    }
}

function expandMobileMenuSubMenu(e) {

    const arrow = e.currentTarget;

    const menuItem = arrow.closest(".tpr-header-menu__nav-menu-item");

    const subMenu = menuItem.querySelector(".tpr-header-menu__nav-sub-menu");
    subMenu.removeAttribute("style");

    subMenu.classList.toggle("tpr-header-menu__nav-sub-menu--active");
    arrow.classList.toggle("tpr-header-menu__arrow-down");

    const expanded = arrow.getAttribute("aria-expanded") === "true";
    arrow.setAttribute("aria-expanded", !expanded);
}

function mobileKeyboardNavigation(e) {

    if (e.key === " ") {
        e.preventDefault();

        expandMobileMenuSubMenu(e);
    }
}

function closeMobileMenuOnFocusLeave() {

    setTimeout(() => {

        const nav = document.querySelector(".tpr-header-menu__nav");
        const toggle = document.querySelector(".tpr-header-menu__button-inner");

        const activeElement = document.activeElement;

        if (activeElement !== toggle && !nav.contains(activeElement)) {
            toggleMobileMenu();
        }
    }, 0);
}

function removeActiveClasses() {

    document.querySelectorAll(".tpr-header-menu__nav-container").forEach((nav) => {
        nav.classList.remove("tpr-header-menu__nav-container--active");
    });

    document.querySelectorAll(".tpr-header-menu__button").forEach((toggle) => {
        toggle.classList.remove("tpr-header-menu__button--open");
        toggle.setAttribute("aria-expanded", "false");

        var toggleText = toggle.querySelector(".tpr-header-menu__button-inner")
        toggleText.classList.remove("tpr-header-menu__button-inner--opened")

        var svg = toggle.querySelector(".tpr-mobile-menu__svg")
        svg.classList.remove("tpr-mobile-menu__svg--hide")

        const button = toggle.querySelector(".tpr-header-menu__button-inner");
        button.textContent = button.getAttribute("data-close-label");
    });

    document.querySelectorAll(".tpr-header-menu__arrow").forEach((arrow) => {
        arrow.classList.remove("tpr-header-menu__arrow-up");
        arrow.classList.remove("tpr-header-menu__arrow-right");
    });

    document.querySelectorAll(".tpr-header-menu__nav-sub-menu").forEach((subMenu) => {
        subMenu.classList.remove("tpr-header-menu__nav-sub-menu--active")
    });
}