
document.addEventListener("DOMContentLoaded", function () {

    const mediaQuery = window.matchMedia("(max-width: 993px)");
    displayOverlay();
    function SelectNavOption(e) {
        if (e.matches) {
            removeActiveClass(e);
            mobileKeyboardNavigation(e);
            toggleMobileMenu();
            expandMobileMenuSubMenu();
            console.log("Mobile")
        } else {
            desktopKeyboardNavigation(e);
            console.log("Desktop")
        }
    }

    SelectNavOption(mediaQuery);
    mediaQuery.addEventListener("change", function () {
        SelectNavOption(mediaQuery);
    });

});

function displayOverlay() {

    const overlay = document.querySelector(".tpr-header-menu__nav-overlay");

    document.querySelectorAll(".tpr-header-menu__nav-menu-item").forEach((item) => {

        var x = item.querySelector(".tpr-header-menu__nav-sub-menu");


        ["mouseenter"].forEach((evt) =>
            item.addEventListener(evt, () => {
                overlay?.classList.add("tpr-header-menu__nav-overlay--visible");
                if (window.innerWidth > 993) {
                    x.style.display = 'grid';
                }
            })

        );
        ["mouseleave"].forEach((evt) =>
            item.addEventListener(evt, () => {
                overlay?.classList.remove("tpr-header-menu__nav-overlay--visible");
                if (window.innerWidth > 993) {
                    x.style.display = 'none';
                }
            })

        );
    });
}

function desktopKeyboardNavigation(e) {
    let menuItems = Array.from(document.querySelectorAll(".tpr-header-menu__nav-menu-item"));

    menuItems.forEach((item) => {
        if (!e.matches) {
            var x = item.querySelector(".tpr-header-menu__nav-sub-menu");
            x.style.display = 'none';

            item.addEventListener("keydown", function (e) {

                const activeElement = document.activeElement;

                const currentItem = activeElement.closest(".tpr-header-menu__nav-menu-item");

                const currentIndex = menuItems.indexOf(currentItem);

                const overlay = document.querySelector(".tpr-header-menu__nav-overlay")

                switch (e.key) {
                    case "ArrowRight":
                        e.preventDefault();
                        menuItems[(currentIndex + 1) % menuItems.length].querySelector('a')?.focus();
                        overlay?.classList.remove("tpr-header-menu__nav-overlay--visible");
                        x.style.display = 'none';
                        break;
                    case "ArrowLeft":
                        e.preventDefault();
                        menuItems[(currentIndex - 1 + menuItems.length) % menuItems.length].querySelector('a')?.focus();
                        overlay?.classList.remove("tpr-header-menu__nav-overlay--visible");
                        x.style.display = 'none';
                        break;
                    case "ArrowUp":
                        e.preventDefault()
                        x.style.display = 'none';
                        overlay?.classList.remove("tpr-header-menu__nav-overlay--visible");
                        break;
                    case "ArrowDown":
                        e.preventDefault();
                        x.style.display = 'grid';
                        overlay?.classList.add("tpr-header-menu__nav-overlay--visible");
                    default:
                        break;
                }
            });
        }
    });
}

function toggleMobileMenu() {
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

    });
}
    function expandMobileMenuSubMenu() {
        document.querySelectorAll(".tpr-mobile-menu__arrow").forEach((arrow) => {
            arrow.addEventListener("click", function () {
                const menu = arrow.closest(".tpr-header-menu__nav-menu-item");
                const item = menu?.querySelector(".tpr-header-menu__nav-sub-menu");
                item?.classList.toggle("tpr-header-menu__nav-sub-menu--active");
                arrow.classList.toggle("tpr-mobile-menu__arrow-down");
            });
        });
    }

    function mobileKeyboardNavigation(e) {
        const menuItems = document.querySelectorAll(".tpr-header-menu__nav-menu-item");

        menuItems.forEach((menuItem) => {
            if (e.matches) {
                menuItem.addEventListener("keydown", function (e) {
                    if (e.key === " ") {
                        e.preventDefault();
                        const subMenu = menuItem.closest(".tpr-header-menu__nav-menu-item");
                        const childItem = subMenu?.querySelector(".tpr-header-menu__nav-sub-menu");
                        childItem?.classList.toggle("tpr-header-menu__nav-sub-menu--active");

                        const arrow = menuItem.querySelector(".tpr-mobile-menu__arrow");
                        arrow.classList.toggle("tpr-mobile-menu__arrow-down");

                    }
                });
            }
        });
    }

    function removeActiveClass(e) {

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
            button.textContent = "Menu";
        })
    }