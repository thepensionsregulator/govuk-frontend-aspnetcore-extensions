# Populate initial values

If you need to select or populate values on your initial GET request, use the `ModelState.SetInitialValues` extension method:

```csharp
/// Controller
using GovUk.Frontend.Umbraco.Validation;

ModelState.SetInitialValue(nameof(viewModel.Field1), Request.Query["field1"]);
```
