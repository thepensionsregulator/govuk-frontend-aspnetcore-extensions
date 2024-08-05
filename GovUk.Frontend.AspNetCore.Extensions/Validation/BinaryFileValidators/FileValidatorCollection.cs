using GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators.FileTypes;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators
{
    public static class FileValidatorCollection
    {
        public static IEnumerable<IFileTypeValidator> GetValidators()
        {
            return new List<IFileTypeValidator>()
            {
                new Excel(),
                new Pdf(),
            };
        }
    }
}