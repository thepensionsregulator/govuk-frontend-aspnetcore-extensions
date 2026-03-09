using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System.Reflection;
using ThePensionsRegulator.GovUk.Frontend.Validation;

namespace ThePensionsRegulator.GovUk.Frontend.UnitTests.Validation
{
    public class ModelPropertyResolverCollectionTests
    {
        private class TestModel
        {
            public string? TestProperty { get; set; }
            public int AnotherProperty { get; set; }
        }

        private class AnotherModel
        {
            public string? DifferentProperty { get; set; }
        }

        [Fact]
        public void Constructor_adds_all_resolvers_to_collection()
        {
            // Arrange
            var resolver1 = Mock.Of<ModelPropertyResolverBase>();
            var resolver2 = Mock.Of<ModelPropertyResolverBase>();
            var resolver3 = Mock.Of<ModelPropertyResolverBase>();

            // Act
            var collection = new ModelPropertyResolverCollection([resolver1, resolver2, resolver3]);

            // Assert
            Assert.Equal(3, collection.Count);
            Assert.Contains(resolver1, collection);
            Assert.Contains(resolver2, collection);
            Assert.Contains(resolver3, collection);
        }

        [Fact]
        public void Constructor_creates_empty_collection_when_no_resolvers_provided()
        {
            // Arrange
            var resolvers = Enumerable.Empty<ModelPropertyResolverBase>();

            // Act
            var collection = new ModelPropertyResolverCollection(resolvers);

            // Assert
            Assert.Empty(collection);
        }

        [Fact]
        public void ResolveModelProperty_returns_property_info_when_resolver_finds_model_type_and_property()
        {
            // Arrange
            var viewContext = new ViewContext();
            var propertyName = nameof(TestModel.TestProperty);
            var expectedProperty = typeof(TestModel).GetProperty(propertyName)!;

            var mockResolver = new Mock<ModelPropertyResolverBase>();
            mockResolver.Setup(r => r.ResolveModelType(viewContext)).Returns(typeof(TestModel));
            mockResolver.Setup(r => r.ResolveModelProperty(typeof(TestModel), propertyName)).Returns(expectedProperty);

            var collection = new ModelPropertyResolverCollection([mockResolver.Object]);

            // Act
            var result = collection.ResolveModelProperty(viewContext, propertyName);

            // Assert
            Assert.Equal(expectedProperty, result);
        }

        [Fact]
        public void ResolveModelProperty_uses_first_resolver_that_can_resolve_model_type()
        {
            // Arrange
            var viewContext = new ViewContext();
            var propertyName = nameof(TestModel.TestProperty);
            var expectedProperty = typeof(TestModel).GetProperty(propertyName)!;

            var mockResolver1 = new Mock<ModelPropertyResolverBase>();
            mockResolver1.Setup(r => r.ResolveModelType(viewContext)).Returns((Type?)null);

            var mockResolver2 = new Mock<ModelPropertyResolverBase>();
            mockResolver2.Setup(r => r.ResolveModelType(viewContext)).Returns(typeof(TestModel));
            mockResolver2.Setup(r => r.ResolveModelProperty(typeof(TestModel), propertyName)).Returns(expectedProperty);

            var mockResolver3 = new Mock<ModelPropertyResolverBase>();
            mockResolver3.Setup(r => r.ResolveModelType(viewContext)).Returns(typeof(AnotherModel));

            var collection = new ModelPropertyResolverCollection([mockResolver1.Object, mockResolver2.Object, mockResolver3.Object]);

            // Act
            var result = collection.ResolveModelProperty(viewContext, propertyName);

            // Assert
            Assert.Equal(expectedProperty, result);
            mockResolver1.Verify(r => r.ResolveModelType(viewContext), Times.Once);
            mockResolver2.Verify(r => r.ResolveModelType(viewContext), Times.Once);
            mockResolver2.Verify(r => r.ResolveModelProperty(typeof(TestModel), propertyName), Times.Once);
            // Third resolver should not be called since the second one succeeded
            mockResolver3.Verify(r => r.ResolveModelType(viewContext), Times.Never);
        }

        [Fact]
        public void ResolveModelProperty_throws_when_no_resolver_can_detect_model_type()
        {
            // Arrange
            var viewContext = new ViewContext();
            var propertyName = nameof(TestModel.TestProperty);

            var mockResolver1 = new Mock<ModelPropertyResolverBase>();
            mockResolver1.Setup(r => r.ResolveModelType(viewContext)).Returns((Type?)null);

            var mockResolver2 = new Mock<ModelPropertyResolverBase>();
            mockResolver2.Setup(r => r.ResolveModelType(viewContext)).Returns((Type?)null);

            var collection = new ModelPropertyResolverCollection([mockResolver1.Object, mockResolver2.Object]);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => collection.ResolveModelProperty(viewContext, propertyName));
        }

        [Fact]
        public void ResolveModelProperty_throws_when_property_not_found_on_model_type()
        {
            // Arrange
            var viewContext = new ViewContext();
            var propertyName = "NonExistentProperty";

            var mockResolver = new Mock<ModelPropertyResolverBase>();
            mockResolver.Setup(r => r.ResolveModelType(viewContext)).Returns(typeof(TestModel));
            mockResolver.Setup(r => r.ResolveModelProperty(typeof(TestModel), propertyName)).Returns((PropertyInfo?)null);

            var collection = new ModelPropertyResolverCollection([mockResolver.Object]);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => collection.ResolveModelProperty(viewContext, propertyName));
        }

        [Fact]
        public void ResolveModelProperty_processes_resolvers_in_Order_property_sequence_not_collection_position()
        {
            // Arrange
            var viewContext = new ViewContext();
            var propertyName = nameof(TestModel.TestProperty);
            var expectedProperty = typeof(TestModel).GetProperty(propertyName)!;
            var incorrectProperty = typeof(TestModel).GetProperty(nameof(TestModel.AnotherProperty))!;

            // Create resolver with Order = 20 (should be processed second)
            var mockResolver1 = new Mock<ModelPropertyResolverBase>();
            mockResolver1.Setup(r => r.Order).Returns(20);
            mockResolver1.Setup(r => r.ResolveModelType(viewContext)).Returns(typeof(TestModel));
            mockResolver1.Setup(r => r.ResolveModelProperty(typeof(TestModel), propertyName)).Returns(incorrectProperty);

            // Create resolver with Order = 10 (should be processed first)
            var mockResolver2 = new Mock<ModelPropertyResolverBase>();
            mockResolver2.Setup(r => r.Order).Returns(10);
            mockResolver2.Setup(r => r.ResolveModelType(viewContext)).Returns(typeof(TestModel));
            mockResolver2.Setup(r => r.ResolveModelProperty(typeof(TestModel), propertyName)).Returns(expectedProperty);

            // Add resolvers in intentionally wrong order (20, 10, 30) to verify they're sorted by Order property
            var collection = new ModelPropertyResolverCollection([mockResolver1.Object, mockResolver2.Object]);

            // Act
            var result = collection.ResolveModelProperty(viewContext, propertyName);

            // Assert
            Assert.Equal(expectedProperty, result);

            // Verify resolver with Order=10 was called first (even though added second)
            mockResolver2.Verify(r => r.ResolveModelType(viewContext), Times.Once);

            // Verify resolver with Order=20 was never called (even though added first)
            mockResolver1.Verify(r => r.ResolveModelType(viewContext), Times.Never);
            mockResolver1.Verify(r => r.ResolveModelProperty(typeof(TestModel), propertyName), Times.Never);
        }
    }
}
