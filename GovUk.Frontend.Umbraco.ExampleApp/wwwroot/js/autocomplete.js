

document.addEventListener("DOMContentLoaded", function () {

   accessibleAutocomplete({
        
       element: document.querySelector('#eac-container-GlobalSearchInputBox'),
       id: 'easy-autocomplete',
      
        source: async (query, populateResults) => {
            const URL_PATH = 'https://www.thepensionsregulator.gov.uk'; 

            if (!query) return;

            try {
                const response = await fetch(`${URL_PATH}/api/feature/search/suggestedsearchresults?query=${encodeURIComponent(query)}`);
                const data = await response.json();
                populateResults(data.results) || [];

            } catch (error) {
                console.error('Error fetching autocomplete results:', error);
            }
        },

       minLength: 2,
       placeholder:'Search'

    });

});