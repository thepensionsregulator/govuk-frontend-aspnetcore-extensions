using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace GovUk.Frontend.Umbraco
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGovUkFrontendUmbraco(this IApplicationBuilder app, IOptions<MvcOptions> mvcOptions, IUmbracoContextAccessor umbracoContextAccessor, IPublishedValueFallback publishedValueFallback)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseGovUkFrontendExtensions();

            mvcOptions.Value.ModelMetadataDetailsProviders.Add(new UmbracoBlockValidationMetadataProvider(umbracoContextAccessor,
                publishedValueFallback,
                new Dictionary<Type, string>
            {
                { typeof(RequiredAttribute), PropertyAliases.ErrorMessageRequired },
                { typeof(RegularExpressionAttribute),  PropertyAliases.ErrorMessageRegex },
                { typeof(EmailAddressAttribute),PropertyAliases.ErrorMessageEmail },
                { typeof(PhoneAttribute), PropertyAliases.ErrorMessagePhone },
                { typeof(StringLengthAttribute), PropertyAliases.ErrorMessageLength },
                { typeof(MinLengthAttribute), PropertyAliases.ErrorMessageMinLength },
                { typeof(MaxLengthAttribute), PropertyAliases.ErrorMessageMaxLength },
                { typeof(RangeAttribute), PropertyAliases.ErrorMessageRange },
                { typeof(DateRangeAttribute), PropertyAliases.ErrorMessageRange },
                { typeof(CompareAttribute), PropertyAliases.ErrorMessageCompare },
                { typeof(MaxFileSizeAttribute), PropertyAliases.ErrorMessageMaxFileSize },
                { typeof(AllowedFileTypesAttribute), PropertyAliases.ErrorMessageAllowedFileTypes },
            }));

            return app;
        }
    }
}