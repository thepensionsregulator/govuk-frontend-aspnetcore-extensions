using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.AspNetCore.ModelBinding;
using GovUk.Frontend.Umbraco.Models;
using GovUk.Frontend.Umbraco.Services;
using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Dictionary;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.ModelBinding
{
    /// <summary>
    /// A copy of the date model binder from the base project, modified to make the error messages configurable in Umbraco
    /// </summary>
    internal class UmbracoDateInputModelBinder : IModelBinder
    {
        private const string DayComponentName = "Day";
        private const string MonthComponentName = "Month";
        private const string YearComponentName = "Year";

        private readonly DateInputModelConverter _dateInputModelConverter;

        public UmbracoDateInputModelBinder(DateInputModelConverter dateInputModelConverter)
        {
            _dateInputModelConverter = Guard.ArgumentNotNull(nameof(dateInputModelConverter), dateInputModelConverter);
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Guard.ArgumentNotNull(nameof(bindingContext), bindingContext);

            var modelType = bindingContext.ModelMetadata.UnderlyingOrModelType;
            if (!_dateInputModelConverter.CanConvertModelType(modelType))
            {
                throw new InvalidOperationException($"Cannot bind {modelType.Name}.");
            }

            var dayModelName = $"{bindingContext.ModelName}.{DayComponentName}";
            var monthModelName = $"{bindingContext.ModelName}.{MonthComponentName}";
            var yearModelName = $"{bindingContext.ModelName}.{YearComponentName}";

            var dayValueProviderResult = bindingContext.ValueProvider.GetValue(dayModelName);
            var monthValueProviderResult = bindingContext.ValueProvider.GetValue(monthModelName);
            var yearValueProviderResult = bindingContext.ValueProvider.GetValue(yearModelName);

            if ((dayValueProviderResult == ValueProviderResult.None || dayValueProviderResult.FirstValue == string.Empty) &&
                (monthValueProviderResult == ValueProviderResult.None || monthValueProviderResult.FirstValue == string.Empty) &&
                (yearValueProviderResult == ValueProviderResult.None || yearValueProviderResult.FirstValue == string.Empty))
            {
                return Task.CompletedTask;
            }

            // If some validation exists where a fully-parsable date is an invalid value, using SetInitialValue here allows the date to repopulate
            // without affecting validation. If SetModelValue is used then the date repopulates, but can never pass validation.
            bindingContext.ModelState.SetInitialValue(dayModelName, dayValueProviderResult.FirstValue!);
            bindingContext.ModelState.SetInitialValue(monthModelName, monthValueProviderResult.FirstValue!);
            bindingContext.ModelState.SetInitialValue(yearModelName, yearValueProviderResult.FirstValue!);

            var parseErrors = Parse(
                dayValueProviderResult.FirstValue,
                monthValueProviderResult.FirstValue,
                yearValueProviderResult.FirstValue,
                out var date);

            if (parseErrors == DateInputParseErrors.None)
            {
                Debug.Assert(date.HasValue);
                var model = _dateInputModelConverter.CreateModelFromDate(modelType, date!.Value);
                bindingContext.ModelState.SetInitialValue(bindingContext.ModelName, date!.Value.ToString("yyyy-MM-dd"));
                bindingContext.Result = ModelBindingResult.Success(model);
            }
            else
            {
                var parseErrorsProvider = bindingContext.HttpContext.RequestServices.GetRequiredService<DateInputParseErrorsProvider>();
                parseErrorsProvider.SetErrorsForModel(bindingContext.ModelName, parseErrors);

                if (_dateInputModelConverter.TryCreateModelFromErrors(modelType, parseErrors, out var model))
                {
                    bindingContext.Result = ModelBindingResult.Success(model);
                }
                else
                {
                    var contentAccessor = bindingContext.HttpContext.RequestServices.GetRequiredService<IUmbracoPublishedContentAccessor>()!;
                    var cultureDictionary = bindingContext.HttpContext.RequestServices.GetRequiredService<ICultureDictionary>()!;

                    var errorMessage = GetModelStateErrorMessage(contentAccessor.PublishedContent, cultureDictionary, parseErrors, bindingContext.ModelMetadata);
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, errorMessage);

                    bindingContext.Result = ModelBindingResult.Failed();
                }
            }

            return Task.CompletedTask;
        }

        // internal for testing
        internal static string GetModelStateErrorMessage(IPublishedContent umbracoContent, ICultureDictionary umbracoDictionary, DateInputParseErrors parseErrors, ModelMetadata modelMetadata)
        {
            Debug.Assert(parseErrors != DateInputParseErrors.None);
            Debug.Assert(parseErrors != (DateInputParseErrors.MissingDay | DateInputParseErrors.MissingMonth | DateInputParseErrors.MissingYear));

            string? displayName = null;
            if (!string.IsNullOrEmpty(modelMetadata.PropertyName))
            {
                displayName = umbracoContent.FindBlockByBoundProperty(modelMetadata.PropertyName)?.Settings?.Value<string>(PropertyAliases.DisplayName)?.Trim();
            }
            if (string.IsNullOrEmpty(displayName)) { displayName = modelMetadata.PropertyName; }

            var mustBeARealDateText = (umbracoDictionary[DictionaryConstants.DateMustBeARealDate] ?? "must be a real date").Trim();

            var missingComponents = new List<string>();

            if ((parseErrors & DateInputParseErrors.MissingDay) != 0)
            {
                missingComponents.Add((umbracoDictionary[DictionaryConstants.DateDay] ?? "day").Trim());
            }
            if ((parseErrors & DateInputParseErrors.MissingMonth) != 0)
            {
                missingComponents.Add((umbracoDictionary[DictionaryConstants.DateMonth] ?? "month").Trim());
            }
            if ((parseErrors & DateInputParseErrors.MissingYear) != 0)
            {
                missingComponents.Add((umbracoDictionary[DictionaryConstants.DateYear] ?? "year").Trim());
            }

            if (missingComponents.Count > 0)
            {
                Debug.Assert(missingComponents.Count <= 2);
                string mustIncludeText = (umbracoDictionary[DictionaryConstants.DateMustInclude] ?? "must include a").Trim();
                string andText = (umbracoDictionary[DictionaryConstants.DateAnd] ?? "and").Trim();
                andText = $" {andText} ";

                return $"{displayName} {mustIncludeText} {string.Join(andText, missingComponents)}";
            }

            return $"{displayName} {mustBeARealDateText}";
        }

        // internal for testing
        internal static DateInputParseErrors Parse(string? day, string? month, string? year, out DateOnly? date)
        {
            day ??= string.Empty;
            month ??= string.Empty;
            year ??= string.Empty;

            var errors = DateInputParseErrors.None;
            int parsedYear = 0, parsedMonth = 0, parsedDay = 0;

            if (string.IsNullOrEmpty(year))
            {
                errors |= DateInputParseErrors.MissingYear;
            }
            else if (!int.TryParse(year, out parsedYear) || parsedYear < 1 || parsedYear > 9999)
            {
                errors |= DateInputParseErrors.InvalidYear;
            }

            if (string.IsNullOrEmpty(month))
            {
                errors |= DateInputParseErrors.MissingMonth;
            }
            else if (!int.TryParse(month, out parsedMonth) || parsedMonth < 1 || parsedMonth > 12)
            {
                errors |= DateInputParseErrors.InvalidMonth;
            }

            if (string.IsNullOrEmpty(day))
            {
                errors |= DateInputParseErrors.MissingDay;
            }
            else if (!int.TryParse(day, out parsedDay) || parsedDay < 1 || parsedDay > 31 ||
                errors == DateInputParseErrors.None && parsedDay > DateTime.DaysInMonth(parsedYear, parsedMonth))
            {
                errors |= DateInputParseErrors.InvalidDay;
            }

            date = errors == DateInputParseErrors.None ? new(parsedYear, parsedMonth, parsedDay) : default;
            return errors;
        }
    }
}