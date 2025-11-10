import { Accordion } from '/govuk-frontend.min.js?v=5.13.0';

let searchResults = [];

let popularContentApiUrl = "";
let searchContentApiUrl = "";
let getContentByIdApiUrl = "";
const SEARCH_TERM_QUERY_PARAM = "searchTerm";
const SHOW_MORE_QUERY_PARAM = "showMore";

let showMoreQuestions = false;
let newHeadingLevel = 3;

const INITIAL_VISIBLE_RESULTS = 2;

function getSearchInput() {
    return document.getElementById("tpr-search-results-ask-input");
}

function setSearchInput(searchTerm){
    const searchInput = getSearchInput();
    if (searchInput != null) {
        searchInput.value = searchTerm;
    }
}

function getShowMoreButton() {
    return document.getElementById("tpr-search-results-show-more-questions");
}

async function fetchJson(url) {
    const response = await fetch(url, { headers: { 'Content-Type': 'application/json' } });
    return response.json();
}

async function initialiseAccordion() {
    let queryStringSearchTerm = getSearchTermQueryString(SEARCH_TERM_QUERY_PARAM);
    let queryStringShowMore = getSearchTermQueryString(SHOW_MORE_QUERY_PARAM);

    resetShowMoreAnswersButton();
    if(queryStringShowMore)
    {
        showMoreQuestions = queryStringShowMore.toLowerCase() === 'true';
    }

    if(queryStringSearchTerm)
    {
        setSearchInput(queryStringSearchTerm);
        await searchWithTerm(queryStringSearchTerm);        
        return;        
    }
    let results = [];

    toggleErrorTextVisibility(false);

    try {
        results = await fetchJson(`${popularContentApiUrl}`);
    } catch (error) {
        console.error("Failed to fetch most popular content:", error);
    }

    results = results || [];

    if (results.length === 0) {
        createNoResultsFoundHeading();
    } else {

        searchResults = results;

        
        displayResults();
    }
}

async function searchWithTerm(searchValue) {
    if (searchValue.length < 3) {
        setErrorText('Your question must be 2 characters or more.', searchValue);
        return;
    }

    toggleErrorTextVisibility(false);

    if (!searchValue || searchValue.trim() === '') {
        // searchInput.value = '';
        setSearchInput('');
        navigateToSearchInput(false);
        toggleErrorTextVisibility(true);
        return;
    }

    setSearchTermQueryString(SEARCH_TERM_QUERY_PARAM, searchValue);
    let searchUrl = new URL(searchContentApiUrl, window.location.origin);
    searchUrl.searchParams.set(SEARCH_TERM_QUERY_PARAM, searchValue);

    let jsonResults = [];

    try {
        jsonResults = await fetchJson(searchUrl.toString());
    } catch (error) {
        console.error("Failed to search content:", error);
    }

    const results = jsonResults.results || [];

    removeAccordion("search-results-accordion");

    if (results.length === 0) {
        setErrorText('Your question returned no results, please try again.', searchValue);
        // toggleShowMoreButton(true);
    } else {
        removeNoResultsFound();
        searchResults = await Promise.all(
            results.map(async (result) => {
                return await fetchContentById(result.key);
            })
        );

        displayResults();
    }
}

function displayResults()
{
    const accordionSections = [];

    let showNow = [];

    if (showMoreQuestions === false) {
        showNow = searchResults.slice(0, INITIAL_VISIBLE_RESULTS);
    }
    else
    {
        showNow = searchResults;
    }

    showNow.forEach(result => {
        accordionSections.push(createAccordionSection(result.name, result.pageContent, result.key))
    });

    createAccordion(accordionSections);
    updateShowMoreButton();
}

function removeAccordion(id) {
    const accordion = document.getElementById(id);

    if (accordion != null) {
        accordion.remove();
    }
}

