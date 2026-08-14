using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Validation;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using Umbraco.Cms.Core.Dictionary;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common;
using Umbraco.Extensions;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.ModelBinding
{
    /// <summary>
    /// A copy of the date model binder from the base project, modified to make the error messages configurable in Umbraco
    /// </summary>
    internal class UmbracoDateInputModelBinder : IModelBinder
    {
        private const string DayComponentName = "Day";
        private const string MonthComponentName = "Month";
        private const string YearComponentName = "Year";

        internal static DateInputItemTypes[] SupportedItemTypes { get; } =
        [
            DateInputItemTypes.DayMonthAndYear,
            DateInputItemTypes.MonthAndYear,
            DateInputItemTypes.DayAndMonth
        ];

        private readonly DateInputModelConverter _dateInputModelConverter;
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly ICultureDictionary _cultureDictionary;
        private readonly IPublishedValueFallback _publishedValueFallback;
        private readonly bool _acceptMonthNamesInDateInputs;
        private readonly IUmbracoHelperAccessor _umbracoHelperAccessor;

        public UmbracoDateInputModelBinder(DateInputModelConverter dateInputModelConverter, IUmbracoContextAccessor umbracoContextAccessor, ICultureDictionary cultureDictionary, IPublishedValueFallback publishedValueFallback, bool acceptMonthNamesInDateInputs, IUmbracoHelperAccessor umbracoHelperAccessor)
        {
            _dateInputModelConverter = Guard.ArgumentNotNull(nameof(dateInputModelConverter), dateInputModelConverter);
            _umbracoContextAccessor = umbracoContextAccessor ?? throw new ArgumentNullException(nameof(umbracoContextAccessor));
            _cultureDictionary = cultureDictionary ?? throw new ArgumentNullException(nameof(cultureDictionary));
            _publishedValueFallback = publishedValueFallback;
            _acceptMonthNamesInDateInputs = acceptMonthNamesInDateInputs;
            _umbracoHelperAccessor = umbracoHelperAccessor;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Guard.ArgumentNotNull(nameof(bindingContext), bindingContext);

            var modelType = bindingContext.ModelMetadata.UnderlyingOrModelType;

            if (_umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext))
            {
                if (!umbracoContext.IsFrontEndUmbracoRequest()) { return Task.CompletedTask; }
            }

            if (umbracoContext!.PublishedRequest?.PublishedContent is null)
            {
                throw new InvalidOperationException("Unable to get Umbraco published content");
            }

            var blockSettings = !string.IsNullOrEmpty(bindingContext.ModelMetadata.PropertyName) ? umbracoContext.PublishedRequest.PublishedContent.FindOverridableBlockModels(_publishedValueFallback).FindBlockByBoundProperty(bindingContext.ModelMetadata.PropertyName)?.Settings : null;
            var dayEnabled = blockSettings is not null ? blockSettings.Value<bool>(_publishedValueFallback, PropertyAliases.DateInputShowDay) : true;
            var yearEnabled = blockSettings is not null ? blockSettings.Value<bool>(_publishedValueFallback, PropertyAliases.DateInputShowYear) : true;

            var dayModelName = $"{bindingContext.ModelName}.{DayComponentName}";
            var monthModelName = $"{bindingContext.ModelName}.{MonthComponentName}";
            var yearModelName = $"{bindingContext.ModelName}.{YearComponentName}";

            var dayValueProviderResult = dayEnabled ? bindingContext.ValueProvider.GetValue(dayModelName) : ValueProviderResult.None;
            var monthValueProviderResult = bindingContext.ValueProvider.GetValue(monthModelName);
            var yearValueProviderResult = yearEnabled ? bindingContext.ValueProvider.GetValue(yearModelName) : ValueProviderResult.None;

            if ((dayValueProviderResult == ValueProviderResult.None || dayValueProviderResult.FirstValue == string.Empty) &&
                (monthValueProviderResult == ValueProviderResult.None || monthValueProviderResult.FirstValue == string.Empty) &&
                (yearValueProviderResult == ValueProviderResult.None || yearValueProviderResult.FirstValue == string.Empty))
            {
                return Task.CompletedTask;
            }

            DateInputItemTypes itemTypes;
            if (!dayEnabled)
            {
                itemTypes = DateInputItemTypes.MonthAndYear;
            }
            else if (!yearEnabled)
            {
                itemTypes = DateInputItemTypes.DayAndMonth;
            }
            else
            {
                itemTypes = DateInputItemTypes.DayMonthAndYear;
            }

            if (!SupportedItemTypes.Contains(itemTypes))
            {
                // Weird combination of fields submitted; we're done
                return Task.CompletedTask;
            }

            // If some validation exists where a fully-parsable date is an invalid value, using SetInitialValue here allows the date to repopulate
            // without affecting validation. If SetModelValue is used then the date repopulates, but can never pass validation.
            bindingContext.ModelState.SetInitialValue(dayModelName, dayValueProviderResult.FirstValue!);
            bindingContext.ModelState.SetInitialValue(monthModelName, monthValueProviderResult.FirstValue!);
            bindingContext.ModelState.SetInitialValue(yearModelName, yearValueProviderResult.FirstValue!);

            var parseErrors = Parse(
                itemTypes,
                dayValueProviderResult.FirstValue,
                monthValueProviderResult.FirstValue,
                yearValueProviderResult.FirstValue,
                _acceptMonthNamesInDateInputs,
                out var dateParts);

            if (parseErrors == DateInputParseErrors.None)
            {
                var createModelContext = new DateInputConvertToModelContext(modelType, itemTypes, dateParts);
                var model = _dateInputModelConverter.ConvertToModel(createModelContext);
                bindingContext.ModelState.SetInitialValue(bindingContext.ModelName, new DateOnly(dateParts.Year!.Value, dateParts.Month!.Value, dateParts.Day!.Value).ToString("yyyy-MM-dd"));
                bindingContext.Result = ModelBindingResult.Success(model);
            }
            else
            {
                if (!_umbracoHelperAccessor.TryGetUmbracoHelper(out var umbracoHelper))
                {
                    throw new InvalidOperationException("Unable to access Umbraco helper");
                }

                var overallAttemptedValueParts = new List<ValueProviderResult>();

                if (itemTypes.HasFlag(DateInputItemTypes.Day))
                {
                    overallAttemptedValueParts.Add(dayValueProviderResult);
                }

                if (itemTypes.HasFlag(DateInputItemTypes.Month))
                {
                    overallAttemptedValueParts.Add(monthValueProviderResult);
                }

                if (itemTypes.HasFlag(DateInputItemTypes.Year))
                {
                    overallAttemptedValueParts.Add(yearValueProviderResult);
                }

                var overallAttemptedValue = string.Join(",", overallAttemptedValueParts.Select(vpr => vpr.FirstValue ?? ""));

                var errorMessage = GetModelStateErrorMessage(blockSettings, _publishedValueFallback, _cultureDictionary, parseErrors, bindingContext.ModelMetadata, umbracoHelper);
                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, rawValue: null, attemptedValue: overallAttemptedValue);
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, errorMessage);

                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }

        // internal for testing
        internal static string GetModelStateErrorMessage(IOverridablePublishedElement? blockSettings, IPublishedValueFallback publishedValueFallback, ICultureDictionary umbracoDictionary, DateInputParseErrors parseErrors, ModelMetadata modelMetadata, UmbracoHelper umbracoHelper)
        {
            Debug.Assert(parseErrors != DateInputParseErrors.None);
            Debug.Assert(parseErrors != (DateInputParseErrors.MissingDay | DateInputParseErrors.MissingMonth | DateInputParseErrors.MissingYear));

            string? displayName = null;
            if (!string.IsNullOrEmpty(modelMetadata.PropertyName))
            {
                displayName = blockSettings?.Value<string>(publishedValueFallback, PropertyAliases.DisplayName)?.Trim();
            }
            if (string.IsNullOrEmpty(displayName)) { displayName = modelMetadata.PropertyName; }

            var missingDay = ((parseErrors & DateInputParseErrors.MissingDay) != 0);
            var missingMonth = ((parseErrors & DateInputParseErrors.MissingMonth) != 0);
            var missingYear = ((parseErrors & DateInputParseErrors.MissingYear) != 0);

            if (missingDay && !missingMonth && !missingYear)
            {
                return string.Format(umbracoHelper.GetDictionaryValueOrDefault(DictionaryConstants.DateMustIncludeADay, "{0} must include a day"), displayName).Trim();
            }
            if (!missingDay && missingMonth && !missingYear)
            {
                return string.Format(umbracoHelper.GetDictionaryValueOrDefault(DictionaryConstants.DateMustIncludeAMonth, "{0} must include a month"), displayName).Trim();
            }
            if (!missingDay && !missingMonth && missingYear)
            {
                return string.Format(umbracoHelper.GetDictionaryValueOrDefault(DictionaryConstants.DateMustIncludeAYear, "{0} must include a year"), displayName).Trim();
            }
            if (missingDay && missingMonth && !missingYear)
            {
                return string.Format(umbracoHelper.GetDictionaryValueOrDefault(DictionaryConstants.DateMustIncludeADayAndMonth, "{0} must include a day and month"), displayName).Trim();
            }
            if (missingDay && !missingMonth && missingYear)
            {
                return string.Format(umbracoHelper.GetDictionaryValueOrDefault(DictionaryConstants.DateMustIncludeADayAndYear, "{0} must include a day and year"), displayName).Trim();
            }
            if (!missingDay && missingMonth && missingYear)
            {
                return string.Format(umbracoHelper.GetDictionaryValueOrDefault(DictionaryConstants.DateMustIncludeAMonthAndYear, "{0} must include a month and year"), displayName).Trim();
            }

            return string.Format(umbracoHelper.GetDictionaryValueOrDefault(DictionaryConstants.DateMustBeARealDate, "{0} must be a real date"), displayName).Trim();
        }

        // internal for testing
        internal static DateInputParseErrors Parse(DateInputItemTypes itemTypes, string? day, string? month, string? year, bool acceptMonthNames, out DateInputItemValues dateParts)
        {
            day ??= string.Empty;
            month ??= string.Empty;
            year ??= string.Empty;

            var errors = DateInputParseErrors.None;
            int parsedYear = 0, parsedMonth = 0, parsedDay = 0;
            int? maxDaysInMonth = null;

            var expectYear = (itemTypes & DateInputItemTypes.Year) != 0;
            Debug.Assert((itemTypes & DateInputItemTypes.Month) != 0);
            var expectMonth = true;
            var expectDay = (itemTypes & DateInputItemTypes.Day) != 0;

            if (expectYear)
            {
                if (string.IsNullOrEmpty(year))
                {
                    errors |= DateInputParseErrors.MissingYear;
                }
                else if (!TryParseYear(year, out parsedYear) || parsedYear < 1 || parsedYear > 9999 || year.Length != 4)
                {
                    errors |= DateInputParseErrors.InvalidYear;
                }
            }

            var yearIsValid = (errors & (DateInputParseErrors.InvalidYear | DateInputParseErrors.MissingYear)) == 0;

            if (expectMonth)
            {
                if (string.IsNullOrEmpty(month))
                {
                    errors |= DateInputParseErrors.MissingMonth;
                }
                else if (!TryParseMonth(month, out parsedMonth) || parsedMonth < 1 || parsedMonth > 12)
                {
                    errors |= DateInputParseErrors.InvalidMonth;
                }
            }

            var monthIsValid = (errors & (DateInputParseErrors.InvalidMonth | DateInputParseErrors.MissingMonth)) == 0;

            if (expectDay)
            {
                // If we know the year and month we can figure out the days in the month.
                // If we only have the month and not a year, we should assume the year could be a leap year.
                if (monthIsValid)
                {
                    var assumedYear = 2000; // 2000 is a leap year
                    if (yearIsValid && parsedYear != 0)
                    {
                        assumedYear = parsedYear;
                    }

                    maxDaysInMonth = DateTime.DaysInMonth(assumedYear, parsedMonth);
                }

                if (string.IsNullOrEmpty(day))
                {
                    errors |= DateInputParseErrors.MissingDay;
                }
                else if (!TryParseDay(day, out parsedDay) || parsedDay < 1 || parsedDay > 31 || parsedDay > maxDaysInMonth)
                {
                    errors |= DateInputParseErrors.InvalidDay;
                }
            }

            dateParts = errors == DateInputParseErrors.None ? new(
                expectDay ? parsedDay : 1,
                expectMonth ? parsedMonth : null,
                expectYear ? parsedYear : 2000) : default;
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
                    result = value.ToLowerInvariant() switch
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

                return result is not 0;
            }

            bool TryParseYear(string value, out int result) => int.TryParse(value, out result);
        }
    }
}