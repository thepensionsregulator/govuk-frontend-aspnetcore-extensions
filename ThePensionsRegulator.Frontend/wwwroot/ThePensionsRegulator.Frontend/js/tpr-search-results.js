import { Accordion } from '/govuk-frontend.min.js?v=5.14.0';

let searchResults = [];
let popularContentApiUrl = "";
let searchContentApiUrl = "";
let getContentByIdApiUrl = "";
let showMoreQuestions = false;
let newHeadingLevel = 3;

const SEARCH_TERM_QUERY_PARAM = "searchTerm";
const SHOW_MORE_QUERY_PARAM = "showMore";
const INITIAL_VISIBLE_RESULTS = 2;

function getSearchInput() {
    return document.getElementById("tpr-search-results-ask-input");
}

function getSearchResultsText() {
    return document.getElementById("tpr-search-results-text");
}

function hideSearchResultsText(hideText) {
    const searchResultsText = getSearchResultsText();
    if (searchResultsText != null) {
        if (hideText) {
            searchResultsText.classList.add('govuk-visually-hidden');
        }
        else {
            searchResultsText.classList.remove('govuk-visually-hidden');
        }
    }
}

function setSearchResultsText(resultsTextContent) {
    const searchResultsText = getSearchResultsText();
    if (searchResultsText != null) {
        searchResultsText.innerHTML = resultsTextContent;
        hideSearchResultsText(false);
    }
}

function sanitizeString(input) {
    if (!input || typeof input !== 'string' || input.trim() === '') {
        return '';
    }

    const trimmedInput = input.trim();
    const parser = new DOMParser();
    const doc = parser.parseFromString(trimmedInput, 'text/html');
    return (doc.body.textContent || '').trim();
}

function resetSearchInput(){
    const searchInput = getSearchInput();
    if (searchInput != null) {
        searchInput.removeAttribute("aria-describedby");
        searchInput.removeAttribute("aria-invalid");
        searchInput.value = '';
    }
}

function getShowMoreQuestionsButton() {
    return document.getElementById("tpr-search-results-show-more-questions");
}

async function fetchJson(url) {
    const response = await fetch(url, { headers: { 'Content-Type': 'application/json' } });
    return response.json();
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

async function initialiseAccordion() {
    let results = [];
    let queryStringSearchTerm = getSearchTermQueryString(SEARCH_TERM_QUERY_PARAM);
    let queryStringShowMore = getSearchTermQueryString(SHOW_MORE_QUERY_PARAM);

    searchResults = [];

    resetShowMoreAnswersButton();
    showErrorTextVisibility(false);

    if (queryStringShowMore) {
        showMoreQuestions = queryStringShowMore.toLowerCase() === 'true';
    }

    if(queryStringSearchTerm)
    {
        await searchWithTerm(queryStringSearchTerm);        
        return;
    }

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
        
        await displayResults();
    }
}

async function searchWithTerm(searchValue) {
    searchValue = (searchValue || '').trim();
    searchValue = sanitizeString(searchValue);
    showErrorTextVisibility(false);
    hideShowMoreQuestionsButton(true);

    if (searchValue.length < 2) {
        setErrorText('Your question must be 2 characters or more.', searchValue);
        return;
    }

    if (!searchValue || searchValue.trim() === '') {
        resetSearchInput();
        navigateToSearchInput(false);
        showErrorTextVisibility(true);
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
        setSearchResultsText(`Your search for <strong>'${searchValue}'</strong> returned no results.`);
    } else {
        removeNoResultsFound();

        setSearchResultsText(`Your search for <strong>'${searchValue}'</strong> returned these results:`);

        searchResults = await Promise.all(
            results.map(async (result) => {
                return await fetchContentById(result.key);
            })
        );

        await displayResults();
    }
}

async function displayResults()
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
        let section = createAccordionSection(result.name, result.pageContent, result.key);
        accordionSections.push(section)
    });

    await createAccordion(accordionSections);
    await updateShowMoreButton();
}

