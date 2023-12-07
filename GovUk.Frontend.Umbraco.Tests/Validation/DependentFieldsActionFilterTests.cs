using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Testing;

namespace GovUk.Frontend.Umbraco.Tests.Validation
{
    [TestFixture]
    public class DependentFieldsActionFilterTests
    {
        private const string PARENT_MODEL_PROPERTY = "Field1";
        private const string DEPENDENT_MODEL_PROPERTY = "Field2";
        private const string RADIO_WITH_DEPENDENT_FIELD_VALUE = "1";
        private const string RADIO_WITHOUT_DEPENDENT_FIELD_VALUE = "2";

        [Test]
        public void Invalid_ModelState_remains_invalid_for_non_dependent_field()
        {
            // Arrange
            UmbracoTestContext testContext = CreateTestContext();

            testContext.CurrentPage.Object.SetupUmbracoBlockListPropertyValue("blocks", BlockListWithOneTextInput(DEPENDENT_MODEL_PROPERTY));

            var modelState = new ModelStateDictionary();
            modelState.AddModelError(DEPENDENT_MODEL_PROPERTY, "Any error");

            var actionExecutingContext = CreateActionExecutingContext(testContext.HttpContext.Object, modelState);

            var filter = new DependentFieldsActionFilter(testContext.UmbracoContextAccessor.Object);

            // Act
            filter.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.That(modelState.Count, Is.EqualTo(1));
            Assert.That(modelState[DEPENDENT_MODEL_PROPERTY]!.ValidationState, Is.EqualTo(ModelValidationState.Invalid));
        }



        [Test]
        public void Invalid_ModelState_set_to_skipped_when_parent_field_is_invalid()
        {
            // Arrange
            UmbracoTestContext testContext = CreateTestContext();

            testContext.CurrentPage.Object.SetupUmbracoBlockListPropertyValue("blocks", BlockListWithRadiosWithOneDependentField());

            var modelState = new ModelStateDictionary();
            modelState.AddModelError(PARENT_MODEL_PROPERTY, "Any error");
            modelState.AddModelError(DEPENDENT_MODEL_PROPERTY, "Any error");

            var actionExecutingContext = CreateActionExecutingContext(testContext.HttpContext.Object, modelState);

            var filter = new DependentFieldsActionFilter(testContext.UmbracoContextAccessor.Object);

            // Act
            filter.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.That(modelState.Count, Is.EqualTo(2));
            Assert.That(modelState[PARENT_MODEL_PROPERTY]!.ValidationState, Is.EqualTo(ModelValidationState.Invalid));
            Assert.That(modelState[DEPENDENT_MODEL_PROPERTY]!.ValidationState, Is.EqualTo(ModelValidationState.Skipped));
        }

        [Test]
        public void Invalid_ModelState_set_to_skipped_when_parent_field_is_valid_but_parent_option_not_selected()
        {
            // Arrange
            UmbracoTestContext testContext = CreateTestContext();

            testContext.CurrentPage.Object.SetupUmbracoBlockListPropertyValue("blocks", BlockListWithRadiosWithOneDependentField());

            var modelState = new ModelStateDictionary();
            modelState.SetModelValue(PARENT_MODEL_PROPERTY, RADIO_WITHOUT_DEPENDENT_FIELD_VALUE, RADIO_WITHOUT_DEPENDENT_FIELD_VALUE);
            modelState.MarkFieldValid(PARENT_MODEL_PROPERTY);
            modelState.AddModelError(DEPENDENT_MODEL_PROPERTY, "Any error");

            var actionExecutingContext = CreateActionExecutingContext(testContext.HttpContext.Object, modelState);

            var filter = new DependentFieldsActionFilter(testContext.UmbracoContextAccessor.Object);

            // Act
            filter.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.That(modelState.Count, Is.EqualTo(2));
            Assert.That(modelState[PARENT_MODEL_PROPERTY]!.ValidationState, Is.EqualTo(ModelValidationState.Valid));
            Assert.That(modelState[DEPENDENT_MODEL_PROPERTY]!.ValidationState, Is.EqualTo(ModelValidationState.Skipped));
        }

