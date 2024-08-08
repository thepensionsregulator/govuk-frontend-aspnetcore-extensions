using System.IO;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators
{
    public interface IFileTypeValidator
    {
        string[] Extensions { get; }

        bool IsMatch(Stream stream);
    }
}