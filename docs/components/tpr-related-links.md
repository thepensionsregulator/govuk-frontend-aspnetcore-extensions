# TPR related links

A design component for the related links column which can be re-used on TPR sites. This component is implemented using tag helpers and can be used in both ASP.NET applications and Umbraco.

## ASP.NET example

```razor
<tpr-related-links>
    <tpr-related-links-heading>Related links example heading</tpr-related-links-heading>
    <tpr-related-link href="/">Example link 1</tpr-related-link>
    <tpr-related-link href="/">Example link 2</tpr-related-link>
    <tpr-related-link href="/">Example link 3</tpr-related-link>
</tpr-related-links>
```

In ASP.NET applications the related links component is structured like so, replacing the example heading and example link placeholders with the required content.

If the tag helpers are arranged like the example above, then it should render the following html code:
```html
<div class='tpr-related-links govuk-body '>
    <ul>
        <li>
            <h2>Related links example heading</h2>
        </li>
        <li>
            <a href="/">Example link 1</a>
        </li>
        <li>
            <a href="/">Example link 2</a>
        </li>
        <li>
            <a href="/">Example link 3</a>
        </li>
    </ul>
</div>
```

## Umbraco example

The 'TPR Related Links' block which is supported on the 'TPR Block Grid' component is used to implement this component in Umbraco.