function createAccordion(accordionSections) {
    const newAccordion = createElementWithClassName("div", "govuk-accordion");
    newAccordion.setAttribute("id", "search-results-accordion");
    newAccordion.setAttribute("data-module", "govuk-accordion");

    accordionSections.forEach((accordionSection) => { newAccordion.appendChild(accordionSection); });

    new Accordion(newAccordion);

    const searchBlock = document.getElementsByClassName("tpr-search-results__form")[0];
    searchBlock.after(newAccordion);
}

function createAccordionSection(heading, content, index) {
    const accordionSection = createElementWithClassName("div", "govuk-accordion__section");

    const accordionHeader = createElementWithClassName("div", "govuk-accordion__section-header");
    const accordionHeadingElement = createElementWithClassName(`h${newHeadingLevel}`, "govuk-accordion__section-heading");

    const accordionHeadingElementText = document.createTextNode(heading);

    const accordionHeadingButtonElement = createElementWithClassName("span", "govuk-accordion__section-button");
    accordionHeadingButtonElement.setAttribute("id", `accordion-heading-${index}`);
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
    try {
        const url = `${getContentByIdApiUrl}/${contentId}`;
        return await fetchJson(url);
    } catch (error) {
        console.error("Failed to fetch content by ID:", error);
        return null;
    }
}

async function toggleErrorTextVisibility(showErrorText, errorTextContent = '') {
    const errorText = document.getElementById("tpr-search-results-error-text");
    const formGroup = document.querySelector('.tpr-search-results__form .govuk-form-group');
    const searchInput = getSearchInput();

    if (errorText != null && searchInput != null && formGroup != null) {
        if (showErrorText) {
            errorText.classList.remove("govuk-visually-hidden");
            errorText.textContent = errorTextContent;
            searchInput.classList.add("govuk-input--error");
            formGroup.classList.add("govuk-form-group--error");
        } else {
            errorText.classList.add("govuk-visually-hidden");
            searchInput.classList.remove("govuk-input--error");
            formGroup.classList.remove("govuk-form-group--error");
        }
    }
}

function setErrorText(errorMessage, value = '') {
    const searchInput = getSearchInput();
    searchInput.value = value;
    navigateToSearchInput(false);
    toggleErrorTextVisibility(true, errorMessage);
}

async function searchButtonOnClick(event) {
    event.preventDefault();

    const searchInput = getSearchInput();
    if (searchInput == null) {
        return;
    }

    const searchValue = searchInput.value?.trim() || '';
    if (!searchValue) {
        setErrorText('Please enter a question or phrase.');
        return;
    }

    searchWithTerm(searchValue);
}

async function resetButtonOnClick() {
    setSearchInput('');
    removeAccordion("search-results-accordion");
    setSearchTermQueryString(SEARCH_TERM_QUERY_PARAM, undefined);
    setSearchTermQueryString(SHOW_MORE_QUERY_PARAM,undefined);
    await initialiseAccordion();
    navigateToSearchInput(false);
}

async function resetShowMoreAnswersButton() {
    showMoreQuestions = false;
    toggleShowMoreButton(false);
    const showMoreButton = getShowMoreButton();
    if (showMoreButton != null) {
        showMoreButton.textContent = 'Show more questions';
    }
}

async function showMoreAnswersOnClick() {
    removeAccordion("search-results-accordion");

    if (showMoreQuestions === false) {
        showMoreQuestions = true;
        // results = searchResults;
        // if (showMoreButton) showMoreButton.textContent = 'Show fewer questions';
        setSearchTermQueryString(SHOW_MORE_QUERY_PARAM,true);
    }
    else {
        showMoreQuestions = false;
        setSearchTermQueryString(SHOW_MORE_QUERY_PARAM,undefined);
    }
    updateShowMoreButton()
    displayResults();
}

function updateShowMoreButton(){

    if(searchResults.length <= INITIAL_VISIBLE_RESULTS)
    {
        toggleShowMoreButton(true);
        return;
    }
    else
    {

        toggleShowMoreButton(false);
        const showMoreButton = getShowMoreButton();
        if (showMoreQuestions === false) {
            if (showMoreButton) showMoreButton.textContent = 'Show more questions';
        }
        else {
            if (showMoreButton) showMoreButton.textContent = 'Show fewer questions';
        }
    }
}

