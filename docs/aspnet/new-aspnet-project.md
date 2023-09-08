# Configure a new ASP.NET project

1. If you're working on a project for The Pensions Regulator add the `ThePensionsRegulator.Frontend` NuGet package. If your application should look like GOV.UK add `ThePensionsRegulator.GovUk.Frontend` NuGet package instead.

2. In `Startup.cs` add the following to the `ConfigureServices` method:

   ```csharp
   using GovUk.Frontend.AspNetCore.Extensions;

   public void ConfigureServices(IServiceCollection services)
   {
       // Other code here

       // If you're using GOV.UK components and styles only, add the following line
       services.AddGovUkFrontendExtensions();

       // If you're using TPR components and styles, add this line instead
       services.AddTprFrontend();
   }
   ```

3. Add partial views and the `govuk-template__body` class to your layout file as shown below. You should also make sure you have a `<main>` element in your markup.

   ```html
   <!DOCTYPE html>
   <html lang="en">
     <head>
       <meta charset="utf-8" />
       <meta name="viewport" content="width=device-width, initial-scale=1.0" />
       <partial name="TPR/Head" />
       @RenderSection("head", required: false)
     </head>
     <body class="govuk-template__body ">
       <partial name="GOVUK/BodyOpen" />
       <main>@RenderBody()</main>
       <partial name="TPR/BodyClosing" />
       <partial name="GOVUK/Validation" />
     </body>
   </html>
   ```

   Note that `TPR/Head` and `TPR/BodyClosing` imports TPR styles and scripts on top of the GOV.UK Design System. Use the partials `GOVUK/Head` and `GOVUK/BodyClosing` instead to use the GOV.UK Design System styles only.

4. Add the following to your `Views/_ViewImports.cshtml` file:

   ```csharp
   @addTagHelper *, GovUk.Frontend.AspNetCore
   @addTagHelper *, GovUk.Frontend.AspNetCore.Extensions
   @addTagHelper *, ThePensionsRegulator.Frontend
   ```

5. [Add validation rules to your model](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/validation?view=aspnetcore-5.0) as you normally would for ASP.NET, using attributes from the [System.ComponentModel.DataAnnotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-5.0) namespace.

   > JQuery is included to support the standard ASP.NET validation. We recommend using vanilla JavaScript for everything else.

6. Add components from the GOV.UK Design System as documented in [ASP.NET Core MVC tag helpers for GOV.UK Design System](https://github.com/gunndabad/govuk-frontend-aspnetcore), but with wrapper tags from `GovUk.Frontend.AspNetCore.Extensions`.

   ```csharp
   <form asp-controller="Home" asp-action="Post" method="post" novalidate>
       <h1 class="govuk-heading-l">My form</h1>

       <govuk-client-side-validation>
           <govuk-input asp-for="MyModelProperty">
               <govuk-input-label>Field label</govuk-input-label>
               <govuk-input-hint>This is the hint</govuk-input-hint>
               <govuk-input-error-message />
           </govuk-input>
       </govuk-client-side-validation>

       <govuk-button type="submit">Submit</govuk-button>
   </form>
   ```
