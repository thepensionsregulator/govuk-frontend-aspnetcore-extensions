# TPR timeline 

You can add a timeline to your razor views by using the provided tag helper tags.

## Example

```razor
@addTagHelper *, ThePensionsRegulator.Frontend
<tpr-timeline>
    <tpr-timeline-item date="9 Jan 2005" heading="Starting title">
    </tpr-timeline-item>
    <tpr-timeline-item date="31 Jan 2025" heading="Today's date" line-colour="orange">
        <tpr-timeline-item-content>
            <h3 class="govuk-heading-m">Example of custom content.</h3>
        <p>You can fill space with your own HTML.</p>
        </tpr-timeline-item-content>
    </tpr-timeline-item>
    <tpr-timeline-item date="9 Jun 2025" heading="Example deadline style" line-colour="red">
        <tpr-timeline-item-content>
            <p>You can add custom content here too.</p>
        </tpr-timeline-item-content>
    </tpr-timeline-item>
</tpr-timeline>
```
![Add a Section cards component](/docs/images/tpr-timeline-example.png)
## API

### `<tpr-timeline>`

_Required_

| Attribute    | Type     | Description                                                                                                                                                                       |
| ------------ | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `date-size`       | `string` | Sets the size of dates on timeline items. Could be left blank or set to 'Large'.                                                                                                                                                  |
| `hide-tail`     | `bool` | Hides the line of the last segment of the timeline. |


### `<tpr-timeline-item>`

Creates a single segment of the timeline. Must be inside a `<tpr-timeline>` element.

| Attribute    | Type     | Description                                                                                                                                                                       |
| ------------ | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `date`       | `string` | Sets the text to represent the date title. It's a string so it can be used to display other text information such as stage of progress.                                                                                                                                                  |
| `heading`     | `string` | Sets the heading that appears under the date. |
| `line-colour`     | `string` | Sets the line colour of the timeline item. If left blank, the default primary colour will be used. With this solution, there are also 'orange' and 'red' colours provided. If you want to assign a custom colour to the timeline item, you can create your own class like the following: .tpr-timeline__item--colour3,.tpr-timeline__item--colour3::before,.tpr-timeline__item--colour3:last-of-type::after{border-color: #7466be;}. You can then set this value to 'colour3'.|


### `<tpr-timeline-item-content>`

Sets the HTML content for timeline item. Must be inside a `<tpr-timeline-item>` element.

