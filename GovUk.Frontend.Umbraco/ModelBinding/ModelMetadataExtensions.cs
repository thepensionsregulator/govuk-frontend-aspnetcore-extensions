using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics.CodeAnalysis;

namespace GovUk.Frontend.Umbraco.ModelBinding
{
    internal static class ModelMetadataExtensions
    {
        public static bool TryGetDateInputModelMetadata(this ModelMetadata modelMetadata, [NotNullWhen(true)] out DateInputModelMetadata? dateInputModelMetadata)
        {
            ArgumentNullException.ThrowIfNull(modelMetadata);

            if (modelMetadata.AdditionalValues.TryGetValue(typeof(DateInputModelMetadata), out var metadataObj))
            {
                dateInputModelMetadata = (DateInputModelMetadata)metadataObj;
                return true;
            }

            dateInputModelMetadata = null;
            return false;
        }
    }
}
