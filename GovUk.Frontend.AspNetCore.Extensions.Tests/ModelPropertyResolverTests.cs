using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NUnit.Framework;
using System;

namespace GovUk.Frontend.AspNetCore.Extensions.Tests
{
    public partial class ModelPropertyResolverTests
    {
        private class DefaultModel
        {
            public string DefaultModelProperty { get; set; }
        };

        private class ModelFromModelType
        {
            public string ModelFromModelTypeProperty { get; set; }
        };

        private class ExampleControllerWithDefaultModel
        {
            public IActionResult Index() => null;
        }

        private class ExampleControllerWithModelType
        {
            [ModelType(typeof(ModelFromModelType))]
            public IActionResult Index() => null;
        }

        [Test]
        public void No_model_type_throws_InvalidOperationException()
        {
            var modelPropertyResolver = new ModelPropertyResolver();
            var viewContext = new ViewContext
            {
                ActionDescriptor = new ControllerActionDescriptor
                {
                    MethodInfo = typeof(ExampleControllerWithDefaultModel).GetMethod("Index"),
                },
                ViewData = null
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
            var metadataProvider = new FakeMetadataProvider(typeof(DefaultModel));
            viewContext.ActionDescriptor = new ControllerActionDescriptor
            {
                MethodInfo = typeof(ExampleControllerWithDefaultModel).GetMethod("Index"),
            };
            viewContext.ViewData = new ViewDataDictionary(metadataProvider, new ModelStateDictionary());

            var modelType = modelPropertyResolver.ResolveModelType(viewContext);

            Assert.AreEqual(modelType, typeof(DefaultModel));
        }

        [Test]
        public void Model_type_is_resolved_using_ModelType_attribute_before_default_model()
        {
            var modelPropertyResolver = new ModelPropertyResolver();
            var viewContext = new ViewContext();
            var metadataProvider = new FakeMetadataProvider(typeof(DefaultModel));
            viewContext.ActionDescriptor = new ControllerActionDescriptor
            {
                MethodInfo = typeof(ExampleControllerWithModelType).GetMethod("Index")
            };
            viewContext.ViewData = new ViewDataDictionary(metadataProvider, new ModelStateDictionary());

            var modelType = modelPropertyResolver.ResolveModelType(viewContext);

            Assert.AreEqual(modelType, typeof(ModelFromModelType));

        }

        [Test]
        public void Property_is_resolved_model_type()
        {
            var modelPropertyResolver = new ModelPropertyResolver();

            var property = modelPropertyResolver.ResolveModelProperty(typeof(DefaultModel), nameof(DefaultModel.DefaultModelProperty));

            Assert.AreEqual(property.DeclaringType, typeof(DefaultModel));
            Assert.AreEqual(property.Name, nameof(DefaultModel.DefaultModelProperty));
        }

    }
}