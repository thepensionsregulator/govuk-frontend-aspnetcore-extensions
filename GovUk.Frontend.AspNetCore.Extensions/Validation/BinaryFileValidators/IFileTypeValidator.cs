using System.IO;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators
{
    public interface IFileTypeValidator
    {
        bool IsMatch(Stream stream)


        bool IsMatch(Stream stream, string filename);
    }
}