using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.Umbraco.Blocks;
using GovUk.Frontend.Umbraco.HtmlGeneration;
using GovUk.Frontend.Umbraco.ModelBinding;
using GovUk.Frontend.Umbraco.PropertyEditors;
using GovUk.Frontend.Umbraco.PropertyEditors.ModelPropertyPicker;
using GovUk.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using GovUk.Frontend.Umbraco.Services;
using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.PropertyEditors;

namespace GovUk.Frontend.Umbraco
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGovUkFrontendUmbraco(this IServiceCollection services)
        {
            return services.AddGovUkFrontendUmbraco(options => { }, options => { });
        }

        public static IServiceCollection AddGovUkFrontendUmbraco(this IServiceCollection services,
            Action<GovUkFrontendOptions> configureGovUkOptions)
        {
            return services.AddGovUkFrontendUmbraco(configureGovUkOptions, options => { });
        }

        public static IServiceCollection AddGovUkFrontendUmbraco(this IServiceCollection services,
            Action<GovUkFrontendUmbracoOptions> configureGovUkUmbracoOptions)
        {
            return services.AddGovUkFrontendUmbraco(options => { }, configureGovUkUmbracoOptions);
        }

        public static IServiceCollection AddGovUkFrontendUmbraco(
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
            services.AddTransient<IRichTextPropertyEditorAliasProvider, GovUkRichTextPropertyEditorAliasProvider>();
            services.AddTransient<IGovUkFieldsetErrorFinder, GovUkFieldsetErrorFinder>();
            services.AddTransient<IGovUkGridClassBuilder, GovUkGridClassBuilder>();
            services.AddTransient<IDefaultColumnClassProvider, GovUkCaptionColumnClassProvider>();
            services.AddTransient<IDefaultColumnClassProvider, GovUkPageHeadingColumnClassProvider>();
            services.AddTransient<IGovUkHeadingClassProvider, GovUkHeadingClassProvider>();
            services.AddTransient<BlockViewService>();
            services.AddTransient<IModelPropertyProvider, ModelTypeAttributeModelPropertyProvider>();

            return services;
        }
    }
}
