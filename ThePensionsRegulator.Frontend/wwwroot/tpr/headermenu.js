
document.addEventListener("DOMContentLoaded", function () {

    const mobileMenus = document.querySelectorAll(".tpr-mobile-menu__toggle");

    mobileMenus.forEach((menu) => {
        menu.removeAttribute("href");
        menu.setAttribute("role", "button");

        menu.addEventListener("click", function () {
            menu.classList.toggle("tpr-mobile-menu__toggle--open");

            const svg = document.querySelector(".tpr-mobile-menu__svg");
            svg?.classList.toggle("tpr-mobile-menu__svg--hide");

            const button = document.querySelector(".tpr-header-menu__button");
            if (button) {
                const isMenu = button.textContent === "Menu";
                button.textContent = isMenu ? "Close" : "Menu";          
                button.classList.toggle("tpr-header-menu__button--opened")
                
            }

            document.querySelectorAll(".tpr-header-menu__nav-container").forEach((nav) => {
                nav.classList.toggle("tpr-header-menu__nav-container--active");
            });
        });

        document.querySelectorAll(".tpr-mobile-menu__arrow").forEach((arrow) => {
            arrow.addEventListener("click", function () {
                const menu = arrow.closest(".tpr-header-menu__nav-menu-item");
                const item = menu?.querySelector(".tpr-header-menu__nav-sub-menu");
                item?.classList.toggle("tpr-header-menu__nav-sub-menu--active");
                arrow.classList.toggle("tpr-mobile-menu__arrow-down");
            });
        });
    });

    document.querySelectorAll(".tpr-header-menu__nav-menu-item").forEach((parentItem) => {
        parentItem.addEventListener("keydown", function (e) {
            if (e.key === " ") {
                e.preventDefault();

                const isExpanded = parentItem.getAttribute("aria-expanded") === "true";
                const subMenu = parentItem.querySelector(".tpr-header-menu__nav-sub-menu");

                if (subMenu) {
                    subMenu.setAttribute("aria-expanded", String(!isExpanded));
                    parentItem.setAttribute("aria-expanded", String(!isExpanded));

                    subMenu.querySelectorAll("a").forEach((link) => {
                        link.setAttribute("tabindex", !isExpanded ? "0" : "-1");
                    });
                }
            }
        });
    });

    const mediaQuery = window.matchMedia("(max-width: 993px)");

    function expandMobileMenu(e) {
        const arrows = document.querySelectorAll(".tpr-mobile-menu__arrow");

        arrows.forEach((arrow) => {
            if (e.matches) {
                arrow.setAttribute("tabindex", "0");
                arrow.addEventListener("keydown", function (e) {
                    if (e.key === " ") {
                        e.preventDefault();
                        const menu = arrow.closest(".tpr-header-menu__nav-menu-item");
                        const item = menu?.querySelector(".tpr-header-menu__nav-sub-menu");
                        item?.classList.toggle("tpr-header-menu__nav-sub-menu--active");
                        arrow.classList.toggle("tpr-mobile-menu__arrow-down");
                    }
                });
            } else {
                if (arrow.getAttribute("tabindex") === "0") {
                    arrow.setAttribute("tabindex", "-1");
                }
            }
        });
    }

    function removeActiveClass(e) {

        document.querySelectorAll(".tpr-header-menu__nav-container").forEach((nav) => {
            nav.classList.remove("tpr-header-menu__nav-container--active");
        });
    }

    mediaQuery.addListener(removeActiveClass);
    removeActiveClass(mediaQuery);

    mediaQuery.addListener(expandMobileMenu);
    expandMobileMenu(mediaQuery);

    const overlay = document.querySelector(".tpr-header-menu__nav-overlay");

    document.querySelectorAll(".tpr-header-menu__nav-menu-item").forEach((item) => {
        ["mouseenter", "focusin"].forEach((evt) =>
            item.addEventListener(evt, () => overlay?.classList.add("tpr-header-menu__nav-overlay--visible"))
        );
        ["mouseleave", "focusout"].forEach((evt) =>
            item.addEventListener(evt, () => overlay?.classList.remove("tpr-header-menu__nav-overlay--visible"))
        );
    });
});
