using GovUk.Frontend.Umbraco.ExampleApp.Middleware;
using GovUk.Frontend.Umbraco.ExampleApp.PropertyEditors.ValueFormatters;
using GovUk.Frontend.Umbraco.ExampleApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ThePensionsRegulator.Frontend.Services;
using ThePensionsRegulator.Frontend.Umbraco;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.GovUk.Frontend.Umbraco;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));
AppConfig? config = builder.Configuration.GetSection("AppConfig").Get<AppConfig>();

if (config?.TPRStyles == true)
{
    builder.Services.AddTprFrontendUmbraco(options => options.RenderWidthContainerForBlocks = true);
}
else
{
    builder.Services.AddTprGovUkFrontendUmbraco(options => options.RenderWidthContainerForBlocks = true);
    builder.Services.AddTransient<IPartialViewPathProvider, TprPartialViewPathProvider>();
    builder.Services.AddTransient<ITprGlobalNavigationService, TprGlobalNavigationService>();
}

builder.Services.AddTransient<IGovUkBreadcrumbLinksService, BreadcrumbLinksServiceForExampleApp>();
builder.Services.AddTransient<ITprSideNavigationLinksService, SideNavigationLinksServiceForExampleApp>();
builder.Services.AddTransient<ITprSearchResultsEndpointUrlProvider, TprQueryBasedSearchResultsEndpointUrlProvider>();
builder.Services.AddTransient<IBlockViewInterceptor, SideNavigationBlockViewInterceptor>();
builder.Services.AddTransient<IPropertyValueFormatter, NoParagraphsPropertyValueFormatter>();

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
