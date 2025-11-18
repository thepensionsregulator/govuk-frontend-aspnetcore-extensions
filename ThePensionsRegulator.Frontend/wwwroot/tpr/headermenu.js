
document.addEventListener("DOMContentLoaded", function () {


    highlightCurrentSection();
    const mediaQuery = window.matchMedia("(max-width: 993px)");
    displayOverlay();
    function SelectNavOption(e) {

     
        const toggles = document.querySelectorAll(".tpr-mobile-menu__toggle");
        const overlay = document.querySelectorAll(".tpr-header-menu__nav-overlay");
        const menuItems = document.querySelectorAll(".tpr-header-menu__nav-menu-item");
        const arrows = document.querySelectorAll(".tpr-mobile-menu__arrow");

        if (e.matches) {

            toggles.forEach(t => t.removeAttribute("href"));
            toggles.forEach(t => t.addEventListener("click", closeMenu))

            overlay.forEach(o => o.addEventListener("click", closeMenu));

            arrows.forEach(a => a.addEventListener("click", expandMobileMenuSubMenu))

            menuItems.forEach(m => m.addEventListener("keydown", mobileKeyboardNavigation));     
            console.log("mobile");
        } else {
            desktopKeyboardNavigation();
        }
    }

    SelectNavOption(mediaQuery);
    mediaQuery.addEventListener("change", function () {
        SelectNavOption(mediaQuery);
    });

function highlightCurrentSection() {

    let url = window.location.href.toString().split(window.location.host)[1];
    let section = Array.from(url).filter(char => char !== '/').join('').replace("-", " ");

    document.querySelectorAll(".tpr-header-menu__nav-menu-item").forEach((item) => {

        const anchor = item.querySelector('a');
        let x = anchor.innerHTML.toLowerCase();

        if (x.includes(section.toLowerCase())) {

            anchor.classList.toggle("tpr-header-menu__nav-menu-item--active")
        }

});
}

function displayOverlay() {

    const overlay = document.querySelector(".tpr-header-menu__nav-overlay");

    document.querySelectorAll(".tpr-header-menu__nav-menu-item").forEach((item) => {

        var subMenu = item.querySelector(".tpr-header-menu__nav-sub-menu");
        if (subMenu != null) {

            ["mouseenter"].forEach((evt) =>
                item.addEventListener(evt, () => {
                    overlay?.classList.add("tpr-header-menu__nav-overlay--visible");
                    if (window.innerWidth > 993) {
                        subMenu.style.display = 'grid';
                    }
                })

            );
            ["mouseleave"].forEach((evt) =>
                item.addEventListener(evt, () => {
                    overlay?.classList.remove("tpr-header-menu__nav-overlay--visible");
                    if (window.innerWidth > 993) {
                        subMenu.style.display = 'none';
                    }
                })

            );
        }
    });
}

function desktopKeyboardNavigation() {
    let menuItems = Array.from(document.querySelectorAll(".tpr-header-menu__nav-menu-item"));

   
    menuItems.forEach((item) => {

        let a = item.querySelector('a');
        a.setAttribute("aria-expanded", false);
        const expanded = a.getAttribute("aria-expanded") === "false";

        let subMenu = item.querySelector(".tpr-header-menu__nav-sub-menu");  
        let hasSubMenu = subMenu != null;

        if (hasSubMenu) {
            subMenu.style.display = 'none';
        }

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
                    if (hasSubMenu) {
                        subMenu.style.display = 'none';
                    }
                    break;
                case "ArrowLeft":
                    e.preventDefault();
                    menuItems[(currentIndex - 1 + menuItems.length) % menuItems.length].querySelector('a')?.focus();
                    overlay?.classList.remove("tpr-header-menu__nav-overlay--visible");
                    if (hasSubMenu) {
                        subMenu.style.display = 'none';
                    }
                    break;
                case "ArrowUp":
                    e.preventDefault()
                    if (hasSubMenu) {
                        subMenu.style.display = 'none';
                        a.setAttribute("aria-expanded", !expanded);
                        menuItems[(currentIndex)].querySelector('a')?.focus();
                    }
                    overlay?.classList.remove("tpr-header-menu__nav-overlay--visible");
                    break;
                case "ArrowDown":
                    e.preventDefault();
                    if (hasSubMenu) {
                        subMenu.style.display = 'grid';
                        overlay?.classList.add("tpr-header-menu__nav-overlay--visible");
                        a.setAttribute("aria-expanded", expanded);
                    }
                default:
                    break;
            }
        });

    });
}

function closeMenu() {
    
        const toggle = document.querySelector(".tpr-mobile-menu__toggle");
        toggle.classList.toggle("tpr-mobile-menu__toggle--open");

        const svg = document.querySelector(".tpr-mobile-menu__svg");
        svg?.classList.toggle("tpr-mobile-menu__svg--hide");

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
    });
}

function removeOverlay() {
    const overlay = document.querySelector(".tpr-header-menu__nav-overlay")
    closeMenu(overlay);
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

    menuItems.forEach((menuItem) => {

        let a = menuItem.querySelector('a');
        a.setAttribute("aria-expanded", false);

        menuItem.addEventListener("keydown", function (e) {
            if (e.key === " ") {
                e.preventDefault();
                const subMenu = menuItem.closest(".tpr-header-menu__nav-menu-item");
                const childItem = subMenu?.querySelector(".tpr-header-menu__nav-sub-menu");
                childItem?.classList.toggle("tpr-header-menu__nav-sub-menu--active");

                const menuItem = e.currentTarget;
                const subMenu = menuItem.querySelector(".tpr-header-menu__nav-sub-menu");
                subMenu.removeAttribute("style");

                const arrow = menuItem.querySelector(".tpr-mobile-menu__arrow");
 
                subMenu?.classList.toggle("tpr-header-menu__nav-sub-menu--active");
                arrow.classList.toggle("tpr-mobile-menu__arrow-down");

                const a = menuItem.querySelector("a");
                const expanded = a.getAttribute("aria-expanded") === "true";
                a.setAttribute("aria-expanded", !expanded);
            }
        });

    });
}

function removeActiveClass() {

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
