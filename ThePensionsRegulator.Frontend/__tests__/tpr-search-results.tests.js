import '@testing-library/jest-dom';

import { jest } from '@jest/globals';
import { initialiseAccordion, searchButtonOnClick, resetButtonOnClick, showMoreAnswersOnClick, setSearchResults, navigateToSearchButtonOnClick, removeNoResultsFound, resetShowMoreAnswersButton, toggleErrorTextVisibility } from '../wwwroot/tpr/tpr-search-results';

const setupBlankComponent = () => {
    document.body.innerHTML =
        `<aside class="tpr-search-results" data-content-by-id-url="/SearchResultsData" data-popular-content-url="/SearchResultsData/popularContentExample.json" data-search-content-url="/SearchResultsData/searchResults.json">
	        <h2 class="govuk-heading-m tpr-search-results__heading">Search Q&amp;As</h2>
	        <form class="tpr-search-results__form">
		        <div class="govuk-form-group">
			        <label class="govuk-label govuk-visually-hidden" for="tpr-search-results-ask-input">Search Q&amp;As</label>
			        <p class="govuk-error-message field-validation-error govuk-visually-hidden" id="tpr-search-results-error-text">
				        <span class="govuk-visually-hidden">Error:</span>
			        </p>
			        <div class="tpr-search-results__input-group">
				        <input aria-describedby="tpr-search-results-error-text" class="govuk-input" id="tpr-search-results-ask-input" required="" type="text">
				        <div class="govuk-button-group">
					        <button class="govuk-button" id="tpr-search-results-ask-button" type="submit">Ask</button>
					        <button class="govuk-button govuk-button--secondary" id="tpr-search-results-reset-button" type="button">Clear</button>
				        </div>
			        </div>
		        </div>
	        </form>
	        <footer class="tpr-search-results__footer">
		        <ul class="govuk-list tpr-search-results__links">
			        <li>
				        <a class="govuk-link" id="tpr-search-results-show-more-questions">Show more questions</a>
			        </li>
			        <li>
				        <a class="govuk-link" href="/">Home</a>
			        </li>
		        </ul>
	        </footer>
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

        await initialiseAccordion();

        const result = document.body.querySelector('.tpr-search-results__no-results-found');
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

        await initialiseAccordion();

        const results = document.querySelectorAll('.govuk-accordion__section');
        expect(results.length).toBe(sectionContent.length);

        results.forEach((result, index) => {
            const heading = result.querySelector(".govuk-accordion__section-heading");
            const content = result.querySelector(".govuk-accordion__section-content");
            expect(heading).toHaveTextContent(sectionContent[index].name);
            expect(content).toHaveTextContent(sectionContent[index].pageContent);
        });
    });

    it('should show error state if no search terms are entered', async () => {
        global.fetch = jest.fn().mockResolvedValueOnce({
            json: async () => {
                return { results: [] };
            }
        });

        setupBlankComponent();

        const searchInput = document.getElementById("tpr-search-results-ask-input");
        searchInput.value = "  ";

        const mockEvent = { preventDefault: jest.fn() };
        await searchButtonOnClick(mockEvent);

        expect(mockEvent.preventDefault).toHaveBeenCalled();
        expect(document.querySelector('.govuk-form-group')).toHaveClass('govuk-form-group--error');
        expect(document.getElementById('tpr-search-results-error-text')).not.toHaveClass('govuk-visually-hidden');
        expect(document.getElementById('tpr-search-results-error-text')).toHaveTextContent('Please enter a question or phrase.');
        expect(searchInput.value).toBe('');
        expect(searchInput).toHaveClass('govuk-input--error');
    });

    it('should show error state if search terms characters are 2 or less', async () => {
        global.fetch = jest.fn().mockResolvedValueOnce({
            json: async () => {
                return { results: [] };
            }
        });

        setupBlankComponent();

        const searchInput = document.getElementById("tpr-search-results-ask-input");
        searchInput.value = "hi";

        const mockEvent = { preventDefault: jest.fn() };
        await searchButtonOnClick(mockEvent);

        expect(mockEvent.preventDefault).toHaveBeenCalled();
        expect(document.querySelector('.govuk-form-group')).toHaveClass('govuk-form-group--error');
        expect(document.getElementById('tpr-search-results-error-text')).not.toHaveClass('govuk-visually-hidden');
        expect(document.getElementById('tpr-search-results-error-text')).toHaveTextContent('Your question must be 2 characters or more.');
        expect(searchInput).toHaveClass('govuk-input--error');
    });

    it('should show error message if search returns no results', async () => {
        global.fetch = jest.fn().mockResolvedValueOnce({
            json: async () => {
                return { results: [] };
            }
        });

        setupBlankComponent();

        const searchInput = document.getElementById("tpr-search-results-ask-input");
        searchInput.value = "test";

        const mockEvent = { preventDefault: jest.fn() };
        await searchButtonOnClick(mockEvent);

        expect(document.querySelector('.govuk-form-group')).toHaveClass('govuk-form-group--error');
        expect(document.getElementById('tpr-search-results-error-text')).not.toHaveClass('govuk-visually-hidden');
        expect(document.getElementById('tpr-search-results-error-text')).toHaveTextContent('Your question returned no results, please try again.');
        expect(document.getElementById('tpr-search-results-ask-input')).toHaveClass('govuk-input--error');

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

        setupBlankComponent();

        document.getElementById('tpr-search-results-ask-input').value = 'test';
        const mockEvent = { preventDefault: jest.fn() };
        await searchButtonOnClick(mockEvent);

        const heading = document.querySelector(".govuk-accordion__section-heading");
        const content = document.querySelector(".govuk-accordion__section-content");
        expect(heading).toHaveTextContent(contentHeading);
        expect(content).toHaveTextContent(pageContent);
        expect(document.querySelectorAll(".govuk-accordion__section").length).toBe(1);
        expect(mockEvent.preventDefault).toHaveBeenCalled();
    });

    it('should show all answers when "show more" is clicked', async () => {
        setupBlankComponent();

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
            <h3 class="tpr-search-results__no-results-found">No results found</h3>
        `;

        removeNoResultsFound();

        const noResultsFound = document.getElementsByClassName('tpr-search-results__no-results-found')[0]
        expect(noResultsFound).toBeUndefined();
    });
});

