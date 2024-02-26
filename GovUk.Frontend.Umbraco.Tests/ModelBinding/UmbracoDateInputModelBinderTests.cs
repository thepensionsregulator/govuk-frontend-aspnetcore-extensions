using GovUk.Frontend.AspNetCore.ModelBinding;
using GovUk.Frontend.Umbraco.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Dictionary;
using Umbraco.Cms.Core.Routing;

namespace GovUk.Frontend.Umbraco.Tests.ModelBinding
{
    public class UmbracoDateInputModelBinderTests
    {
#nullable disable
        private UmbracoTestContext _testContext;
#nullable enable

        [SetUp]
        public void SetUp()
        {
            _testContext = new();
            _testContext.ServiceProvider.Setup(mock => mock.GetService(typeof(DateInputParseErrorsProvider))).Returns(new DateInputParseErrorsProvider());
        }

        private UmbracoDateInputModelBinder CreateModelBinder(Mock<DateInputModelConverter> converterMock)
            => new UmbracoDateInputModelBinder(
                    converterMock.Object,
                    _testContext.UmbracoContextAccessor.Object,
                    Mock.Of<ICultureDictionary>(),
                    _testContext.PublishedValueFallback.Object,
                    false
                );

        [Test]
        public async Task BindModelAsync_BackOfficeRequest_DoesNotBind()
        {
            // Arrange
            var modelType = typeof(DateOnly);

            ModelBindingContext bindingContext = new DefaultModelBindingContext()
            {
                ActionContext = CreateActionContext(),
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType),
                ModelName = "TheModelName",
                ModelState = new ModelStateDictionary(),
                ValueProvider = new SimpleValueProvider()
                {
                    { "TheModelName.Day", "1" },
                    { "TheModelName.Month", "4" },
                    { "TheModelName.Year", "2020" }
                }
            };

            var converterMock = new Mock<DateInputModelConverter>();
            converterMock.Setup(mock => mock.CanConvertModelType(modelType)).Returns(true);

            var modelBinder = CreateModelBinder(converterMock);

            _testContext.UmbracoContext.Setup(x => x.PublishedRequest).Returns<IPublishedRequest?>(null);

            // Act
            await modelBinder.BindModelAsync(bindingContext);

            // Assert
            Assert.False(bindingContext.Result.IsModelSet);
        }

        [Test]
        public async Task BindModelAsync_AllComponentsEmpty_DoesNotBind()
        {
            // Arrange
            var modelType = typeof(DateOnly);

            ModelBindingContext bindingContext = new DefaultModelBindingContext()
            {
                ActionContext = CreateActionContext(),
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType),
                ModelName = "TheModelName",
                ModelState = new ModelStateDictionary(),
                ValueProvider = new SimpleValueProvider()
            };

            var converterMock = new Mock<DateInputModelConverter>();
            converterMock.Setup(mock => mock.CanConvertModelType(modelType)).Returns(true);

            var modelBinder = CreateModelBinder(converterMock);

            // Act
            await modelBinder.BindModelAsync(bindingContext);

