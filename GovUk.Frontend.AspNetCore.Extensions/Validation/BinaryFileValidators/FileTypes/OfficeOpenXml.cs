using System.IO.Compression;
using System.IO;
using System;
using System.Linq;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation.BinaryFileValidators.FileTypes
{
    /// <summary>
    /// Specifies the format of an Office Open XML file.
    /// </summary>
    public abstract class OfficeOpenXml : Zip
    {
        /// <summary>
        /// Gets the entry in the file which can be used to identify the format.
        /// </summary>
        /// <remarks>
        public readonly string IdentifiableEntry;

        /// <summary>
        /// Initializes a new instance of the OfficeOpenXmlFormat class which matches an archive containing a unique entry.
        /// </summary>
        /// <param name="identifiableEntry">The entry in the archive which is used to identify the format.</param>
        /// <param name="mediaType">The media type of the format.</param>
        /// <param name="extension">The appropriate extension for the format.</param>
        protected OfficeOpenXml(string identifiableEntry, string mediaType, string extension) : base(int.MaxValue, mediaType, extension)
        {
            if (string.IsNullOrEmpty(identifiableEntry))
            {
                throw new ArgumentNullException(nameof(identifiableEntry));
            }

            IdentifiableEntry = identifiableEntry;
        }

        public override bool IsMatch(Stream stream)
        {
            var isValid = base.IsMatch(stream);
            var zipFile = ToZipArchive(stream);
            if (isValid && (zipFile) is ZipArchive archive)
            {
                // Match archives which contain a non-standard version of the identifiable entry, e.g. document2.xml instead of document.xml.
                var index = Math.Max(0, IdentifiableEntry.LastIndexOf('.'));
                var fileName = IdentifiableEntry.Substring(0, IdentifiableEntry.Length - index);
                var extension = IdentifiableEntry.Substring(index);
                return archive.Entries.Any(e => e.FullName.StartsWith(fileName, StringComparison.OrdinalIgnoreCase)
                        && e.FullName.EndsWith(extension, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                return false;
            }
        }

        private IDisposable? ToZipArchive(Stream stream)
        {
            try
            {
                return new ZipArchive(stream, ZipArchiveMode.Read, true);
            }
            catch (InvalidDataException)
            {
                return null;
            }
        }
    }
}