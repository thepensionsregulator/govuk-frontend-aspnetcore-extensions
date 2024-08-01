using GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators.FileTypes;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators
{
    public class FileValidatorCollection
    {
        private readonly List<IFileTypeValidator> _validators;

        public FileValidatorCollection()
        {
            _validators = new List<IFileTypeValidator>()
            {
                new Excel(),
                new Pdf(),
            };
        }

        public IEnumerable<IFileTypeValidator> GetValidators()
        {
            return _validators;
        }
    }
}