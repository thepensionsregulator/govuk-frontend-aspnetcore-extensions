using System.IO;
using System.Security.Cryptography.Xml;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators.FileTypes
{
    /// <summary>
    /// Specifies the format of a Portable Document Format (PDF) file.
    /// </summary>
    public class Pdf : FileType, IFileTypeValidator
    {
        private const uint MaxFileHeaderSize = 1024;

        public Pdf() : this([0x25, 0x50, 0x44, 0x46])
        {
        }

        protected Pdf(byte[] signature) : base(signature, "application/pdf", [".pdf"], 0)
        {
        }

        public override bool IsMatch(Stream stream)
        {
            if (stream == null || stream.Length < HeaderLength)
            {
                return false;
            }

            stream.Position = 0;
            var signatureValidationIndex = 0;
            int fileByte;

            while (stream.Position < MaxFileHeaderSize && (fileByte = stream.ReadByte()) != -1)
            {
                if (CompareFileByteToSignatureAt((byte)fileByte, signatureValidationIndex))
                {
                    signatureValidationIndex++;
                }
                else
                {
                    signatureValidationIndex = 0;
                }

                if (signatureValidationIndex == Signature.Count)
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual bool CompareFileByteToSignatureAt(byte fileByte, int signatureIndex)
        {
            return fileByte == Signature[signatureIndex];
        }
    }
}