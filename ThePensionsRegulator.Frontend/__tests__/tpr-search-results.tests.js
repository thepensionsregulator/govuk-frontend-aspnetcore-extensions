import '@testing-library/jest-dom';

import { jest } from '@jest/globals';
import { initaliseAccordion, buttonOnClick, showMoreAnswersOnClick, setSearchResults, navigateToSearchButtonOnClick, removeNoResultsFound } from '../wwwroot/tpr/tpr-search-results';


const setupBlankComponent = () => {
    document.body.innerHTML = `
        <aside class="tpr-search-results" data-content-by-id-url="/SearchResultsData" data-popular-content-url="/SearchResultsData/popularContentExample.json" data-search-content-url="/SearchResultsData/searchResults.json">
            <div>
                <h2 class="govuk-heading-m tpr-search-results__heading">Search Q&amp;As</h2>
                <div class="tpr-search-results__input">
                    <div class="govuk-form-group">
                        <label class="govuk-label govuk-visually-hidden" for="tpr-search-results-ask-input">Search Q&amp;As</label>
                        <input class="govuk-input" id="tpr-search-results-ask-input" type="text">
                    </div>
                    <button class="govuk-button tpr-button--no-next-step" id="search-results-ask-button">Ask</button>
                </div>
            <footer><ul class="govuk-list tpr-search-results__links"><li><a class="govuk-link" id="search-results-show-more-questions">Show more questions</a></li><li><a class="govuk-link" href="/">Home</a></li></ul></footer>
        </aside>`
};

const setupComponentWithAccordion = () => {
    document.body.innerHTML = `
        <aside class="tpr-search-results" data-content-by-id-url="/SearchResultsData" data-popular-content-url="/SearchResultsData/popularContentExample.json" data-search-content-url="/SearchResultsData/searchResults.json">
            <div>
                <h2 class="govuk-heading-m tpr-search-results__heading">Search Q&amp;As</h2>
                <div class="tpr-search-results__input">
                    <div class="govuk-form-group">
                        <label class="govuk-label govuk-visually-hidden" for="tpr-search-results-ask-input">Search Q&amp;As</label>
                        <input class="govuk-input" id="tpr-search-results-ask-input" type="text">
                    </div>
                    <button class="govuk-button tpr-button--no-next-step" id="search-results-ask-button">Ask</button>
                </div>
                <div class="govuk-accordion" id="search-results-accordion" data-module="govuk-accordion" data-govuk-accordion-init=""></div>
            <footer><ul class="govuk-list tpr-search-results__links"><li><a class="govuk-link" id="search-results-show-more-questions">Show more questions</a></li><li><a class="govuk-link" href="/">Home</a></li></ul></footer>
        </aside>`
};

