
document.addEventListener("DOMContentLoaded", function () {
    const mobileMenus = document.querySelectorAll(".tpr-mobile-menu-toggle");

    mobileMenus.forEach((menu) => {
        menu.removeAttribute("href");
        menu.setAttribute("role", "button");

        menu.addEventListener("click", function () {
            menu.classList.toggle("open");

            const svg = document.querySelector(".tpr-mobile-menu__svg");
            svg?.classList.toggle("hide");

            const buttonText = document.querySelector(".closed");
            if (buttonText) {
                const isMenu = buttonText.textContent === "Menu";
                buttonText.textContent = isMenu ? "Close" : "Menu";
                buttonText.style.color = isMenu ? "#006ebc" : "white";
            }

            document.querySelectorAll(".tpr-header-menu-nav").forEach((nav) => {
                nav.classList.toggle("active");
            });
        });

        document.querySelectorAll(".arrow").forEach((arrow) => {
            arrow.addEventListener("click", function () {
                const menu = arrow.closest(".navigation__menu-item");
                const item = menu?.querySelector(".navigation__sub-menu");
                item?.classList.toggle("active");
                arrow.classList.toggle("down");
            });
        });
    });

    document.querySelectorAll(".navigation__menu-item").forEach((parentItem) => {
        parentItem.addEventListener("keydown", function (e) {
            if (e.key === " ") {
                e.preventDefault();

                const isExpanded = parentItem.getAttribute("aria-expanded") === "true";
                const subMenu = parentItem.querySelector(".navigation__sub-menu");

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
        const arrows = document.querySelectorAll(".arrow");

        arrows.forEach((arrow) => {
            if (e.matches) {
                arrow.setAttribute("tabindex", "0");
                arrow.addEventListener("keydown", function (e) {
                    if (e.key === " ") {
                        e.preventDefault();
                        const menu = arrow.closest(".navigation__menu-item");
                        const item = menu?.querySelector(".navigation__sub-menu");
                        item?.classList.toggle("active");
                        arrow.classList.toggle("down");
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

        document.querySelectorAll(".tpr-header-menu-nav").forEach((nav) => {
            nav.classList.remove("active");
        });
    }

    mediaQuery.addListener(removeActiveClass);
    removeActiveClass(mediaQuery);

    mediaQuery.addListener(expandMobileMenu);
    expandMobileMenu(mediaQuery);

    const overlay = document.querySelector(".nav-overlay");

    document.querySelectorAll(".navigation__menu-item").forEach((item) => {
        ["mouseenter", "focusin"].forEach((evt) =>
            item.addEventListener(evt, () => overlay?.classList.add("visible"))
        );
        ["mouseleave", "focusout"].forEach((evt) =>
            item.addEventListener(evt, () => overlay?.classList.remove("visible"))
        );
    });
});
