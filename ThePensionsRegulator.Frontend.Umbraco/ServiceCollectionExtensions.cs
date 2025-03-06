using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.Umbraco;
using GovUk.Frontend.Umbraco.Blocks;
using GovUk.Frontend.Umbraco.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using ThePensionsRegulator.Frontend.Services;
using ThePensionsRegulator.Frontend.Umbraco.PropertyEditors;
using ThePensionsRegulator.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco.PropertyEditors;

namespace ThePensionsRegulator.Frontend.Umbraco
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddTprFrontendUmbraco(this IServiceCollection services)
		{
			return services.AddTprFrontendUmbraco(options => { }, options => { });
		}

		public static IServiceCollection AddTprFrontendUmbraco(this IServiceCollection services,
			Action<GovUkFrontendAspNetCoreOptions> configureGovUkOptions)
		{
			return services.AddTprFrontendUmbraco(configureGovUkOptions, options => { });
		}

		public static IServiceCollection AddTprFrontendUmbraco(this IServiceCollection services,
			Action<GovUkFrontendUmbracoOptions> configureGovUkUmbracoOptions)
		{
			return services.AddTprFrontendUmbraco(options => { }, configureGovUkUmbracoOptions);
		}

		public static IServiceCollection AddTprFrontendUmbraco(
			this IServiceCollection services,
			Action<GovUkFrontendAspNetCoreOptions> configureGovUkOptions,
			Action<GovUkFrontendUmbracoOptions> configureGovUkUmbracoOptions)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}

			services.AddGovUkFrontendUmbraco(configureGovUkOptions, configureGovUkUmbracoOptions);

			services.AddTransient<IContextAwareHostUpdater, TprHostUpdater>();
			services.AddTransient<IPropertyValueFormatter, HostNameInRichTextEditorPropertyValueFormatter>();
			services.AddTransient<IPropertyValueFormatter, HostNameInMultiUrlPickerPropertyValueFormatter>();
			services.AddTransient<IPropertyValueFormatter, NoParagraphsPropertyValueFormatter>();
			services.AddTransient<IPartialViewPathProvider, TprPartialViewPathProvider>();
			services.AddTransient<IRichTextPropertyEditorAliasProvider, TprRichTextPropertyEditorAliasProvider>();
			services.AddTransient<IBlockViewInterceptor, TprBoxViewInterceptor>();
			services.AddTransient<IBlockViewInterceptor, TprDividerViewInterceptor>();
            services.AddTransient<IDefaultColumnClassProvider, TPRSectionCardsColumnClassProvider>();

            return services;
		}
	}
}