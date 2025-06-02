
document.addEventListener("DOMContentLoaded", function () {

    const searchBars = document.querySelectorAll('.tpr-autocomplete-container');
    
    searchBars.forEach((searchBar) => {

        let url = searchBar.getAttribute('data-autocomplete-url')
              
        const inputs = document.querySelectorAll('.tpr-header-search__input');
        let placeholderText;

        inputs.forEach((input) => { 
            input.style.display = 'none';
             placeholderText = input.getAttribute("placeholder");
             inputNameText = input.getAttribute("name");
        });

        accessibleAutocomplete({

            element: searchBar,
            id: 'tpr-header-search-autocomplete',
            source: function (query, populateResults) {
                fetch(url)
                    .then(response => response.json())
                    .then(data => {
                        const results = data.filter(item => item.title.toLowerCase().includes(query.toLowerCase())).map(item => item.title);
                        populateResults(results.slice(0,5))                 
                    })
                    .catch(error => {
                        console.error('An error has occured with tpr-header-search fetch operation:', error);
                    });
        },
            minLength: 2,
            placeholder: placeholderText,
            inputClasses: 'govuk-input',   
            name: inputNameText
        });
    });
});