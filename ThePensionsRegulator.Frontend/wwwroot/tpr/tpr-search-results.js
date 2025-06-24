import { Accordion } from '/govuk/all.min.js';

let searchResults = [];

let popularContentApiUrl = "";
let searchContentApiUrl = "";
let getContentByIdApiUrl = "";

let newHeadingLevel = 3;

async function initaliseAccordion() {
    const response = await fetch(`${popularContentApiUrl}`, { method: 'GET', headers: { 'Content-Type': 'application/json' } });

    const results = (await response.json());

    if (results.length === 0) {
        const noResultsHeading = createNoResultsFoundHeading();
        const searchBlock = document.getElementsByClassName("tpr-search-results__input")[0];

        searchBlock.after(noResultsHeading);
    } else {
        const accordionSections = [];

        searchResults = results;

        const showNow = searchResults.slice(0, 2);

        showNow.forEach(result => {
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

    const accordionHeadingElement = createElementWithClassName(`h${newHeadingLevel}`, "govuk-accordion__section-heading");

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
    const response = await fetch(`${getContentByIdApiUrl}/${contentId}`, { method: 'GET', headers: { 'Content-Type': 'application/json' } });
    return await response.json();
}

async function buttonOnClick(event) {
    event.preventDefault();

    const searchInput = document.getElementById("tpr-search-results-ask-input");
    const searchValue = searchInput.value

    const response = await fetch(`${searchContentApiUrl}?&searchTerm=${searchValue}`, { method: 'GET', headers: { 'Content-Type': 'application/json' } });

    const results = (await response.json()).results;

    removeAccordion("search-results-accordion");

    if (results.length === 0) {
        const noResultsHeading = createNoResultsFoundHeading();
        const searchBlock = document.getElementsByClassName("tpr-search-results__input")[0];
        searchBlock.after(noResultsHeading);
    } else {

        searchResults = [];

        await Promise.all(results.map(async (result) => {
            const content = await fetchContentById(result.key);
            searchResults.push(content);
        }));

        const showNow = searchResults.slice(0, 2);
        const accordionSections = [];
        showNow.forEach(result => {
            accordionSections.push(createAccordionSection(result.name, result.pageContent, result.key))
        });

        createAccordion(accordionSections);
    }
}

async function showMoreAnswersOnClick() {
    removeAccordion("search-results-accordion");

    const accordionSections = [];

    searchResults.forEach(result => {
        accordionSections.push(createAccordionSection(result.name, result.pageContent, result.key));
    });

    createAccordion(accordionSections);
}

function createElementWithClassName(elementName, className) {
    const element = document.createElement(elementName);
    element.className = className;

    return element;
}

function createNoResultsFoundHeading() {
    const noResultsHeading = document.createElement(`h${newHeadingLevel}`);
    noResultsHeading.className = "govuk-heading-m tpr-search-results-no-results-found";
    noResultsHeading.textContent = "No results found";
    return noResultsHeading;
}

function setSearchResults(toSet) {
    searchResults = toSet;
}

function navigateToSearchButtonOnClick(event) {
    const searchResultsAside = document.getElementById("tpr-search-results-ask-input");
    if (searchResultsAside != null) {
        searchResultsAside.scrollIntoView({ behavior: 'smooth', block: 'center' });
        searchResultsAside.focus({ preventScroll: true });
    }

    event.preventDefault();
}

document.addEventListener("DOMContentLoaded", function () {
    const searchButton = document.getElementById("tpr-search-results-ask-button");
    searchButton.addEventListener("click", buttonOnClick);

    const showMoreButton = document.getElementById("tpr-search-results-show-more-questions");
    showMoreButton.addEventListener("click", showMoreAnswersOnClick);

    const searchAside = document.getElementsByClassName("tpr-search-results")[0];
    popularContentApiUrl = searchAside.getAttribute("data-popular-content-url");
    searchContentApiUrl = searchAside.getAttribute("data-search-content-url");
    getContentByIdApiUrl = searchAside.getAttribute("data-content-by-id-url");

    const searchResultsSection = document.getElementsByClassName("govuk-heading-m tpr-search-results__heading")[0];
    const headingLevel = searchResultsSection.tagName.toLowerCase();

    const headingLevelNumber = parseInt(headingLevel.replace('h', ''));

    if (headingLevelNumber >= 6) {
        newHeadingLevel = 6;
    }
    else {
        newHeadingLevel = headingLevelNumber + 1;
    }

    initaliseAccordion(popularContentApiUrl);

    const navigateToSearchButton = document.getElementsByClassName("tpr-search-results-nav-button")[0];
    if (navigateToSearchButton != null) {
        const newTabSpan = navigateToSearchButton.querySelector("span.govuk-visually-hidden");
        if (newTabSpan) {
            newTabSpan.remove();
        }

        if (navigateToSearchButton.hasAttribute("rel")) {
            navigateToSearchButton.removeAttribute("rel");
        }

        navigateToSearchButton.addEventListener("click", navigateToSearchButtonOnClick);
    }
});

export { initaliseAccordion, buttonOnClick, showMoreAnswersOnClick, setSearchResults, navigateToSearchButtonOnClick }