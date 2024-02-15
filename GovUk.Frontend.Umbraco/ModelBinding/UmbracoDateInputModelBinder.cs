using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.AspNetCore.ModelBinding;
using GovUk.Frontend.Umbraco.Blocks;
using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Dictionary;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
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
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly ICultureDictionary _cultureDictionary;
        private readonly IPublishedValueFallback? _publishedValueFallback;
        private readonly bool _acceptMonthNamesInDateInputs;

        public UmbracoDateInputModelBinder(DateInputModelConverter dateInputModelConverter, IUmbracoContextAccessor umbracoContextAccessor, ICultureDictionary cultureDictionary, IPublishedValueFallback? publishedValueFallback, bool acceptMonthNamesInDateInputs)
        {
            _dateInputModelConverter = Guard.ArgumentNotNull(nameof(dateInputModelConverter), dateInputModelConverter);
            _umbracoContextAccessor = umbracoContextAccessor ?? throw new ArgumentNullException(nameof(umbracoContextAccessor));
            _cultureDictionary = cultureDictionary ?? throw new ArgumentNullException(nameof(cultureDictionary));
            _publishedValueFallback = publishedValueFallback;
            _acceptMonthNamesInDateInputs = acceptMonthNamesInDateInputs;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Guard.ArgumentNotNull(nameof(bindingContext), bindingContext);

            var modelType = bindingContext.ModelMetadata.UnderlyingOrModelType;
            if (!_dateInputModelConverter.CanConvertModelType(modelType))
            {
                throw new InvalidOperationException($"Cannot bind {modelType.Name}.");
            }

            if (_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext))
            {
                if (!umbracoContext.IsFrontEndUmbracoRequest()) { return Task.CompletedTask; }
            }

            if (umbracoContext!.PublishedRequest?.PublishedContent is null)
            {
                throw new InvalidOperationException("Unable to get Umbraco published content");
            }

            var blockSettings = !string.IsNullOrEmpty(bindingContext.ModelMetadata.PropertyName) ? umbracoContext.PublishedRequest.PublishedContent.FindOverridableBlockModels(_publishedValueFallback).FindBlockByBoundProperty(bindingContext.ModelMetadata.PropertyName)?.Settings : null;
            var dayEnabled = blockSettings is not null ? blockSettings.Value<bool>(PropertyAliases.DateInputShowDay) : true;

            var dayModelName = $"{bindingContext.ModelName}.{DayComponentName}";
            var monthModelName = $"{bindingContext.ModelName}.{MonthComponentName}";
            var yearModelName = $"{bindingContext.ModelName}.{YearComponentName}";

            var dayValueProviderResult = dayEnabled ? bindingContext.ValueProvider.GetValue(dayModelName) : ValueProviderResult.None;
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
                dayEnabled,
                dayValueProviderResult.FirstValue,
                monthValueProviderResult.FirstValue,
                yearValueProviderResult.FirstValue,
                _acceptMonthNamesInDateInputs,
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
                    var errorMessage = GetModelStateErrorMessage(blockSettings, _cultureDictionary, parseErrors, bindingContext.ModelMetadata);
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, errorMessage);

                    bindingContext.Result = ModelBindingResult.Failed();
                }
            }

            return Task.CompletedTask;
        }

        // internal for testing
        internal static string GetModelStateErrorMessage(IOverridablePublishedElement? blockSettings, ICultureDictionary umbracoDictionary, DateInputParseErrors parseErrors, ModelMetadata modelMetadata)
        {
            Debug.Assert(parseErrors != DateInputParseErrors.None);
            Debug.Assert(parseErrors != (DateInputParseErrors.MissingDay | DateInputParseErrors.MissingMonth | DateInputParseErrors.MissingYear));

            string? displayName = null;
            if (!string.IsNullOrEmpty(modelMetadata.PropertyName))
            {
                displayName = blockSettings?.Value<string>(PropertyAliases.DisplayName)?.Trim();
            }
            if (string.IsNullOrEmpty(displayName)) { displayName = modelMetadata.PropertyName; }

            var missingDay = ((parseErrors & DateInputParseErrors.MissingDay) != 0);
            var missingMonth = ((parseErrors & DateInputParseErrors.MissingMonth) != 0);
            var missingYear = ((parseErrors & DateInputParseErrors.MissingYear) != 0);

            if (missingDay && !missingMonth && !missingYear)
            {
                return string.Format(umbracoDictionary[DictionaryConstants.DateMustIncludeADay] ?? "{0} must include a day", displayName).Trim();
            }
            if (!missingDay && missingMonth && !missingYear)
            {
                return string.Format(umbracoDictionary[DictionaryConstants.DateMustIncludeAMonth] ?? "{0} must include a month", displayName).Trim();
            }
            if (!missingDay && !missingMonth && missingYear)
            {
                return string.Format(umbracoDictionary[DictionaryConstants.DateMustIncludeAYear] ?? "{0} must include a year", displayName).Trim();
            }
            if (missingDay && missingMonth && !missingYear)
            {
                return string.Format(umbracoDictionary[DictionaryConstants.DateMustIncludeADayAndMonth] ?? "{0} must include a day and month", displayName).Trim();
            }
            if (missingDay && !missingMonth && missingYear)
            {
                return string.Format(umbracoDictionary[DictionaryConstants.DateMustIncludeADayAndYear] ?? "{0} must include a day and year", displayName).Trim();
            }
            if (!missingDay && missingMonth && missingYear)
            {
                return string.Format(umbracoDictionary[DictionaryConstants.DateMustIncludeAMonthAndYear] ?? "{0} must include a month and year", displayName).Trim();
            }

            return string.Format(umbracoDictionary[DictionaryConstants.DateMustBeARealDate] ?? "{0} must be a real date", displayName).Trim();
        }

        // internal for testing
        internal static DateInputParseErrors Parse(bool dayEnabled, string? day, string? month, string? year, bool acceptMonthNames, out DateOnly? date)
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
            else if (!TryParseYear(year, out parsedYear) || parsedYear < 1 || parsedYear > 9999)
            {
                errors |= DateInputParseErrors.InvalidYear;
            }

            if (string.IsNullOrEmpty(month))
            {
                errors |= DateInputParseErrors.MissingMonth;
            }
            else if (!TryParseMonth(month, out parsedMonth) || parsedMonth < 1 || parsedMonth > 12)
            {
                errors |= DateInputParseErrors.InvalidMonth;
            }

            if (dayEnabled)
            {
                if (string.IsNullOrEmpty(day))
                {
                    errors |= DateInputParseErrors.MissingDay;
                }
                else if (!TryParseDay(day, out parsedDay) || parsedDay < 1 || parsedDay > 31 ||
                    errors == DateInputParseErrors.None && parsedDay > DateTime.DaysInMonth(parsedYear, parsedMonth))
                {
                    errors |= DateInputParseErrors.InvalidDay;
                }
            }
            else
            {
                parsedDay = 1;
            }

            date = errors == DateInputParseErrors.None ? new(parsedYear, parsedMonth, parsedDay) : default;
            return errors;

            bool TryParseDay(string value, out int result) => int.TryParse(value, out result);

            bool TryParseMonth(string value, out int result)
            {
                if (string.IsNullOrEmpty(value))
                {
                    result = 0;
                    return false;
                }

                if (!int.TryParse(value, out result) && acceptMonthNames)
                {
                    result = value.ToLower() switch
                    {
                        "jan" => 1,
                        "january" => 1,
                        "feb" => 2,
                        "february" => 2,
                        "mar" => 3,
                        "march" => 3,
                        "apr" => 4,
                        "april" => 4,
                        "may" => 5,
                        "jun" => 6,
                        "june" => 6,
                        "jul" => 7,
                        "july" => 7,
                        "aug" => 8,
                        "august" => 8,
                        "sep" => 9,
                        "september" => 9,
                        "oct" => 10,
                        "october" => 10,
                        "nov" => 11,
                        "november" => 11,
                        "dec" => 12,
                        "december" => 12,
                        _ => 0
                    };
                }

                return result != 0;
            }

            bool TryParseYear(string value, out int result) => int.TryParse(value, out result);
        }
    }
}