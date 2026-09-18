import { createLink } from "../../Scripts/govuk-components/link";

describe("createLink", () => {
  it("should create an anchor element", () => {
    const link = createLink({ labelText: "Back to search" });

    expect(link.tagName).toEqual("A");
  });

  it("should apply the GOV.UK link class by default", () => {
    const link = createLink({ labelText: "Back to search" });

    expect(link.getAttribute("class")).toEqual("govuk-link");
  });

  it("should apply the back link class for the back variant", () => {
    const link = createLink({ labelText: "Back", variant: "back" });

    expect(link.getAttribute("class")).toEqual("govuk-back-link");
  });

  it("should default the href to # when not provided", () => {
    const link = createLink({ labelText: "Back to search" });

    expect(link.getAttribute("href")).toEqual("#");
  });

  it("should use the provided href", () => {
    const link = createLink({ labelText: "Back to search", href: "/search" });

    expect(link.getAttribute("href")).toEqual("/search");
  });

  it("should use the label text as the link text", () => {
    const link = createLink({ labelText: "Back to search" });

    expect(link.textContent).toEqual("Back to search");
    expect(link.childNodes).toHaveLength(1);
    expect(link.childNodes[0].nodeType).toEqual(Node.TEXT_NODE);
  });

  it("should escape HTML in the label text", () => {
    const link = createLink({ labelText: "<span>Back to search</span>" });

    expect(link.querySelector("span")).toBeNull();
    expect(link.innerHTML).toEqual("&lt;span&gt;Back to search&lt;/span&gt;");
  });

  it("should apply additional attributes", () => {
    const link = createLink({
      labelText: "Back to search",
      attributes: {
        id: "back-to-search-link",
        "aria-label": "Back to the search form",
      },
    });

    expect(link.getAttribute("id")).toEqual("back-to-search-link");
    expect(link.getAttribute("aria-label")).toEqual("Back to the search form");
  });

  it("should produce the expected HTML", () => {
    const link = createLink({ labelText: "Back to search", href: "/search" });

    expect(link.outerHTML).toEqual(
      '<a class="govuk-link" href="/search">Back to search</a>'
    );
  });
});
