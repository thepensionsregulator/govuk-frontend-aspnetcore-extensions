import {
	ensureErrorSummary,
	updateErrorSummary,
	updateTitle,
} from "../../Scripts/govuk-components/error-summary.js";
import "@testing-library/jest-dom";

describe("ensureErrorSummary", () => {
	beforeEach(() => {
		document.body.innerHTML = "<main><div id='page-content'></div></main>";
	});

	it("should create and insert an error summary as the first child of main", () => {
		const summary = ensureErrorSummary();
		const main = document.querySelector("main");

		expect(summary).toBe(main?.firstElementChild);
		expect(summary).toHaveClass("govuk-error-summary", "govuk-!-display-none");
	});

	it("should return the created error summary", () => {
		const summary = ensureErrorSummary();

		expect(summary).toBe(document.querySelector(".govuk-error-summary"));
	});

	it("should create the GOV.UK error summary markup", () => {
		const summary = ensureErrorSummary();
		const heading = summary.querySelector("h2");
		const errorList = summary.querySelector("ul");

		expect(summary).toHaveAttribute("role", "alert");
		expect(summary).toHaveAttribute("data-module", "govuk-error-summary");
		expect(summary).toHaveAttribute("aria-labelledby", "error-summary-title");
		expect(heading).toHaveAttribute("id", "error-summary-title");
		expect(heading).toHaveClass("govuk-error-summary__title");
		expect(heading).toHaveTextContent("There is a problem");
		expect(errorList).toHaveClass("govuk-list", "govuk-error-summary__list");
		expect(errorList?.parentElement).toHaveClass("govuk-error-summary__body");
	});

	it("should return an existing error summary without creating a duplicate", () => {
		const existingSummary = document.createElement("div");
		existingSummary.className = "govuk-error-summary";
		document.querySelector("main")?.prepend(existingSummary);

		const summary = ensureErrorSummary();

		expect(summary).toBe(existingSummary);
		expect(document.querySelectorAll(".govuk-error-summary")).toHaveLength(1);
		expect(existingSummary).toBeEmptyDOMElement();
	});
});

describe("updateTitle", () => {
	beforeEach(() => {
		document.head.innerHTML = "<title>Example</title>";
		document.body.innerHTML = "";
	});

	it("should add the default error prefix when an error has visible content", () => {
		document.body.innerHTML = `
			<p class="govuk-error-message">
				<span class="govuk-visually-hidden">Error: </span>
				Enter a valid postcode
			</p>`;

		updateTitle();

		expect(document.title).toBe("Error: Example");
	});

	it("should not add the error prefix for an empty error placeholder", () => {
		document.body.innerHTML = `
			<p class="govuk-error-message">
				<span class="govuk-visually-hidden">Error: </span>
			</p>`;

		updateTitle();

		expect(document.title).toBe("Example");
	});

	it("should not add the error prefix multiple times", () => {
		document.title = "Error: Example";
		document.body.innerHTML = "<p class='govuk-error-message'>Enter a valid postcode</p>";

		updateTitle();

		expect(document.title).toBe("Error: Example");
	});

	it("should remove the error prefix when no visible errors remain", () => {
		document.title = "Error: Example";

		updateTitle();

		expect(document.title).toBe("Example");
	});

	it("should use a configured error prefix from the title element", () => {
		const title = document.querySelector("title");
		title?.setAttribute("data-govuk-error-prefix", "There is a problem: ");
		document.body.innerHTML = "<p class='govuk-error-message'>Enter a valid postcode</p>";

		updateTitle();

		expect(document.title).toBe("There is a problem: Example");
	});
});

describe("updateErrorSummary", () => {
	beforeEach(() => {
		document.body.innerHTML = "<main></main>";
		ensureErrorSummary();
	});

	it("should display the summary and link to each error message", () => {
		const firstError = document.createElement("p");
		firstError.id = "postcode-error";
		firstError.className = "govuk-error-message";
		firstError.textContent = "Enter a valid postcode";
		document.body.appendChild(firstError);

		const secondError = document.createElement("p");
		secondError.id = "country-error";
		secondError.className = "govuk-error-message";
		secondError.textContent = "Select a country";
		document.body.appendChild(secondError);

		updateErrorSummary();

		const summary = document.querySelector(".govuk-error-summary");
		const links = summary?.querySelectorAll(".govuk-error-summary__list a");

		expect(summary).toHaveClass("govuk-!-display-block");
		expect(links).toHaveLength(2);
		expect(links?.[0]).toHaveAttribute("href", "#postcode");
		expect(links?.[0]).toHaveTextContent("Enter a valid postcode");
		expect(links?.[1]).toHaveAttribute("href", "#country");
	});

	it("should not copy the visually hidden error prefix into a summary link", () => {
		const error = document.createElement("p");
		error.id = "postcode-error";
		error.className = "govuk-error-message";

		const prefix = document.createElement("span");
		prefix.className = "govuk-visually-hidden";
		prefix.textContent = "Error: ";
		error.appendChild(prefix);
		error.appendChild(document.createTextNode("Enter a valid postcode"));
		document.body.appendChild(error);

		updateErrorSummary();

		const link = document.querySelector(".govuk-error-summary__list a");

		expect(link).toHaveTextContent("Enter a valid postcode");
		expect(link?.querySelector(".govuk-visually-hidden")).toBeNull();
	});

	it("should ignore error messages without visible content", () => {
		const error = document.createElement("p");
		error.id = "postcode-error";
		error.className = "govuk-error-message";
		error.innerHTML = "<span class='govuk-visually-hidden'>Error: </span> ";
		document.body.appendChild(error);

		updateErrorSummary();

		const summary = document.querySelector(".govuk-error-summary");

		expect(summary?.querySelectorAll(".govuk-error-summary__list li")).toHaveLength(0);
		expect(summary).toHaveClass("govuk-!-display-none");
	});

	it("should remove stale links and hide the summary when errors are removed", () => {
		const error = document.createElement("p");
		error.id = "postcode-error";
		error.className = "govuk-error-message";
		error.textContent = "Enter a valid postcode";
		document.body.appendChild(error);

		updateErrorSummary();
		error.remove();
		updateErrorSummary();

		const summary = document.querySelector(".govuk-error-summary");

		expect(summary?.querySelectorAll(".govuk-error-summary__list li")).toHaveLength(0);
		expect(summary).toHaveClass("govuk-!-display-none");
	});

	it("should create an error list when the summary body does not contain one", () => {
		document.body.innerHTML = `
			<div class="govuk-error-summary">
				<div class="govuk-error-summary__body"></div>
			</div>
			<p id="postcode-error" class="govuk-error-message">Enter a valid postcode</p>`;

		updateErrorSummary();

		expect(document.querySelector(".govuk-error-summary__list")).not.toBeNull();
	});
});
