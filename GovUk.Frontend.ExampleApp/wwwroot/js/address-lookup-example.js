import { createAddressSearch } from "/ThePensionsRegulator.Frontend/js/address-lookup/index.js";

document.addEventListener("DOMContentLoaded", function () {
    const target = document.querySelector(".address-lookup-target");
    const addressSearch = createAddressSearch();

    target.appendChild(addressSearch);
});