function createElementWithClassName(elementName, className) {
    const element = document.createElement(elementName);
    element.className = className;

    return element;
}

function createNoResultsFoundHeading() {
    const existingHeading = document.getElementsByClassName('tpr-search-results__no-results-found')[0];
    if (existingHeading == null) {
        const noResultsHeading = document.createElement(`h${newHeadingLevel}`);
        noResultsHeading.className = "govuk-heading-m tpr-search-results__no-results-found";
        noResultsHeading.textContent = "No results found";

        const searchBlock = document.getElementsByClassName("tpr-search-results__form")[0];
        searchBlock.after(noResultsHeading);
    }

    toggleShowMoreButton(true);
}

function toggleShowMoreButton(hideButton) {
    const showMoreButton = getShowMoreButton();
    if (showMoreButton != null) {
        if (hideButton) {
            showMoreButton.classList.add('govuk-visually-hidden');
        }
        else {
            showMoreButton.classList.remove('govuk-visually-hidden');
        }
    }
}

function removeNoResultsFound() {
    const element = document.getElementsByClassName('tpr-search-results__no-results-found')[0];
    if (element != null) {
        element.remove();
    }
}

function setSearchResults(toSet) {
    searchResults = toSet;
}

function navigateToSearchButtonOnClick(event) {
    navigateToSearchInput(true);
    event.preventDefault();
}

function navigateToSearchInput(scrollIntoView) {
    const searchInput = getSearchInput();
    if (searchInput != null) {
        if (scrollIntoView === true) {
            searchInput.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
        searchInput.focus({ preventScroll: true });
    }
}

function setSearchTermQueryString(key,value) {
    const url = new URL(window.location.href); // Get the current URL
    const params = url.searchParams; // Access query parameters

    if(value)
    {
        params.set(key, value); // Add or update the search parameter
    }
    else
    {
        params.delete(key); // Remove the search parameter
    }

    if(params.size > 0)
    {
        window.history.replaceState({}, '', `${url.pathname}?${params.toString()}`); // Update the URL without reloading
    }
    else
    {
        window.history.replaceState({}, '', url.pathname); // Update the URL without reloading
    }
}

function getSearchTermQueryString(key) {
    const url = new URL(window.location.href); // Get the current URL
    const params = url.searchParams; // Access query parameters

    return params.get(key);
}
document.addEventListener("DOMContentLoaded", function () {
    const searchButton = document.getElementById("tpr-search-results-ask-button");
    const resetButton = document.getElementById("tpr-search-results-reset-button");
    const showMoreButton = getShowMoreButton();

    if (searchButton == null || showMoreButton == null) {
        return;
    }

    searchButton.addEventListener("click", searchButtonOnClick);
    resetButton.addEventListener("click", resetButtonOnClick);
    showMoreButton.addEventListener("click", showMoreAnswersOnClick);

    const searchAside = document.getElementsByClassName("tpr-search-results")[0];
    popularContentApiUrl = searchAside.getAttribute("data-popular-content-url");
    searchContentApiUrl = searchAside.getAttribute("data-search-content-url");
    getContentByIdApiUrl = searchAside.getAttribute("data-content-by-id-url");

    const searchResultsSection = document.getElementsByClassName("tpr-search-results__heading")[0];
    const headingLevel = searchResultsSection.tagName.toLowerCase();

    const headingLevelNumber = parseInt(headingLevel.replace('h', ''));

    if (headingLevelNumber >= 6) {
        newHeadingLevel = 6;
    }
    else {
        newHeadingLevel = headingLevelNumber + 1;
    }

    initialiseAccordion();

    const navigateToSearchButton = document.getElementsByClassName("tpr-search-results__nav-button")[0];
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

export { initialiseAccordion, searchButtonOnClick, resetButtonOnClick, showMoreAnswersOnClick, setSearchResults, navigateToSearchButtonOnClick, removeNoResultsFound, resetShowMoreAnswersButton, toggleErrorTextVisibility }