describe('resetButtonOnClick', () => {
    beforeEach(() => {
        // Ensure a clean fetch mock for each test
        global.fetch = undefined;
        setupBlankComponent();
    });

    it('clears input, recreates accordion and resets show more state', async () => {
        const initialPopular = [
            { name: 'Initial 1', pageContent: 'Initial content 1', key: 1 },
            { name: 'Initial 2', pageContent: 'Initial content 2', key: 2 }
        ];

        const refreshedPopular = [
            { name: 'After 1', pageContent: 'After content 1', key: 11 }
        ];

        global.fetch = jest.fn()
            .mockResolvedValueOnce({ json: async () => initialPopular })
            .mockResolvedValueOnce({ json: async () => refreshedPopular });

        await initialiseAccordion();

        const input = document.getElementById('tpr-search-results-ask-input');
        input.value = 'Some previous search';

        expect(document.querySelectorAll('.govuk-accordion__section').length).toBe(initialPopular.length);

        await resetButtonOnClick();

        expect(input.value).toBe('');

        const refreshedSections = document.querySelectorAll('.govuk-accordion__section');
        expect(refreshedSections.length).toBe(refreshedPopular.length);

        const showMoreButton = document.getElementById('tpr-search-results-show-more-questions');
        expect(showMoreButton).toHaveTextContent('Show more questions');
    });

    it('handles missing input element without throwing', async () => {
        global.fetch = jest.fn().mockResolvedValueOnce({ json: async () => [] });

        // DOM without input
        document.body.innerHTML = `
        <aside class="tpr-search-results">
            <form class="tpr-search-results__form">
                <div class="govuk-form-group"></div>
            </form>
            <a id="tpr-search-results-show-more-questions" class="govuk-link">Show more questions</a>
        </aside>`;

        await expect(resetButtonOnClick()).resolves.not.toThrow();
    });

    it('resets show more answers view', async () => {
        setupBlankComponent();

        resetShowMoreAnswersButton();

        const showMoreButton = document.getElementById('tpr-search-results-show-more-questions');
        expect(showMoreButton).toHaveTextContent('Show more questions');
        expect(showMoreButton).not.toHaveClass('govuk-visually-hidden');
    });

    it('resets the form to remove error text and error classes on elements', async () => {
        document.body.innerHTML = `
	    <form class="tpr-search-results__form">
		    <div class="govuk-form-group govuk-form-group--error">
			    <label class="govuk-label govuk-visually-hidden" for="tpr-search-results-ask-input">Search Q&amp;As</label>
			    <p class="govuk-error-message field-validation-error" id="tpr-search-results-error-text">
				    <span class="govuk-visually-hidden">Error:</span>Enter a search term to find questions and answers</p>
			    <div class="tpr-search-results__input-group">
				    <input aria-describedby="tpr-search-results-error-text" class="govuk-input govuk-input--error" id="tpr-search-results-ask-input" required="" type="text">
				</div>
			</div>
		</form>`;

        toggleErrorTextVisibility();

        expect(document.querySelector('.govuk-form-group')).not.toHaveClass('govuk-form-group--error');
        expect(document.getElementById('tpr-search-results-error-text')).toHaveClass('govuk-visually-hidden');
        expect(document.getElementById('tpr-search-results-ask-input')).not.toHaveClass('govuk-input--error');
    });
});