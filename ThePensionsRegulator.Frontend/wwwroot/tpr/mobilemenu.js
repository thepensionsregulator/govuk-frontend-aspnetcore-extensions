
document.addEventListener("DOMContentLoaded", function () {

    const mobileMenus = document.querySelectorAll('.tpr-mobile-menu-toggle');
 
    mobileMenus.forEach((menu) => {
        menu.addEventListener("click", function () {

            menu.classList.toggle('open');   

            let svg = document.querySelector(".tpr-mobile-menu__svg");
            svg.classList.toggle("hide");

            let buttonText = document.querySelector(".closed");

            if (buttonText.textContent == "Menu") {
                buttonText.textContent = "Close";
                buttonText.style.color = "#006ebc";
                
            } else {
                buttonText.textContent = "Menu";
                buttonText.style.color = "White"
            }
          
            const mobileMenuNavs = document.querySelectorAll(".tpr-mobile-menu-nav");
           
            mobileMenuNavs.forEach((nav) => {

                nav.classList.toggle('active');                                           
            });                 
        });

        let arrows = document.querySelectorAll(".arrow");

        arrows.forEach((arrow) => { 
            arrow.addEventListener('click', function () {

               var menu = arrow.closest(".navigation__menu-item");
       
                let item = menu.querySelector(".navigation__sub-menu")

                item.classList.toggle("active");
                arrow.classList.toggle("down");
            });
        });
    });
});