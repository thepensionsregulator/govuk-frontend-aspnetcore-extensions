using GovUk.Frontend.Umbraco.ExampleApp;
using GovUk.Frontend.Umbraco.ExampleApp.Middleware;
using GovUk.Frontend.Umbraco.ExampleApp.PropertyEditors.ValueFormatters;
using GovUk.Frontend.Umbraco.ExampleApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using ThePensionsRegulator.Frontend;
using ThePensionsRegulator.Frontend.Services;
using ThePensionsRegulator.Frontend.Umbraco;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.GovUk.Frontend.Umbraco;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));
AppConfig? config = builder.Configuration.GetSection("AppConfig").Get<AppConfig>();

if (config?.TPRStyles == true)
{
    builder.Services.AddTprFrontendUmbraco(
        umbracoOptions => { umbracoOptions.RenderWidthContainerForBlocks = true; },
        tprOptions => { tprOptions.EnableTableCsvDownload = true; }
    );
    builder.Services.Remove(builder.Services.First(builder => builder.ServiceType == typeof(ITprSideNavigationLinksService)));
    builder.Services.AddTransient<ITprSideNavigationLinksService, SideNavigationLinksServiceForExampleApp>();
}
else
{
    builder.Services.AddTprGovUkFrontendUmbraco(options => { options.RenderWidthContainerForBlocks = true; });
    builder.Services.Configure<RazorViewEngineOptions>(options => options.ViewLocationFormats.Add("/Views/Shared/TPR/{0}.cshtml"));
    builder.Services.AddTransient<ITprGlobalNavigationService, TprGlobalNavigationService>();
}

builder.Services.AddTransient<ITprSearchResultsEndpointUrlProvider, TprQueryBasedSearchResultsEndpointUrlProvider>();
builder.Services.AddTransient<IBlockViewInterceptor, SideNavigationBlockViewInterceptor>();
builder.Services.AddTransient<IPropertyValueFormatter, ExampleAppNoParagraphsPropertyValueFormatter>();

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddComposers()
    .Build();

WebApplication app = builder.Build();
var mvcOptions = app.Services.GetRequiredService<IOptions<MvcOptions>>();
var umbracoContextAccessor = app.Services.GetRequiredService<IUmbracoContextAccessor>();
var publishedValueFallback = app.Services.GetRequiredService<IPublishedValueFallback>();
app.UseTprFrontendUmbraco(mvcOptions, umbracoContextAccessor, publishedValueFallback);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseSecurityHeaders();

await app.BootUmbracoAsync();

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
