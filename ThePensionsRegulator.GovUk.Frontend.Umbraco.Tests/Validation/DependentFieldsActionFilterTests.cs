using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Validation;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using ThePensionsRegulator.Umbraco.Testing;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Tests.Validation
{
    public class DependentFieldsActionFilterTests : IDisposable
    {
        private const string PARENT_MODEL_PROPERTY = "Field1";
        private const string DEPENDENT_MODEL_PROPERTY = "Field2";
        private const string RADIO_WITH_DEPENDENT_FIELD_VALUE = "1";
        private const string RADIO_WITHOUT_DEPENDENT_FIELD_VALUE = "2";

        private readonly UmbracoTestContext _testContext;

        public DependentFieldsActionFilterTests()
        {
            _testContext = new UmbracoTestContext();
            _testContext.Request.Setup(x => x.Method).Returns(HttpMethod.Post.Method);
        }

        public void Dispose() => _testContext.Dispose();

        [Fact]
        public void Invalid_ModelState_remains_invalid_for_non_dependent_field()
        {
            // Arrange
            _testContext.CurrentPage.Object.SetupUmbracoBlockListPropertyValue("blocks", BlockListWithOneTextInput(DEPENDENT_MODEL_PROPERTY));

            var modelState = new ModelStateDictionary();
            modelState.AddModelError(DEPENDENT_MODEL_PROPERTY, "Any error");

            var actionExecutingContext = CreateActionExecutingContext(_testContext.HttpContext.Object, modelState);

            var filter = new DependentFieldsActionFilter(_testContext.UmbracoContextAccessor.Object, _testContext.PublishedValueFallback.Object);

            // Act
            filter.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.Single(modelState);
            Assert.Equal(ModelValidationState.Invalid, modelState[DEPENDENT_MODEL_PROPERTY]!.ValidationState);
        }



        [Fact]
        public void Invalid_ModelState_set_to_skipped_when_parent_field_is_invalid()
        {
            // Arrange
            _testContext.CurrentPage.Object.SetupUmbracoBlockListPropertyValue("blocks", BlockListWithRadiosWithOneDependentField());

            var modelState = new ModelStateDictionary();
            modelState.AddModelError(PARENT_MODEL_PROPERTY, "Any error");
            modelState.AddModelError(DEPENDENT_MODEL_PROPERTY, "Any error");

            var actionExecutingContext = CreateActionExecutingContext(_testContext.HttpContext.Object, modelState);

            var filter = new DependentFieldsActionFilter(_testContext.UmbracoContextAccessor.Object, _testContext.PublishedValueFallback.Object);

            // Act
            filter.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.Equal(2, modelState.Count);
            Assert.Equal(ModelValidationState.Invalid, modelState[PARENT_MODEL_PROPERTY]!.ValidationState);
            Assert.Equal(ModelValidationState.Skipped, modelState[DEPENDENT_MODEL_PROPERTY]!.ValidationState);
        }

        [Fact]
        public void Invalid_ModelState_set_to_skipped_when_parent_field_is_valid_but_parent_option_not_selected()
        {
            // Arrange
            _testContext.CurrentPage.Object.SetupUmbracoBlockListPropertyValue("blocks", BlockListWithRadiosWithOneDependentField());

            var modelState = new ModelStateDictionary();
            modelState.SetModelValue(PARENT_MODEL_PROPERTY, RADIO_WITHOUT_DEPENDENT_FIELD_VALUE, RADIO_WITHOUT_DEPENDENT_FIELD_VALUE);
            modelState.MarkFieldValid(PARENT_MODEL_PROPERTY);
            modelState.AddModelError(DEPENDENT_MODEL_PROPERTY, "Any error");

            var actionExecutingContext = CreateActionExecutingContext(_testContext.HttpContext.Object, modelState);

            var filter = new DependentFieldsActionFilter(_testContext.UmbracoContextAccessor.Object, _testContext.PublishedValueFallback.Object);

            // Act
            filter.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.Equal(2, modelState.Count);
            Assert.Equal(ModelValidationState.Valid, modelState[PARENT_MODEL_PROPERTY]!.ValidationState);
            Assert.Equal(ModelValidationState.Skipped, modelState[DEPENDENT_MODEL_PROPERTY]!.ValidationState);
        }

        [Fact]
        public void Invalid_ModelState_remains_invalid_when_parent_field_is_valid_and_parent_option_selected()
        {
            // Arrange
            _testContext.CurrentPage.Object.SetupUmbracoBlockListPropertyValue("blocks", BlockListWithRadiosWithOneDependentField());

            var modelState = new ModelStateDictionary();
            modelState.SetModelValue(PARENT_MODEL_PROPERTY, RADIO_WITH_DEPENDENT_FIELD_VALUE, RADIO_WITH_DEPENDENT_FIELD_VALUE);
            modelState.MarkFieldValid(PARENT_MODEL_PROPERTY);
            modelState.AddModelError(DEPENDENT_MODEL_PROPERTY, "Any error");

            var actionExecutingContext = CreateActionExecutingContext(_testContext.HttpContext.Object, modelState);

            var filter = new DependentFieldsActionFilter(_testContext.UmbracoContextAccessor.Object, _testContext.PublishedValueFallback.Object);

            // Act
            filter.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.Equal(2, modelState.Count);
            Assert.Equal(ModelValidationState.Valid, modelState[PARENT_MODEL_PROPERTY]!.ValidationState);
            Assert.Equal(ModelValidationState.Invalid, modelState[DEPENDENT_MODEL_PROPERTY]!.ValidationState);
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
