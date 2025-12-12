export const manifests: Array<UmbExtensionManifest> = [
    {
        "type": "tiptapToolbarExtension",
        "kind": "styleMenu",
        "alias": "GovUk.Frontend.Umbraco.Styles",
        "name": "GovUk.Frontend.Umbraco Styles",
        "meta": {
            "alias": "myCustomStyleMenu",
            "icon": "icon-palette",
            "label": "Styles"
        },
        "items": [
            {
                "label": "Headings",
                "items": [
                    {
                        "label": "Heading 2",
                        "data": { "tag": "h2" },
                        "appearance": { "icon": "icon-heading-2", style: "font-size: 20px; font-weight: bold" }
                    },
                    {
                        "label": "Heading 3",
                        "data": { "tag": "h3" },
                        "appearance": { "icon": "icon-heading-3", style: "font-size: 18px; font-weight: bold" }
                    },
                    {
                        "label": "Heading 4",
                        "data": { "tag": "h4" },
                        "appearance": { "icon": "icon-heading-4", style: "font-size: 16px; font-weight: bold" }
                    },
                    {
                        "label": "Heading 5",
                        "data": { "tag": "h5" },
                        "appearance": { "icon": "icon-heading-5", style: "font-size: 14px; font-weight: bold" }
                    },
                    {
                        "label": "Heading 6",
                        "data": { "tag": "h6" },
                        "appearance": { "icon": "icon-heading-6", style: "font-size: 12px; font-weight: bold" }
                    }
                ]
            },
            {
                "label": "Bullet Lists",
                "items": [
                    {
                        "label": "Circle",
                        "data": { "tag": "ul", class: "govuk-list--circle" },
                        appearance: { "style": "list-style-type: circle" }
                    },
                    {
                        "label": "Square",
                        "data": { "tag": "ul", class: "govuk-list--square" },
                        appearance: { "style": "list-style-type: square" }
                    }
                ]
            },
            {
                "label": "Ordered Lists",
                "items": [
                    {
                        "label": "Lower alpha",
                        "data": { "tag": "ol", class: "govuk-list govuk-list--lower-alpha" },
                        appearance: { "style": "list-style-type: lower-alpha" }
                    },
                    {
                        "label": "Lower Greek",
                        "data": { "tag": "ol", class: "govuk-list govuk-list--lower-greek" },
                        appearance: { "style": "list-style-type: lower-greek" }
                    },
                    {
                        "label": "Lower Roman",
                        "data": { "tag": "ol", class: "govuk-list govuk-list--lower-roman" },
                        appearance: { "style": "list-style-type: lower-roman" }
                    },
                    {
                        "label": "Upper alpha",
                        "data": { "tag": "ol", class: "govuk-list govuk-list--upper-alpha" },
                        appearance: { "style": "list-style-type: upper-alpha" }
                    },
                    {
                        "label": "Upper Roman",
                        "data": { "tag": "ol", class: "govuk-list govuk-list--upper-roman" },
                        appearance: { "style": "list-style-type: upper-roman" }
                    }
                ]
            }
        ]
    }
];