        [Test]
        public void Invalid_ModelState_remains_invalid_when_parent_field_is_valid_and_parent_option_selected()
        {
            // Arrange
            UmbracoTestContext testContext = CreateTestContext();

            testContext.CurrentPage.Object.SetupUmbracoBlockListPropertyValue("blocks", BlockListWithRadiosWithOneDependentField());

            var modelState = new ModelStateDictionary();
            modelState.SetModelValue(PARENT_MODEL_PROPERTY, RADIO_WITH_DEPENDENT_FIELD_VALUE, RADIO_WITH_DEPENDENT_FIELD_VALUE);
            modelState.MarkFieldValid(PARENT_MODEL_PROPERTY);
            modelState.AddModelError(DEPENDENT_MODEL_PROPERTY, "Any error");

            var actionExecutingContext = CreateActionExecutingContext(testContext.HttpContext.Object, modelState);

            var filter = new DependentFieldsActionFilter(testContext.UmbracoContextAccessor.Object);

            // Act
            filter.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.That(modelState.Count, Is.EqualTo(2));
            Assert.That(modelState[PARENT_MODEL_PROPERTY]!.ValidationState, Is.EqualTo(ModelValidationState.Valid));
            Assert.That(modelState[DEPENDENT_MODEL_PROPERTY]!.ValidationState, Is.EqualTo(ModelValidationState.Invalid));
        }

        private static OverridableBlockListModel BlockListWithRadiosWithOneDependentField()
        {
            var radioButtons = UmbracoBlockListFactory.CreateOverridableBlockListModel(new[]
            {
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.Radio)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.RadioButtonValue, RADIO_WITH_DEPENDENT_FIELD_VALUE)
                        .SetupUmbracoBlockListPropertyValue(PropertyAliases.RadioConditionalBlocks, BlockListWithOneTextInput(DEPENDENT_MODEL_PROPERTY))
                        .Object
                ),
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.Radio)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.RadioButtonValue, RADIO_WITHOUT_DEPENDENT_FIELD_VALUE)
                        .Object
                )
            });

            return UmbracoBlockListFactory.CreateOverridableBlockListModel(
                        UmbracoBlockListFactory.CreateOverridableBlock(
                            UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.Radios)
                                .SetupUmbracoBlockListPropertyValue(PropertyAliases.RadioButtons, radioButtons)
                                .Object,
                            UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.RadiosSettings)
                                .SetupUmbracoTextboxPropertyValue(PropertyAliases.ModelProperty, PARENT_MODEL_PROPERTY)
                                .Object
                            )
                        );
        }

        private static OverridableBlockListModel BlockListWithOneTextInput(string modelProperty)
        {
            return UmbracoBlockListFactory.CreateOverridableBlockListModel(
                        UmbracoBlockListFactory.CreateOverridableBlock(
                            UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.TextInput).Object,
                            UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.TextInputSettings)
                                .SetupUmbracoTextboxPropertyValue(PropertyAliases.ModelProperty, modelProperty)
                            .Object
                            )
                        );
        }

        private static UmbracoTestContext CreateTestContext()
        {
            var testContext = new UmbracoTestContext();
            testContext.Request.Setup(x => x.Method).Returns(HttpMethod.Post.Method);
            return testContext;
        }
        private static ActionExecutingContext CreateActionExecutingContext(HttpContext httpContext, ModelStateDictionary modelState)
        {
            var actionContext = new ActionContext(
                                httpContext,
                                Mock.Of<RouteData>(),
                                Mock.Of<ActionDescriptor>(),
                                modelState
                            );
            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object?>(),
                Mock.Of<Controller>()
            );
            return actionExecutingContext;
        }
    }
}
