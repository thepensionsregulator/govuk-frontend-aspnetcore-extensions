using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Extensions.UnitTests
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

        private class NestedModelLevel1
        {
            public string? Field { get; set; }
            public Level2? Child { get; set; }
        }

        private class Level2
        {
            public string? Level2Field { get; set; }
            public Level3? Child { get; set; }
        }

        private class Level3
        {
            public Level4? Child { get; set; }
            public string? Level3Field { get; set; }
        }
        private class Level4
        {
            public Level5? Child { get; set; }
            public string? Level4Field { get; set; }
        }
        private class Level5
        {
            public string? Level5Field { get; set; }
        }

        [Fact]
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

        [Fact]
        public void Invalid_property_name_throws_InvalidOperationException()
        {
            var modelPropertyResolver = new ModelPropertyResolver();

            Assert.Throws<InvalidOperationException>(() => modelPropertyResolver.ResolveModelProperty(typeof(DefaultModel), "InvalidProperty"));
        }

        [Fact]
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

            Assert.Equal(modelType, typeof(DefaultModel));
        }

        private static ViewDataDictionary CreateViewData()
        {
            var metadataProvider = new FakeMetadataProvider(typeof(DefaultModel));
            return new ViewDataDictionary(metadataProvider, new ModelStateDictionary());
        }

        [Fact]
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

            Assert.Equal(modelType, typeof(ModelFromModelType));

        }

        [Fact]
        public void Property_is_resolved_from_model_type()
        {
            var modelPropertyResolver = new ModelPropertyResolver();

            var property = modelPropertyResolver.ResolveModelProperty(typeof(DefaultModel), nameof(DefaultModel.DefaultModelProperty));

            Assert.Equal(property.DeclaringType, typeof(DefaultModel));
            Assert.Equal(property.Name, nameof(DefaultModel.DefaultModelProperty));
        }


        [Fact]
        public void Date_property_is_resolved_from_date_internal_fields()
        {
            var modelPropertyResolver = new ModelPropertyResolver();

            var propertyFromDayField = modelPropertyResolver.ResolveModelProperty(typeof(DefaultModel), nameof(DefaultModel.DateTimeProperty.Day));
            var propertyFromMonthField = modelPropertyResolver.ResolveModelProperty(typeof(DefaultModel), nameof(DefaultModel.DateTimeProperty.Month));
            var propertyFromYearField = modelPropertyResolver.ResolveModelProperty(typeof(DefaultModel), nameof(DefaultModel.DateTimeProperty.Year));

            Assert.Equal(propertyFromDayField.DeclaringType, typeof(DateTime));
            Assert.Equal(propertyFromDayField.Name, nameof(DefaultModel.DateTimeProperty.Day));
            Assert.Equal(propertyFromMonthField.DeclaringType, typeof(DateTime));
            Assert.Equal(propertyFromMonthField.Name, nameof(DefaultModel.DateTimeProperty.Month));
            Assert.Equal(propertyFromYearField.DeclaringType, typeof(DateTime));
            Assert.Equal(propertyFromYearField.Name, nameof(DefaultModel.DateTimeProperty.Year));
        }

        [Fact]
        public void ChildProperty_is_resolved_from_Parent_Model()
        {
            var modelPropertyResolver = new ModelPropertyResolver();

            var childProperty = modelPropertyResolver.ResolveModelProperty(typeof(ParentModel), nameof(ParentModel.Child.ChildModelProperty));
            Assert.Equal(childProperty.DeclaringType, typeof(ChildModel));
            Assert.Equal(childProperty.Name, nameof(ParentModel.Child.ChildModelProperty));


        }


        [Fact]
        public void ChildProperty_String_is_resolved_from_Parent_Model()
        {
            var modelPropertyResolver = new ModelPropertyResolver();

            var childProperty = modelPropertyResolver.ResolveModelProperty(typeof(ParentModel), "Child.ChildModelProperty");
            Assert.Equal(childProperty.DeclaringType, typeof(ChildModel));
            Assert.Equal(childProperty.Name, nameof(ParentModel.Child.ChildModelProperty));
        }

        [Fact]
        public void ChildProperty_is_resolved_from_Iterative_Model()
        {
            var modelPropertyResolver = new ModelPropertyResolver();

            var childProperty = modelPropertyResolver.ResolveModelProperty(typeof(IterativeModel), "List[0].ChildModelProperty");
            Assert.Equal(childProperty.DeclaringType, typeof(ChildModel));

            var childType = typeof(IterativeModel).GetProperty("List")!.PropertyType.GenericTypeArguments[0];
            Assert.Equal(typeof(ChildModel), childType);
            Assert.Equal(childProperty.Name, nameof(ChildModel.ChildModelProperty));
        }

        [Fact]
        public void ArrayProperty_is_resolved_from_Iterative_Model()
        {
            var modelPropertyResolver = new ModelPropertyResolver();

            var childProperty = modelPropertyResolver.ResolveModelProperty(typeof(IterativeModel), "Array[0]");
            Assert.Equal(childProperty.PropertyType, typeof(IterativeModel).GetProperty("Array")!.PropertyType);
            Assert.Equal(childProperty.Name, nameof(IterativeModel.Array));
        }

        [Fact]
        public void Nested_property_throws_InvalidOperationException_when_nested_5_levels()
        {
            var modelPropertyResolver = new ModelPropertyResolver();
            Assert.Throws<InvalidOperationException>(() => modelPropertyResolver.ResolveModelProperty(typeof(NestedModelLevel1), nameof(Level5.Level5Field)));
        }

        [Theory]
        [InlineData(typeof(NestedModelLevel1), nameof(NestedModelLevel1.Field))]
        [InlineData(typeof(Level2), nameof(Level2.Level2Field))]
        [InlineData(typeof(Level3), nameof(Level3.Level3Field))]
        [InlineData(typeof(Level4), nameof(Level4.Level4Field))]
        public void Nested_property_is_resolved_when_nested_less_than_5_levels(Type t, string field)
        {
            var modelPropertyResolver = new ModelPropertyResolver();

            var childProperty = modelPropertyResolver.ResolveModelProperty(typeof(NestedModelLevel1), field);
            Assert.Equal(childProperty.DeclaringType, t);
            Assert.Equal(childProperty.Name, field);

        }
    }
}