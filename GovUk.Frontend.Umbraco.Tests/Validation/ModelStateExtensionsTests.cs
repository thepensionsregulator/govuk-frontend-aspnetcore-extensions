using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;

namespace GovUk.Frontend.Umbraco.Tests.Validation
{
    public class ModelStateExtensionsTests
    {
        [Test]
        public void Initial_value_is_set_without_setting_IsValid_to_false()
        {
            var modelState = new ModelStateDictionary();

            modelState.SetInitialValue("fieldname", "value");

            Assert.True(modelState.ContainsKey("fieldname"));
            Assert.AreEqual("value", modelState["fieldname"]!.AttemptedValue);
            Assert.True(modelState.IsValid);
        }

        [Test]
        public void Initial_values_are_set_without_setting_IsValid_to_false()
        {
            var modelState = new ModelStateDictionary();

            modelState.SetInitialValue("fieldname", new StringValues(new[] { "value1", "value2" }));

            Assert.True(modelState.ContainsKey("fieldname"));
            Assert.AreEqual("value1,value2", modelState["fieldname"]!.AttemptedValue);
            Assert.True(modelState.IsValid);
        }
    }
}
