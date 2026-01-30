using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using ThePensionsRegulator.Frontend.Umbraco.Services;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
    public class AddressFieldStateHelperTests
    {
        private readonly IAddressFieldStateHelper _sut;

        public AddressFieldStateHelperTests()
        {
            _sut = new AddressFieldStateHelper();
        }

        [Fact]
        public void GetFieldState_WithUmbracoLabel_ReturnsUmbracoLabel()
        {
            // Arrange
            var modelState = new ModelStateDictionary();
            var umbracoLabel = "Custom Label";
            var defaultLabel = "Default Label";

            // Act
            var result = _sut.GetFieldState("PropertyName", umbracoLabel, defaultLabel, modelState);

            // Assert
            Assert.Equal(umbracoLabel, result.Label); ;
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GetFieldState_WithEmptyOrNullUmbracoLabel_ReturnsDefaultLabel(string? umbracoLabel)
        {
            // Arrange
            var modelState = new ModelStateDictionary();
            var defaultLabel = "Default Label";

            // Act
            var result = _sut.GetFieldState("PropertyName", umbracoLabel, defaultLabel, modelState);

            // Assert
            Assert.Equal(defaultLabel, result.Label);
        }

        [Fact]
        public void GetFieldState_WithNullModelPropertyName_ReturnsEmptyModelPropertyName()
        {
            // Arrange
            var modelState = new ModelStateDictionary();

            // Act
            var result = _sut.GetFieldState(null, "Label", "Default", modelState);

            // Assert
            Assert.Equal(string.Empty, result.ModelPropertyName);
        }

        [Fact]
        public void GetFieldState_WithValidModelState_ReturnsNotInvalid()
        {
            // Arrange
            var modelState = new ModelStateDictionary();

            // Act
            var result = _sut.GetFieldState("PropertyName", null, "Default", modelState);

            // Assert
            Assert.Equal("false", result.InvalidAriaLabel);
            Assert.False(result.HasErrorMessage);
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public void GetFieldState_WithInvalidModelState_ReturnsInvalid()
        {
            // Arrange
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("PropertyName", "This field is required");

            // Act
            var result = _sut.GetFieldState("PropertyName", null, "Default", modelState);

            // Assert
            Assert.Equal("true", result.InvalidAriaLabel);
            Assert.True(result.HasErrorMessage);
            Assert.Equal("This field is required", result.ErrorMessage);
        }

        [Fact]
        public void GetFieldState_WithMultipleErrors_JoinsErrorMessages()
        {
            // Arrange
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("PropertyName", "First error");
            modelState.AddModelError("PropertyName", "Second error");

            // Act
            var result = _sut.GetFieldState("PropertyName", null, "Default", modelState);

            // Assert
            Assert.True(result.HasErrorMessage);
            Assert.Equal("First error. Second error", result.ErrorMessage);
        }

        [Fact]
        public void GetFieldState_WithFieldsetError_FiltersOutFieldsetError()
        {
            // Arrange
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("PropertyName", ValidationConstants.FIELDSET_ERROR);

            // Act
            var result = _sut.GetFieldState("PropertyName", null, "Default", modelState);

            // Assert
            Assert.Equal("true", result.InvalidAriaLabel);
            Assert.False(result.HasErrorMessage);
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public void GetFieldState_WithMixedErrors_FiltersOutFieldsetErrorButKeepsOthers()
        {
            // Arrange
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("PropertyName", "Real error");
            modelState.AddModelError("PropertyName", ValidationConstants.FIELDSET_ERROR);
            modelState.AddModelError("PropertyName", "Another error");

            // Act
            var result = _sut.GetFieldState("PropertyName", null, "Default", modelState);

            // Assert
            Assert.Equal("true", result.InvalidAriaLabel);
            Assert.True(result.HasErrorMessage);
            Assert.Equal("Real error. Another error", result.ErrorMessage);
        }

        [Fact]
        public void GetFieldState_WithAttemptedValue_ReturnsAttemptedValue()
        {
            // Arrange
            var modelState = new ModelStateDictionary();
            var attemptedValue = "attempted value";
            modelState.SetModelValue("PropertyName", new ValueProviderResult(attemptedValue));

            // Act
            var result = _sut.GetFieldState("PropertyName", null, "Default", modelState);

            // Assert
            Assert.Equal(attemptedValue, result.AttemptedValue);
        }

        [Fact]
        public void GetFieldState_WithoutAttemptedValue_ReturnsNullAttemptedValue()
        {
            // Arrange
            var modelState = new ModelStateDictionary();

            // Act
            var result = _sut.GetFieldState("PropertyName", null, "Default", modelState);

            // Assert
            Assert.Null(result.AttemptedValue);
        }

        [Fact]
        public void GetFieldState_WithNonMatchingPropertyName_ReturnsNotInvalid()
        {
            // Arrange
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("OtherProperty", "Error message");

            // Act
            var result = _sut.GetFieldState("PropertyName", null, "Default", modelState);

            // Assert
            Assert.Equal("false", result.InvalidAriaLabel);
            Assert.False(result.HasErrorMessage);
        }

        [Fact]
        public void GetFieldState_WithEmptyModelPropertyName_ReturnsNotInvalid()
        {
            // Arrange
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("PropertyName", "Error message");

            // Act
            var result = _sut.GetFieldState("", null, "Default", modelState);

            // Assert
            Assert.Equal("false", result.InvalidAriaLabel);
            Assert.False(result.HasErrorMessage);
            Assert.Equal(string.Empty, result.ModelPropertyName);
        }

        [Fact]
        public void GetFieldState_ReturnsAllFieldsPopulated()
        {
            // Arrange
            var modelState = new ModelStateDictionary();
            modelState.SetModelValue("PropertyName", new ValueProviderResult("test value"));
            modelState.AddModelError("PropertyName", "Error message");

            // Act
            var result = _sut.GetFieldState("PropertyName", "Custom Label", "Default", modelState);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("PropertyName", result.ModelPropertyName);
            Assert.Equal("Custom Label", result.Label);
            Assert.Equal("true", result.InvalidAriaLabel);
            Assert.True(result.HasErrorMessage);
            Assert.Equal("test value", result.AttemptedValue);
            Assert.Equal("Error message", result.ErrorMessage);
        }
    }
}
