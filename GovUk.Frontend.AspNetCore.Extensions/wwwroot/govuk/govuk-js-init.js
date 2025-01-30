import { initAll } from '/govuk/all.min.js';

const [html] = document.getElementsByTagName("html");
const lang = html.getAttribute("lang");
const config = lang === 'cy' || lang === 'cy-GB' ? {
    characterCount: {
        i18n: {
            charactersAtLimit: 'Mae gennych chi 0 nod ar ô',
            charactersUnderLimit: {
                one: 'Mae gennych chi %{count} nod ar ôl',
                other: 'Mae gennych chi %{count} nod ar ô'
            },
            charactersOverLimit: {
                one: 'Mae gennych chi %{count} nod yn ormod',
                other: 'Mae gennych chi %{count} o nodau’n ormod'
            },
            wordsUnderLimit: {
                one: 'Mae gennych chi %{count} gair ar ôl',
                other: 'Mae gennych chi %{count} o eiriau ar ôl'
            },
            wordsAtLimit: 'Mae gennych chi 0 o eiriau ar ôl',
            wordsOverLimit: {
                one: 'Mae gennych chi %{count} gair yn ormod',
                other: 'Mae gennych chi %{count} o eiriau’n ormod'
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
