# Configure a new ASP.NET project (TPR)

1. Create a new project using the 'ASP.NET Core Web App (Model-View-Controller)' template in Visual Studio.

2. Add the `ThePensionsRegulator.Frontend` NuGet package to your project.

3. Download the `Tools-TPRGitHooks` repository to a sibling folder of your new solution. Copy `Install-TPRGitHooks.ps1` to your repository and run it. This requires [PowerShell Core](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows).

4. In `Startup.cs` add the following to the `ConfigureServices` method:

   ```csharp
   using GovUk.Frontend.AspNetCore.Extensions;

   public void ConfigureServices(IServiceCollection services)
   {
       // Other code here

       services.AddTprFrontend();
   }
   ```

5. Add partial views and the `govuk-template__body` class to `Views/Shared/_Layout.cshtml` as shown below. You should also make sure you have a `<main>` element in your markup.

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
       <div class="tpr-main-wrapper">
         <div class="govuk-width-container">
           <main id="main" class="govuk-main-wrapper">@RenderBody()</main>
         </div>
       </div>
       <partial name="TPR/BodyClosing" />
       <partial name="GOVUK/Validation" />
     </body>
   </html>
   ```

6. Add the following to your `Views/_ViewImports.cshtml` file:

   ```csharp
   @addTagHelper *, GovUk.Frontend.AspNetCore
   @addTagHelper *, GovUk.Frontend.AspNetCore.Extensions
   @addTagHelper *, ThePensionsRegulator.Frontend
   ```

7. [Add validation rules to your model](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/validation?view=aspnetcore-5.0) as you normally would for ASP.NET, using attributes from the [System.ComponentModel.DataAnnotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-5.0) namespace.

   > JQuery is included to support the standard ASP.NET validation. We recommend using vanilla JavaScript for everything else.

8. Add components from the GOV.UK Design System as documented in [ASP.NET Core MVC tag helpers for GOV.UK Design System](https://github.com/gunndabad/govuk-frontend-aspnetcore), but with wrapper tags from `GovUk.Frontend.AspNetCore.Extensions`.

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
