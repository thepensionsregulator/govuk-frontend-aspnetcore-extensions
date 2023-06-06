using GovUk.Frontend.AspNetCore.Extensions.Validation;
using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Core.Web;

namespace GovUk.Frontend.Umbraco
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGovUkFrontendUmbracoExtensions(this IApplicationBuilder app, IOptions<MvcOptions> mvcOptions, IUmbracoContextAccessor umbracoContextAccessor)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            mvcOptions.Value.ModelMetadataDetailsProviders.Add(new UmbracoBlockListValidationMetadataProvider(umbracoContextAccessor, new Dictionary<Type, string>
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
                { typeof(CompareAttribute), PropertyAliases.ErrorMessageCompare }
            }));

            return app;
        }
    }
}
