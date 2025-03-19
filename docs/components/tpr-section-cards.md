# TPR section cards 

In TPR pages you can add cards that can display a linked heading and description.

## HTML example (for ASP.NET applications)

```razor
<ul class="tpr-sectioncards-container govuk-list ">
    <li class="tpr-sectioncards">
        <div class="tpr-sectioncards__body">
            <h2 class="tpr-sectioncards__title">
                <a class="govuk-link" href="/accordion/">Title of card</a>
            </h2>
            <p>Description of card</p>
        </div>
    </li>
</ul>
```

## Umbraco block grid

Add a 'Section cards' component anywhere in a block grid using the 'TPR block grid' data type. For best results, add the component to a full width container.  

![Add a box component](/docs/images/tpr-section-cards-add.png)

You can then add content to the Cards field.

![Add content](/docs/images/tpr-section-cards-add-content.png)

You will then have two options:
- Section cards children - This list all of the children of the current page. Unless the child page's umbracoNaviHide property = true.
- Section cards card - This allows you to add a single card to the list.

![Add content](/docs/images/tpr-section-cards-add-content-options.png)

## Section cards children

This block doesn't require any more fields to be filled in. It will simply display all of the children of the current page.

## Section cards card

Once you add this block, you will have to fill in the link and description fields.



![Add content](/docs/images/tpr-section-cards-add-content-card.png)

## Settings 

The section cards block has the following fields for configuration:
-  Title field name - is the internal field name of the field that contains the title for child items. This defaults to the node name.
- Description field name - is the internal field name of the field that contains the description for child items. This defaults to 'description'.
- Css classes - Adds the classes to the container.

![Add content](/docs/images/tpr-section-cards-settings-cog.png)
![Add content](/docs/images/tpr-section-cards-settings-options.png)
