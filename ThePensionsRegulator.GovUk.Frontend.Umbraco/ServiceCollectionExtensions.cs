using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ThePensionsRegulator.GovUk.Frontend.Caching;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Caching;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.HtmlGeneration;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.ModelBinding;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.PropertyEditors.ModelPropertyPicker;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Services;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Validation;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTprGovUkFrontendUmbraco(this IServiceCollection services)
        {
            return services.AddTprGovUkFrontendUmbraco(options => { }, options => { });
        }

        public static IServiceCollection AddTprGovUkFrontendUmbraco(this IServiceCollection services,
            Action<GovUkFrontendOptions> configureGovUkOptions)
        {
            return services.AddTprGovUkFrontendUmbraco(configureGovUkOptions, options => { });
        }

        public static IServiceCollection AddTprGovUkFrontendUmbraco(this IServiceCollection services,
            Action<GovUkFrontendUmbracoOptions> configureGovUkUmbracoOptions)
        {
            return services.AddTprGovUkFrontendUmbraco(options => { }, configureGovUkUmbracoOptions);
        }

        public static IServiceCollection AddTprGovUkFrontendUmbraco(
            this IServiceCollection services,
            Action<GovUkFrontendOptions> configureGovUkOptions,
            Action<GovUkFrontendUmbracoOptions> configureGovUkUmbracoOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddGovUkFrontendExtensions(configureGovUkOptions);

            var govukUmbracoOptions = new GovUkFrontendUmbracoOptions();
            if (configureGovUkUmbracoOptions is not null) { configureGovUkUmbracoOptions(govukUmbracoOptions); }
            services.AddTransient((services) => Options.Create(govukUmbracoOptions));

            services.AddTransient<IUmbracoPublishedContentAccessor, UmbracoPublishedContentAccessor>();
            services.AddTransient<IUmbracoPaginationFactory, UmbracoPaginationFactory>();
            services.AddSingleton<IConfigureOptions<MvcOptions>, ModelBindingMvcConfiguration>();
            services.AddSingleton<IConfigureOptions<MvcOptions>, ValidationMvcConfiguration>();
            services.AddTransient<ITaskListTaskStatusProvider, TaskListTaskStatusProvider>();
            services.AddTransient<IPropertyValueFormatter, GovUkTypographyPropertyValueFormatter>();
            services.AddTransient<IPropertyValueFormatter, NoParagraphPropertyValueFormatter>();
            services.AddTransient<IPropertyValueFormatter, NoParagraphInversePropertyValueFormatter>();
            services.AddTransient<IPartialViewPathProvider, GovUkPartialViewPathProvider>();
            services.AddTransient<IDateInputHtmlEnhancer, DateInputHtmlEnhancer>();
            services.AddTransient<IGovUkFieldsetErrorFinder, GovUkFieldsetErrorFinder>();
            services.AddTransient<IGovUkGridClassBuilder, GovUkGridClassBuilder>();
            services.AddTransient<IDefaultColumnClassProvider, GovUkCaptionColumnClassProvider>();
            services.AddTransient<IDefaultColumnClassProvider, GovUkPageHeadingColumnClassProvider>();
            services.AddTransient<IGovUkHeadingClassProvider, GovUkHeadingClassProvider>();
            services.AddTransient<BlockViewService>();
            services.AddTransient<IModelPropertyProvider, ModelTypeAttributeModelPropertyProvider>();
            services.AddTransient<IStaticFileCachePolicy, GovUkUmbracoStaticFileCachePolicy>();

            return services;
        }
    }
}
