import { createAddressSearch, FetchAddressSearchService } from "/ThePensionsRegulator.Frontend/js/address-lookup/index.js";

document.addEventListener("DOMContentLoaded", function () {
    const target = document.querySelector(".address-lookup-target");

    const searchService = new FetchAddressSearchService({searchEndpoint: "/AddressLookupData/Address.json"});

    const addressSearch = createAddressSearch({
        searchService: searchService,
        onResults: (results) => console.log(results)
    });

    target.appendChild(addressSearch);
});