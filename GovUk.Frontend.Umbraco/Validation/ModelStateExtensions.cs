using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;

namespace GovUk.Frontend.Umbraco.Validation
{
    public static class ModelStateExtensions
    {
        /// <summary>
        /// Sets the initial values for a field that supports multiple selected values, without marking the field as invalid
        /// </summary>
        /// <param name="modelState">The <see cref="ModelStateDictionary"/> that contains the state of the model and of model-binding validation</param>
        /// <param name="key">The name attribute of the field in the HTML</param>
        /// <param name="initialValues">The initial values to select</param>
        /// <exception cref="ArgumentNullException">modelState</exception>
        public static void SetInitialValue(this ModelStateDictionary modelState, string key, StringValues initialValues)
        {
            if (modelState is null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            modelState.SetModelValue(key, null, initialValues);
            modelState.MarkFieldSkipped(key);
        }

        /// <summary>
        /// Sets the initial value for a field, without marking the field as invalid
        /// </summary>
        /// <param name="modelState">The <see cref="ModelStateDictionary"/> that contains the state of the model and of model-binding validation</param>
        /// <param name="key">The name attribute of the field in the HTML</param>
        /// <param name="initialValues">The initial value to select or populate</param>
        /// <exception cref="ArgumentNullException">modelState</exception>
        public static void SetInitialValue(this ModelStateDictionary modelState, string key, string? initialValue)
        {
            if (modelState is null)
            {
                throw new ArgumentNullException(nameof(modelState));
            }

            modelState.SetModelValue(key, null, initialValue);
            modelState.MarkFieldSkipped(key);
        }
    }
}
