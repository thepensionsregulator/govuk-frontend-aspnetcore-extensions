# Configure a new ASP.NET project (GOV.UK)

1. Create a new project using the 'ASP.NET Core Web App (Model-View-Controller)' template in Visual Studio 2026 or later. Select .NET 10.0 or later as the Framework.

2. Add the `ThePensionsRegulator.GovUk.Frontend` NuGet package to your project.

3. In `Program.cs` add the following:

   ```csharp
   using ThePensionsRegulator.GovUk.Frontend;

   var builder = WebApplication.CreateBuilder(args);
   builder.Services.AddTprGovUkFrontend();

   // other code to configure builder.Services

   var app = builder.Build();
   app.UseTprGovUkFrontend();

   // other code to configure app

   app.Run();
   ```

   You shouldn't need to configure support for static assets as it's done for you, but if you do it must be called after `app.UseTprGovUkFrontend()`.

4. Replace the contents of `Views/Shared/_Layout.cshtml` with the code shown below.

   ```html
   <!DOCTYPE html>
   <html lang="en">
     <head>
       <meta charset="utf-8" />
       <meta name="viewport" content="width=device-width, initial-scale=1.0" />
       <partial name="GOVUK/Head" />
       @RenderSection("head", required: false)
     </head>
     <body class="govuk-template__body">
       <partial name="GOVUK/BodyOpen" />
       <div class="govuk-width-container">
         <main class="govuk-main-wrapper" id="main">@RenderBody()</main>
       </div>
       <partial name="GOVUK/BodyClosing" />
       <partial name="GOVUK/Validation" />
     </body>
   </html>
   ```

5. Add the following to your `Views/_ViewImports.cshtml` file:

   ```csharp
   @addTagHelper *, GovUk.Frontend.AspNetCore
   @addTagHelper *, ThePensionsRegulator.GovUk.Frontend.AspNetCore
   ```

6. [Add validation rules to your model](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/validation?view=aspnetcore-5.0) as you normally would for ASP.NET, using attributes from the [System.ComponentModel.DataAnnotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-5.0) namespace.

   > JQuery is included to support the standard ASP.NET validation. We recommend using vanilla JavaScript for everything else.

7. Add components from the GOV.UK Design System as documented in [ASP.NET Core MVC tag helpers for GOV.UK Design System](https://github.com/gunndabad/govuk-frontend-aspnetcore), but with wrapper tags from `ThePensionsRegulator.GovUk.Frontend`.

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
