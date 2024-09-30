import { initAll } from '/govuk/all.min.js';
const isWelsh = true;
const config = isWelsh ? {
    characterCount: {
        i18n: {
            charactersAtLimit: 'No characters left (W)',
            charactersUnderLimit: {
                other: '%{count} characters to go (W)',
                one: 'One character to go (W)'
            },
            charactersOverLimit: {
                one: 'One character too many (W)',
                other: 'You have %{count} characters too many (W)'
            },
            wordsUnderLimit: {
                one: 'You have %{count} word remaining (W)',
                other: 'You have %{count} words remaining (W)'
            },
            wordsAtLimit: 'You have 0 words remaining (W)',
            wordsOverLimit: {
                one: 'You have %{count} word too many (W)',
                other: 'You have %{count} words too many (W)'
            }
        }
    }
} : {}
initAll(config);
Array.prototype.forEach.call(
  document.querySelectorAll(".govuk-button[type=submit]"),
  function (button) {
    button.addEventListener("click", function (e) {
      if (e.target.getAttribute("aria-disabled") == "true") {
        e.preventDefault();
      }
    });
  }
);
