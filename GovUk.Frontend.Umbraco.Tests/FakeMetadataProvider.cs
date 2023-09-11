using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Moq;
using System;
using System.Collections.Generic;

namespace GovUk.Frontend.Umbraco.Tests
{
    public class FakeMetadataProvider : IModelMetadataProvider
    {
        private readonly Type _modelType;

        public FakeMetadataProvider(Type modelType)
        {
            _modelType = modelType ?? throw new ArgumentNullException(nameof(modelType));
        }

        public IEnumerable<ModelMetadata> GetMetadataForProperties(Type modelType) => throw new NotImplementedException();

        public ModelMetadata GetMetadataForType(Type modelType)
        {
            var attributes = ModelAttributes.GetAttributesForType(_modelType);
            var identity = ModelMetadataIdentity.ForType(_modelType);
            return new DefaultModelMetadata(this, Mock.Of<ICompositeMetadataDetailsProvider>(), new DefaultMetadataDetails(identity, attributes));
        }
    }
}