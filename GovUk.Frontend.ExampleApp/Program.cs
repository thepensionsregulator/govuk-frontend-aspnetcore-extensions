using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.ExampleApp.Middleware;
using GovUk.Frontend.ExampleApp.Models.Validators;
using GovUk.Frontend.ExampleSharedResource;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using System.Reflection;
using ThePensionsRegulator.Frontend;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
if (builder.Configuration.GetValue<bool>("TPRStyles"))
{
    builder.Services.AddTprFrontend();
}
else
{
    builder.Services.AddGovUkFrontendExtensions();
}

#region Localization services

builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(o =>
    {
        o.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName!);

            return new DataAnnotationStringLocalizer(
                factory.Create(type),
                factory.Create(nameof(SharedResource), assemblyName.Name!)
            );
        };
    });

builder.Services.AddLocalization(p => p.ResourcesPath = "Resources");

builder.Services.Configure<RequestLocalizationOptions>(options =>
{

    var supportedCultures = new List<CultureInfo> {
                    new CultureInfo("en"),
                    new CultureInfo("cy")
    };
    options.DefaultRequestCulture = new RequestCulture("en-GB");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
#endregion

builder.Services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidationAttributeAdapterProvider>();
builder.Services.AddSingleton<IValidatorAttributeAdapterFactory, CustomValidatorAttributeAdapterFactory>();

var app = builder.Build();
app.UseTprFrontend();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSecurityHeaders();

#region Localization configuration
var supportedCultures = new[] {
                new CultureInfo("en"),
                new CultureInfo("cy")
            };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});
#endregion

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