            // Assert
            Assert.False(bindingContext.Result.IsModelSet);
        }

        [Test]
        public async Task BindModelAsync_AllComponentsProvided_PassesValuesToConverterAndBindsResult()
        {
            // Arrange
            var modelType = typeof(DateOnly);

            ModelBindingContext bindingContext = new DefaultModelBindingContext()
            {
                ActionContext = CreateActionContext(),
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType),
                ModelName = "TheModelName",
                ModelState = new ModelStateDictionary(),
                ValueProvider = new SimpleValueProvider()
                {
                    { "TheModelName.Day", "1" },
                    { "TheModelName.Month", "4" },
                    { "TheModelName.Year", "2020" }
                }
            };

            var converterMock = new Mock<DateInputModelConverter>();
            converterMock.Setup(mock => mock.CanConvertModelType(modelType)).Returns(true);

            converterMock
                .Setup(mock => mock.CreateModelFromDate(modelType, new DateOnly(2020, 4, 1)))
                .Returns(new DateOnly(2020, 4, 1))
                .Verifiable();

            var modelBinder = CreateModelBinder(converterMock);

            // Act
            await modelBinder.BindModelAsync(bindingContext);

            // Assert
            converterMock.Verify();

            Assert.True(bindingContext.Result.IsModelSet);

            Assert.IsInstanceOf<DateOnly>(bindingContext.Result.Model);
            var date = (DateOnly)bindingContext.Result.Model!;
            Assert.AreEqual(2020, date.Year);
            Assert.AreEqual(4, date.Month);
            Assert.AreEqual(1, date.Day);

            Assert.AreEqual("2020", bindingContext.ModelState["TheModelName.Year"]?.AttemptedValue);
            Assert.AreEqual("4", bindingContext.ModelState["TheModelName.Month"]?.AttemptedValue);
            Assert.AreEqual("1", bindingContext.ModelState["TheModelName.Day"]?.AttemptedValue);

            Assert.AreEqual(0, bindingContext.ModelState.ErrorCount);
        }

        [TestCase("", "4", "2020")]
        [TestCase("1", "", "2020")]
        [TestCase("1", "4", "")]
        [TestCase("0", "4", "2020")]
        [TestCase("-1", "4", "2020")]
        [TestCase("32", "4", "2020")]
        [TestCase("1", "0", "2020")]
        [TestCase("1", "-1", "2020")]
        [TestCase("1", "13", "2020")]
        [TestCase("1", "4", "0")]
        [TestCase("1", "4", "-1")]
        [TestCase("1", "4", "10000")]
        [TestCase("x", "y", "z")]
        public async Task BindModelAsync_MissingOrInvalidComponents_FailsBinding(string day, string month, string year)
        {
            // Arrange
            var modelType = typeof(DateOnly);

            var valueProvider = new SimpleValueProvider();

            if (day != null)
            {
                valueProvider.Add("TheModelName.Day", day);
            }

            if (month != null)
            {
                valueProvider.Add("TheModelName.Month", month);
            }

            if (year != null)
            {
                valueProvider.Add("TheModelName.Year", year);
            }

            ModelBindingContext bindingContext = new DefaultModelBindingContext()
            {
                ActionContext = CreateActionContext(),
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType),
                ModelName = "TheModelName",
                ModelState = new ModelStateDictionary(),
                ValueProvider = valueProvider
            };

            var converterMock = new Mock<DateInputModelConverter>();
            converterMock.Setup(mock => mock.CanConvertModelType(modelType)).Returns(true);

            var modelBinder = CreateModelBinder(converterMock);

            // Act
            await modelBinder.BindModelAsync(bindingContext);

            // Assert
            Assert.AreEqual(ModelBindingResult.Failed(), bindingContext.Result);

            Assert.AreEqual(day, bindingContext.ModelState["TheModelName.Day"]?.AttemptedValue);
            Assert.AreEqual(month, bindingContext.ModelState["TheModelName.Month"]?.AttemptedValue);
            Assert.AreEqual(year, bindingContext.ModelState["TheModelName.Year"]?.AttemptedValue);
        }

        [Test]
        public async Task BindModelAsync_MissingOrInvalidComponentsAndConverterCanCreateModelFromErrors_PassesValuesToConverterAndBindsResult()
        {
            // Arrange
            var modelType = typeof(CustomDateType);

            var day = "1";
            var month = "4";
            var year = "-1";

            ModelBindingContext bindingContext = new DefaultModelBindingContext()
            {
                ActionContext = CreateActionContext(),
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType),
                ModelName = "TheModelName",
                ModelState = new ModelStateDictionary(),
                ValueProvider = new SimpleValueProvider()
                {
                    { "TheModelName.Day", day },
                    { "TheModelName.Month", month },
                    { "TheModelName.Year", year }
                }
            };

            var parseErrors = DateInputParseErrors.InvalidYear;
            object? modelFromErrors = new CustomDateType() { ParseErrors = parseErrors };

            var converterMock = new Mock<DateInputModelConverter>();
            converterMock.Setup(mock => mock.CanConvertModelType(modelType)).Returns(true);

            converterMock
                .Setup(mock => mock.TryCreateModelFromErrors(modelType, parseErrors, out modelFromErrors))
                .Returns(true)
                .Verifiable();

            var modelBinder = CreateModelBinder(converterMock);

            // Act
            await modelBinder.BindModelAsync(bindingContext);

            // Assert
            converterMock.Verify();

            Assert.True(bindingContext.Result.IsModelSet);

            Assert.AreSame(modelFromErrors, bindingContext.Result.Model);

            Assert.AreEqual(day, bindingContext.ModelState["TheModelName.Day"]?.AttemptedValue);
            Assert.AreEqual(month, bindingContext.ModelState["TheModelName.Month"]?.AttemptedValue);
            Assert.AreEqual(year, bindingContext.ModelState["TheModelName.Year"]?.AttemptedValue);

            Assert.AreEqual(0, bindingContext.ModelState.ErrorCount);
        }

        [TestCase(DateInputParseErrors.MissingYear, $"{nameof(ExampleModel.DateProperty)} must include a year")]
        [TestCase(DateInputParseErrors.InvalidYear, $"{nameof(ExampleModel.DateProperty)} must be a real date")]
        [TestCase(DateInputParseErrors.MissingMonth, $"{nameof(ExampleModel.DateProperty)} must include a month")]
        [TestCase(DateInputParseErrors.InvalidMonth, $"{nameof(ExampleModel.DateProperty)} must be a real date")]
        [TestCase(DateInputParseErrors.InvalidDay, $"{nameof(ExampleModel.DateProperty)} must be a real date")]
        [TestCase(DateInputParseErrors.MissingDay, $"{nameof(ExampleModel.DateProperty)} must include a day")]
        [TestCase(DateInputParseErrors.MissingYear | DateInputParseErrors.MissingMonth, $"{nameof(ExampleModel.DateProperty)} must include a month and year")]
        [TestCase(DateInputParseErrors.MissingYear | DateInputParseErrors.MissingDay, $"{nameof(ExampleModel.DateProperty)} must include a day and year")]
        [TestCase(DateInputParseErrors.MissingMonth | DateInputParseErrors.MissingDay, $"{nameof(ExampleModel.DateProperty)} must include a day and month")]
        [TestCase(DateInputParseErrors.InvalidYear | DateInputParseErrors.InvalidMonth, $"{nameof(ExampleModel.DateProperty)} must be a real date")]
        [TestCase(DateInputParseErrors.InvalidYear | DateInputParseErrors.InvalidMonth | DateInputParseErrors.InvalidDay, $"{nameof(ExampleModel.DateProperty)} must be a real date")]
        [TestCase(DateInputParseErrors.InvalidMonth | DateInputParseErrors.InvalidDay, $"{nameof(ExampleModel.DateProperty)} must be a real date")]
        public void GetModelStateErrorMessage(DateInputParseErrors parseErrors, string expectedMessage)
        {
            // Arrange
            var modelMetadata = new ModelMetadataForProperty(typeof(ExampleModel).GetProperty(nameof(ExampleModel.DateProperty))!);

            // Act
            var result = UmbracoDateInputModelBinder.GetModelStateErrorMessage(Mock.Of<IOverridablePublishedElement>(), Mock.Of<ICultureDictionary>(), parseErrors, modelMetadata);

            // Assert
            Assert.AreEqual(expectedMessage, result);
        }

        [TestCase(false, null, "3", 3, "2020")]
        [TestCase(true, "1", "1", 1, "2020")]
        [TestCase(true, "29", "2", 2, "2020")]
        [TestCase(true, "31", "12", 12, "2020")]
        [TestCase(true, "31", "dec", 12, "2020")]
        [TestCase(true, "31", "January", 1, "2020")]
        public void Parse_ValidDate_Returns_Date(bool dayEnabled, string? day, string month, int expectedMonth, string year)
        {
            // Arrange

            // Act
            var result = UmbracoDateInputModelBinder.Parse(dayEnabled, day, month, year, true, out var parsed);

            // Assert
            Assert.That(result, Is.EqualTo(DateInputParseErrors.None));

            var expectedDay = dayEnabled ? int.Parse(day!) : 1;
            Assert.That(new DateOnly(int.Parse(year), expectedMonth, expectedDay), Is.EqualTo(parsed));
        }

        [TestCase("", "4", "2020", false, DateInputParseErrors.MissingDay)]
        [TestCase(null, "4", "2020", false, DateInputParseErrors.MissingDay)]
        [TestCase("1", "", "2020", false, DateInputParseErrors.MissingMonth)]
        [TestCase("1", null, "2020", false, DateInputParseErrors.MissingMonth)]
        [TestCase("1", "4", null, false, DateInputParseErrors.MissingYear)]
        [TestCase("1", "4", "", false, DateInputParseErrors.MissingYear)]
        [TestCase("0", "4", "2020", false, DateInputParseErrors.InvalidDay)]
        [TestCase("-1", "4", "2020", false, DateInputParseErrors.InvalidDay)]
        [TestCase("32", "4", "2020", false, DateInputParseErrors.InvalidDay)]
        [TestCase("x", "4", "2020", false, DateInputParseErrors.InvalidDay)]
        [TestCase("1", "0", "2020", false, DateInputParseErrors.InvalidMonth)]
        [TestCase("1", "-1", "2020", false, DateInputParseErrors.InvalidMonth)]
        [TestCase("1", "13", "2020", false, DateInputParseErrors.InvalidMonth)]
        [TestCase("1", "x", "2020", false, DateInputParseErrors.InvalidMonth)]
        [TestCase("1", "4", "0", false, DateInputParseErrors.InvalidYear)]
        [TestCase("1", "4", "-1", false, DateInputParseErrors.InvalidYear)]
        [TestCase("1", "4", "10000", false, DateInputParseErrors.InvalidYear)]
        [TestCase("1", "4", "x", false, DateInputParseErrors.InvalidYear)]
        [TestCase("1", "x", "2020", true, DateInputParseErrors.InvalidMonth)]
        [TestCase("1", "dec", "2020", false, DateInputParseErrors.InvalidMonth)]
        [TestCase("31", "January", "2020", false, DateInputParseErrors.InvalidMonth)]
        public void Parse_InvalidDate_ComputesExpectedParseErrors(
            string day, string month, string year, bool acceptMonthNames, DateInputParseErrors expectedParseErrors)
        {
            // Arrange

            // Act
            var result = UmbracoDateInputModelBinder.Parse(true, day, month, year, acceptMonthNames, out var dateComponents);

            // Assert
            Assert.AreEqual(default, dateComponents);
            Assert.AreEqual(expectedParseErrors, result);
        }

        private ActionContext CreateActionContext() => new ActionContext(_testContext.HttpContext.Object, new RouteData(), new ActionDescriptor());

        private class ExampleModel
        {
            public DateOnly DateProperty { get; set; }
        }

        private class ModelMetadataForProperty : ModelMetadata
        {
            public ModelMetadataForProperty(PropertyInfo propertyInfo)
                : base(ModelMetadataIdentity.ForProperty(propertyInfo, propertyInfo.PropertyType, propertyInfo.DeclaringType!))
            {
            }

            public override IReadOnlyDictionary<object, object> AdditionalValues => throw new NotImplementedException();

            public override ModelPropertyCollection Properties => throw new NotImplementedException();

            public override string BinderModelName => throw new NotImplementedException();

            public override Type BinderType => throw new NotImplementedException();

            public override BindingSource BindingSource => throw new NotImplementedException();

            public override bool ConvertEmptyStringToNull => throw new NotImplementedException();

            public override string DataTypeName => throw new NotImplementedException();

            public override string Description => throw new NotImplementedException();

            public override string DisplayFormatString => throw new NotImplementedException();

            public override string DisplayName => throw new NotImplementedException();

            public override string EditFormatString => throw new NotImplementedException();

            public override ModelMetadata ElementMetadata => throw new NotImplementedException();

            public override IEnumerable<KeyValuePair<EnumGroupAndName, string>> EnumGroupedDisplayNamesAndValues => throw new NotImplementedException();

            public override IReadOnlyDictionary<string, string> EnumNamesAndValues => throw new NotImplementedException();

            public override bool HasNonDefaultEditFormat => throw new NotImplementedException();

            public override bool HtmlEncode => throw new NotImplementedException();

            public override bool HideSurroundingHtml => throw new NotImplementedException();

            public override bool IsBindingAllowed => throw new NotImplementedException();

            public override bool IsBindingRequired => throw new NotImplementedException();

            public override bool IsEnum => throw new NotImplementedException();

            public override bool IsFlagsEnum => throw new NotImplementedException();

            public override bool IsReadOnly => throw new NotImplementedException();

            public override bool IsRequired => throw new NotImplementedException();

            public override ModelBindingMessageProvider ModelBindingMessageProvider => throw new NotImplementedException();

            public override int Order => throw new NotImplementedException();

            public override string Placeholder => throw new NotImplementedException();

            public override string NullDisplayText => throw new NotImplementedException();

            public override IPropertyFilterProvider PropertyFilterProvider => throw new NotImplementedException();

            public override bool ShowForDisplay => throw new NotImplementedException();

            public override bool ShowForEdit => throw new NotImplementedException();

            public override string SimpleDisplayProperty => throw new NotImplementedException();

            public override string TemplateHint => throw new NotImplementedException();

            public override bool ValidateChildren => throw new NotImplementedException();

            public override IReadOnlyList<object> ValidatorMetadata => throw new NotImplementedException();

            public override Func<object, object> PropertyGetter => throw new NotImplementedException();

            public override Action<object, object?> PropertySetter => throw new NotImplementedException();
        }

        private class CustomDateType
        {
            public DateInputParseErrors ParseErrors { get; set; }
        }
    }
}