describe('initialise accordion', () => {
    beforeEach(() => {
        jest.resetModules();
    });

    it('should display "No results found" if popular content API returns empty', async () => {
        global.fetch = jest.fn().mockResolvedValueOnce({
            json: async () => [],
        });

        setupBlankComponent();

        await initaliseAccordion();

        const result = document.body.querySelector('.tpr-search-results-no-results-found');
        const searchHeading = document.getElementsByClassName('tpr-search-results__heading')[0];
        const headingLevel = searchHeading.tagName.toLowerCase();
        const headingLevelNumber = parseInt(headingLevel.replace('h', ''));

        expect(result).toHaveTextContent('No results found');
        expect(result.tagName).toBe(`H${headingLevelNumber + 1}`);
    });

    it('should create accordion sections if popular content API returns results', async () => {
        const sectionContent = [
            { name: 'Test 1', pageContent: 'Content 1', key: 1 },
            { name: 'Test 2', pageContent: 'Content 2', key: 2 },
        ];

        global.fetch = jest.fn().mockResolvedValueOnce({
            json: async () => sectionContent,
        });

        setupBlankComponent();

        await initaliseAccordion();

        const results = document.querySelectorAll('.govuk-accordion__section');
        expect(results.length).toBe(sectionContent.length);

        results.forEach((result, index) => {
            const heading = result.querySelector(".govuk-accordion__section-heading");
            const content = result.querySelector(".govuk-accordion__section-content");
            expect(heading).toHaveTextContent(sectionContent[index].name);
            expect(content).toHaveTextContent(sectionContent[index].pageContent);
        });
    });

    it('should show "No results found" if search returns no results', async () => {
        global.fetch = jest.fn().mockResolvedValueOnce({
            json: async () => {
                return { results: [] };
            }
        });

        setupComponentWithAccordion();

        const searchInput = document.getElementById("tpr-search-results-ask-input");
        searchInput.value = "test";

        const mockEvent = { preventDefault: jest.fn() };
        await buttonOnClick(mockEvent);

        var result = document.body.querySelector('.tpr-search-results-no-results-found');

        expect(result).toHaveTextContent('No results found');
        expect(mockEvent.preventDefault).toHaveBeenCalled();
    });

    it('should fetch and display search results on ask button click', async () => {
        const contentHeading = "Heading 1";
        const pageContent = "Content test 1";

        global.fetch = jest.fn()
            .mockResolvedValueOnce({
                json: async () => {
                    return {
                        results: [
                            {
                                key: "1",
                                name: "test"
                            }
                        ]
                    };
                }
            })
            .mockResolvedValueOnce({
                json: async () => {
                    return {
                        key: "test",
                        name: contentHeading,
                        "pageContent": pageContent
                    }
                }
            });

        setupComponentWithAccordion();

        document.getElementById('tpr-search-results-ask-input').value = 'test';
        const mockEvent = { preventDefault: jest.fn() };
        await buttonOnClick(mockEvent);

        const heading = document.querySelector(".govuk-accordion__section-heading");
        const content = document.querySelector(".govuk-accordion__section-content");
        expect(heading).toHaveTextContent(contentHeading);
        expect(content).toHaveTextContent(pageContent);
        expect(document.querySelectorAll(".govuk-accordion__section").length).toBe(1);
        expect(mockEvent.preventDefault).toHaveBeenCalled();
    });

    it('should show all answers when "show more" is clicked', async () => {

        setupComponentWithAccordion();

        const lastSearchResults = [
            { name: "heading 1", pageContent: "content 1", key: "1" },
            { name: "heading 2", pageContent: "content 2", key: "2" },
            { name: "heading 3", pageContent: "content 3", key: "3" },
            { name: "heading 4", pageContent: "content 4", key: "4" },
        ];

        setSearchResults(lastSearchResults);

        await showMoreAnswersOnClick();

        const results = document.querySelectorAll('.govuk-accordion__section');
        expect(results.length).toBe(lastSearchResults.length);

        results.forEach((result, index) => {
            const heading = result.querySelector(".govuk-accordion__section-heading");
            const content = result.querySelector(".govuk-accordion__section-content");
            expect(heading).toHaveTextContent(lastSearchResults[index].name);
            expect(content).toHaveTextContent(lastSearchResults[index].pageContent);
        });
    });
});

describe('navigateToSearchButtonOnClick', () => {
    it('focuses the input and prevents default', () => {
        document.body.innerHTML = `
            <input id="tpr-search-results-ask-input" type="text" />
        `;

        const input = document.getElementById('tpr-search-results-ask-input');
        input.scrollIntoView = jest.fn();
        input.focus = jest.fn();
        const mockEvent = { preventDefault: jest.fn() };

        navigateToSearchButtonOnClick(mockEvent);

        expect(input.focus).toHaveBeenCalled();
        expect(mockEvent.preventDefault).toHaveBeenCalled();
    });

    it('does not throw if input is not found', () => {
        const mockEvent = { preventDefault: jest.fn() };

        expect(() => navigateToSearchButtonOnClick(mockEvent)).not.toThrow();
        expect(mockEvent.preventDefault).toHaveBeenCalled();
    });
});

describe('removeNoResultsFound', () => {
    it('removes the no result found heading if available', () => {
        document.body.innerHTML = `
            <h3 class="tpr-search-results-no-results-found">No results found</h3>
        `;

        removeNoResultsFound();

        const noResultsFound = document.getElementsByClassName('tpr-search-results-no-results-found')[0]
        expect(noResultsFound).toBeUndefined();
    });
});