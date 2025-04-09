
document.addEventListener("DOMContentLoaded", function () {

    const searchBars = document.querySelectorAll('.tpr-autocomplete-container');
   
    searchBars.forEach((searchBar) => {

        accessibleAutocomplete({

            element: searchBar,
            id: 'easy-autocomplete',
            source: function (query, populateResults) {
                fetch('/data.json')
                    .then(response => response.json())
                    .then(data => {
                        const results = data.filter(item => item.title.toLowerCase().includes(query.toLowerCase())).map(item => item.title);
                        populateResults(results.slice(0,5))                 
                    })
                    .catch(error => {
                        console.error('An error has occured with your fetch operation:', error);
                    });
        },
            minLength: 2,
            placeholder: 'Search'

        });
    });
});