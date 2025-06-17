import { Accordion } from '/govuk/all.min.js';
document.addEventListener("DOMContentLoaded", function () {

    const searchButton = document.getElementById("ask-button");
    searchButton.addEventListener("click", buttonOnClick);

    const searchAside = document.getElementsByClassName("tpr-search")[0];
    const apiUrl = searchAside.getAttribute("data-api-url");

    async function initaliseAccordion() {
        const response = await fetch(`${apiUrl}/api/faq/content/popular`, { method: 'GET', headers: { 'Content-Type': 'application/json' } });

        const results = (await response.json());

        if (results.length === 0) {
            const noResultsHeading = createNoResultsFoundHeading();
            const searchBlock = document.getElementsByClassName("tpr-search-results__input")[0];
            searchBlock.after(noResultsHeading);
        } else {
            const accordionSections = [];
            results.forEach(result => {
                accordionSections.push(createAccordionSection(result.name, result.pageContent, result.key));
            });
            createAccordion(accordionSections);
        }
    }

    function removeAccordion(id) {
        const accordion = document.getElementById(id);
        accordion.remove();
    }

    function createAccordion(accordionSections) {
        const newAccordion = createElementWithClassName("div", "govuk-accordion");
        newAccordion.setAttribute("id", "search-results-accordion");
        newAccordion.setAttribute("data-module", "govuk-accordion");

        accordionSections.forEach((accordionSection) => { newAccordion.appendChild(accordionSection); });

        new Accordion(newAccordion);

        const searchBlock = document.getElementsByClassName("tpr-search-results__input")[0];
        searchBlock.after(newAccordion);
    }

    function createAccordionSection(heading, content, index) {
        const accordionSection = createElementWithClassName("div", "govuk-accordion__section");

        const accordionHeader = createElementWithClassName("div", "govuk-accordion__section-header");

        const accordionHeadingElement = createElementWithClassName("h3", "govuk-accordion__section-heading"); 

        const accordionHeadingElementText = document.createTextNode(heading);

        const accordionHeadingButtonElement = createElementWithClassName("span", "govuk-accordion__section-button");
        accordionHeadingButtonElement.setAttribute("id", `accordion1-heading-${index}`);
        accordionHeadingButtonElement.appendChild(accordionHeadingElementText);

        accordionHeadingElement.appendChild(accordionHeadingButtonElement);
        accordionHeader.appendChild(accordionHeadingElement);

        const accordionContent = document.createElement("div");
        accordionContent.className = "govuk-accordion__section-content";
        accordionContent.innerHTML = content;

        accordionSection.appendChild(accordionHeader);
        accordionSection.appendChild(accordionContent);

        return accordionSection;
    }

    async function fetchContentById(contentId) {
        const response = await fetch(`${apiUrl}/api/faq/content/${contentId}`, { method: 'GET', headers: { 'Content-Type': 'application/json' } });
        return await response.json(); 
    }

    async function buttonOnClick() {
        const searchInput = document.getElementById("ask-input");
        const searchValue = searchInput.value

        const response = await fetch(`${apiUrl}/api/faq/content/search?searchTerm=${searchValue}&page=1&pageSize=5`, { method: 'GET', headers: { 'Content-Type': 'application/json' } });


        const results = (await response.json()).results;

        removeAccordion("search-results-accordion");

        if (results.length === 0) {
            const noResultsHeading = createNoResultsFoundHeading();
            const searchBlock = document.getElementsByClassName("tpr-search-results__input")[0];
            searchBlock.after(noResultsHeading);
        } else {

            const accordionSections = [];
            await Promise.all(results.map(async (result, index) => {
                const content = await fetchContentById(result.key);
                const header = content.name;
                const pageContent = content.pageContent;

                accordionSections.push(createAccordionSection(header, pageContent, index))
            }));
            createAccordion(accordionSections);

        }

    }

    function createElementWithClassName(elementName, className) {
        const element = document.createElement(elementName);
        element.className = className;

        return element;
    }

    function createNoResultsFoundHeading() {
        const noResultsHeading = document.createElement("h3");
        noResultsHeading.className = "govuk-heading-m";
        noResultsHeading.textContent = "No Results Found";
        return noResultsHeading;

    }

    initaliseAccordion();
});