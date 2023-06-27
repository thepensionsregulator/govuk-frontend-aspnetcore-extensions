using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Extensions.Tests
{
    public partial class ModelPropertyResolverTests
    {
        private class DefaultModel
        {
            public string? DefaultModelProperty { get; set; }
            public DateTime DateTimeProperty { get; set; }
        };

        private class ModelFromModelType
        {
            public string? ModelFromModelTypeProperty { get; set; }
        };

        private class ExampleControllerWithDefaultModel
        {
            public IActionResult? Index() => null;
        }

        private class ExampleControllerWithModelType
        {
            [ModelType(typeof(ModelFromModelType))]
            public IActionResult? Index() => null;
        }

        private class ChildModel
        {
            public string? ChildModelProperty { get; set; }
        }

        private class ParentModel
        {
            public string? ParentModelProperty { get; set; }
            public ChildModel? Child { get; set; }
        }

        private class IterativeModel
        {
            public string? Field { get; set; }
            public IList<ChildModel>? List { get; set; }

            public string[] Array { get; set; } = System.Array.Empty<string>();
        }


        [Test]
        public void No_model_type_throws_InvalidOperationException()
        {
            var modelPropertyResolver = new ModelPropertyResolver();
            var viewContext = new ViewContext
            {
                ActionDescriptor = new ControllerActionDescriptor
                {
                    MethodInfo = typeof(ExampleControllerWithDefaultModel).GetMethod("Index")!,
                },
#nullable disable
                ViewData = null
#nullable enable
            };

            Assert.Throws<InvalidOperationException>(() => modelPropertyResolver.ResolveModelType(viewContext));
        }

        [Test]
        public void Invalid_property_name_throws_InvalidOperationException()
        {
            var modelPropertyResolver = new ModelPropertyResolver();

            Assert.Throws<InvalidOperationException>(() => modelPropertyResolver.ResolveModelProperty(typeof(DefaultModel), "InvalidProperty"));
        }

        [Test]
        public void Model_type_is_resolved_from_default_model()
        {
            var modelPropertyResolver = new ModelPropertyResolver();
            var viewContext = new ViewContext();
            viewContext.ActionDescriptor = new ControllerActionDescriptor
            {
                MethodInfo = typeof(ExampleControllerWithDefaultModel).GetMethod("Index")!,
            };
            viewContext.ViewData = CreateViewData();

            var modelType = modelPropertyResolver.ResolveModelType(viewContext);

            Assert.AreEqual(modelType, typeof(DefaultModel));
        }

        private static ViewDataDictionary CreateViewData()
        {
            var metadataProvider = new FakeMetadataProvider(typeof(DefaultModel));
            return new ViewDataDictionary(metadataProvider, new ModelStateDictionary());
        }

        [Test]
        public void Model_type_is_resolved_using_ModelType_attribute_before_default_model()
        {
            var modelPropertyResolver = new ModelPropertyResolver();
            var viewContext = new ViewContext();
            viewContext.ActionDescriptor = new ControllerActionDescriptor
            {
                MethodInfo = typeof(ExampleControllerWithModelType).GetMethod("Index")!
            };
            viewContext.ViewData = CreateViewData();

            var modelType = modelPropertyResolver.ResolveModelType(viewContext);

            Assert.AreEqual(modelType, typeof(ModelFromModelType));

        }

        [Test]
        public void Property_is_resolved_from_model_type()
        {
            var modelPropertyResolver = new ModelPropertyResolver();

            var property = modelPropertyResolver.ResolveModelProperty(typeof(DefaultModel), nameof(DefaultModel.DefaultModelProperty));

            Assert.AreEqual(property.DeclaringType, typeof(DefaultModel));
            Assert.AreEqual(property.Name, nameof(DefaultModel.DefaultModelProperty));
        }


        [Test]
        public void Date_property_is_resolved_from_date_internal_fields()
        {
            var modelPropertyResolver = new ModelPropertyResolver();

            var propertyFromDayField = modelPropertyResolver.ResolveModelProperty(typeof(DefaultModel), nameof(DefaultModel.DateTimeProperty.Day));
            var propertyFromMonthField = modelPropertyResolver.ResolveModelProperty(typeof(DefaultModel), nameof(DefaultModel.DateTimeProperty.Month));
            var propertyFromYearField = modelPropertyResolver.ResolveModelProperty(typeof(DefaultModel), nameof(DefaultModel.DateTimeProperty.Year));

            Assert.AreEqual(propertyFromDayField.DeclaringType, typeof(DateTime));
            Assert.AreEqual(propertyFromDayField.Name, nameof(DefaultModel.DateTimeProperty.Day));
            Assert.AreEqual(propertyFromMonthField.DeclaringType, typeof(DateTime));
            Assert.AreEqual(propertyFromMonthField.Name, nameof(DefaultModel.DateTimeProperty.Month));
            Assert.AreEqual(propertyFromYearField.DeclaringType, typeof(DateTime));
            Assert.AreEqual(propertyFromYearField.Name, nameof(DefaultModel.DateTimeProperty.Year));
        }

        [Test]
        public void ChildProperty_is_resolved_from_Parent_Model()
        {
            var modelPropertyResolver = new ModelPropertyResolver();

            var childProperty = modelPropertyResolver.ResolveModelProperty(typeof(ParentModel), nameof(ParentModel.Child.ChildModelProperty));
            Assert.AreEqual(childProperty.DeclaringType, typeof(ChildModel));
            Assert.AreEqual(childProperty.Name, nameof(ParentModel.Child.ChildModelProperty));


        }


        [Test]
        public void ChildProperty_String_is_resolved_from_Parent_Model()
        {
            var modelPropertyResolver = new ModelPropertyResolver();

            var childProperty = modelPropertyResolver.ResolveModelProperty(typeof(ParentModel), "Child.ChildModelProperty");
            Assert.AreEqual(childProperty.DeclaringType, typeof(ChildModel));
            Assert.AreEqual(childProperty.Name, nameof(ParentModel.Child.ChildModelProperty));
        }

        [Test]
        public void ChildProperty_is_resolved_from_Iterative_Model()
        {
            var modelPropertyResolver = new ModelPropertyResolver();

            var childProperty = modelPropertyResolver.ResolveModelProperty(typeof(IterativeModel), "List[0].ChildModelProperty");
            Assert.AreEqual(childProperty.DeclaringType, typeof(ChildModel));

            var childType = typeof(IterativeModel).GetProperty("List")!.PropertyType.GenericTypeArguments[0];
            Assert.AreEqual(typeof(ChildModel), childType);
            Assert.AreEqual(childProperty.Name, nameof(ChildModel.ChildModelProperty));
        }

        [Test]
        public void ArrayProperty_is_resolved_from_Iterative_Model()
        {
            var modelPropertyResolver = new ModelPropertyResolver();

            var childProperty = modelPropertyResolver.ResolveModelProperty(typeof(IterativeModel), "Array[0]");
            Assert.AreEqual(childProperty.PropertyType, typeof(IterativeModel).GetProperty("Array")!.PropertyType);
            Assert.AreEqual(childProperty.Name, nameof(IterativeModel.Array));
        }

    }
}