function removeAccordion(id) {
    const accordion = document.getElementById(id);

    if (accordion != null) {
        accordion.remove();
    }
}

async function createAccordion(accordionSections) {
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

function showErrorTextVisibility(showErrorText, errorTextContent = '') {
    const errorText = document.getElementById("tpr-search-results-error-text");
    const formGroup = document.querySelector('.tpr-search-results__form .govuk-form-group');
    const searchInput = getSearchInput();

    if (errorText != null && searchInput != null && formGroup != null) {
        if (showErrorText) {
            errorText.classList.remove("govuk-visually-hidden");
            errorText.textContent = errorTextContent;
            searchInput.classList.add("govuk-input--error");
            searchInput.setAttribute("aria-describedby","tpr-search-results-error-text");
            searchInput.setAttribute("aria-invalid","true");
            formGroup.classList.add("govuk-form-group--error");
        } else {
            errorText.classList.add("govuk-visually-hidden");
            searchInput.classList.remove("govuk-input--error");
            formGroup.classList.remove("govuk-form-group--error");
            searchInput.removeAttribute("aria-describedby");
            searchInput.removeAttribute("aria-invalid");
        }
    }
}

function setErrorText(errorMessage, value = '') {
    const searchInput = getSearchInput();
    searchInput.value = value;
    navigateToSearchInput(false);
    showErrorTextVisibility(true, errorMessage);
}

async function searchButtonOnClick(event) {
    event.preventDefault();

    const searchInput = getSearchInput();
    if (searchInput == null) {
        return;
    }

    const searchValue = (searchInput.value || '').trim() || '';
    if (!searchValue) {
        setErrorText('Please enter a question or phrase.');
        return;
    }

    await searchWithTerm(searchValue);
}

async function resetButtonOnClick() {
    resetSearchInput();
    removeAccordion("search-results-accordion");
    setSearchTermQueryString(SEARCH_TERM_QUERY_PARAM, undefined);
    setSearchTermQueryString(SHOW_MORE_QUERY_PARAM, undefined);
    await initialiseAccordion();
    hideSearchResultsText(true);
    navigateToSearchInput(false);
}

async function resetShowMoreAnswersButton() {
    showMoreQuestions = false;
    hideShowMoreQuestionsButton(false);
    const showMoreButton = getShowMoreQuestionsButton();
    if (showMoreButton != null) {
        showMoreButton.textContent = 'Show more questions';
    }
}

async function showMoreAnswersOnClick() {
    removeAccordion("search-results-accordion");

    if (showMoreQuestions === false) {
        showMoreQuestions = true;
        setSearchTermQueryString(SHOW_MORE_QUERY_PARAM,true);
    }
    else {
        showMoreQuestions = false;
        setSearchTermQueryString(SHOW_MORE_QUERY_PARAM,undefined);
    }
    updateShowMoreButton()
    await displayResults();
}

async function updateShowMoreButton(){

    if(searchResults.length <= INITIAL_VISIBLE_RESULTS)
    {
        hideShowMoreQuestionsButton(true);
        return;
    }
    else
    {
        hideShowMoreQuestionsButton(false);
        const showMoreButton = getShowMoreQuestionsButton();
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

    hideShowMoreQuestionsButton(true);
}

function hideShowMoreQuestionsButton(hideButton) {
    const showMoreButton = getShowMoreQuestionsButton();
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
    const showMoreButton = getShowMoreQuestionsButton();

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

    const navigateToSearchButtons = document.getElementsByClassName("tpr-search-results__nav-button");
    for (const navigateToSearchButton of navigateToSearchButtons) {
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

export { initialiseAccordion, searchButtonOnClick, resetButtonOnClick, showMoreAnswersOnClick, setSearchResults, navigateToSearchButtonOnClick, removeNoResultsFound, resetShowMoreAnswersButton, showErrorTextVisibility, setSearchTermQueryString, SEARCH_TERM_QUERY_PARAM, SHOW_MORE_QUERY_PARAM, sanitizeString };