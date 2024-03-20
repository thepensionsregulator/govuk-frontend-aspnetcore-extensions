import { initAll } from '/govuk/all.min.js';
initAll();
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
