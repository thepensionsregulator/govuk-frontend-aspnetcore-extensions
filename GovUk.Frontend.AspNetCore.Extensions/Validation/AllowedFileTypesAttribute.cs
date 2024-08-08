using GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation
{
    public class AllowedFileTypesAttribute : ValidationAttribute
    {
        private readonly IList<IFileTypeValidator> _fileTypeValidators;

        public AllowedFileTypesAttribute(IEnumerable<Type> validTypes)
            : this(FileValidatorCollection.GetValidators(), validTypes)
        { }

        public AllowedFileTypesAttribute(IEnumerable<IFileTypeValidator> validators, IEnumerable<Type> validTypes)
        {
            _fileTypeValidators = new List<IFileTypeValidator>();

            foreach (var t in validTypes)
            {
                var validator = validators.Where(x => x.GetType() == t).SingleOrDefault();
                if (validator is null)
                {
                    throw new ArgumentException($"{t} is not defined");
                }
                _fileTypeValidators.Add(validator);
            }
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success!;
            }

            if (value is not IFormFile file)
            {
                throw new InvalidOperationException($"Target property for {nameof(AllowedFileTypesAttribute)} must be {nameof(IFormFile)}");
            }
            var ext = Path.GetExtension(file.FileName);

            var extensionsMatch = true;
            foreach (var v in _fileTypeValidators)
            {
                extensionsMatch = v.Extensions.Any(x => x.Equals(ext, StringComparison.InvariantCultureIgnoreCase));
            }

            if (!extensionsMatch)
            {
                return new ValidationResult(ErrorMessage);
            }

            var fileValidationResult = ValidateFileSignature(file);
            if (fileValidationResult.Count > 0 && fileValidationResult.All(x => x.Value == false))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success!;
        }

        private Dictionary<Type, bool> ValidateFileSignature(IFormFile file)
        {
            var result = new Dictionary<Type, bool>();
            foreach (var v in _fileTypeValidators)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    var isMatch = v.IsMatch(memoryStream);
                    result.Add(v.GetType(), isMatch);
                }
            }
            return result;
        }
    }
}