# Update TPR hostnames in non-production environments

TPR deploys its web applications to a series of non-production environments before promoting to production. It is useful to be able to hard-code the link to the public URL of an application, and have the hostname replaced with the non-production hostname of the same application when running in a non-production environment.

When using `ThePensionsRegulator.Frontend` you can inject an instance of `IContextAwareHostUpdater`, pass it the HTML containing the link, and get back the updated HTML. When using `ThePensionsRegulator.Frontend.Umbraco` this happens automatically for Umbraco content.

You can disable this behaviour or configure it to apply for a specific allow list of hosts. This is useful in case of false positives, such as an application hosted by a third party on a TPR domain, which does not exist in our non-production environments. Modify your `Startup.cs` or `Program.cs` as follows:

```csharp
// Update all TPR hostnames
services.AddTprFrontendUmbraco();

// Do not update any TPR hostnames
services.AddTprFrontendUmbraco(options => options.UpdateDestinationHostnames = []);

// Update only specific TPR hostnames, eg example.thepensionsregulator.gov.uk
services.AddTprFrontendUmbraco(options => options.UpdateDestinationHostnames = ["example"]);
```
