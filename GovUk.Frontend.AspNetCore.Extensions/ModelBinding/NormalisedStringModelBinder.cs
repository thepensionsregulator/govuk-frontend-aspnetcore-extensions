using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.ModelBinding
{
    /// <remarks>
    /// When running in a Windows environment, MaxLengthAttribute and StringLengthAttribute treat a new line as two characters, \r\n.
    /// This does not align with users' expectations or with client-side validation using jQuery validate, so remove the \r to leave 
    /// a single character.
    /// </remarks>
    public class NormalisedStringModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (!string.IsNullOrEmpty(value.FirstValue))
            {
                bindingContext.Result = ModelBindingResult.Success(value.FirstValue.Replace("\r\n", "\n"));
            }
            return Task.CompletedTask;
        }
    }
}
