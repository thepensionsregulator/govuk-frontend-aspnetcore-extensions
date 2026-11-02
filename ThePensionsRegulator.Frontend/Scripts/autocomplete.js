
document.addEventListener("DOMContentLoaded", function () {

    const containers = document.querySelectorAll('.tpr-autocomplete-container');

    containers.forEach((container, index) => {

        let url = container.getAttribute('data-autocomplete-url')

        const input = container.querySelector('.govuk-input');
        input.style.display = 'none';

        let inputNameText = input.getAttribute('name');
        input.removeAttribute('name');

        let placeholderText = input.getAttribute('placeholder');

        accessibleAutocomplete({
            element: container,
            id: `tpr-autocomplete__input-${index + 1}`,
            source: function (query, populateResults) {
                fetch(url)
                    .then(response => response.json())
                    .then(data => {
                        const results = data.filter(item => item.title.toLowerCase().includes(query.toLowerCase())).map(item => item.title);
                        populateResults(results.slice(0,5))
                    })
                    .catch(error => {
                        console.error('An error has occured with autocomplete fetch operation:', error);
                    });
            },
            minLength: 2,
            placeholder: placeholderText,
            inputClasses: 'govuk-input',
            name: inputNameText,
        });
    });
});