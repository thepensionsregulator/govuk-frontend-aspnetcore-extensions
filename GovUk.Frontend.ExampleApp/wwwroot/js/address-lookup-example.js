import { createAddressLookup } from "/ThePensionsRegulator.Frontend/js/address-lookup/index.js";

document.addEventListener("DOMContentLoaded", function () {
    const target = document.querySelector(".address-lookup-target");

    const addressLookup = createAddressLookup({searchEndpoint: "/AddressLookupData/Address.json"});

    target.appendChild(addressLookup);
});