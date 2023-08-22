# Format Umbraco property values

A [property value converter](https://docs.umbraco.com/umbraco-cms/extending/property-editors/property-value-converters/) is an Umbraco feature which takes the raw value of a property in the database, and converts it to a type you want to work with. For example it might deserialize a JSON string into a custom class.

A property value formatter is a similar feature introduced by this project, which takes the value returned by a property value converter and updates it. For example, govuk-frontend classes are added to the HTML from a rich text field. The property value formatter also runs when you [override a property value in a block list](./override-property-values.md).

## Implement a property value formatter

### Step 1: The property value converter

The property value converter for the property type needs to inject `IEnumerable<IPropertyValueFormatter>` and call its `ApplyFormatters` method to update the property value. Default property value converters don't do this. If you create a new property value converter that does apply property value formatters, the new property value converter will be discovered automatically. If there is an existing property value converter for the property type you need to remove that first. The [property value converter](https://docs.umbraco.com/umbraco-cms/extending/property-editors/property-value-converters/) documentation explains how to do this.

```csharp
using Umbraco.Cms.Core.PropertyEditors;
using ThePensionsRegulator.Umbraco.PropertyEditors;

public class ExamplePropertyValueConverter : PropertyValueConverterBase
{
    private readonly IEnumerable<IPropertyValueFormatter> _propertyValueFormatters;

    public ExamplePropertyValueConverter(IEnumerable<IPropertyValueFormatter> propertyValueFormatters)
    {
        _propertyValueFormatters = propertyValueFormatters ?? throw new ArgumentNullException(nameof(propertyValueFormatters));
    }

    /// override other methods from PropertyValueConverterBase

    public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview)
    {
        // do any conversion needed on the intermediate value
        var value = inter?.ToString() ?? string.Empty;

        return _propertyValueFormatters.ApplyFormatters(propertyType, value);
    }
}
```

### Step 2: The property value formatter

Implement the `IPropertyValueFormatter` interface, specifying the alias of the property editor whose value can be formatted.

```csharp
using ThePensionsRegulator.Umbraco.PropertyEditors;

public class ExamplePropertyValueFormatter : IPropertyValueFormatter
{
    public bool IsFormatter(IPublishedPropertyType propertyType) => "propertyEditorAliasToConvertFrom".Equals(propertyType.EditorAlias);

    // format the value somehow, don't just return it as shown here
    public object FormatValue(object? value) => value;
}
```

Register the `IPropertyValueFormatter` with dependency injection. You can register multiple formatters by mapping `IPropertyValueFormatter` to each one as shown below.

```csharp
using ThePensionsRegulator.Umbraco.PropertyEditors;

public void ConfigureServices(IServiceCollection services)
{
    // ...more services here...

    services.AddTransient<IPropertyValueFormatter, ExamplePropertyValueFormatter>();
    services.AddTransient<IPropertyValueFormatter, AnotherExamplePropertyValueFormatter>();
}
```
