import { createFieldset } from "../../Scripts/govuk-components/fieldset";
import "@testing-library/jest-dom";

describe("createFieldset", () => {
	it("should render a fieldset with the GOV.UK fieldset class", () => {
		const fieldset = createFieldset({
			legendText: "Your address",
			children: [],
		});

		expect(fieldset.tagName).toBe("FIELDSET");
		expect(fieldset).toHaveClass("govuk-fieldset");
	});

	it("should render a GOV.UK legend with the supplied text", () => {
		const fieldset = createFieldset({
			legendText: "Your address",
			children: [],
		});

		const legend = fieldset.querySelector("legend");

		expect(legend).toHaveClass("govuk-fieldset__legend");
		expect(legend).toHaveTextContent("Your address");
	});

	it.each([
		["small", "govuk-fieldset__legend--s"],
		["medium", "govuk-fieldset__legend--m"],
		["large", "govuk-fieldset__legend--l"],
		["x-large", "govuk-fieldset__legend--xl"],
	] as const)("should map the %s legend size to %s", (legendSize, expectedClass) => {
		const fieldset = createFieldset({
			legendText: "Your address",
			legendSize,
			children: [],
		});

		expect(fieldset.querySelector("legend")).toHaveClass(expectedClass);
	});

	it("should append child elements after the legend in the supplied order", () => {
		const firstChild = document.createElement("div");
		const secondChild = document.createElement("div");

		const fieldset = createFieldset({
			legendText: "Your address",
			children: [firstChild, secondChild],
		});

		expect(fieldset.children[0].tagName).toBe("LEGEND");
		expect(fieldset.children[1]).toBe(firstChild);
		expect(fieldset.children[2]).toBe(secondChild);
	});

	it("should render legend text as text rather than HTML", () => {
		const fieldset = createFieldset({
			legendText: "<strong>Your address</strong>",
			children: [],
		});

		const legend = fieldset.querySelector("legend");

		expect(legend).toHaveTextContent("<strong>Your address</strong>");
		expect(legend?.querySelector("strong")).toBeNull();
	});

	it("should apply custom fieldset attributes", () => {
		const fieldset = createFieldset({
			legendText: "Your address",
			children: [],
			attributes: {
				"data-testid": "address-fieldset",
			},
		});

		expect(fieldset).toHaveAttribute("data-testid", "address-fieldset");
	});
});
